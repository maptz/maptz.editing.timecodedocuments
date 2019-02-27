using System;
using System.IO;
using Maptz.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Maptz.Editing.TimeCodeDocuments.Converters.All.Tests
{


    public class UnitTestFixture : InjectedServicesFixture
    {

        public UnitTestFixture(Action<IServiceCollection> action = null) : base(action)
        {
            var workspace = this.ServiceProvider.GetService<ITempDirectoryWorkspace>();

            workspace.ExtractNamedResource(this.GetType().Assembly, "resx.SourceFile.txt", "SourceFile.txt");
            this.SourceFilePath = Path.Combine(workspace.TempDirectoryPath, "SourceFile.txt");

            string sourceFileContent;
            using (var fi = new FileInfo(this.SourceFilePath).OpenText())
            {
                sourceFileContent = fi.ReadToEnd();
            }
            this.SourceFileContent = sourceFileContent;
        }

        public string SourceFilePath { get; }
        public string SourceFileContent { get; }

        /* #region Public Constructors */

        protected override void AddServices(ServiceCollection servicesCollection)
        {
            base.AddServices(servicesCollection);
            servicesCollection.AddTransient<ITempDirectoryWorkspace, TempDirectoryWorkspace>();
        }
      
        /* #endregion Public Constructors */
    }
}