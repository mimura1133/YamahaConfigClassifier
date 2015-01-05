using System.Collections.Generic;
using System.Linq;
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
            var snap = _buffer.CurrentSnapshot;
            var tpos = session.GetTriggerPoint(_buffer);
            var pos = tpos.GetPosition(snap);
            var line =
                snap.GetLineFromLineNumber(
                    snap.GetLineNumberFromPosition(pos));

            if (line.End == pos)
            {
                var linetext = line.GetText();
                var start = line.Start.Position;

                while (true)
                {
                    if (linetext == "") break;

                    if (linetext[0] == ' ' || linetext[0] == '\t')
                    {
                        start++;
                        linetext = linetext.Substring(1);
                    }
                    else
                    {
                        break;
                    }
                }

                if (linetext == "")
                    return;

                var complist =
                    YamahaData.CompList.Where(
                        n => n.InsertionText.Length >= linetext.Length && n.InsertionText.Substring(0, linetext.Length).ToLower() == linetext.ToLower());

                var navigator = _provider.NavigatorService.GetTextStructureNavigator(_buffer);
                var span = snap.CreateTrackingSpan(start,line.End - start, SpanTrackingMode.EdgeInclusive);

                    completionSets.Add(new CompletionSet(
                        "Tokens", //the non-localized title of the tab
                        "Tokens", //the display title of the tab
                        span,
                        complist,
                        null));
            }
        }

        public void Dispose()
        {
        }
    }
}