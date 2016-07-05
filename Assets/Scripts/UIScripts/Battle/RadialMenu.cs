using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[System.Serializable]
public struct Action
{
    [SerializeField]
    public Color color;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public string title;
}

public class RadialMenu : MonoBehaviour
{
    public RadialButton buttonPrefab;
    public RadialButton selected;
    public GameObject toolTip;
    public Action[] coreActions;
    public float distribution;
    public int ourRing;
    Text label;

    // Use this for initialization
    public void SpawnButtons(BattlePlayer obj, int ring, int cat)
    {
        ourRing = ring;

        if (ring == 1) //inner circle
        {
            for (int i = 0; i < coreActions.Length; i++)
            {
                RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
                newButton.transform.SetParent(transform, false);
                //Evenly distributed around circle
                float theta = (2 * Mathf.PI / distribution) * i;
                float xPos = Mathf.Sin(theta);
                float yPos = Mathf.Cos(theta);
                newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 100.0f;

                newButton.circle.color = coreActions[i].color;
                newButton.icon.sprite = coreActions[i].sprite;
                newButton.title = coreActions[i].title;
                newButton.myMenu = this;
                newButton.ring = ring;
                newButton.p = obj;
                newButton.cat = i;
            }
        }
        else if(ring == 2) //outer circle
        {
            //Spawn the label area
            GameObject l = Instantiate(toolTip) as GameObject;
            l.transform.SetParent(transform, false);
            l.transform.localPosition = new Vector3(0, 200, 0);
            label = l.GetComponent<Text>();

            switch (cat)
            {
                case 0:
                    for (int i = 0; i < obj.attacks.Count; i++)
                    {
                        RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
                        newButton.transform.SetParent(transform, false);
                        //Evenly distributed around circle
                        float theta = (2 * Mathf.PI / distribution) * i;
                        float xPos = Mathf.Sin(theta);
                        float yPos = Mathf.Cos(theta);
                        newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 150.0f;

                        newButton.circle.color = obj.attacks[i].uiColor;
                        newButton.icon.sprite = obj.attacks[i].uiIcon;
                        newButton.title = obj.attacks[i].title;
                        newButton.myMenu = this;
                        newButton.ring = ring;
                        newButton.p = obj;
                        newButton.cat = cat;
                        newButton.index = i;
                    }
                    break;
                case 1:
                    for (int i = 0; i < obj.abilities.Count; i++)
                    {
                        RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
                        newButton.transform.SetParent(transform, false);
                        //Evenly distributed around circle
                        float theta = (2 * Mathf.PI / distribution) * i;
                        float xPos = Mathf.Sin(theta);
                        float yPos = Mathf.Cos(theta);
                        newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 150.0f;

                        newButton.circle.color = obj.abilities[i].uiColor;
                        newButton.icon.sprite = obj.abilities[i].uiIcon;
                        newButton.title = obj.abilities[i].title;
                        newButton.myMenu = this;
                        newButton.ring = ring;
                        newButton.p = obj;
                        newButton.cat = cat;
                        newButton.index = i;
                    }
                    break;
                case 2:
                    for (int i = 0; i < obj.items.Count; i++)
                    {
                        RadialButton newButton = Instantiate(buttonPrefab) as RadialButton;
                        newButton.transform.SetParent(transform, false);
                        //Evenly distributed around circle
                        float theta = (2 * Mathf.PI / distribution) * i;
                        float xPos = Mathf.Sin(theta);
                        float yPos = Mathf.Cos(theta);
                        newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 150.0f;

                        newButton.circle.color = obj.items[i].uiColor;
                        newButton.icon.sprite = obj.items[i].uiIcon;
                        newButton.title = obj.items[i].title;
                        newButton.myMenu = this;
                        newButton.ring = ring;
                        newButton.p = obj;
                        newButton.cat = cat;
                        newButton.index = i;
                    }
                    break;
            }
        }
    }

    void Update()
    {
        //if we have a selected, pass its title to the text on the UI
        if(selected != null && label != null)
        {
            label.text = selected.title;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(selected && ourRing == 2)
            {
                //button action
                selected.Act();
            }
            Destroy(gameObject);
        }
    }
}
