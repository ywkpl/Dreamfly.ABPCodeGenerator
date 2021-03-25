using System.Collections.Generic;

namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string OutputPath { get; set; }
        public string PackagePath { get; set; }
        public string Version { get; set; }
        public Author Author { get; set; }
        public bool HasApi { get; set; }
        public Template[] Templates { get; set; }
    }

    public class Template
    {
        public string File { get; set; }
        public string Remark { get; set; }
        public bool IsExecute { get; set; }
        public string OutputFolder { get; set; }
        public string OutputName { get; set; }

        private static List<string> ApiProjectFiles=new List<string>
        {
            "Templates/App/Client.cshtml",
            "Templates/App/CreateRequest.cshtml",
            "Templates/App/Response.cshtml",
            "Templates/App/AllResponse.cshtml",
            "Templates/App/GetAllRequest.cshtml",
            "Templates/App/UpdateRequest.cshtml"
        };

        public bool InApiProject => ApiProjectFiles.Contains(File);
    }
}