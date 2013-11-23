﻿using System.Xml.Linq;
using NUnit.Framework;

namespace ReasonCodeExample.XPathInformation.Tests
{
    [TestFixture]
    public class XPathRepositoryTests
    {
        [Test]
        public void XPathIsStored()
        {
            // Arrange
            XElement expectedValue = new XElement("value");
            XPathRepository repository = new XPathRepository();
            XPathRepository otherRepository = new XPathRepository();

            // Act
            repository.Put(expectedValue);

            // Assert
            Assert.That(otherRepository.Get(), Is.EqualTo(expectedValue));
        }
    }
}