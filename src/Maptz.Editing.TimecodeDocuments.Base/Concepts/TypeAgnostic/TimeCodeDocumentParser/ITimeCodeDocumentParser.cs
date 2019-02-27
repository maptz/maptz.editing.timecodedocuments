using System.Collections.Generic;
namespace Maptz.Editing.TimeCodeDocuments
{

    /// <summary>
    /// Service used to parse timecode content spans from a string. 
    /// </summary>
    public interface ITimeCodeDocumentParser<T>
    {
        /// <summary>
        /// Parse a loose timecode content list into an array of timecode content items. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        ITimeCodeDocument<T> Parse(string input);
    }
}