using System.IO;
using Dreamfly.ABPCodeGenerator.Core.Interface;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public abstract class InsertToFile:InsertCodeToFileBase
    {
        protected string FilePath;
        protected const string NewLine = "\n";
        protected const string Tab = "\t";
        protected string Code;

        protected InsertToFile(Entity entity) : base(entity)
        {
        }

        protected string ReadCode()
        {
            return File.ReadAllText(FilePath);
        }

        protected void WriteToFile(string changedCode)
        {
            File.WriteAllText(FilePath,changedCode);
        }

        public abstract string GetInsertCode();
    }
}