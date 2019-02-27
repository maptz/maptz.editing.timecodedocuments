using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maptz.DependencyInjection.TypeRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public class TimeCodeDocumentConverterRepository : TypeRegistrationRepository<ITimeCodeDocumentConverterInfo, ITimeCodeDocumentConverter>, ITimeCodeDocumentConverterRepository
    {
        public TimeCodeDocumentConverterRepository(IOptions<TimeCodeDocumentConverterRepositorySettings> settings, IServiceProvider serviceProvider) : base(settings, serviceProvider)
        {
        }

    }
}