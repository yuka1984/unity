// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Utility;
using Xunit;

namespace Microsoft.Practices.Unity.InterceptionExtension.Tests
{
     
    public class AttributeDrivenPolicyFixture
    {
        private MethodImplementationInfo nothingSpecialMethod;
        private MethodImplementationInfo doSomethingMethod;
        private MethodImplementationInfo getCriticalInfoMethod;
        private MethodImplementationInfo mustBeFastMethod;
        private MethodImplementationInfo getNameMethod;
        private MethodImplementationInfo hasAttributeMethod;
        private MethodImplementationInfo doesntHaveAttributeMethod;
        private MethodImplementationInfo newMethod;
        private MethodImplementationInfo getNewNameMethod;

        private MethodImplementationInfo getItemMethod;
        private MethodImplementationInfo setItemMethod;
        private MethodImplementationInfo getItemIntMethod;
        private MethodImplementationInfo setItemIntMethod;
        private MethodImplementationInfo getItemStringMethod;
        private MethodImplementationInfo setItemStringMethod;

        public AttributeDrivenPolicyFixture()
        {
            nothingSpecialMethod = MakeMethodImpl<AttributeTestTarget>("NothingSpecial");
            doSomethingMethod = MakeMethodImpl<AttributeTestTarget>("DoSomething");
            getCriticalInfoMethod = MakeMethodImpl<AttributeTestTarget>("GetCriticalInformation");
            mustBeFastMethod = MakeMethodImpl<AttributeTestTarget>("MustBeFast");
            getNameMethod = new MethodImplementationInfo(null, typeof(AttributeTestTarget).GetProperty("Name").GetGetMethod());
            hasAttributeMethod = MakeMethodImpl<SecondAttributeTestTarget>("HasAttribute");
            doesntHaveAttributeMethod = MakeMethodImpl<SecondAttributeTestTarget>("DoesntHaveAttribute");
            newMethod = MakeMethodImpl<DerivedAttributeTestTarget>("ANewMethod");
            getNewNameMethod = new MethodImplementationInfo(null, StaticReflection.GetPropertyGetMethodInfo((DerivedAttributeTestTarget t) => t.Name));

            getItemMethod = new MethodImplementationInfo(null, StaticReflection.GetPropertyGetMethodInfo((DerivedAttributeTestTarget t) => t.Item));
            setItemMethod = new MethodImplementationInfo(null, StaticReflection.GetPropertySetMethodInfo((DerivedAttributeTestTarget t) => t.Item));

            getItemIntMethod = new MethodImplementationInfo(null, typeof(DerivedAttributeTestTarget).GetMethod("get_Item", new[] { typeof(int) }));
            setItemIntMethod = new MethodImplementationInfo(null, typeof(DerivedAttributeTestTarget).GetMethod("set_Item", new[] { typeof(int), typeof(object) }));

            getItemStringMethod = new MethodImplementationInfo(null, typeof(DerivedAttributeTestTarget).GetMethod("get_Item", new[] { typeof(string), typeof(double) }));
            setItemStringMethod = new MethodImplementationInfo(null, typeof(DerivedAttributeTestTarget).GetMethod("set_Item", new[] { typeof(string), typeof(double), typeof(object) }));
        }

        private static MethodImplementationInfo MakeMethodImpl<T>(string name)
        {
            return new MethodImplementationInfo(null, typeof(T).GetMethod(name));
        }

        [Fact]
        public void MatchingRuleMatchesForAllMethodsInAttributeTestTarget()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();
            Assert.True(rule.Matches(nothingSpecialMethod.ImplementationMethodInfo));
            Assert.True(rule.Matches(doSomethingMethod.ImplementationMethodInfo));
            Assert.True(rule.Matches(getCriticalInfoMethod.ImplementationMethodInfo));
            Assert.True(rule.Matches(mustBeFastMethod.ImplementationMethodInfo));
        }

        [Fact]
        public void MatchingRuleOnlyMatchesOnMethodsWithAttributes()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();

