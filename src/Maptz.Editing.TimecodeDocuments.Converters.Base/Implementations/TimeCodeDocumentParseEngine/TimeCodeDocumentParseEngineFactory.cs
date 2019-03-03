using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public class TimeCodeDocumentParseEngineFactory<TContent> : ITimeCodeDocumentParseEngineFactory
    {
        public TimeCodeDocumentParseEngineFactory(IServiceProvider serviceProvider, ITimeCodeDocumentConverterRepository timeCodeDocumentConverterRepository)
        {
            this.ServiceProvider = serviceProvider;
            this.TimeCodeDocumentConverterRepository = timeCodeDocumentConverterRepository;
        }

        public IServiceProvider ServiceProvider { get; }
        public ITimeCodeDocumentConverterRepository TimeCodeDocumentConverterRepository { get; }

        public async Task<ITimeCodeDocumentParseEngine<TResult>> GetParserEngineAsync<TResult>(string converterId)
        {
            var timeCodeDocumentConverter = await this.TimeCodeDocumentConverterRepository.CreateInstanceAsync<ITimeCodeDocumentConverter<TResult, TContent>>(converterId);
            var loggerFactory = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var timeCodeDocumentParser = this.ServiceProvider.GetRequiredService<ITimeCodeDocumentParser<TContent>>();
            var timeCodeDocumentTimeValidator = this.ServiceProvider.GetRequiredService<ITimeCodeDocumentTimeValidator<TContent>>();
            var timeCodeDocumentContentValidator = this.ServiceProvider.GetRequiredService<ITimeCodeDocumentContentValidator<TContent>>();
            var options = this.ServiceProvider.GetRequiredService<IOptions<TimeCodeDocumentParseEngineOptions<TResult, TContent>>>();
            var timeCodeDocumentParseEngine = new TimeCodeDocumentParseEngine<TResult, TContent>(options, loggerFactory, timeCodeDocumentParser, timeCodeDocumentTimeValidator, timeCodeDocumentContentValidator, timeCodeDocumentConverter);
            return timeCodeDocumentParseEngine;
        }
    }
}