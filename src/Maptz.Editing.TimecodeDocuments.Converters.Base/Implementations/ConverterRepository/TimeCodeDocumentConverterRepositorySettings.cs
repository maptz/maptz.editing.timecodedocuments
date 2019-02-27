using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Maptz.DependencyInjection.TypeRegistration;
namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public class TimeCodeDocumentConverterRepositorySettings : TypeRegistrationRepositorySettings<ITimeCodeDocumentConverterInfo, ITimeCodeDocumentConverter>
    {

    }
}