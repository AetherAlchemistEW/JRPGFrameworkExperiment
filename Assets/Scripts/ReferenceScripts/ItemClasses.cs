using UnityEngine;

//This library will handle all item related scriptable objects and associated structs/enums
public enum GearSlot { Helm, Chest, Gloves, Boots, Greaves, Weapon }

//Items
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField]
    public Texture icon;
    [SerializeField]
    public string itemName;
    [SerializeField]
    public string tooltip;
}

//Gear
[System.Serializable]
public class Gear : Item
{
    [SerializeField]
    public GearSlot slot;
    [SerializeField]
    public Stats stats;
}

[System.Serializable]
public class Useable : Item
{
    [SerializeField]
    public Ability result;
}