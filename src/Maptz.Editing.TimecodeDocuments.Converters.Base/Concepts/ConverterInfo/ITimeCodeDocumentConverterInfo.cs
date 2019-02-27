using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Linq;
using Maptz.DependencyInjection.TypeRegistration;
namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public interface ITimeCodeDocumentConverterInfo :ITypeRegistrationInfo
    {
    }
}