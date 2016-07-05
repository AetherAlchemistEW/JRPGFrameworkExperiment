using UnityEngine;

//This library will contain all the action related scriptable objects and associated structs/enums

//Type of damage, none is reserved for non-elemental and attacks which don't deal damage
public enum DamageType {none, fire, water, air, earth, light, dark};

//Ability, includes spells and items
[System.Serializable]
public class Ability : ScriptableObject
{
    [SerializeField]
    public DamageType damageType;
    [SerializeField]
    public float damage;
    [SerializeField]
    public string tooltip;
    [SerializeField]
    public GameObject spellEffect;
    [SerializeField]
    public float atbPenalty;
    [SerializeField]
    public Sprite uiIcon;
    [SerializeField]
    public Color uiColor;
    [SerializeField]
    public string title;
}