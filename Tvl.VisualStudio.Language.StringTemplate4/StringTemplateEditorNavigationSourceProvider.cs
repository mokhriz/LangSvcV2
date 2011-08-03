﻿namespace Tvl.VisualStudio.Language.StringTemplate4
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Threading;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Text.Navigation;
    using Action = System.Action;
    using ImageSource = System.Windows.Media.ImageSource;
    using IOutputWindowService = Tvl.VisualStudio.Shell.OutputWindow.IOutputWindowService;
    using ReaderWriterLockSlim = System.Threading.ReaderWriterLockSlim;
    using IDispatcherGlyphService = Tvl.VisualStudio.Language.Intellisense.IDispatcherGlyphService;

    [Export(typeof(IEditorNavigationSourceProvider))]
    [ContentType(StringTemplateConstants.StringTemplateContentType)]
    public sealed class StringTemplateEditorNavigationSourceProvider : IEditorNavigationSourceProvider
    {
        [Import]
        public IBackgroundParserFactoryService BackgroundParserFactoryService
        {
            get;
            private set;
        }

        [Import]
        public IEditorNavigationTypeRegistryService EditorNavigationTypeRegistryService
        {
            get;
            private set;
        }

        [Import]
        public IDispatcherGlyphService GlyphService
        {
            get;
            private set;
        }

        [Import]
        public IOutputWindowService OutputWindowService
        {
            get;
            private set;
        }

        public IEditorNavigationSource TryCreateEditorNavigationSource(ITextBuffer textBuffer)
        {
            var backgroundParser = BackgroundParserFactoryService.GetBackgroundParser(textBuffer);
            if (backgroundParser == null)
                return null;

            return new StringTemplateEditorNavigationSource(textBuffer, backgroundParser, this);
        }
    }
}
