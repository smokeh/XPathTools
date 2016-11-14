﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReasonCodeExample.XPathInformation
{
    internal class XmlRepository
    {
        private int? _cachedXmlHashCode;
        private XElement _rootElement;
        private XObject _stored;

        public void SetSelected(XObject obj)
        {
            _stored = obj;
        }

        public XObject GetSelected()
        {
            return _stored;
        }

        public void LoadXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return;

            if (_cachedXmlHashCode == xml.GetHashCode())
                return;

            _rootElement = null;
            _cachedXmlHashCode = null;
            XDocument document;
            try
            {
                document = XDocument.Parse(xml, LoadOptions.SetLineInfo);
            }
            catch (Exception)
            {
                return;
            }
            _rootElement = document.Root;
            _cachedXmlHashCode = xml.GetHashCode();
        }

        public XElement GetRootElement()
        {
            return _rootElement;
        }

        public int? GetNodeCount(XObject xml, string xpath)
        {
            if (xml == null)
            {
                return null;
            }
            if (xml.Document == null)
            {
                return null;
            }
            if (xml.Document.Root == null)
            {
                return null;
            }
            try
            {
                var namespaceResolver = new SimpleXmlNamespaceResolver(xml.Document);
                var results = (IEnumerable)xml.Document.XPathEvaluate(xpath, namespaceResolver);
                return results.OfType<XObject>().Count();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the XML node located at the specified line and position.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="lineNumber">One-based line number.</param>
        /// <param name="linePosition">
        /// One-based line position. The line position of an XML element is equal to the position 
        /// of the first character in its name, not the position of the opening angle bracket.
        /// E.g. the XML fragment "&lt;element /&gt;" has line position 2.
        /// </param>
        /// <returns>The matching XML node or <c>null</c> if no match was found.</returns>
        public XElement GetElement(XElement rootElement, int lineNumber, int linePosition)
        {
            if (rootElement == null)
                return null;

            var elements = rootElement.DescendantsAndSelf();
            var matchingElement = (from element in elements
                                        where IsCorrectLine(element, lineNumber)
                                        where IsCorrectPosition(element, linePosition)
                                        select element).LastOrDefault();
            if (matchingElement != null)
                return matchingElement;

            var matchingAttribute = GetAttribute(elements, lineNumber, linePosition);
            return matchingAttribute == null ? null : matchingAttribute.Parent;
        }

        private bool IsCorrectLine(IXmlLineInfo lineInfo, int lineNumber)
        {
            return lineInfo.LineNumber == lineNumber;
        }

        private bool IsCorrectPosition(IXmlLineInfo lineInfo, int linePosition)
        {
            return lineInfo.LinePosition <= linePosition;
        }

        private XAttribute GetAttribute(IEnumerable<XElement> elements, int lineNumber, int linePosition)
        {
            return (from element in elements
                    from attribute in element.Attributes()
                    where IsCorrectLine(attribute, lineNumber)
                    where IsCorrectPosition(attribute, linePosition)
                    select attribute).LastOrDefault();
        }

        public XAttribute GetAttribute(XElement element, int lineNumber, int linePosition)
        {
            if (element == null)
                return null;
            return GetAttribute(new[] { element }, lineNumber, linePosition);
        }
    }
}