using System.IO;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl.Inserts
{
    public abstract class InsertToFile : InsertCodeToFileBase
    {
        protected string FilePath;
        protected const string NewLine = "\n";
        protected const string Tab = "\t";
        protected string Code;

        protected InsertToFile(EntityDto entity) : base(entity)
        {
        }

        protected string ReadCode()
        {
            return File.ReadAllText(FilePath);
        }

        protected void WriteToFile(string changedCode)
        {
            File.WriteAllText(FilePath, changedCode);
        }

        public abstract string GetInsertCode();
    }
}