using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace YamahaClassifier
{
    internal static class YamahaConfigContentTypeDefinitions
    {
        [Export] [Name("yamahaconf")] [BaseDefinition("text")] internal static ContentTypeDefinition
            hidingContentTypeDefinition;

        [Export] [FileExtension(".yconf")] [ContentType("yamahaconf")] internal static
            FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition;
    }
}