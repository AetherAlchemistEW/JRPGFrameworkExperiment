using UnityEngine;
using System.Collections.Generic;

//Sequence of attacks making up a combo
[System.Serializable]
public class AttackCombo : ScriptableObject
{
    [SerializeField]
    public DamageType damageType;
    [SerializeField]
    public Attack[] attacks;
    [SerializeField]
    public float critChance;
    [SerializeField]
    public string tooltip;
    [SerializeField]
    public float atbPenalty;
    [SerializeField]
    public Sprite uiIcon;
    [SerializeField]
    public Color uiColor;
    [SerializeField]
    public string title;
}

//Component part of a combo
[System.Serializable]
public struct Attack
{
    [SerializeField]
    public float damage;
    [SerializeField]
    public Region tapRegion;
    [SerializeField]
    public float timeFrame;
    [SerializeField]
    public AnimationClip animation;
}

//Screen region which is tapped to do the attack combo
public enum Region { topLeft, top, topRight, bottomLeft, bottom, bottomRight };
