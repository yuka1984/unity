// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Configuration;
using System.IO;
using System.Xml;
using Microsoft.Practices.Unity.TestSupport;
using Xunit;

namespace Microsoft.Practices.Unity.Configuration.Tests
{
     
    public class When_DeserializingParameterElement
    {
        [Fact]
        public void Then_CanDeserializeSingleInjectionValueChild()
        {
            var elementXml = @"
                <param name=""connectionString"">
                    <value value=""northwind"" />
                </param>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new ParameterElement();

            element.Deserialize(reader);

            Assert.Same(typeof(ValueElement), element.Value.GetType());
            Assert.Equal("northwind", ((ValueElement)element.Value).Value);
        }

        [Fact]
        public void Then_DeserializingMultipleInjectionValueChildrenThrows()
        {
            var elementXml = @"
                <param name=""connectionString"">
                    <value value=""northwind"" />
                    <value value=""northwind"" />
                </param>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new ParameterElement();

            AssertExtensions.AssertException<ConfigurationErrorsException>(() =>
                {
                    element.Deserialize(reader);
                });
        }

        [Fact]
        public void Then_DeserializingWithParametersAndValueChildrenThrows()
        {
            var elementXml = @"
                <param name=""connectionString"" value=""northwind2"">
                    <value value=""northwind"" />
                </param>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new ParameterElement();
            AssertExtensions.AssertException<ConfigurationErrorsException>(() =>
            {
                element.Deserialize(reader);
            });
        }
    }

     
    public class When_DeserializingPropertyElement
    {
        [Fact]
        public void Then_CanDeserializeSingleInjectionValueChild()
        {
            var elementXml = @"
                <property name=""connectionString"">
                    <value value=""northwind"" />
                </property>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new PropertyElement();

            element.Deserialize(reader);

            Assert.Same(typeof(ValueElement), element.Value.GetType());
            Assert.Equal("northwind", ((ValueElement)element.Value).Value);
        }

        [Fact]
        public void Then_DeserializingMultipleInjectionValueChildrenThrows()
        {
            var elementXml = @"
                <property name=""connectionString"">
                    <value value=""northwind"" />
                    <value value=""northwind"" />
                </property>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new PropertyElement();
            AssertExtensions.AssertException<ConfigurationErrorsException>(() =>
                {
                    element.Deserialize(reader);
                });
        }

        [Fact]
        public void Then_DeserializingWithParametersAndValueChildrenThrows()
        {
            var elementXml = @"
                <property name=""connectionString"" value=""northwind2"">
                    <value value=""northwind"" />
                </property>";

            var reader = new XmlTextReader(new StringReader(elementXml));
            var result = reader.MoveToContent();
            var element = new PropertyElement();
            AssertExtensions.AssertException<ConfigurationErrorsException>(() =>
                {
                    element.Deserialize(reader);
                });
        }
    }
}
