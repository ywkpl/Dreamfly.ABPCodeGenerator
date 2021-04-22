using System;
using System.IO;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class InitilizeSql
    {
        private readonly Project _project;
        private readonly String filePath;
        public InitilizeSql(Project project)
        {
            this._project = project;
            filePath = Path.Combine(_project.OutputPath,
                "..\\..\\baseinfo-service\\src\\main\\resources\\Initilize.sql");

        }

        public static void GeneratorCheckSetting()
        {
            int startId = 10, detailStartId=10;

        }
    }
}