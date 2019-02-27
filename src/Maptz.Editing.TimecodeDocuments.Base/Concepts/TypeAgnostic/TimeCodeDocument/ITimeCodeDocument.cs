using System.Collections.Generic;
namespace Maptz.Editing.TimeCodeDocuments
{

    public interface ITimeCodeDocument
    {
        IEnumerable<ITimeCodeDocumentItem> Items { get; }
    }

    public interface ITimeCodeDocument<T> : ITimeCodeDocument
    {
        new IEnumerable<ITimeCodeDocumentItem<T>> Items { get; }
    }
}