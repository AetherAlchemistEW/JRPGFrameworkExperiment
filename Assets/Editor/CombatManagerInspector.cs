using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//[CustomEditor(typeof(CombatManager))]
public class CombatManagerInspector : Editor
{
    CombatManager cm;

    public override void OnInspectorGUI()
    {
        /*
        cm = (CombatManager)target;

        //check for the lists, make a 1-1 if they're null
        if (cm.enemyPool == null)
        {
            cm.enemyPool = new List<List<Character>>();
            cm.enemyPool.Add(new List<Character>());
            cm.enemyPool[0].Add(null);
        }

        base.OnInspectorGUI();

        //Enemy Pool Section, will migrate to a world element that starts the encounter
        EditorGUILayout.HelpBox("Each vertical list represents a possible encounter", MessageType.Info);
        EditorGUILayout.LabelField("Potential Enemy Encounter Groups");
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < cm.enemyPool.Count; i++)
        {
            //Across
            EditorGUILayout.BeginVertical();
            for (int j = 0; j < cm.enemyPool[i].Count; j++)
            {
                //Down
                cm.enemyPool[i][j] = (Character)EditorGUILayout.ObjectField(cm.enemyPool[i][j], typeof(Character), false);
            }
            if (GUILayout.Button("Add Enemy To Squad " + i.ToString()))
            {
                cm.enemyPool[i].Add(null);
            }
            EditorGUILayout.EndVertical();
        }
        if (GUILayout.Button("Add Enemy Squad"))
        {
            cm.enemyPool.Add(new List<Character>());
            cm.enemyPool[cm.enemyPool.Count-1].Add(null);
        }
        EditorGUILayout.EndHorizontal();
        */
    }
}
