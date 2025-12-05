using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Prototype.SaveLoad
{
    public abstract class ASaveLoadable : MonoBehaviour
    {

        public static readonly string mainFolder = Path.Combine(Application.persistentDataPath, "SceneSorce");
        [SerializeField] private string targetName = null;

        public virtual void SaveDataToSceneSorce()
        {
            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
        }

        public virtual void LoadDataFromSceneSorce()
        {
            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
        }

        public virtual void SaveData()
        {

        }

        public virtual void LoadData()
        {

        }

        public virtual void DeleteSaveFromSorce()
        {

        }

        public static void DeleteSceneSaveScene()
        {

            try
            {
                if (Directory.Exists(mainFolder))
                {
                    Directory.Delete(mainFolder);
                }
            }
            catch
            {
                Debug.Log("No Scene Directory");
            }
        }
    }
}
