using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{
    [Export(typeof (ICompletionSourceProvider))]
    [ContentType("yamahaconf")]
    [Name("Yamaha Completion")]
    [Order(Before = "default")]
    internal class YamahaCompletion : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new YamahaCompletionSource(this, textBuffer);
        }
    }
}