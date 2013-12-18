﻿using System.Xml.Linq;

namespace ReasonCodeExample.XPathInformation.Formatters
{
    public interface IXPathFormatter
    {
        /// <summary>
        /// Returns the GetXPath of the element or attribute. 
        /// E.g. "/configuration/ns:settings" or "/configuration/ns:settings[@ns:name]".
        /// </summary>
        string Format(XObject obj);

        /// <summary>
        /// Returns the GetXPath of the element. E.g. "/configuration/ns:settings".
        /// </summary>
        string Format(XElement element);

        /// <summary>
        /// Returns the local GetXPath of the attribute. E.g. "[@ns:name]".
        /// </summary>
        string Format(XAttribute attribute);
    }
}