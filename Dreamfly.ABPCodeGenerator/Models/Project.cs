using System.Collections.Generic;

namespace Dreamfly.ABPCodeGenerator.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string OutputPath { get; set; }
        public string Version { get; set; }
        public string Entity { get; set; }
        public string Module { get; set; }
        public Author Author { get; set; }
        public BuildTask BuildTask { get; set; }
    }

    public class BuildTask
    {
        public List<string> ApiProjectNames { get; set; }
        public Template[] Templates { get; set; }
    }

    public class Template
    {
        public string File { get; set; }
        public string Remark { get; set; }
        public bool IsExecute { get; set; }
        public Output Output { get; set; }
    }

    public class Output
    {
        public string Folder { get; set; }
        public string Name { get; set; }
    }
}