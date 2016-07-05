using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleAI : BattleAgent
{
    //VARIABLES
    [Range(0,1)] //0 .. 1 attack or ability (respectively) liklihood
    public float attackOrAbility;
    [Range(0, 3)]
    public float reactionVariance;
    [Range(0,5)]
    public float baseReactionTime;
    [Range(0,1)]
    public float attackAccuracy;

    void Start()
    {
        //Init();
    }

    void Update()
    {
        if(canAct)
        {
            StartCoroutine("ChooseAttack");
            canAct = false;
        }
        if(attackTarget == null)
        {
            StartCoroutine("ChooseTarget");
        }
    }

    IEnumerator ChooseTarget()
    {
        //choose random enemy target (check all characters that have isPlayer)
        List<GameObject> enemies = new List<GameObject>();
        foreach (BattleAgent e in FindObjectsOfType<BattleAgent>())
        {
            if (!e.character.isPlayer)
            {
                continue;
            }
            else if(e.enabled == true)
            {
                enemies.Add(e.gameObject);
            }
        }
        if (enemies.Count > 0)
        {
            //any other logic for determining the best target would go here
            attackTarget = enemies[Random.Range(0, enemies.Count)].transform;
        }
        yield return null;
    }

    //Decision Making
    IEnumerator ChooseAttack()
    {
        //wait a moment
        float waitDur = Random.Range(-reactionVariance, reactionVariance) + baseReactionTime;
        waitDur = Mathf.Abs(waitDur);
        yield return new WaitForSeconds(waitDur);
        //choose an attack based on internal logic
        if(Random.Range(0f,1f) > attackOrAbility)
        {
            //Attack combo
            curAbility = null;
            curAttack = attacks[Random.Range(0, attacks.Count)];
        }
        else
        {
            //Ability combo
            curAttack = null;
            curAbility = abilities[Random.Range(0, abilities.Count)];
        }

        AddToQueue();
        yield return null;
    }

    //AI combo attacks
    public new IEnumerator ComboSystem()
    {
        //Debug.Log(gameObject.name + "Attacking!");
        StartCoroutine("ApproachTarget");
        yield return new WaitUntil(ReachedTarget);

        int i = 0;
        while(i < curAttack.attacks.Length)
        {
            //Rect regionBounds = GenerateRegion(curAttack.attacks[i].tapRegion);

            Debug.Log(i.ToString());
            //take a varied amount of time to attack
            float attackSpeed = Random.Range(0.2f, curAttack.attacks[i].timeFrame + 0.2f);
            //wait that long, then try to attack
            yield return new WaitForSeconds(attackSpeed);
            //Spawn Button at attack region
            if(Random.Range(0f,1f) < attackAccuracy && attackSpeed < curAttack.attacks[i].timeFrame)
            {
                //Succeed
                int d = CalcComboDamage(i);
                attackTarget.GetComponent<BattleAgent>().TakeDamage(d, curAttack.damageType);
                i++;
                Debug.Log(gameObject.name + " Combo Success");
            }
            else
            {
                break;
            }
            yield return null;
        }
        StartCoroutine("ReturnToPosition");
        Reset();
        yield return null;
    }

    //Ability use is handled by base class
}
