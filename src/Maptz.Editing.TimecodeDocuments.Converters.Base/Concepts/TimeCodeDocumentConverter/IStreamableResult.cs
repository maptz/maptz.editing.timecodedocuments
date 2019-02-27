using System.IO;

namespace Maptz.Editing.TimeCodeDocuments
{
    //Context.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename=\"0}.docx\"", MyDocxTitle));
    //            mem.Position = 0;
    //            mem.CopyTo(Context.Response.OutputStream);
    //            Context.Response.Flush();
    //            Context.Response.End();
    public interface IStreamableResult
    {
        Stream GetStream();
        string ContentType { get; }
        string DefaultFileExtension { get; }
    }
}