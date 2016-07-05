using UnityEngine;
using UnityEditor;
using System.Collections;

public class RPGFrameworkObjects : Editor
{
    //Menu Items
    [MenuItem("RPG Framework/Combat Assets/Ability")]
    static void MakeAbility()
    {
        CreateObject<Ability>("Ability");
    }

    [MenuItem("RPG Framework/Combat Assets/AttackCombo")]
    static void MakeCombo()
    {
        CreateObject<AttackCombo>("Combo");
    }

    [MenuItem("RPG Framework/Items/General Item")]
    static void MakeItem()
    {
        CreateObject<Item>("MiscItem");
    }

    [MenuItem("RPG Framework/Items/Useable Item")]
    static void MakeUseable()
    {
        CreateObject<Useable>("UseableItem");
    }

    [MenuItem("RPG Framework/Items/Gear")]
    static void MakeGear()
    {
        CreateObject<Gear>("Gear");
    }

    [MenuItem("RPG Framework/Entities/Character")]
    static void MakeCharacter()
    {
        CreateObject<Character>("Character");
    }

    [MenuItem("RPG Framework/Entities/Encounter")]
    static void MakeEncounter()
    {
        CreateObject<Encounter>("Encounter");
    }

    [MenuItem("RPG Framework/Misc/Playthrough")]
    static void MakePlaythrough()
    {
        CreateObject<PlaythroughData>("PTData");
    }

    //Create the given scriptable object according to type and argument provided
    static void CreateObject<T>(string exten) where T : ScriptableObject
    {
        //Save File Panel to find path/name
        string path = EditorUtility.SaveFilePanelInProject("Create " + exten, "new" + exten + ".asset", "asset", "");

        //error checking path
        if (path == null)
        {
            return;
        }

        //new instance of item, this only works because we are ensuring T inherits from Scriptable Object
        T newItem = CreateInstance<T>();
        //new asset from item
        AssetDatabase.CreateAsset(newItem, path);
        //save the asset
        AssetDatabase.SaveAssets();
    }
}
