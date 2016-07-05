using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class RadialButton : MonoBehaviour, IPointerEnterHandler
{
    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu myMenu;
    public int ring;
    public BattlePlayer p;
    public int cat;
    public int index;
    RadialMenu subMenu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //you've entered a button in the first ring other than yourself, so tell it to deselect
        if(myMenu.selected != null && myMenu.selected != this && ring == 1)
        {
            myMenu.selected.DeSelect();
            //Debug.Log(ring.ToString() + " : " + cat.ToString());
        }
        //we're selected now
        myMenu.selected = this;
        //store defaults

        //if we're an inner ring button, spawn the outer ring
        if (ring == 1 && subMenu == null)
        {
            subMenu = RadialMenuSpawner.ins.SpawnMenu(p, 2, cat);
            //Debug.Log("Spawn Sub-Menu " + cat.ToString());
        }
    }

    void DeSelect()
    {
        //revert to defaults
        //Debug.Log(subMenu.ourRing.ToString());
        for(int i = subMenu.transform.childCount-1; i >=0; i--)
        {
            Destroy(subMenu.transform.GetChild(i).gameObject);
        }
        Destroy(subMenu);
    }

    public void Act()
    {
        p.PerformAction(new PlayerAction(cat, index));
    }

    //Coroutine for selection animation
    //Coroutine for deselection animation
    //Coroutine for selected animation
}
