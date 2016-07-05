using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Encounter : ScriptableObject
{
    [SerializeField]
    public List<Character> enemies;
    [SerializeField]
    public List<Item> inventory;
}
