using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BattlePlayer : BattleAgent
{
    public float errorMargin;

    //Start the atb
    void Start()
    {
        //Init();
    }

    //Temp solution
    Transform FindTarget()
    {
        Transform target = null;
        Debug.Log("find target");
        List<GameObject> enemies = new List<GameObject>();
        foreach(BattleAgent e in FindObjectsOfType<BattleAgent>())
        {
            Debug.Log("Looping");
            if(e.character.isPlayer)
            {
                continue;
            }
            else if (e.enabled == true)
            {
                enemies.Add(e.gameObject);
            }
        }
        if(enemies.Count > 0)
        {
            target = enemies[Random.Range(0, enemies.Count)].transform;
        }
        return target;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PerformAction(new PlayerAction(0,0));
        }
    }

    void OnMouseDown()
    {
        //tell the canvas to spawn the inner ring menu
        if (canAct)
        {
            RadialMenuSpawner.ins.SpawnMenu(this, 1, -1);
        }
    }

    public void PerformAction(PlayerAction act)
    {
        switch(act.category)
        {
            case 0:
                curAttack = attacks[act.index];
                break;
            case 1:
                curAbility = abilities[act.index];
                break;
            case 2:
                Debug.Log("Item used");
                break;
        }
        //target selection
        StartCoroutine("TargetSelection");
    }

    IEnumerator TargetSelection()
    {
        while(attackTarget == null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.GetComponent<BattleAgent>())
                    {
                        attackTarget = hit.transform;
                    }
                }
            }
            yield return null;
        }
        AddToQueue();
        yield return null;
    }

    //move through the current attack combo, generate the interactable region from the current attack,
    //if it's tapped/clicked in time it succeeds and moves to the next, else it fails
    public new IEnumerator ComboSystem()
    {
        //Debug.Log(gameObject.name + "Attacking!");
        StartCoroutine("ApproachTarget");
        yield return new WaitUntil(ReachedTarget);

        float timer = 0;
        int i = 0;
        GameObject curUI = null;
        while (i < curAttack.attacks.Length)
        {
            //Generate the screen region we can interact with from the current attack
            Rect regionBounds = GenerateRegion(curAttack.attacks[i].tapRegion);
            //draw
            //Vector3 leftScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(regionBounds.xMin, regionBounds.yMin, Camera.main.nearClipPlane));
            //Vector3 rightScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(regionBounds.xMax, regionBounds.yMax, Camera.main.nearClipPlane));
            //Debug.DrawLine(leftScreenPoint, rightScreenPoint);

            //float midLength = (rightScreenPoint - leftScreenPoint).magnitude / 2;
            //Vector2 UIPos = (rightScreenPoint - leftScreenPoint).normalized * midLength;

            if (curUI == null)
            {
                curUI = Instantiate(attackComboUI) as GameObject;
                curUI.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
                curUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, regionBounds.width);
                curUI.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, regionBounds.height);
                curUI.GetComponent<RectTransform>().position = regionBounds.position;
            }

            if (timer < curAttack.attacks[i].timeFrame)
            {
                timer += Time.smoothDeltaTime;
                Vector2 updateScale = new Vector2(regionBounds.width / (curAttack.attacks[i].timeFrame / timer), regionBounds.height / (curAttack.attacks[i].timeFrame / timer));
                curUI.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, updateScale.x);
                curUI.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, updateScale.y);

                if (Input.GetMouseButtonDown(0))
                {
                    if(ClickOverlapsRegion(regionBounds) && curAttack.attacks[i].timeFrame - timer < curAttack.attacks[i].timeFrame*errorMargin)
                    {
                        int d = CalcComboDamage(i);
                        attackTarget.GetComponent<BattleAgent>().TakeDamage(d, curAttack.damageType);
                        Destroy(curUI);
                        timer = 0;
                        i++;
                        //Debug.Log(gameObject.name + " Combo Success");
                    }
                    else
                    {
                        //Debug.Log(gameObject.name + " Missed");
                        Destroy(curUI);
                        break;
                    }
                }
            }
            else
            {
                Destroy(curUI);
                break;
            }
            yield return null;
        }
        Debug.Log("Combo Ended");
        StartCoroutine("ReturnToPosition");
        Reset();
        //move back to original position
        yield return null;
    }

    //For checking that we clicked in the screen region
    public bool ClickOverlapsRegion(Rect square)
    {
        bool result = false;
        //Debug.Log(square.xMin.ToString() + " : " + Input.mousePosition.x + " : " + square.xMax.ToString());
        //Debug.Log(square.yMin.ToString() + " : " + Input.mousePosition.y + " : " + square.yMax.ToString());
        if (Input.mousePosition.x > square.xMin && Input.mousePosition.x < square.xMax)
        {
            if (Input.mousePosition.y > square.yMin && Input.mousePosition.y < square.yMax)
            {
                result = true;
            }
        }
        return result;
    }

    //Ability use is handled by base class
}

public struct PlayerAction
{
    public int category, index;

    public PlayerAction(int _cat, int _i)
    {
        category = _cat;
        index = _i;
    }
}
