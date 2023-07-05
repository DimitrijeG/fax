using System.Collections.Generic;
using System.IO;

namespace HealthCare.DataManagment.Serialize
{
    public abstract class Serializer<T> where T : ISerializable, new()
    {
        private static readonly char _sep = ',';

        public static void SerializeAll(string filepath, List<T> objects)
        {
            ValidateFile(filepath);

            using (StreamWriter streamWriter = File.CreateText(filepath))
            {
                foreach (T obj in objects)
                {
                    string line = string.Join(_sep, obj.Serialize());
                    streamWriter.WriteLine(line);
                }
            }
        }

        public static List<T> DeserializeAll(string filepath)
        {
            ValidateFile(filepath);

            List<T> objects = new List<T>();
            foreach (string line in File.ReadLines(filepath))
            {
                if (line.Trim() == "") continue;

                string[] csvValues = line.Split(_sep);
                T obj = new T();
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