            Assert.True(rule.Matches(hasAttributeMethod.ImplementationMethodInfo));
            Assert.False(rule.Matches(doesntHaveAttributeMethod.ImplementationMethodInfo));
        }

        [Fact]
        public void ShouldMatchInheritedHandlerAttributes()
        {
            IMatchingRule rule = new AttributeDrivenPolicyMatchingRule();
            Assert.True(rule.Matches(newMethod.ImplementationMethodInfo));
        }

        [Fact]
        public void ShouldHaveAttributesCauseMatchesOnMethods()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();

            Assert.True(policy.Matches(nothingSpecialMethod));
            Assert.False(policy.Matches(mustBeFastMethod));
        }

        [Fact]
        public void ShouldGetCorrectHandlersForMethods()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(nothingSpecialMethod, new UnityContainer()));

            Assert.Equal(1, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
        }

        [Fact]
        public void ShouldGetHandlersFromClassAndMethodAttributes()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(doSomethingMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldGetNoHandlersIfApplyNoPoliciesIsPresent()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(mustBeFastMethod, new UnityContainer()));
            Assert.Equal(0, handlers.Count);
        }

        [Fact]
        public void ShouldHaveLoggingHandlerForNothingSpecial()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers
                = new List<ICallHandler>(policy.GetHandlersFor(nothingSpecialMethod, new UnityContainer()));
            Assert.Equal(1, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
        }

        [Fact]
        public void ShouldHaveLoggingAndValidationForDoSomething()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers
                = new List<ICallHandler>(policy.GetHandlersFor(doSomethingMethod, new UnityContainer()));

            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersIfAttributesAreOnProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getNameMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersIfAttributesAreOnNewProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getNewNameMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler1), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToGetterIfAttributesAreOnItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getItemMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToSetterIfAttributesAreOnItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(setItemMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToGetterIfAttributesAreOnIndexedItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getItemIntMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler1), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToSetterIfAttributesAreOnIndexedItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(setItemIntMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler1), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToGetterIfAttributesAreOnSecondIndexedItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(getItemStringMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldApplyHandlersToSetterIfAttributesAreOnSecondIndexedItemProperty()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers =
                new List<ICallHandler>(policy.GetHandlersFor(setItemStringMethod, new UnityContainer()));
            Assert.Equal(2, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
            Assert.Same(typeof(CallHandler3), handlers[1].GetType());
        }

        [Fact]
        public void ShouldInheritHandlersFromBaseClass()
        {
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers
                = new List<ICallHandler>(policy.GetHandlersFor(newMethod, new UnityContainer()));
            Assert.Equal(1, handlers.Count);
            Assert.Same(typeof(CallHandler2), handlers[0].GetType());
        }

        [Fact]
        public void ShouldInheritHandlersFromInterface()
        {
            MethodImplementationInfo getNewsMethod = new MethodImplementationInfo(
                typeof(INewsService).GetMethod("GetNews"),
                typeof(NewsService).GetMethod("GetNews"));
            AttributeDrivenPolicy policy = new AttributeDrivenPolicy();
            List<ICallHandler> handlers
                = new List<ICallHandler>(policy.GetHandlersFor(getNewsMethod, new UnityContainer()));
            Assert.Equal(1, handlers.Count);
            Assert.Same(typeof(CallHandler1), handlers[0].GetType());
        }
    }

    [CallHandler2]//(Categories = new string[] { "one", "two" }, Priority = 34)]
    internal class AttributeTestTarget
    {
        [CallHandler3]
        public string Name
        {
            get { return "someName"; }
            set { }
        }

        [CallHandler3]
        public string DoSomething(string key,
                                  int value)
        {
            return "I did something";
        }

        public int GetCriticalInformation(string key)
        {
            return 42;
        }

        [ApplyNoPolicies]
        public void MustBeFast() { }

        public int NothingSpecial()
        {
            return 43;
        }

        [CallHandler1]
        public object this[int ignored] { get { return null; } set { } }

        [CallHandler3]
        public object this[string ignored, double ignored2] { get { return null; } set { } }
    }

    internal class SecondAttributeTestTarget
    {
        public void DoesntHaveAttribute() { }

        [CallHandler2]
        public void HasAttribute() { }
    }

    internal class DerivedAttributeTestTarget : AttributeTestTarget
    {
        public void ANewMethod() { }

        [CallHandler1]
        public new string Name
        {
            get { return "someOtherName"; }
            set { }
        }

        [CallHandler3]
        public object Item { get; set; }
    }

    public interface INewsService
    {
        [CallHandler1]//(0, 0, 30)]
        IList GetNews();
    }

    public class NewsService : INewsService
    {
        public IList GetNews()
        {
            return new List<string> { "News1", "News2", "News3" };
        }
    }

    public class CallHandler1Attribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new CallHandler1();
        }
    }

    public class CallHandler1 : ICallHandler
    {
        #region ICallHandler Members

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Order
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }

    public class CallHandler2Attribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new CallHandler2();
        }
    }

    public class CallHandler2 : ICallHandler
    {
        #region ICallHandler Members

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Order
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }

    public class CallHandler3Attribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new CallHandler3();
        }
    }

    public class CallHandler3 : ICallHandler
    {
        #region ICallHandler Members

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Order
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
