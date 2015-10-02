// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.TransparaentProxyInterception.Tests
{
     
    public class InterceptingRealProxyFixture
    {
        [Fact]
        public void CanProxyMBROMethods()
        {
            MBROWithOneMethod original = new MBROWithOneMethod();
            MBROWithOneMethod proxy = new InterceptingRealProxy(original, typeof(MBROWithOneMethod))
                .GetTransparentProxy() as MBROWithOneMethod;

            Assert.NotNull(proxy);
        }

        [Fact]
        public void ProxyImplementsIInterceptingProxy()
        {
            MBROWithOneMethod original = new MBROWithOneMethod();
            MBROWithOneMethod proxy = new InterceptingRealProxy(original, typeof(MBROWithOneMethod))
                .GetTransparentProxy() as MBROWithOneMethod;

            Assert.NotNull(proxy as IInterceptingProxy);
        }

        [Fact]
        public void CanInterceptMethodsThroughProxy()
        {
            CallCountInterceptionBehavior interceptor = new CallCountInterceptionBehavior();

            MBROWithOneMethod original = new MBROWithOneMethod();
            MBROWithOneMethod intercepted = new InterceptingRealProxy(original, typeof(MBROWithOneMethod))
                .GetTransparentProxy() as MBROWithOneMethod;

            IInterceptingProxy proxy = (IInterceptingProxy)intercepted;
            proxy.AddInterceptionBehavior(interceptor);

            int result = intercepted.DoSomething(5);

            Assert.Equal(5 * 3, result);
            Assert.Equal(1, interceptor.CallCount);
        }

        [Fact]
        public void ProxyInterceptsAddingAHandlerToAnEvent()
        {
            // arrange
            CallCountInterceptionBehavior interceptor
                = new CallCountInterceptionBehavior();

            MBROWithAnEvent original = new MBROWithAnEvent();
            MBROWithAnEvent intercepted = new InterceptingRealProxy(original, typeof(MBROWithAnEvent))
                .GetTransparentProxy() as MBROWithAnEvent;

            ((IInterceptingProxy)intercepted).AddInterceptionBehavior(interceptor);

            // act
            intercepted.SomeEvent += (s, a) => { };

            // assert
            Assert.Equal(1, interceptor.CallCount);
        }

        [Fact]
        public void ProxySendsOriginalWhenRaisingEvent()
        {
            // arrange
            CallCountInterceptionBehavior interceptor = new CallCountInterceptionBehavior();

            MBROWithAnEvent original = new MBROWithAnEvent();
            MBROWithAnEvent intercepted = new InterceptingRealProxy(original, typeof(MBROWithAnEvent))
                .GetTransparentProxy() as MBROWithAnEvent;

            ((IInterceptingProxy)intercepted).AddInterceptionBehavior(interceptor);
            object sender = null;
            intercepted.SomeEvent += (s, a) => { sender = s; };

            // act
            intercepted.TriggerIt();

            // assert
            Assert.Same(original, sender);
            Assert.Equal(2, interceptor.CallCount);  // adding + calling TriggerIt
        }

        [Fact]
        public void CanCreateProxyWithAdditionalInterfaces()
        {
            MBROWithOneMethod original = new MBROWithOneMethod();
            MBROWithOneMethod proxy =
                new InterceptingRealProxy(original, typeof(MBROWithOneMethod), typeof(InterfaceOne))
                .GetTransparentProxy() as MBROWithOneMethod;

            Assert.True(proxy is InterfaceOne);
        }

        [Fact]
        public void InvokingMethodOnAdditionalInterfaceThrowsIfNotHandledByInterceptor()
        {
            MBROWithOneMethod original = new MBROWithOneMethod();
            InterfaceOne proxy =
                new InterceptingRealProxy(original, typeof(MBROWithOneMethod), typeof(InterfaceOne))
                .GetTransparentProxy() as InterfaceOne;

            try
            {
                proxy.Something();
                Assert.True(false, string.Format("should have thrown"));
            }
            catch (InvalidOperationException)
            {
                // expected
            }
        }

        [Fact]
        public void CanSuccessfullyInvokeAnAdditionalInterfaceMethodIfAnInterceptorDoesNotForwardTheCall()
        {
            MBROWithOneMethod original = new MBROWithOneMethod();
            InterfaceOne proxy =
                new InterceptingRealProxy(original, typeof(MBROWithOneMethod), typeof(InterfaceOne))
                .GetTransparentProxy() as InterfaceOne;
            bool invoked = false;
            ((IInterceptingProxy)proxy).AddInterceptionBehavior(
                new DelegateInterceptionBehavior(
                    (input, getNext) => { invoked = true; return input.CreateMethodReturn(null); }));

            proxy.Something();

            Assert.True(invoked);
        }

        [Fact]
        public void CanImplementINotifyPropertyChanged()
        {
            MBROWithOneProperty target = new MBROWithOneProperty();
            MBROWithOneProperty proxy =
                new InterceptingRealProxy(target, typeof(MBROWithOneProperty), typeof(INotifyPropertyChanged))
                .GetTransparentProxy() as MBROWithOneProperty;
            ((IInterceptingProxy)proxy).AddInterceptionBehavior(new NaiveINotifyPropertyChangedInterceptionBehavior());

            string changeProperty;
            PropertyChangedEventHandler handler = (sender, args) => changeProperty = args.PropertyName;

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged += handler;

            proxy.TheProperty = 100;

            Assert.Equal(100, proxy.TheProperty);
            Assert.Equal("TheProperty", changeProperty);

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged -= handler;

            proxy.TheProperty = 200;

            Assert.Equal(200, proxy.TheProperty);
            Assert.Equal(null, changeProperty);
        }

        [Fact]
        public void CanImplementINotifyPropertyChangedThroughInterface()
        {
            ObjectWithOnePropertyForImplicitlyImplementedInterface target = new ObjectWithOnePropertyForImplicitlyImplementedInterface();
            IInterfaceWithOneProperty proxy =
                new InterceptingRealProxy(target, typeof(IInterfaceWithOneProperty), typeof(INotifyPropertyChanged))
                .GetTransparentProxy() as IInterfaceWithOneProperty;
            ((IInterceptingProxy)proxy).AddInterceptionBehavior(new NaiveINotifyPropertyChangedInterceptionBehavior());

            string changeProperty;
            PropertyChangedEventHandler handler = (sender, args) => changeProperty = args.PropertyName;

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged += handler;

            proxy.TheProperty = 100;

            Assert.Equal(100, proxy.TheProperty);
            Assert.Equal("TheProperty", changeProperty);

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged -= handler;

            proxy.TheProperty = 200;

            Assert.Equal(200, proxy.TheProperty);
            Assert.Equal(null, changeProperty);
        }

        [Fact]
        public void CanImplementINotifyPropertyChangedThroughExplicitInterface()
        {
            ObjectWithOnePropertyForExplicitlyImplementedInterface target = new ObjectWithOnePropertyForExplicitlyImplementedInterface();
            IInterfaceWithOneProperty proxy =
                new InterceptingRealProxy(target, typeof(IInterfaceWithOneProperty), typeof(INotifyPropertyChanged))
                .GetTransparentProxy() as IInterfaceWithOneProperty;
            ((IInterceptingProxy)proxy).AddInterceptionBehavior(new NaiveINotifyPropertyChangedInterceptionBehavior());

            string changeProperty;
            PropertyChangedEventHandler handler = (sender, args) => changeProperty = args.PropertyName;

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged += handler;

            proxy.TheProperty = 100;

            Assert.Equal(100, proxy.TheProperty);
            Assert.Equal("TheProperty", changeProperty);

            changeProperty = null;
            ((INotifyPropertyChanged)proxy).PropertyChanged -= handler;

            proxy.TheProperty = 200;

            Assert.Equal(200, proxy.TheProperty);
            Assert.Equal(null, changeProperty);
        }

        [Fact]
        public void CanInterceptGenericMethodOnInterface()
        {
            var interceptor = new CallCountInterceptionBehavior();

            var original = new ObjectWithGenericMethod();
            var intercepted = new InterceptingRealProxy(original, typeof(IInterfaceWithGenericMethod))
                .GetTransparentProxy() as IInterfaceWithGenericMethod;

            IInterceptingProxy proxy = (IInterceptingProxy)intercepted;
            proxy.AddInterceptionBehavior(interceptor);

            var result = intercepted.GetTypeName(6);

            Assert.Equal("Int32", result);
            Assert.Equal(1, interceptor.CallCount);
        }

        internal class MBROWithOneMethod : MarshalByRefObject
        {
            public int DoSomething(int i)
            {
                return i * 3;
            }
        }

        internal interface InterfaceOne
        {
            void Something();
        }

        internal class MBROWithInterface : MarshalByRefObject, InterfaceOne
        {
            public void Something()
            {
            }
        }

        public class MBROWithAnEvent : MarshalByRefObject
        {
            public event EventHandler<EventArgs> SomeEvent;

            public void TriggerIt()
            {
                this.SomeEvent(this, new EventArgs());
            }
        }

        internal class MBROWithOneProperty : MarshalByRefObject
        {
            public int TheProperty { get; set; }
        }

        internal class ObjectWithOnePropertyForImplicitlyImplementedInterface : IInterfaceWithOneProperty
        {
            public int TheProperty { get; set; }
        }

        internal class ObjectWithOnePropertyForExplicitlyImplementedInterface : IInterfaceWithOneProperty
        {
            int IInterfaceWithOneProperty.TheProperty { get; set; }
        }

        internal interface IInterfaceWithOneProperty
        {
            int TheProperty { get; set; }
        }

        internal interface IInterfaceWithGenericMethod
        {
            string GetTypeName<T>(T argument);
        }

        internal class ObjectWithGenericMethod : IInterfaceWithGenericMethod
        {
            #region IInterfaceWithGenericMethod Members

            public string GetTypeName<T>(T argument)
            {
                return argument.GetType().Name;
            }

            #endregion
        }
    }
}
