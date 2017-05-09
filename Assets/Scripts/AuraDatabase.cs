using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

public class AuraDatabase : MonoBehaviour {
    static Dictionary<int, Aura> loadedAuras;
    private static AuraDatabase instance = null;
    public static AuraDatabase AuraDatabaseInstance
    {
        get { return instance; }
    }

	void Awake()
    {
        instance = this;
        LoadAuraData();
    }

    void SaveAuraData()
    {
        FileStream save = File.Create(Application.dataPath + "/Data/Auras.dat");
        BinaryFormatter format = new BinaryFormatter();
        format.Serialize(save, loadedAuras);
        save.Close();
    }

    public void LoadLocalizedText(string locale)
    {
        if (loadedAuras != null)
        {
            string fileName = Application.dataPath + "\\XML\\" + locale + "\\Auras.xml";
            if (File.Exists(fileName))
            {
                XmlReader fileReader = XmlReader.Create(fileName);
                fileReader.MoveToContent();
                while (fileReader.ReadToFollowing("aura"))
                {
                    int id = int.Parse(fileReader.GetAttribute("id"));
                    if (loadedAuras[id] != null)
                    {
                        fileReader.ReadToDescendant("name");
                        loadedAuras[id].auraName = fileReader.ReadString();
                        fileReader.ReadToNextSibling("desc");
                        loadedAuras[id].desc = fileReader.ReadString();
                    }
                }
                fileReader.Close();
            }
        }
    }

    void LoadAuraData()
    {
        loadedAuras = new Dictionary<int, Aura>();
        string fileName = Application.dataPath + "/Data/Auras.dat";
        if (File.Exists(fileName))
        {
            Debug.Log("Loading auras from .dat file");
            BinaryFormatter format = new BinaryFormatter();
            FileStream load = File.Open(fileName, FileMode.Open);
            loadedAuras = (Dictionary<int, Aura>)format.Deserialize(load);
            load.Close();
            LoadLocalizedText("enUS");
        }
        else
        {
            fileName = Application.dataPath + "/Development/Auras.xml";
            XmlReader fileReader = XmlReader.Create(fileName);
            fileReader.MoveToContent();
            while (fileReader.ReadToFollowing("aura"))
            {
                Aura newAura = new Aura();
                newAura.id = int.Parse(fileReader.GetAttribute("id"));
                fileReader.ReadToDescendant("duration");
                newAura.duration = float.Parse(fileReader.ReadString());
                fileReader.ReadToNextSibling("oneUse");
                newAura.consumedOnUse = bool.Parse(fileReader.ReadString());
                fileReader.ReadToNextSibling("isDebuff");
                newAura.isDebuff = bool.Parse(fileReader.ReadString());
                fileReader.ReadToNextSibling("mods");
                string[] mods = fileReader.ReadString().Split('/');
                foreach (string mod in mods)
                {
                    Aura.Modifier newMod = new Aura.Modifier();
                    string[] innerMod = mod.Split(':');
                    newMod.spellID = int.Parse(innerMod[0]);
                    newMod.mod = (AuraMods)System.Enum.Parse(typeof(AuraMods), innerMod[1]);
                    newMod.value = float.Parse(innerMod[2]);
                    newAura.mods.Add(newMod);
                }
                fileReader.ReadToNextSibling("texture");
                newAura.texture = fileReader.ReadString();
                loadedAuras.Add(newAura.id, newAura);
            }
            fileReader.Close();
            LoadLocalizedText("enUS"); // TODO: Store locale to be loaded via UserPref settings
            SaveAuraData();
        }
    }
}
