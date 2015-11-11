// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

// Duplicate using statements to avoid ordering using warning SA1210. 
// ifdefs seem to be confusing stylecop
#if NETFX_CORE
#if !WINDOWS_PHONE
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ObjectBuilder2;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.System.Threading;
#else
using System.Linq;
using System.Threading;
using ObjectBuilder2;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
#elif __IOS__
using System.Linq;
using System.Threading;
using ObjectBuilder2;
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
#else
using System.Linq;
using System.Threading;
using ObjectBuilder2;
using Xunit;
#endif

namespace Unity.Tests
{
     
    public class PerThreadLifetimeManagerFixture
    {
        [Fact]
        public void CanCreateLifetimeManager()
        {
            new PerThreadLifetimeManager();
        }

        [Fact]
        public void NewLifetimeManagerReturnsNullForObject()
        {
            LifetimeManager ltm = new PerThreadLifetimeManager();
            Assert.Null(ltm.GetValue());
        }

        [Fact]
        public void LifetimeManagerReturnsValueThatWasSetOnSameThread()
        {
            LifetimeManager ltm = new PerThreadLifetimeManager();
            string expected = "Here's the value";

            ltm.SetValue(expected);
            object result = ltm.GetValue();
            Assert.Same(expected, result);
        }

        [Fact]
        public void DifferentLifetimeContainerInstancesHoldDifferentObjects()
        {
            LifetimeManager ltm1 = new PerThreadLifetimeManager();
            LifetimeManager ltm2 = new PerThreadLifetimeManager();
            string expected1 = "Here's the first value";
            string expected2 = "Here's the second value";

            ltm1.SetValue(expected1);
            ltm2.SetValue(expected2);

            object result1 = ltm1.GetValue();
            object result2 = ltm2.GetValue();
            Assert.Same(expected1, result1);
            Assert.Same(expected2, result2);
        }

        [Fact]
        public void LifetimeManagerReturnsNullIfCalledOnADifferentThreadFromTheOneThatSetTheValue()
        {
            LifetimeManager ltm = new PerThreadLifetimeManager();
            string expected = "Here's the value";

            ltm.SetValue(expected);

            // Providing dummy initializers so we can prove the values are different coming out of the LTM
            var otherThreadResult = new object();

            RunInParallel(() => { otherThreadResult = ltm.GetValue(); });

            Assert.Same(expected, ltm.GetValue());
            Assert.Null(otherThreadResult);
        }

        [Fact]
        public void LifetimeManagerReturnsDifferentValuesForEachThread()
        {
            LifetimeManager ltm = new PerThreadLifetimeManager();
            string one = "one";
            string two = "two";
            string three = "three";

            object valueOne = null;
            object valueTwo = null;
            object valueThree = null;

            var barrier = new Barrier(3);
            RunInParallel(
                delegate
                {
                    ltm.SetValue(one);
                    barrier.SignalAndWait();
                    valueOne = ltm.GetValue();
                },
                delegate
                {
                    ltm.SetValue(three);
                    barrier.SignalAndWait();
                    valueThree = ltm.GetValue();
                },
                delegate
                {
                    ltm.SetValue(two);
                    barrier.SignalAndWait();
                    valueTwo = ltm.GetValue();
                });

            Assert.Same(one, valueOne);
            Assert.Same(two, valueTwo);
            Assert.Same(three, valueThree);
        }

        [Fact]
        public void CanRegisterLifetimeManagerInContainerAndUseItOnOneThread()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<object>(new PerThreadLifetimeManager());

            object result1 = container.Resolve<object>();
            object result2 = container.Resolve<object>();

            Assert.Same(result1, result2);
        }

        [Fact]
        public void ReturnsDifferentObjectsOnDifferentThreadsFromContainer()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<object>(new PerThreadLifetimeManager());

            object result1 = null;
            object result2 = null;

            RunInParallel(
                delegate { result1 = container.Resolve<object>(); },
                delegate { result2 = container.Resolve<object>(); });

            Assert.NotNull(result1);
            Assert.NotNull(result2);

            Assert.NotSame(result1, result2);
        }

        [Fact]
        public void RegisteringAnInstanceInAThreadSetsPerThreadLifetimeManagerWhenResolvingInOtherThreads()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<object>(new PerThreadLifetimeManager());
            LifetimeManager manager = new PerThreadLifetimeManager();

            object registered = new object();
            object result1A = null;
            object result1B = null;
            object result2A = null;
            object result2B = null;

            container.RegisterInstance(registered, manager);

            var barrier = new Barrier(2);
            RunInParallel(
                delegate
                {
                    result1A = container.Resolve<object>();
                    barrier.SignalAndWait();
                    result1B = container.Resolve<object>();
                },
                delegate
                {
                    result2A = container.Resolve<object>();
                    barrier.SignalAndWait();
                    result2B = container.Resolve<object>();
                });
            object result = container.Resolve<object>();

            Assert.NotNull(result1A);
            Assert.NotNull(result2A);
            Assert.NotNull(result);

            Assert.NotSame(result1A, result2A);
            Assert.NotSame(registered, result1A);
            Assert.NotSame(registered, result2A);
            Assert.Same(result1A, result1B);
            Assert.Same(result2A, result2B);
            Assert.Same(registered, result);
        }

#if NETFX_CORE && !WINDOWS_PHONE
        // Helper method to run a bunch of delegates, each on a separate thread.
        // It runs them and then waits for them all to complete.
        private static void RunInParallel(params System.Action[] actions)
        {
            var barrier = new Barrier(actions.Length);
            var tasks = actions.Select(a =>
                    ThreadPool.RunAsync(
                        _ =>
                        {
                            barrier.SignalAndWait();
                            a();
                        }).AsTask()).ToArray();

            Task.WaitAll(tasks);
        }
#else
        // Helper method to run a bunch of delegates, each on a separate thread.
        // It runs them and then waits for them all to complete.
        private static void RunInParallel(params ThreadStart[] actions)
        {
            // We use explicit threads here rather than delegate.BeginInvoke
            // because the latter uses the threadpool, and could reuse thread
            // pool threads depending on timing. We want to guarantee different
            // threads for these tests.

            Thread[] threads = actions.Select(a => new Thread(a)).ToArray();

            // Start them all...
            threads.ForEach(t => t.Start());

            // And wait for them all to finish
            threads.ForEach(t => t.Join());
        }
#endif
    }
}
