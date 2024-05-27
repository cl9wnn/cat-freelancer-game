using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

public class MySave {
    static public void SaveFileJSON (object obj, string name) {
        string path = Application.persistentDataPath + "/Saves";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path += "/" + name;
        
        File.WriteAllText (path, JsonConvert.SerializeObject (obj));
    }

    static public void SaveFileBinary (object obj, string name) {
        string path = Application.persistentDataPath + "/Saves";

        if (!Directory.Exists (path))
            Directory.CreateDirectory(path);

        path += "/" + name;

        BinaryFormatter formatter = new BinaryFormatter ();
        using (FileStream fs = new FileStream (path, FileMode.OpenOrCreate)) {
            formatter.Serialize (fs, obj);
        }
    }
}