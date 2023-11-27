using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

// Прототип для шаблонів даних
[DataContract]
public class DataTemplate : ICloneable
{
    [DataMember]
    public string Field1 { get; set; }

    [DataMember]
    public string Field2 { get; set; }

    public object Clone()
    {
        // Використовуємо глибоке клонування для копіювання об'єкта
        using (var stream = new MemoryStream())
        {
            var serializer = new DataContractJsonSerializer(typeof(DataTemplate));
            serializer.WriteObject(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            return serializer.ReadObject(stream);
        }
    }
}

// Адаптер для забезпечення сумісності між форматами
public interface IDataAdapter
{
    string ConvertData(object data);
}

// Адаптер для конвертації в JSON
public class JsonDataAdapter : IDataAdapter
{
    public string ConvertData(object data)
    {
        using (var stream = new MemoryStream())
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            serializer.WriteObject(stream, data);
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

// Адаптер для конвертації в XML
public class XmlDataAdapter : IDataAdapter
{
    public string ConvertData(object data)
    {
        using (var stream = new MemoryStream())
        {
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(stream, data);
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

// Система імпорту та експорту
public class DataConverter
{
    private readonly Dictionary<string, IDataAdapter> adapters;

    public DataConverter()
    {
        adapters = new Dictionary<string, IDataAdapter>
        {
            {"json", new JsonDataAdapter()},
            {"xml", new XmlDataAdapter()}
            // Додайте адаптери для інших форматів за необхідності
        };
    }

    public string ConvertData(object data, string sourceFormat, string targetFormat)
    {
        if (adapters.TryGetValue(sourceFormat.ToLower(), out var sourceAdapter) &&
            adapters.TryGetValue(targetFormat.ToLower(), out var targetAdapter))
        {
            // Конвертація даних з одного формату в інший через проміжний об'єкт
            var intermediateData = sourceAdapter.ConvertData(data);
            return targetAdapter.ConvertData(intermediateData);
        }

        return "Невідомий формат адаптера";
    }
}

class Program
{
    static void Main()
    {
        DataConverter dataConverter = new DataConverter();

        Console.WriteLine("Введіть дані для конвертації:");
        Console.Write("Field1: ");
        string field1 = Console.ReadLine();
        Console.Write("Field2: ");
        string field2 = Console.ReadLine();

        DataTemplate dataTemplate = new DataTemplate { Field1 = field1, Field2 = field2 };

        Console.WriteLine("Введіть формат вихідних даних (json/xml):");
        string sourceFormat = Console.ReadLine();

        Console.WriteLine("Введіть формат цільових даних (json/xml):");
        string targetFormat = Console.ReadLine();

        string result = dataConverter.ConvertData(dataTemplate, sourceFormat, targetFormat);
        Console.WriteLine("Результат конвертації:");
        Console.WriteLine(result);
    }
}
