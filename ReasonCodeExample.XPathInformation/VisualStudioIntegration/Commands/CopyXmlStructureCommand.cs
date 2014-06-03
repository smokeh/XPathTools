﻿using System;
using ReasonCodeExample.XPathInformation.Writers;

namespace ReasonCodeExample.XPathInformation.VisualStudioIntegration.Commands
{
    internal class CopyXmlStructureCommand : CopyCommand
    {
        public CopyXmlStructureCommand(int id, XmlRepository repository, Func<IWriter> writerProvider)
            : base(id, repository, writerProvider)
        {
        }

        protected override void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            var xml = Repository.Get();
            var writer = WriterProvider();
            Output = writer.Write(xml);
            Command.Visible = !string.IsNullOrEmpty(Output);
        }
    }
}