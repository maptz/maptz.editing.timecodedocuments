using System;
using Xunit;
using Maptz.Testing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Maptz.Editing.TimeCodeDocuments.Converters.Avid;
using Maptz.Editing.TimeCodeDocuments.Converters.PlainText;
using Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro;
using Maptz.Editing.TimeCodeDocuments.Converters.SRT;
using Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT;
using Maptz.Editing.TimeCodeDocuments.StringDocuments;

namespace Maptz.Editing.TimeCodeDocuments.Converters.All.Tests
{

    public class TimeCodeDocumentConvertersTests
    {
        /* #region Public Methods */
        [Fact]
        public async Task AvidDSParser_Works()
        {
            using (var fixture = new UnitTestFixture(serviceCollection=>
            {
                serviceCollection.AddOptions();
                serviceCollection.AddLogging();
                serviceCollection.AddTimeCodeDocumentConverters();
                serviceCollection.Configure<TimeCodeStringDocumentParserSettings>(settings =>
                {
                    settings.FrameRate = SmpteFrameRate.Smpte25;
                });
                serviceCollection.Configure<TimeCodeDocumentTimeValidatorSettings>(settings =>
                {
                    settings.DefaultDurationFrames = 10;
                });
            }))
            {
                /* #region Arrange */
                var avidDSParser = await TimeCodeDocumentConverters.GetAvidDSParserAsync(fixture.ServiceProvider);
                /* #endregion */

                /* #region Act */
                var avidDSResult = avidDSParser.Parse(fixture.SourceFileContent);
                /* #endregion */

                /* #region Assert */
                Assert.NotNull(avidDSResult);
                Assert.NotNull(((AvidDSResult)avidDSResult).Content);
                /* #endregion */
            }

        }


        [Fact]
        public async Task FinalCutXMLParser_Works()
        {
            using (var fixture = new UnitTestFixture(serviceCollection =>
            {
                serviceCollection.AddOptions();
                serviceCollection.AddLogging();
                serviceCollection.AddTimeCodeDocumentConverters();
                serviceCollection.Configure<TimeCodeStringDocumentParserSettings>(settings =>
                {
                    settings.FrameRate = SmpteFrameRate.Smpte25;
                });
                serviceCollection.Configure<TimeCodeDocumentTimeValidatorSettings>(settings =>
                {
                    settings.DefaultDurationFrames = 10;
                });
            }))
            {
                /* #region Arrange */
                var avidDSParser = await TimeCodeDocumentConverters.GetFinalCutParserAsync(fixture.ServiceProvider);
                /* #endregion */

                /* #region Act */
                var result = avidDSParser.Parse(fixture.SourceFileContent);
                /* #endregion */

                /* #region Assert */
                Assert.NotNull(result);
                Assert.NotNull(((FinalCutXMLResult)result).Content);
                /* #endregion */
            }

        }


        [Fact]
        public async Task MarkdownParser_Works()
        {
            using (var fixture = new UnitTestFixture(serviceCollection =>
            {
                serviceCollection.AddOptions();
                serviceCollection.AddLogging();
                serviceCollection.AddTimeCodeDocumentConverters();
                serviceCollection.Configure<TimeCodeStringDocumentParserSettings>(settings =>
                {
                    settings.FrameRate = SmpteFrameRate.Smpte25;
                });
                serviceCollection.Configure<TimeCodeDocumentTimeValidatorSettings>(settings =>
                {
                    settings.DefaultDurationFrames = 10;
                });
            }))
            {
                /* #region Arrange */
                var avidDSParser = await TimeCodeDocumentConverters.GetMarkdownParserAsync(fixture.ServiceProvider);
                /* #endregion */

                /* #region Act */
                var result = avidDSParser.Parse(fixture.SourceFileContent);
                /* #endregion */

                /* #region Assert */
                Assert.NotNull(result);
                Assert.NotNull(((MarkdownResult)result).Content);
                /* #endregion */
            }

        }

        [Fact]
        public async Task SRTParser_Works()
        {
            using (var fixture = new UnitTestFixture(serviceCollection =>
            {
                serviceCollection.AddOptions();
                serviceCollection.AddLogging();
                serviceCollection.AddTimeCodeDocumentConverters();
                serviceCollection.Configure<TimeCodeStringDocumentParserSettings>(settings =>
                {
                    settings.FrameRate = SmpteFrameRate.Smpte25;
                });
                serviceCollection.Configure<TimeCodeDocumentTimeValidatorSettings>(settings =>
                {
                    settings.DefaultDurationFrames = 10;
                });
            }))
            {
                /* #region Arrange */
                var parser = await TimeCodeDocumentConverters.GetSRTParserAsync(fixture.ServiceProvider);
                /* #endregion */

                /* #region Act */
                var result = parser.Parse(fixture.SourceFileContent);
                /* #endregion */

                /* #region Assert */
                Assert.NotNull(result);
                Assert.NotNull(((SRTResult)result).Content);
                /* #endregion */
            }

        }

        [Fact]
        public async Task SMPTETTParser_Works()
        {
            using (var fixture = new UnitTestFixture(serviceCollection =>
            {
                serviceCollection.AddOptions();
                serviceCollection.AddLogging();
                serviceCollection.AddTimeCodeDocumentConverters();
                serviceCollection.Configure<TimeCodeStringDocumentParserSettings>(settings =>
                {
                    settings.FrameRate = SmpteFrameRate.Smpte25;
                });
                serviceCollection.Configure<TimeCodeDocumentTimeValidatorSettings>(settings =>
                {
                    settings.DefaultDurationFrames = 10;
                });
            }))
            {
                /* #region Arrange */
                var parser = await TimeCodeDocumentConverters.GetSMPTETTParserAsync(fixture.ServiceProvider);
                /* #endregion */

                /* #region Act */
                var result = parser.Parse(fixture.SourceFileContent);
                /* #endregion */

                /* #region Assert */
                Assert.NotNull(result);
                Assert.NotNull(((SMPTETTResult)result).Content);
                /* #endregion */
            }

        }
        /* #endregion Public Methods */


    }
}