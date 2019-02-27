using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Maptz.Editing.TimeCodeDocuments.Converters;
using Microsoft.Extensions.Logging;
namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public interface ITimeCodeDocumentParseEngineFactory
    {
        Task<ITimeCodeDocumentParseEngine<TResult>> GetParserEngineAsync<TResult>(string converterId);
    }
}