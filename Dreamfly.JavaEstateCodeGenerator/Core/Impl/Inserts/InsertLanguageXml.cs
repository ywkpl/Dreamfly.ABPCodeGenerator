using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dreamfly.JavaEstateCodeGenerator.Core.Interface;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Impl.Inserts
{
    public class InsertLanguageXml : InsertCodeToFileBase
    {
        private const string ZH_TW = "zh-Hant";
        private const string ZH_CN = "zh-Hans";

        private XDocument _xDocument;
        private readonly Dictionary<string, string> _languageFilePaths = new Dictionary<string, string>();

        public override void Insert()
        {
            EachXml(filePath =>
            {
                string description = GetDescription(filePath);

                var exElement = new XElement("text",
                    new XAttribute("name", $"Pages_{entity.Module}_{entity.Name}"),
                    description);

                if (_xDocument.Root?.Element("texts") != null)
                {
                    _xDocument.Root.Element("texts")?.Add(exElement);
                }
            });
        }

        private string GetDescription(KeyValuePair<string, string> filePath)
        {
            return filePath.Key == ZH_CN
                ? TransferLanguage.ToSimplified(entity.Description)
                : entity.Description;
        }

        private void EachXml(Action<KeyValuePair<string, string>> doEach)
        {
            foreach (var filePath in _languageFilePaths)
            {
                _xDocument = XDocument.Load(filePath.Value);
                doEach(filePath);
                _xDocument.Save(filePath.Value);
            }
        }

        public override void Remove()
        {
            EachXml(filePath =>
            {
                _xDocument.Root?.Element("texts")?.Elements("text")
                    .Where(p => p.Attribute("name")?.Value == $"Pages_{entity.Module}_{entity.Name}")
                    .Remove();
            });
        }

        public InsertLanguageXml(Entity entity) : base(entity)
        {
            _languageFilePaths.Add(ZH_CN, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_CN}.xml"));
            _languageFilePaths.Add(ZH_TW, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_TW}.xml"));
        }
    }
}