using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibraryCirculation.DataManagement.Serialize
{
    public abstract class Serializer<T> where T : ISerializable, new()
    {
        private const string Separator = "|";

        public static void SerializeAll(string filepath, List<T> objects)
        {
            ValidateFile(filepath);

            using var streamWriter = File.CreateText(filepath);
            foreach (var line in objects.Select(obj => string.Join(Separator, obj.Serialize())))
                streamWriter.WriteLine(line);
        }

        public static List<T> DeserializeAll(string filepath)
        {
            ValidateFile(filepath);

            var objects = new List<T>();
            foreach (var line in File.ReadLines(filepath))
            {
                if (line.Trim() == "") continue;

                var csvValues = line.Split(Separator);
                var obj = new T();
                obj.Deserialize(csvValues);
                objects.Add(obj);
            }

            return objects;
        }

        public static void ValidateFile(string filepath)
        {
            if (!File.Exists(filepath))
                File.Create(filepath).Dispose();
        }
    }
}