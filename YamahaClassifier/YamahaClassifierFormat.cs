using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{

    #region Format definition

    /// <summary>
    ///     Defines an editor format for the YamahaClassifier type that has a purple background
    ///     and is underlined.
    /// </summary>
    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierValidFormat")]
    [Name("YamahaClassifierValidFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierValidFormat : ClassificationFormatDefinition
    {
        /// <summary>
        ///     Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierValidFormat()
        {
            DisplayName = "YamahaClassifierValidFormat"; //human readable version of the name
            ForegroundColor = Colors.Blue;
        }
    }


    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierNormalFormat")]
    [Name("YamahaClassifierNormalFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierNormalFormat : ClassificationFormatDefinition
    {
        /// <summary>
        ///     Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierNormalFormat()
        {
            DisplayName = "YamahaClassifierNormalFormat"; //human readable version of the name
            ForegroundColor = Colors.Black;
        }
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierCommentOutFormat")]
    [Name("YamahaClassifierCommentOutFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierCommentOutFormat : ClassificationFormatDefinition
    {
        /// <summary>
        ///     Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierCommentOutFormat()
        {
            DisplayName = "YamahaClassifierCommentOutFormat"; //human readable version of the name
            ForegroundColor = Colors.Gray;
        }
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierKeywordFormat")]
    [Name("YamahaClassifierKeywordFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierKeywordFormat : ClassificationFormatDefinition
    {
        /// <summary>
        ///     Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierKeywordFormat()
        {
            DisplayName = "YamahaClassifierKeywordFormat"; //human readable version of the name
            ForegroundColor = Colors.DarkGreen;
        }
    }

    #endregion //Format definition
}