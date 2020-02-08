using Maptz.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Maptz.Editing.TimeCodeDocuments
{


    /// <summary>
    /// A cleaner used to clean enumerables of ITImeCodeContentSpan instances. 
    /// </summary>
    public interface ITimeCodeDocumentTimeValidator<T>
    {
        IEnumerable<string> IssueWarnings(ITimeCodeDocument<T> timeCodeDocument);
        ITimeCodeDocument<T> EnsureValidTimes(ITimeCodeDocument<T> document);
    }
}