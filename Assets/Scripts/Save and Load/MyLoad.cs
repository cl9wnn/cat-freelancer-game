using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

public class MyLoad
{
    static public T LoadFileJSON<T>(string name)
    {
        string path = Application.persistentDataPath + "/Saves/" + name;
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        return default;
    }

    static public T LoadFileBinary<T>(string name)
    {
        string path = Application.persistentDataPath + "/Saves/" + name;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (T)formatter.Deserialize(fs);
            }
        }
        else
            return default;
    }
}
