using System.Diagnostics;
namespace Maptz.Editing.TimeCodeDocuments
{

    public class TextSpan : ITextSpan
    {
        public TextSpan(int start, int length, string document)
        {
            this.Start = start;
            this.Length = length;
            this.Document = document;
        }
        public int Start { get;  }
        public int Length { get;  }
        public string Document { get; }

        public override string ToString()
        {
            return this.Document?.Substring(this.Start, this.Length);
        }
    }
}