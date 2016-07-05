using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Base class for all agents in battle phases
public class BattleAgent : MonoBehaviour
{
    #region Variables
    //VARIABLES
    [HideInInspector]
    public CombatManager manager;
    [HideInInspector]
    public BattleCamera cam;
    public Character character; 

    [HideInInspector]
    public Stats stats;
    [HideInInspector]
    public Canvas localUI;
    [HideInInspector]
    public List<Gear> equip;

    //[HideInInspector]
    public List<AttackCombo> attacks;
    //[HideInInspector]
    public List<Ability> abilities;
    //[HideInInspector]
    public List<Ability> items;
    //[HideInInspector]
    public AttackCombo curAttack;
    //[HideInInspector]
    public Ability curAbility;
    //[HideInInspector]
    public Transform attackTarget;

    //ATB System
    //[HideInInspector]
    public float atbCurrent;
    //[Range(0.1f,10.0f)]
    public float atbMax;
    //[HideInInspector]
    public bool canAct;
    [HideInInspector]
    public Vector3 originalPosition;
    [HideInInspector]
    public Quaternion originalOrientation;

    public float approachSpeed;

    public GameObject attackComboUI;
    #endregion

    public void Init()
    {
        //Debug.Log(gameObject.name + " Initialising...");
        manager = FindObjectOfType<CombatManager>();
        cam = FindObjectOfType<BattleCamera>();
        stats = character.StatsTotal();
        gameObject.name = character.charName;
        //localUI = GetComponentInChildren<Canvas>();
        equip = character.gear;
        attacks = character.combos;
        abilities = character.abilities;
        StartCoroutine("atbFill");
        //derive atbMax from agility
        //atbMax = CalcATBMax(0);
        originalPosition = transform.position;
        originalOrientation = transform.rotation;
        //Set up visual elements(model, animations, ect)
    }

    //Fill the atb bar and then enable actions
    public IEnumerator atbFill()
    {
        while(atbCurrent < atbMax)
        {
            if (manager.runATB)
            {
                atbCurrent += Time.smoothDeltaTime;
            }
            //update atb feedback
            yield return null;
        }
        canAct = true;
        yield return null;
    }

    public void AddToQueue()
    {
        Debug.Log(gameObject.name + "Adding to Queue");
        canAct = false;
        atbCurrent = 0;
        manager.attackQueue.Add((BattleAgent)this);
    }

    public IEnumerator RunAction()
    {
        cam.activeAgent = this.transform;
        cam.targetAgent = attackTarget;
        Debug.Log(gameObject.name + " Run action!");
        manager.runATB = false;
        yield return null;
        if(curAttack != null)
        {
            Debug.Log("Combo calls");
            StartCoroutine("ComboSystem");
        }
        else if (curAbility != null)
        {
            Debug.Log("Ability calls");
            StartCoroutine("AbilitySystem");
        }
    }

    public IEnumerator ComboSystem()
    {
        yield return null;
    }

    public IEnumerator AbilitySystem()
    {
        Debug.Log(gameObject.name + " Ability called");
        //spawn the ability, hang while it exists and let it do its thing,
        GameObject ab = (GameObject)Instantiate(curAbility.spellEffect, transform.position, transform.rotation);
        //once it's gone, continue
        while (ab != null)
        {
            yield return null;
        }
        attackTarget.GetComponent<BattleAgent>().TakeDamage(curAbility.damage, curAbility.damageType);
        Reset();
        //ability related stat experience
        yield return null;
    }

    public void Reset()
    {
        cam.StartCoroutine("StandardConfig");
        cam.activeAgent = null;
        cam.targetAgent = null;
        manager.attackQueue.Remove((BattleAgent)this);
        manager.runATB = true;
        StartCoroutine("atbFill");
        attackTarget = null;
        curAttack = null;
        curAbility = null;
    }

    public IEnumerator ApproachTarget()
    {
        //move towards target if we're more than 2 units away
        while(Vector3.Distance(transform.position, attackTarget.position) > 2)
        {
            //Debug.Log("Move to target");
            //play movement animations
            transform.LookAt(attackTarget);
            //are we on the left or the right?
            int side = transform.position.x < attackTarget.position.x ? 1 : -1;
            Vector3 targPos = new Vector3(attackTarget.position.x + side, attackTarget.position.y, attackTarget.position.z);
            transform.position = Vector3.Lerp(transform.position, targPos, Time.smoothDeltaTime * approachSpeed);
            cam.StartCoroutine("FollowConfig");
            yield return null;
        }
        //Debug.Log("There");
        //then play stop animation
        yield return null;
    }

    public bool ReachedTarget()
    {
        //Debug.Log(transform.name + " : " + attackTarget.name);
        float dist = Vector3.Distance(transform.position, attackTarget.position);
        //Debug.Log(dist);
        bool result = dist < 2;
        //Debug.Log(result);
        return result;
    }

