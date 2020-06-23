using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dreamfly.ABPCodeGenerator.Models;

namespace Dreamfly.ABPCodeGenerator.Core.Impl
{
    public class InsertLanguageXml
    {
        private const string ZH_TW = "zh-Hant";
        private const string ZH_CN = "zh-Hans";

        private XDocument _xDocument;
        private Dictionary<string, string> _languageFilePaths=new Dictionary<string, string>();


        public void Insert(Entity entity)
        {
            _languageFilePaths.Add(ZH_CN, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_CN}.xml"));
            _languageFilePaths.Add(ZH_TW, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_TW}.xml"));
            
            foreach (var filePath in _languageFilePaths)
            {
                _xDocument = XDocument.Load(filePath.Value);
                string description = filePath.Key == ZH_CN ? TransferLanguage.ToSimplified(entity.Description) : entity.Description;

                var exElement = new XElement("text",
                    new XAttribute("name", entity.Name),
                    description);
                
                if (_xDocument.Root?.Element("texts") != null)
                {
                    _xDocument.Root.Element("texts")?.Add(exElement);
                    _xDocument.Save(filePath.Value);
                }
            }
        }

        public void Remove(Entity entity)
        {
            _languageFilePaths.Add(ZH_CN, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_CN}.xml"));
            _languageFilePaths.Add(ZH_TW, Path.Combine(entity.Project.OutputPath, "aspnet-core", "src",
                "Newcity.ReEstate.Core", "Localization", "SourceFiles", $"ReEstate-{ZH_TW}.xml"));
            
            foreach (var filePath in _languageFilePaths)
            {
                _xDocument = XDocument.Load(filePath.Value);
                _xDocument.Root?.Element("texts")?.Elements("text")
                    .Where(p => p.Attribute("name")?.Value == entity.Name)
                    .Remove();
                _xDocument.Save(filePath.Value);
            }
        }
    }
}