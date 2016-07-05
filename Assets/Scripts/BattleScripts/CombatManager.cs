using UnityEngine;
using System.Collections.Generic;

//This class will handle battle-phase combat
public class CombatManager : MonoBehaviour
{
    //load in potential enemies and players
    [SerializeField]
    public List<Encounter> enemyPool;
    public List<Character> playerPool;
    public List<BattleAgent> attackQueue;
    public bool runATB;
    public bool scriptedLoss;
    public GameObject agentUI;

    //spawned scene agents
    List<BattleAgent> players = new List<BattleAgent>();
    List<BattleAgent> ai = new List<BattleAgent>();

    //set up the scene, load in the players and enemies. TODO
    void Start()
    {
        foreach(BattlePlayer b in FindObjectsOfType<BattlePlayer>())
        {
            players.Add(b);
        }
        foreach (BattleAI b in FindObjectsOfType<BattleAI>())
        {
            ai.Add(b);
        }
        //go through each battleplayer and assign players from 0...
        for (int i = 0; i < players.Count; i++)
        {
            //add to players as they're spawned
            players[i].character = playerPool[i];
            players[i].Init();
            //spawn UI panel
            GameObject ui = Instantiate(agentUI) as GameObject;
            ui.GetComponent<AgentUIHandler>().myAgent = players[i];
            ui.transform.SetParent(FindObjectOfType<Canvas>().transform);
        }

        //choose random enemy pool
        List<Character> chosenPool = enemyPool[Random.Range(0, enemyPool.Count)].enemies;
        for (int i = 0; i < ai.Count; i++)
        {
            //add to enemies as they're spawned
            ai[i].character = chosenPool[i];
            ai[i].Init();
            //spawn UI panel
            GameObject ui = Instantiate(agentUI) as GameObject;
            ui.GetComponent<AgentUIHandler>().myAgent = ai[i];
            ui.transform.SetParent(FindObjectOfType<Canvas>().transform);
        }

        FindObjectOfType<BattleCamera>().StartCoroutine("StandardConfig");
    }

    void Update()
    {
        if(attackQueue.Count > 0 && runATB)
        {
            attackQueue[0].StartCoroutine("RunAction");
        }
    }

    public void CheckOutcome()
    {
        //Count active players
        int count = players.Count;
        foreach (BattleAgent a in players)
        {
            if(a.character.health.value <= 0)
            {
                count--;
            }
        }
        if(count == 0)
        {
            EnemyWin();
        }
        //Count active enemies
        count = ai.Count;
        foreach (BattleAgent a in ai)
        {
            if (a.character.health.value <= 0)
            {
                count--;
            }
        }
        if (count == 0)
        {
            PlayerWin();
        }
    }
    
    //TODO
    void PlayerWin()
    {

    }

    //TODO
    void EnemyWin()
    {
        if(!scriptedLoss)
        {
            //game over
        }
        else
        {
            //same as win
        }
    }
}
