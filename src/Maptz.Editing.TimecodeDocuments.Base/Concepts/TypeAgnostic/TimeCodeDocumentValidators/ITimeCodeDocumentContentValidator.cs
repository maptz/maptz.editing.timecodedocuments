using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
namespace Maptz.Editing.TimeCodeDocuments
{

    public interface ITimeCodeDocumentContentValidator<T>
    {
        ITimeCodeDocument<T> EnsureValidContent(ITimeCodeDocument<T> document);
    }
}