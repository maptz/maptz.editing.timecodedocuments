using System;
using System.Threading.Tasks;
using Maptz.Editing.TimeCodeDocuments.Converters.Avid;
using Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro;
using Maptz.Editing.TimeCodeDocuments.Converters.PlainText;
using Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT;
using Maptz.Editing.TimeCodeDocuments.Converters.SRT;
using Microsoft.Extensions.DependencyInjection;
namespace Maptz.Editing.TimeCodeDocuments.Converters.All
{
    public static class TimeCodeDocumentConverters
    {
        private static IServiceProvider GetDefaultServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.AddLogging();
            serviceCollection.AddTimeCodeDocumentConverters();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        public static async Task<ITimeCodeDocumentParseEngine<TResult>> GetParserEngineAsync<TResult>(string id, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                serviceProvider = GetDefaultServiceProvider();
            var parserFactory = serviceProvider.GetRequiredService<ITimeCodeDocumentParseEngineFactory>();
            var engine = await parserFactory.GetParserEngineAsync<TResult>(id);
            return engine;
        }

        public static async Task<ITimeCodeDocumentParseEngine<IAvidDSResult>> GetAvidDSParserAsync(IServiceProvider serviceProvider)
        {
            return await GetParserEngineAsync<IAvidDSResult>(AvidDSConstants.Id, serviceProvider);

        }

        public static async Task<ITimeCodeDocumentParseEngine<IFinalCutXMLResult>> GetFinalCutParserAsync(IServiceProvider serviceProvider)
        {
            return await GetParserEngineAsync<IFinalCutXMLResult>(FinalCutProConstants.Id, serviceProvider);

        }

        public static async Task<ITimeCodeDocumentParseEngine<IMarkdownResult>> GetMarkdownParserAsync(IServiceProvider serviceProvider)
        {
            return await GetParserEngineAsync<IMarkdownResult>(MarkdownConstants.Id, serviceProvider);

        }

        public static async Task<ITimeCodeDocumentParseEngine<ISMPTETTResult>> GetSMPTETTParserAsync(IServiceProvider serviceProvider)
        {
            return await GetParserEngineAsync<ISMPTETTResult>(SMPTETTConstants.Id, serviceProvider);

        }

        public static async Task<ITimeCodeDocumentParseEngine<ISRTResult>> GetSRTParserAsync(IServiceProvider serviceProvider)
        {
            return await GetParserEngineAsync<ISRTResult>(SRTConstants.Id, serviceProvider);

        }


    }
}