using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace Inlämning4.Classes
{
    public class JsonFileHandler
    {
        public JsonFileHandler(string filePath)
        {
            JsonFilePath = filePath;
        }
        public string JsonFilePath { get; }
        public void SaveChanges(IEnumerable<Produkt> products)
        {
            var jsonString = JsonSerializer.Serialize(products);
            var fs = new FileStream(JsonFilePath, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine(jsonString);
        }
        public IEnumerable<Produkt> Retrieve()
        {
            using StreamReader reader = new StreamReader(JsonFilePath);
            string json = reader.ReadToEnd();
            var records = new List<Produkt>();
            try
            {
                 records = JsonSerializer.Deserialize<List<Produkt>>(json);
                return records;
            }
            catch (Exception ex) when (ex is JsonException || ex is ArgumentNullException)
            {
                throw new FormatException();
            }
            
        }
    }
}
