using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    private struct SaveStruct
    {
        public string characterName;
        public int strIncrement, intIncrement, agiIncrement, resIncrement, staIncrement, vitIncrement, level, resets, currentXP;
        public int str, intel, agi, res, sta, vit;
    }

    [SerializeField]
    private string characterName = "Test";
    [SerializeField]
    private int level, resets;
    [SerializeField]
    private RPG_ResourceTypes resourceType;
    [SerializeField]
    private int baseStr = 10, baseInt = 10, baseAgi = 10, baseRes = 5, baseSta = 5, baseVit = 5;
    [SerializeField]
    private int strIncrement=5, intIncrement=5, agiIncrement=5, resIncrement=1, staIncrement=2, vitIncrement=5;
    public int weaponDamage=1, armorStr=0, armorInt=0, armorAgi=0, armorDef=0, armorRes=0, armorSta=0, armorVit=0, armorResourceRegen=0, armorBonusResource=0;
    public float armorArmorPenitration, armorResistPenetration, armorSpeed;
    private int strength, intelligence, agility, resistance, stamina, vitality;
    private float maxHealth, currentHealth, healthRegen;
    private int expToLevel, currentExp;
    private float currentResource, maxResource, resourceRegen;
    private float attackSpeed = 1.0f;

    #region ACCESSORS
    public string CharName
    {
        get { return characterName; }
    }
    public int MaxHealth
    {
        get { return (int)maxHealth; }
    }
    public int CurrentHealth
    {
        get { return (int)currentHealth; }
        set
        {
            currentHealth = value;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            else if (currentHealth < 0)
                currentHealth = 0;
        }
    }
    public int MaxResource
    {
        get { return (int)maxResource; }
    }
    public int CurrentResource
    {
        get { return (int)currentResource; }
        set
        {
            currentResource = value;
            if (currentResource > maxResource)
                currentResource = maxResource;
            else if (currentResource < 0)
                currentResource = 0;
        }
    }
    public int MaxXp
    {
        get { return expToLevel; }
    }
    public int CurrentXp
    {
        get { return currentExp; }
    }
    public int Level
    {
        get { return level; }
    }
    public int Resets
    {
        get { return resets; }
    }
    public RPG_ResourceTypes ResourceType
    {
        get { return resourceType; }
    }

    public int Strength
    {
        get
        {
            return strength + armorStr;
        }
    }
    public int Intelligence
    {
        get
        {
            return intelligence + armorInt;
        }
    }
    public int Agility
    {
        get
        {
            return agility + armorAgi;
        }
    }
    public int Resistance
    {
        get
        {
            return resistance + (Intelligence/2) + Strength;
        }
    }
    public int Stamina
    {
        get
        {
            return stamina + armorSta;
        }
    }
    public int Vitality
    {
        get
        {
            return vitality + armorVit;
        }
    }

    public float IncreasedMovementSpeed
    {
        get { return Mathf.Clamp(armorSpeed/10, 1, 1.25f); }
    }
    public float AttackSpeed
    {
        get { return Mathf.Clamp(attackSpeed - (armorSpeed / 10), .25f, 4.0f); }
    }

    public int PhysicalDamage
    {
        get { return weaponDamage + (Strength * 2 + Agility); }
    }
    public int MagicalDamage
    {
        get { return weaponDamage + (Intelligence * 2 + Agility); }
    }
    public float CritChance
    {
        get { return Mathf.Clamp((float)Agility / 50, 5, 100); }
    }
    public float DodgeChance
    {
        get { return Mathf.Clamp((float)Agility / 25, 0, 100); }
    }

    public int StrIncrement
    {
        get { return strIncrement; }
    }
    public int IntIncrement
    {
        get { return intIncrement; }
    }
    public int AgiIncrement
    {
        get { return agiIncrement; }
    }
    public int ResIncrement
    {
        get { return resIncrement; }
    }
    public int StaIncrement
    {
        get { return staIncrement; }
    }
    public int VitIncrement
    {
        get { return vitIncrement; }
    }

    #endregion

    private void UpdateHealthAndResource()
    {
        maxHealth = (Vitality * 10 + Stamina * 2);
        healthRegen = Mathf.Clamp((Stamina / 10), 3, int.MaxValue);
        currentHealth = maxHealth;
        currentResource = maxResource;
        UIController.GetInstance().UpdateHealthAndResource();
    }

    private void UpdateStats()
    {
        strength =      baseStr;
        intelligence =  baseInt;
        agility =       baseAgi;
        resistance =    baseRes;
        stamina =       baseSta;
        vitality =      baseVit;
        switch (resourceType)
        {
            case RPG_ResourceTypes.Mana:
                maxResource = Intelligence * 5;
                resourceRegen = Mathf.Clamp(Intelligence / 5, 10, int.MaxValue);
                break;
            case RPG_ResourceTypes.Rage:
                maxResource = 200;
                resourceRegen = -1;
                break;
            case RPG_ResourceTypes.Energy:
                maxResource = 100 + Stamina * 2;
                resourceRegen = 2;
                break;
            case RPG_ResourceTypes.Souls:
                maxResource = 5;
                resourceRegen = 0;
                break;
            default:
                maxResource = 100;
                resourceRegen = 100;
                break;
        }
        resourceRegen += armorResourceRegen;
        maxResource += armorBonusResource;
        expToLevel = 300 * level;
        if (level >= 40)
            expToLevel += (int)Mathf.Pow(expToLevel, 1.1f) + 100 * level;
        else if (level >= 30)
            expToLevel += (int)Mathf.Pow(expToLevel, 1.07f) + 75 * level;
        else if (level >= 20)
            expToLevel += (int)Mathf.Pow(expToLevel, 1.04f) + 50 * level;
        else if (level >= 10)
            expToLevel += (int)Mathf.Pow(expToLevel, 1.02f) + 25 * level;
        float temp = expToLevel / 100;
        expToLevel = (int)temp * 100;
        UIController.GetInstance().Begin(this, gameObject.GetComponent<PlayerSpells>(), gameObject.GetComponent<PlayerController>());
        UpdateHealthAndResource();
    }

    void IncrementBaseStats()
    {
        baseStr += strIncrement;
        baseInt += intIncrement;
        baseAgi += agiIncrement;
        baseRes += resIncrement;
        baseSta += staIncrement;
        baseVit += vitIncrement;
    }

    private void ModifyLevel(int value = 1)
    {
        level += value;
        while(value > 0)
        {
            IncrementBaseStats();
            value--;
        }
        UpdateStats();
        GameVariables.UpdateVariables();
        UIController.GetInstance().UpdateLevelAndResets();
    }

    public void ResetLevel()
    {
        currentExp = 0;
        level = 1;
        resets++;
        IncrementBaseStats();
        UpdateStats();
        GameVariables.UpdateVariables();
        UIController.GetInstance().UpdateLevelAndResets();
    }

    public void AddXp(int value, int enemyLevel = 1)
    {
        int xpToAdd = value + (int)(value * .1f) * enemyLevel;
        currentExp += xpToAdd;
        if (currentExp >= expToLevel && level < GameVariables.maxPlayerLevel)
        {
            currentExp -= expToLevel;
            ModifyLevel(1);
        }
        else if (currentExp > expToLevel)
            currentExp = expToLevel;
        UIController.GetInstance().UpdateXP();
    }

    public bool CanReset()
    {
        return (level == GameVariables.maxPlayerLevel && currentExp == expToLevel);
    }

    IEnumerator Regen()
    {
        while (true)
        {
            if (currentResource < maxResource)
            {
                currentResource += resourceRegen / 50;
                if (currentResource > maxResource)
                    currentResource = maxResource;
            }
            if (currentHealth < maxHealth)
            {
                currentHealth += healthRegen / 50;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
            }
            UIController.GetInstance().UpdateHealthAndResource();
            yield return new WaitForSeconds(.1f);
        }
    }

    // Use this for initialization
    void Start()
    {
        LoadStats();
        currentHealth = maxHealth;
        currentResource = maxResource;
        StartCoroutine(Regen());
    }

    private void OnDestroy()
    {
        SaveStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SaveStats()
    {
        SaveStruct newSave = new SaveStruct();
        newSave.str = baseStr;
        newSave.intel = baseInt;
        newSave.agi = baseAgi;
        newSave.sta = baseSta;
        newSave.res = baseRes;
        newSave.vit = baseVit;
        newSave.strIncrement = strIncrement;
        newSave.agiIncrement = agiIncrement;
        newSave.intIncrement = intIncrement;
        newSave.staIncrement = staIncrement;
        newSave.resIncrement = resIncrement;
        newSave.vitIncrement = vitIncrement;
        newSave.level = level;
        newSave.resets = resets;
        newSave.characterName = characterName;
        newSave.currentXP = currentExp;
        FileStream save = File.Create(Application.persistentDataPath + "/"+characterName+"Stats.dat");
        BinaryFormatter format = new BinaryFormatter();
        format.Serialize(save, newSave);
        save.Close();
    }

    public void LoadStats()
    {
        if (File.Exists(Application.persistentDataPath + "/" + characterName + "Stats.dat"))
        {
            SaveStruct newSave = new SaveStruct();
            FileStream load = File.Open(Application.persistentDataPath + "/" + characterName + "Stats.dat", FileMode.Open);
            BinaryFormatter format = new BinaryFormatter();
            newSave = (SaveStruct)format.Deserialize(load);
            load.Close();

            characterName = newSave.characterName;
            level = newSave.level;
            resets = newSave.resets;
            strIncrement = newSave.strIncrement;
            agiIncrement = newSave.agiIncrement;
            intIncrement = newSave.intIncrement;
            resIncrement = newSave.resIncrement;
            staIncrement = newSave.staIncrement;
            vitIncrement = newSave.vitIncrement;
            baseStr = newSave.str;
            baseInt = newSave.intel;
            baseAgi = newSave.agi;
            baseRes = newSave.res;
            baseSta = newSave.sta;
            baseVit = newSave.vit;
            currentExp = newSave.currentXP;
        }
        UpdateStats();
    }
}
