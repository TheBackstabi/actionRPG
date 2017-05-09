using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

public class SpellDatabase : MonoBehaviour {
    static Dictionary<int, Spell> loadedSpells;
    private static SpellDatabase instance = null;
    public static SpellDatabase Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        LoadSpellData();
    }

    void SaveSpellData()
    {
        FileStream save = File.Create(Application.dataPath + "/Data/Spells.dat");
        BinaryFormatter format = new BinaryFormatter();
        format.Serialize(save, loadedSpells);
        save.Close();
    }

    public void LoadLocalizedText(string locale)
    {
        if (loadedSpells != null)
        {
            string filePath = Application.dataPath + "\\XML\\" + locale + "\\Spells.xml";
            if (File.Exists(filePath))
            {
                XmlReader fileReader = XmlReader.Create(filePath);
                fileReader.MoveToContent();
                while (fileReader.ReadToFollowing("spell"))
                {
                    int id = int.Parse(fileReader.GetAttribute("id"));
                    if (loadedSpells[id] != null)
                    {
                        fileReader.ReadToDescendant("name");
                        loadedSpells[id].name = fileReader.ReadString();
                        fileReader.ReadToNextSibling("desc");
                        loadedSpells[id].desc = fileReader.ReadString();
                    }
                }
                fileReader.Close();
            }
        }
    }

    // Use this for initialization
    void LoadSpellData()
    {
        loadedSpells = new Dictionary<int,Spell>();
        string fileName = Application.dataPath + "/Data/Spells.dat";
        if (File.Exists(fileName))
        {
            Debug.Log("Loading spell data from .dat file");
            BinaryFormatter format = new BinaryFormatter();
            FileStream load = File.Open(fileName, FileMode.Open);
            loadedSpells = (Dictionary<int, Spell>)format.Deserialize(load);
            load.Close();
            LoadLocalizedText("enUS");
        }
        else
        {
            fileName = Application.dataPath + "/Development/Spells.xml";
            XmlReader fileReader = XmlReader.Create(fileName);
            while (fileReader.Read())
            {
                if (fileReader.IsStartElement() && fileReader.Name == "spell")
                {
                    Spell newSpell = new Spell();
                    newSpell.id = int.Parse(fileReader.GetAttribute("id"));
                    fileReader.ReadToDescendant("manaCost");
                    newSpell.manaCost = int.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("potency");
                    newSpell.damageValue = int.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("range");
                    newSpell.range = int.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("texture");
                    newSpell.texture = fileReader.ReadString();
                    fileReader.ReadToNextSibling("cooldown");
                    newSpell.cooldown = int.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("ignoreGCD");
                    newSpell.ignoreGCD = bool.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("school");
                    newSpell.school = int.Parse(fileReader.ReadString());
                    fileReader.ReadToNextSibling("gameObject");
                    newSpell.objectPath = fileReader.ReadString();
                    if (fileReader.ReadToNextSibling("extraFlags"))
                    {
                        fileReader.ReadToDescendant("type");
                        newSpell.type = fileReader.ReadString();
                        switch (newSpell.type)
                        {
                            case "DoT": // duration, ticks
                                fileReader.ReadToNextSibling("duration");
                                newSpell.duration = int.Parse(fileReader.ReadString());
                                fileReader.ReadToNextSibling("ticks");
                                newSpell.ticks = int.Parse(fileReader.ReadString());
                                break;
                            case "AoE": // radius
                                fileReader.ReadToNextSibling("radius");
                                newSpell.radius = int.Parse(fileReader.ReadString());
                                break;
                            case "AoEDoT": // duration, ticks, radius
                                fileReader.ReadToNextSibling("duration");
                                newSpell.duration = int.Parse(fileReader.ReadString());
                                fileReader.ReadToNextSibling("ticks");
                                newSpell.ticks = int.Parse(fileReader.ReadString());
                                fileReader.ReadToNextSibling("radius");
                                newSpell.radius = int.Parse(fileReader.ReadString());
                                break;
                        }
                    }
                    loadedSpells.Add(newSpell.id, newSpell);
                }
            }
            fileReader.Close();
            LoadLocalizedText("enUS"); // TODO: Store locale to be loaded via UserPref settings
            SaveSpellData();
        }
        //foreach (KeyValuePair<int, Spell> entry in loadedSpells)
        //    entry.Value.DebugPrint();
	}

    public static Spell GetSpellData(int id)
    {
        if (loadedSpells.ContainsKey(id))
            return loadedSpells[id];
        return null;
    }

    public static int GetNumSpells()
    {
        return loadedSpells.Count;
    }
}
