﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using Unity.Configuration.ConfigurationHelpers;
using Unity.Configuration.Properties;

namespace Unity.Configuration
{
    /// <summary>
    /// A class representing a property configuration element.
    /// </summary>
    public class PropertyElement : InjectionMemberElement, IValueProvidingElement
    {
        private const string NamePropertyName = "name";
        private readonly ValueElementHelper valueElementHelper;
        private ParameterValueElement valueElement;

        /// <summary>
        /// Construct a new instance of <see cref="PropertyElement"/>
        /// </summary>
        public PropertyElement()
        {
            this.valueElementHelper = new ValueElementHelper(this);
        }

        /// <summary>
        /// Name of the property that will be set.
        /// </summary>
        [ConfigurationProperty(NamePropertyName, IsRequired = true)]
        public string Name
        {
            get { return (string)base[NamePropertyName]; }
            set { base[NamePropertyName] = value; }
        }

        /// <summary>
        /// Each element must have a unique key, which is generated by the subclasses.
        /// </summary>
        public override string Key
        {
            get { return "property:" + this.Name; }
        }

        /// <summary>
        /// String that will be deserialized to provide the value.
        /// </summary>
        public ParameterValueElement Value
        {
            get { return ValueElementHelper.GetValue(this.valueElement); }
            set { this.valueElement = value; }
        }

        ParameterValueElement IValueProvidingElement.Value
        {
            get { return this.valueElement; }
            set { this.Value = value; }
        }

        /// <summary>
        /// A string describing where the value this element contains
        /// is being used. For example, if setting a property Prop1,
        /// this should return "property Prop1" (in english).
        /// </summary>
        public string DestinationName
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
                    Resources.DestinationNameFormat,
                    Resources.Property, this.Name);
            }
        }

        /// <summary>
        /// Element name to use to serialize this into XML.
        /// </summary>
        public override string ElementName
        {
            get { return "property"; }
        }

        /// <summary>
        /// Reads XML from the configuration file.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> that reads from the configuration file.</param>
        /// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The element to read is locked.
        /// - or -
        /// An attribute of the current node is not recognized.
        /// - or -
        /// The lock status of the current node cannot be determined.  
        /// </exception>
        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
            this.valueElementHelper.CompleteValueElement(reader);
        }

        /// <summary>
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization.
        /// </summary>
        /// <returns>
        /// true when an unknown attribute is encountered while deserializing; otherwise, false.
        /// </returns>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return this.valueElementHelper.DeserializeUnrecognizedAttribute(name, value);
        }

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <returns>
        /// true when an unknown element is encountered while deserializing; otherwise, false.
        /// </returns>
        /// <param name="elementName">The name of the unknown subelement.</param>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> being used for deserialization.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The element identified by <paramref name="elementName"/> is locked.
        /// - or -
        /// One or more of the element's attributes is locked.
        /// - or -
        /// <paramref name="elementName"/> is unrecognized, or the element has an unrecognized attribute.
        /// - or -
        /// The element has a Boolean attribute with an invalid value.
        /// - or -
        /// An attempt was made to deserialize a property more than once.
        /// - or -
        /// An attempt was made to deserialize a property that is not a valid member of the element.
        /// - or -
        /// The element cannot contain a CDATA or text element.
        /// </exception>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            return
                this.valueElementHelper.DeserializeUnknownElement(elementName, reader) ||
                base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        /// <summary>
        /// Write the contents of this element to the given <see cref="XmlWriter"/>.
        /// </summary>
        /// <remarks>The caller of this method has already written the start element tag before
        /// calling this method, so deriving classes only need to write the element content, not
        /// the start or end tags.</remarks>
        /// <param name="writer">Writer to send XML content to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
            Justification = "Validation done by Guard class")]
        public override void SerializeContent(XmlWriter writer)
        {
            Guard.ArgumentNotNull(writer, "writer");
            writer.WriteAttributeString(NamePropertyName, this.Name);
            ValueElementHelper.SerializeParameterValueElement(writer, this.Value, false);
        }

        /// <summary>
        /// Return the set of <see cref="InjectionMember"/>s that are needed
        /// to configure the container according to this configuration element.
        /// </summary>
        /// <param name="container">Container that is being configured.</param>
        /// <param name="fromType">Type that is being registered.</param>
        /// <param name="toType">Type that <paramref name="fromType"/> is being mapped to.</param>
        /// <param name="name">Name this registration is under.</param>
        /// <returns>One or more <see cref="InjectionMember"/> objects that should be
        /// applied to the container registration.</returns>
        public override IEnumerable<InjectionMember> GetInjectionMembers(IUnityContainer container, Type fromType, Type toType, string name)
        {
            return new[] { new InjectionProperty(this.Name, this.Value.GetInjectionParameterValue(container, this.GetPropertyType(toType))) };
        }

        private Type GetPropertyType(Type typeContainingProperty)
        {
            var propertyInfo = typeContainingProperty.GetProperty(this.Name);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        Resources.NoSuchProperty,
                        typeContainingProperty.Name, this.Name));
            }

            return propertyInfo.PropertyType;
        }
    }
}
