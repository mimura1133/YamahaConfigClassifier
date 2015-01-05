using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace YamahaClassifier
{
    internal class YamahaCompletionSource : ICompletionSource
    {
        private readonly ITextBuffer _buffer;
        private readonly YamahaCompletion _provider;
        private List<Completion> _compList;

        public YamahaCompletionSource(YamahaCompletion provider, ITextBuffer buffer)
        {
            _buffer = buffer;
            _provider = provider;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            completionSets.Add(new CompletionSet(
                "Tokens", //the non-localized title of the tab
                "Tokens", //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(_buffer),
                    session),
                YamahaData.CompList,
                null));
        }

        public void Dispose()
        {
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            var currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            var navigator = _provider.NavigatorService.GetTextStructureNavigator(_buffer);
            var extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }
    }
}