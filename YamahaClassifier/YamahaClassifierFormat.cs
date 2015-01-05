using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{
    #region Format definition
    /// <summary>
    /// Defines an editor format for the YamahaClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierValidFormat")]
    [Name("YamahaClassifierValidFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierValidFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierValidFormat()
        {
            this.DisplayName = "YamahaClassifierValidFormat"; //human readable version of the name
            this.ForegroundColor = Colors.LightSkyBlue;
        }
    }


    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierNormalFormat")]
    [Name("YamahaClassifierNormalFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierNormalFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierNormalFormat()
        {
            this.DisplayName = "YamahaClassifierNormalFormat"; //human readable version of the name
            this.ForegroundColor = Colors.Black;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierCommentOutFormat")]
    [Name("YamahaClassifierCommentOutFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierCommentOutFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierCommentOutFormat()
        {
            this.DisplayName = "YamahaClassifierCommentOutFormat"; //human readable version of the name
            this.ForegroundColor = Colors.LightCyan;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "YamahaClassifierKeywordFormat")]
    [Name("YamahaClassifierKeywordFormat")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class YamahaClassifierKeywordFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "YamahaClassifier" classification type
        /// </summary>
        public YamahaClassifierKeywordFormat()
        {
            this.DisplayName = "YamahaClassifierKeywordFormat"; //human readable version of the name
            this.ForegroundColor = Colors.LightGreen;
        }
    }
    #endregion //Format definition
}
