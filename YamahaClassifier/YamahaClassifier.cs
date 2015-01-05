using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{

    #region Provider definition

    /// <summary>
    ///     This class causes a classifier to be added to the set of classifiers. Since
    ///     the content type is set to "text", this classifier applies to all text files
    /// </summary>
    [Export(typeof (IClassifierProvider))]
    [ContentType("yamahaconf")]
    internal class YamahaClassifierProvider : IClassifierProvider
    {
        /// <summary>
        ///     Import the classification registry to be used for getting a reference
        ///     to the custom classification type later.
        /// </summary>
        [Import] internal IClassificationTypeRegistryService ClassificationRegistry; // Set via MEF

        public YamahaClassifierProvider()
        {
            YamahaData.LoadData();
        }

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return
                buffer.Properties.GetOrCreateSingletonProperty(
                    delegate { return new YamahaClassifier(ClassificationRegistry); });
        }
    }

    #endregion //provider def

    #region Classifier

    /// <summary>
    ///     Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    internal class YamahaClassifier : IClassifier
    {
        private readonly IClassificationType _commentoutType;
        private readonly IClassificationType _keywordType;
        private readonly IClassificationType _validType;
        private IClassificationType _normalType;

        internal YamahaClassifier(IClassificationTypeRegistryService registry)
        {
            _validType = registry.GetClassificationType("YamahaClassifierValidFormat");
            _normalType = registry.GetClassificationType("YamahaClassifierNormalFormat");

            _commentoutType = registry.GetClassificationType("YamahaClassifierCommentOutFormat");
            _keywordType = registry.GetClassificationType("YamahaClassifierKeywordFormat");
        }

        /// <summary>
        ///     This method scans the given SnapshotSpan for potential matches for this classification.
        ///     In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </summary>
        /// <param name="trackingSpan">The span currently being classified</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var classifications = new List<ClassificationSpan>();
            var text = span.GetText();
            int pt = span.Start;

            while (true)
            {
                if (text == "") break;
                if (text[0] == ' ' || text[0] == '\t')
                {
                    pt++;
                    text = text.Substring(1);
                }
                else break;
            }


            if (text == "")
            {
                return classifications;
            }

            if (text[0] != '#')
            {
                var greensw = false;
                var split = text.Split(" ".ToCharArray());
                for (var i = 0; i < split.Length; i++)
                {
                    var str = "";
                    for (var j = 0; j <= i; j++)
                    {
                        str += split[j] + " ";
                    }

                    if (YamahaData.Keywords.Contains(str.Substring(0, str.Length - 1)))
                    {
                        classifications.Add(
                            new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(pt, split[i].Length)),
                                _validType));

                        pt += split[i].Length + 1;
                    }
                    else
                    {
                        if (greensw ||
                            YamahaData.Keywords.Count(n => n.IndexOf(str.Substring(0, str.Length - 1)) >= 0) == 1 ||
                            str.Length > 6)
                        {
                            greensw = true;

                            classifications.Add(
                                new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(pt, split[i].Length)),
                                    _keywordType));

                            pt += split[i].Length + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                classifications.Add(
                    new ClassificationSpan(new SnapshotSpan(span.Snapshot, span),
                        _commentoutType));
            }

            //create a list to hold the results


            return classifications;
        }
#pragma warning disable 67
        // This event gets raised if a non-text change would affect the classification in some way,
        // for example typing /* would cause the classification to change in C# without directly
        // affecting the span.
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67
    }

    #endregion //Classifier
}