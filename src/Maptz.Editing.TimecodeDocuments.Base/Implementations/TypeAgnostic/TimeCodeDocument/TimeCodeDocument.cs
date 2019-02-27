using System.Collections.Generic;
namespace Maptz.Editing.TimeCodeDocuments
{

    public class TimeCodeDocument<T> : ITimeCodeDocument<T>
    {
        public IEnumerable<ITimeCodeDocumentItem<T>> Items { get; set; }

        IEnumerable<ITimeCodeDocumentItem> ITimeCodeDocument.Items => this.Items;
    }
}