using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Models;
using Newtonsoft.Json;

namespace Dreamfly.JavaEstateCodeGenerator.Helper
{
    public class JsonDataHelper
    {
        public const String JsonEntityDataFilePath = "data\\entitys.json";

        public static List<Entity> ReadEntities()
        {
            String json = ReadJson();
            return JsonConvert.DeserializeObject<List<Entity>>(json);
        }

        public static void SaveEntities(List<Entity> entities)
        {
            string json = JsonConvert.SerializeObject(entities);
            WriteJson(json);
        }

        private static void WriteJson(string json)
        {
            using FileStream fs = new FileStream(JsonEntityDataFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(json);
        }

        private static String ReadJson()
        {
            using FileStream fs = new FileStream(JsonEntityDataFilePath, FileMode.OpenOrCreate);
            using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}