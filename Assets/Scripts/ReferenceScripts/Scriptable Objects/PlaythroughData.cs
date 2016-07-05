using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlaythroughData : ScriptableObject
{
    [SerializeField]
    public List<Item> inventory;
    [SerializeField]
    public List<Character> characters;
    [SerializeField]
    public Vector3 worldPosition;
    [SerializeField]
    public int sceneNumber;
    [SerializeField]
    public Dictionary<string, bool> storyEvents;
}