    public IEnumerator ReturnToPosition()
    {
        //Debug.Log("Return to original position");
        transform.rotation = originalOrientation;
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            //lerp back there
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.smoothDeltaTime * approachSpeed * 2);
            cam.StartCoroutine("StandardConfig");
            yield return null;
        }
        yield return null;
    }

    //INCOMPLETE
    public int CalcComboDamage(int i)
    {
        float d = curAttack.attacks[i].damage;
        //determine damage of combo hit based on attack damage and stats
        float damageVariance = curAttack.attacks[i].damage / 10;
        d = curAttack.attacks[i].damage + stats.strength.value + Random.Range(-damageVariance, damageVariance); //add weapon damage
        d = Random.Range(0, 100) < curAttack.critChance ? d*1.5f : d;
        //Debug.Log("Damage dealt: " + d.ToString());
        return Mathf.RoundToInt(d);
    }

    //INCOMPLETE
    public int CalcAbilityDamage()
    {
        int d;
        //determine damage of ability based on ability damage and stats
        d = Mathf.RoundToInt(curAbility.damage + stats.intelligence.value);
        return d;
    }

    //INCOMPLETE
    public float CalcATBMax(float penalty)
    {
        float p;
        //calc from agility
        p = penalty - stats.agility.value;
        //if penalty > 0
        //add to agility experience (based on the penalty)
        return p;
    }

    //INCOMPLETE
    public void TakeDamage(float damage, DamageType type)
    {
        //Debug.Log("Recieveing Damage: " + damage);
        float modifier = 0;
        //apply your armour
        modifier += stats.resistences.baseArmour;
        //Debug.Log("Armour amount: " + modifier);
        //resistance calculations
        //resistance stat experience
        switch (type)
        {
            case DamageType.none:
                break;
            case DamageType.fire:
                modifier = stats.resistences.fire.value;
                stats.resistences.fire.experience += modifier < damage ? 1 : 0;
                break;
            case DamageType.water:
                modifier = stats.resistences.water.value;
                stats.resistences.water.experience += modifier < damage ? 1 : 0;
                break;
            case DamageType.earth:
                modifier = stats.resistences.earth.value;
                stats.resistences.earth.experience += modifier < damage ? 1 : 0;
                break;
            case DamageType.air:
                modifier = stats.resistences.air.value;
                stats.resistences.air.experience += modifier < damage ? 1 : 0;
                break;
            case DamageType.light:
                modifier = stats.resistences.light.value;
                stats.resistences.light.experience += modifier < damage ? 1 : 0; ;
                break;
            case DamageType.dark:
                modifier = stats.resistences.dark.value;
                stats.resistences.dark.experience += modifier < damage ? 1 : 0;
                break;
        }
        //Debug.Log("type amount: " + modifier);
        //if it went over the damage, cap it off
        modifier = modifier >= damage ? damage : modifier;

        //Debug.Log("Capping: " + modifier);

        character.health.value -= (damage - modifier);
        //Debug.Log("Damage taken: " + (damage - modifier).ToString());

        if (character.health.value <= 0)
        {
            character.health.value = 0;
            Death();
        }
    }

    //INCOMPLETE
    public void Death()
    {
        //Death Animation
        manager.CheckOutcome();
        enabled = false;
    }

    public Rect GenerateRegion(Region tap)
    {
        Rect regionBounds = new Rect();
        float sWidth = Screen.width;
        float sHeight = Screen.height;
        //Debug.Log(curAttack.attacks[i].tapRegion.ToString());
        switch (tap)
        {
            case Region.topLeft:
                regionBounds.xMin = 0;
                regionBounds.xMax = sWidth / 3;
                regionBounds.yMax = sHeight;
                regionBounds.yMin = sHeight / 2;
                break;
            case Region.top:
                regionBounds.xMin = sWidth / 3;
                regionBounds.xMax = sWidth / 3 * 2;
                regionBounds.yMax = sHeight;
                regionBounds.yMin = sHeight / 2;
                break;
            case Region.topRight:
                regionBounds.xMin = sWidth / 3 * 2;
                regionBounds.xMax = sWidth;
                regionBounds.yMax = sHeight;
                regionBounds.yMin = sHeight / 2;
                break;
            case Region.bottomLeft:
                regionBounds.xMin = 0;
                regionBounds.xMax = sWidth / 3;
                regionBounds.yMax = sHeight / 2;
                regionBounds.yMin = 0;
                break;
            case Region.bottom:
                regionBounds.xMin = sWidth / 3;
                regionBounds.xMax = sWidth / 3 * 2;
                regionBounds.yMax = sHeight / 2;
                regionBounds.yMin = 0;
                break;
            case Region.bottomRight:
                regionBounds.xMin = sWidth / 3 * 2;
                regionBounds.xMax = sWidth;
                regionBounds.yMax = sHeight / 2;
                regionBounds.yMin = 0;
                break;
        }

        return regionBounds;
    }
}
