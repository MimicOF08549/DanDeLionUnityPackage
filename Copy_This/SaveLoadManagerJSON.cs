using System;
using UnityEngine;
using System.IO;

namespace Prototype.SaveLoad
{
    public static class SaveLoadManagerJSON<T>
    {
        // Change this path to your desired file location
        private static string savePath = Application.persistentDataPath;

        public static void SaveGameOnPath(T data, string savepathInput)
        {
            try
            {
                string jsonData = JsonUtility.ToJson(data);
                File.WriteAllText(Path.Combine(savePath, savepathInput), jsonData);
            }
            catch
            {
            }
        }

        public static T LoadGameOnPath(string savepathInput)
        {

            string filePath = Path.Combine(savePath, savepathInput);

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonData = File.ReadAllText(filePath);
                    T data = JsonUtility.FromJson<T>(jsonData);
                    return data;
                }
                catch
                {
                }
            }

            return default(T);
        }
    }
}