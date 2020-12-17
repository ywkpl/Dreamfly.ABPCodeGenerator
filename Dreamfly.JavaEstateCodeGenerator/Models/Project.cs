namespace Dreamfly.JavaEstateCodeGenerator.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string OutputPath { get; set; }
        public string PackagePath { get; set; }
        public string Version { get; set; }
        public Author Author { get; set; }
        public Template[] Templates { get; set; }
    }

    public class Template
    {
        public string File { get; set; }
        public string Remark { get; set; }
        public bool IsExecute { get; set; }
        public string OutputFolder { get; set; }
        public string OutputName { get; set; }
        public string ProjectFile { get; set; }
    }
}