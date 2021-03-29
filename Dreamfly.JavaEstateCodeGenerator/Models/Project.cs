using System;
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
        public bool IsShare { get; set; }
        public Template[] Templates { get; set; }
    }

    public class Template
    {
        public string File { get; set; }
        public string Remark { get; set; }
        public bool IsExecute { get; set; }
        public string OutputFolder { get; set; }
        public string OutputName { get; set; }


        private static List<string> GetApiProjectFiles(bool isShare)
        {

            var files = new List<string>
            {
                "Templates/App/Client.cshtml",
                "Templates/App/CreateRequest.cshtml",
                "Templates/App/Response.cshtml",
                "Templates/App/AllResponse.cshtml",
                "Templates/App/GetAllRequest.cshtml",
                "Templates/App/UpdateRequest.cshtml"
            };
            if (isShare)
            {
                files.Add("Templates/App/Mapper.cshtml");
                files.Add("Templates/App/Entity.cshtml");
            }

            return files;
        }

        public bool InApiProject(bool isShare)
        {
            return GetApiProjectFiles(isShare).Contains(File);
        }
    }
}