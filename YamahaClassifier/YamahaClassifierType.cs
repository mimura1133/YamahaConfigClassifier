using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{
    internal static class YamahaClassifierClassificationDefinition
    {
        /// <summary>
        ///     Defines the "YamahaClassifier" classification type.
        /// </summary>
        [Export(typeof (ClassificationTypeDefinition))] [Name("YamahaClassifierValidFormat")] internal static
            ClassificationTypeDefinition YamahaClassifierValidFormat = null;

        [Export(typeof (ClassificationTypeDefinition))] [Name("YamahaClassifierNormalFormat")] internal static
            ClassificationTypeDefinition YamahaClassifierNormalFormat = null;

        [Export(typeof (ClassificationTypeDefinition))] [Name("YamahaClassifierCommentOutFormat")] internal static
            ClassificationTypeDefinition YamahaClassifierCommentOutFormat = null;

        [Export(typeof (ClassificationTypeDefinition))] [Name("YamahaClassifierKeywordFormat")] internal static
            ClassificationTypeDefinition YamahaClassifierKeywordFormat = null;
    }
}