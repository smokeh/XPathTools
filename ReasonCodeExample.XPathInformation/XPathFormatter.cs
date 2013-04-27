﻿using System.Linq;
using System.Xml.Linq;

namespace ReasonCodeExample.XPathInformation
{
    internal class XPathFormatter
    {
        /// <summary>
        /// Returns the XPath of the element. E.g. "/configuration/ns:settings".
        /// </summary>
        public string Format(XElement element)
        {
            if (element == null)
                return string.Empty;
            return GetElementXPath(element);
        }

        private string GetElementXPath(XElement element)
        {
            return element.AncestorsAndSelf().Reverse().Select(GetElementName).Aggregate(string.Empty, ConcatenateXPath);
        }

        private string GetElementName(XElement element)
        {
            if (string.IsNullOrEmpty(element.Name.NamespaceName))
                return element.Name.LocalName;

            string namespacePrefix = element.GetPrefixOfNamespace(element.Name.Namespace);
            if (string.IsNullOrEmpty(namespacePrefix))
                return element.Name.LocalName;

            return namespacePrefix + ":" + element.Name.LocalName;
        }

        private string ConcatenateXPath(string current, string next)
        {
            return current + "/" + next;
        }

        /// <summary>
        /// Returns the local XPath of the attribute. E.g. "[@ns:name]".
        /// </summary>
        public string Format(XAttribute attribute)
        {
            if (attribute == null)
                return string.Empty;

            if (string.IsNullOrEmpty(attribute.Name.NamespaceName))
                return string.Format("[@{0}]", attribute.Name.LocalName);

            string namespacePrefix = attribute.Parent.GetPrefixOfNamespace(attribute.Name.Namespace);
            if (string.IsNullOrEmpty(namespacePrefix))
                return string.Format("[@{0}]", attribute.Name.LocalName);

            return string.Format("[@{0}:{1}]", namespacePrefix, attribute.Name.LocalName);
        }
    }
}