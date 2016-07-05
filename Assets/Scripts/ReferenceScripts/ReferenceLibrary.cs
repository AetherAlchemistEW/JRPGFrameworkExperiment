using UnityEngine;

//This library will handle misc scriptable objects and associated structs/enums as well as some structs/enums used on a variety of classes
[System.Serializable]
public struct Stat
{
    public float value;
    public int level;
    public int experience;

    void SetLevel()
    {
        //calc level from exp
    }
}

//Stats, used by gear and characters
[System.Serializable]
public struct Stats
{
    [SerializeField]
    public Stat maxHealth;
    [SerializeField]
    public Stat maxMana;
    [SerializeField]
    public Stat strength;
    [SerializeField]
    public Stat intelligence;
    [SerializeField]
    public Stat agility;
    [SerializeField]
    public Resistences resistences;
}

//Resistences
[System.Serializable]
public struct Resistences
{
    //elemental resistance
    [SerializeField]
    public Stat fire;
    [SerializeField]
    public Stat water;
    [SerializeField]
    public Stat air;
    [SerializeField]
    public Stat earth;
    [SerializeField]
    public Stat light;
    [SerializeField]
    public Stat dark;

    //natural resistance
    [SerializeField]
    public float slashing;
    [SerializeField]
    public float blugeoning;
    [SerializeField]
    public float baseArmour;

    //can be...
    //[SerializeField]
    //public bool isTargetable;
}

