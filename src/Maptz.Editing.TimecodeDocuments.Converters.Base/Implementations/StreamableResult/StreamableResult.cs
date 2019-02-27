using System.IO;

namespace Maptz.Editing.TimeCodeDocuments
{

    public abstract class StreamableResult : IStreamableResult
    {
        public StreamableResult(string contentType, string defaultFileExtension)
        {
            this.ContentType = contentType;
            this.DefaultFileExtension = defaultFileExtension;
        }

        public string ContentType { get; }
        public string DefaultFileExtension { get; }

        public abstract Stream GetStream();
    }
}