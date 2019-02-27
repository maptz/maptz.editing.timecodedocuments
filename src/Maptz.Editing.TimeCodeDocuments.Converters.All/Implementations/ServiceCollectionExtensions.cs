using Maptz.Editing.TimeCodeDocuments.Converters.Avid;
using Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro;
using Maptz.Editing.TimeCodeDocuments.Converters.PlainText;
using Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT;
using Maptz.Editing.TimeCodeDocuments.Converters.SRT;
using Maptz.Editing.TimeCodeDocuments.StringDocuments;
using Maptz.Text;
using Microsoft.Extensions.DependencyInjection;
namespace Maptz.Editing.TimeCodeDocuments.Converters.All
{


    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTimeCodeDocumentConverters(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddTransient<ITextCleanerService, DefaultTextCleanerService>();
            //Instantiate Maptz.Editing.TimeCodeDocuments.Base services.
            serviceCollection.AddTransient<ITimeCodeDocumentTimeValidator<string>, TimeCodeDocumentTimeValidator<string>>();
            serviceCollection.AddTransient<ITimeCodeDocumentParser<string>, TimeCodeStringDocumentParser>();
            serviceCollection.AddTransient<ITimeCodeDocumentContentValidator<string>, TimeCodeStringDocumentContentValidator>();
            //Instantiate Maptz.Editing.TimeCodeDocuments.Converters.Base services
            serviceCollection.AddTransient<ITimeCodeDocumentConverterRepository, TimeCodeDocumentConverterRepository>();
            serviceCollection.AddTransient<ITimeCodeDocumentParseEngineFactory, TimeCodeDocumentParseEngineFactory<string>>();
            serviceCollection.Configure<TimeCodeDocumentConverterRepositorySettings>(settings =>
            {
                settings.OnInitialized =async (repository) =>
                {
                    await repository.RegisterTypeAsync<ITimeCodeDocumentToAvidDSConverter>(AvidDSConstants.Id, new AvidDSConverterInfo());
                    await repository.RegisterTypeAsync<ITimeCodeDocumentToFinalCutXMLConverter>(FinalCutProConstants.Id, new FinalCutXMLConverterInfo());
                    await repository.RegisterTypeAsync<ITimeCodeDocumentToMarkdownConverter>(MarkdownConstants.Id, new MarkdownConverterInfo());
                    await repository.RegisterTypeAsync<ITimeCodeDocumentToSMPTETTConverter>(SMPTETTConstants.Id, new SMPTETTConverterInfo());
                    await repository.RegisterTypeAsync<ITimeCodeDocumentToSRTConverter>(SRTConstants.Id, new SRTConverterInfo());
                };
            });

            serviceCollection.AddTransient<ITimeCodeDocumentToAvidDSConverter, TimeCodeToAvidDSConverter>();
            serviceCollection.AddTransient<ITimeCodeDocumentToFinalCutXMLConverter, TimeCodeToFinalCutXmlConverter>();
            serviceCollection.AddTransient<ITimeCodeDocumentToMarkdownConverter, TimeCodeToMarkdownConverter>();
            serviceCollection.AddTransient<ITimeCodeDocumentToSMPTETTConverter, TimeCodeToSMPTETTConverter>();
            serviceCollection.AddTransient<ITimeCodeDocumentToSRTConverter, TimeCodeDocumentToSRTConverter>();


            return serviceCollection;
        }

    }
}