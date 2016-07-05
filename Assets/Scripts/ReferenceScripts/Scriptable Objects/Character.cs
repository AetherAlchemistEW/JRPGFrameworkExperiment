using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Character : ScriptableObject
{
    [SerializeField]
    public string charName;
    [SerializeField]
    public string bio;
    [SerializeField]
    public Stats baseStats;
    [SerializeField]
    public Stat health;
    [SerializeField]
    public Stat mana;
    //Stats will level up independantly based on useage
    [SerializeField]
    public List<Gear> gear;
    [SerializeField]
    public List<AttackCombo> combos;
    [SerializeField]
    public List<Ability> abilities;
    [SerializeField]
    public bool isPlayer;

    //AI related fields, on custom inspector, show only if 'isPlayer' is false
    //[Header("AI logic variables")]
    [SerializeField]
    //[Range(0, 1)] //0 .. 1 attack or ability (respectively) liklihood
    public float attackOrAbility;
    [SerializeField]
    //[Range(0, 3)]
    public float reactionVariance;
    [SerializeField]
    //[Range(0, 5)]
    public float baseReactionTime;
    [SerializeField]
    //[Range(0, 1)]
    public float attackAccuracy;

    //Total up your stats from gear
    public Stats StatsTotal()
    {
        Stats s = new Stats();
        s = this.baseStats;
        foreach (Gear g in this.gear)
        {
            s.agility.value += g.stats.agility.value;
            s.intelligence.value += g.stats.intelligence.value;
            s.maxHealth.value += g.stats.maxHealth.value;
            s.resistences.air.value += g.stats.resistences.air.value;
            s.resistences.baseArmour += g.stats.resistences.baseArmour;
            s.resistences.blugeoning += g.stats.resistences.blugeoning;
            s.resistences.dark.value += g.stats.resistences.dark.value;
            s.resistences.earth.value += g.stats.resistences.earth.value;
            s.resistences.water.value += g.stats.resistences.water.value;
            s.resistences.slashing += g.stats.resistences.slashing;
            s.resistences.light.value += g.stats.resistences.light.value;
        }
        return s;
    }
}