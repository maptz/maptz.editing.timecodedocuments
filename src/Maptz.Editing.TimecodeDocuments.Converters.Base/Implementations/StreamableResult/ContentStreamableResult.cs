using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Maptz.Editing.TimeCodeDocuments
{

    public class ContentStreamableResult : StreamableResult
    {
        public ContentStreamableResult(string content, string contentType, string defaultFileExtension) : base(contentType, defaultFileExtension)
        {
            this.Content = content;
        }

        public string Content { get; }

        public override Stream GetStream()
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(this.Content);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}