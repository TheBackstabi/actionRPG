using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;


public class BindableInput : MonoBehaviour {
    private static string bindingSaveLoc = "/Bindings.dat";
    private static BindableInput instance;
    public static BindableInput Instance
    {
        get { return instance; }
    }

    [System.Serializable]
    public struct Binding
    {
        public string name;
        public KeyCode input, altInput;
        public Binding(string _name, KeyCode _input, KeyCode _altInput)
        {
            name = _name;
            input = _input;
            altInput = _altInput;
        }
    }

    static Dictionary<string,Binding> binds;

    private static void SaveBinds()
    {
        FileStream save = File.Create(Application.persistentDataPath+bindingSaveLoc);
        BinaryFormatter format = new BinaryFormatter();
        format.Serialize(save, binds);
        save.Close();
    }

    public static void LoadDefaultBinds()
    {
        binds = new Dictionary<string, Binding>();
        XmlReader reader = XmlReader.Create(Application.dataPath + "/XML/DefaultBinds.xml");
        reader.MoveToContent();
        while (reader.ReadToFollowing("bind"))
        {
            Binding newBinding;
            reader.ReadToDescendant("name");
            newBinding.name = reader.ReadString();
            reader.ReadToNextSibling("input");
            newBinding.input = (KeyCode)System.Enum.Parse(typeof(KeyCode), reader.ReadString());
            reader.ReadToNextSibling("altInput");
            newBinding.altInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), reader.ReadString());
            binds.Add(newBinding.name, newBinding);
        }
        SaveBinds();
    }

    private static void LoadBinds()
    {
        if (File.Exists(Application.persistentDataPath + bindingSaveLoc))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream load = File.Open(Application.persistentDataPath + bindingSaveLoc, FileMode.Open);
            binds = (Dictionary<string, Binding>)format.Deserialize(load);
            load.Close();
        }
        else
        {
            LoadDefaultBinds();
        }
    }

    void Awake()
    {
        instance = this;
        LoadBinds();
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void OnDestroy()
    {
        SaveBinds();
    }

    public static void CreateBind(string _name, KeyCode _input, KeyCode _altInput)
    {
        if (!binds.ContainsKey(_name))
        {
            Binding newBind = new Binding(_name, _input, _altInput);
            binds.Add(_name, newBind);
            SaveBinds();
        }
    }

    public static bool KeyAlreadyBound(KeyCode key, string ignore = null)
    {
        foreach(Binding bind in binds.Values)
        {
            if (bind.name != ignore)
            {
                if (bind.input == key || bind.altInput == key)
                    return true;
            }
        }
        return false;
    }

    static void RemoveBoundKey(KeyCode key)
    {
        foreach (Binding bind in binds.Values)
        {
            if (bind.input == key)
            {
                UpdatePrimaryBind(bind.name, KeyCode.None);
                break;
            }
            if (bind.altInput == key)
            {
                UpdateAltBind(bind.name, KeyCode.None);
                break;
            }
        }
    }

    public static void UpdatePrimaryBind(string _name, KeyCode _input)
    {
        if (_input != KeyCode.None)
            RemoveBoundKey(_input);
        if (binds.ContainsKey(_name))
        {
            Binding updatedBind = binds[_name];
            if (_input == KeyCode.None)
            {
                updatedBind.input = GetAltBind(_name);
            }
            else
                updatedBind.input = _input;
            binds.Remove(_name);
            binds.Add(_name, updatedBind);
            if (_input == KeyCode.None)
                UpdateAltBind(_name, KeyCode.None);
        }
    }

    public static void UpdateAltBind(string _name, KeyCode _input)
    {
        if (_input != KeyCode.None)
            RemoveBoundKey(_input);
        if (binds.ContainsKey(_name))
        {
            Binding updatedBind = binds[_name];
            updatedBind.altInput = _input;
            binds.Remove(_name);
            binds.Add(_name, updatedBind);
        }
    }

    public static KeyCode GetBind(string _name)
    {
        if (binds.ContainsKey(_name))
        {
            return binds[_name].input;
        }
        return KeyCode.None;
    }

    public static KeyCode GetAltBind(string _name)
    {
        if (binds.ContainsKey(_name))
        {
            return binds[_name].altInput;
        }
        return KeyCode.None;
    }

    public static string GetBindString(string _name)
    {
        if (binds.ContainsKey(_name))
        {
            string bind = binds[_name].input.ToString();
            if (bind.Contains("Alpha"))
                return bind.Remove(0, 5);
            return bind;
        }
        return "";
    }

    public static string GetAltBindString(string _name)
    {
        if (binds.ContainsKey(_name))
        {
            string bind = binds[_name].altInput.ToString();
            if (bind.Contains("Alpha"))
                return bind.Remove(0, 5);
            return bind;
        }
        return "";
    }
    public static bool BindDown(string _name, bool acceptHold = false)
    {
        if (binds.ContainsKey(_name))
        {
            Binding bind = binds[_name];
            if (acceptHold)
            {
                if (Input.GetKey(bind.input) || Input.GetKey(bind.altInput))
                {
                    return true;
                }
            }
            else
            {
                if(Input.GetKeyDown(bind.input) || Input.GetKeyDown(bind.altInput))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool BindUp(string _name)
    {
        if (binds.ContainsKey(_name))
        {
            Binding bind = binds[_name];
            if (Input.GetKeyUp(bind.input) || Input.GetKeyUp(bind.altInput))
            {
                return true;
            }
        }
        return false;
    }

    public static string GetBindName(KeyCode key)
    {
        foreach(Binding bind in binds.Values)
        {
            if (bind.input == key || bind.altInput == key)
                return bind.name;
        }
        return "";
    }
}