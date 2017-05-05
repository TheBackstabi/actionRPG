using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TextureLoader : MonoBehaviour {
    [System.Serializable]
    public struct Texture
    {
        public string name;
        public byte[] data;
    }

    private Dictionary<string, Texture> Textures;
    public byte[] this[string key]
    {
        get
        {
            if (Textures.ContainsKey(key))
                return Textures[key].data;
            return null;
        }
    }

    private static TextureLoader instance = null;
    public static TextureLoader Instance
    {
        get
        {  
            return instance;
        }
    }

    private void SaveTextureData()
    {
        FileStream save = File.Create(Application.dataPath + "/Data/Textures.dat");
        BinaryFormatter format = new BinaryFormatter();
        format.Serialize(save, Textures);
        save.Close();
    }

    private void Awake()
    {
        instance = this;
        Textures = new Dictionary<string, Texture>();
        string path = Application.dataPath + "/Data/Textures.dat";
        if (!File.Exists(path))
        {
            path = Application.dataPath + "/Development/Textures";
            foreach (string file in Directory.GetFileSystemEntries(path, "*.png"))
            {
                Texture newTex = new Texture();
                string texName = file.Split('\\')[1];
                texName = texName.Split('.')[0];
                newTex.name = texName;
                newTex.data = File.ReadAllBytes(file);
                Textures.Add(newTex.name, newTex);
            }
            SaveTextureData();
        }
        else
        {
            Debug.Log("Loading texture data from .dat file");
            BinaryFormatter format = new BinaryFormatter();
            FileStream load = File.Open(path, FileMode.Open);
            Textures = (Dictionary<string, Texture>)format.Deserialize(load);
            load.Close();
        }
    }
}
