using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
namespace Maptz.Editing.TimeCodeDocuments
{
    public interface ITimeCodeDocumentParseEngine<T>
    {
        T Parse(string str);
    }
}