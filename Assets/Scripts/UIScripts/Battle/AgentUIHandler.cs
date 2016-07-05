using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgentUIHandler : MonoBehaviour
{
    //VARIABLES
    //Health/Mana/ATB
    public Image healthBar;
    public Image manaBar;
    public Image atbBar;
    public Text healthText;
    public Text manaText;
    public Text atbText;

    public Color atbBaseColour;
    public Color atbActingColour;

    public BattleAgent myAgent;

    //Cursor Control
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = (Camera.main.WorldToScreenPoint(myAgent.transform.position) + (Vector3.down * 50));
	    //ATB functionality
        if(myAgent.atbCurrent < myAgent.atbMax)
        {
            atbText.text = Mathf.Round(myAgent.atbCurrent) + " / " + myAgent.atbMax;
            atbBar.fillAmount = myAgent.atbCurrent / myAgent.atbMax;
            atbBar.color = atbBaseColour;
        }
        else
        {
            atbText.text = "";
            atbBar.fillAmount = 1;
            atbBar.color = atbActingColour;
        }

        //Health functionality
        healthText.text = "HP: " + myAgent.character.health.value + " / " + myAgent.stats.maxHealth.value;
        healthBar.fillAmount = myAgent.character.health.value / myAgent.stats.maxHealth.value;

        //Mana functionality
        manaText.text = "MP: " + myAgent.character.mana.value + " / " + myAgent.stats.maxMana.value;
        manaBar.fillAmount = myAgent.character.mana.value / myAgent.stats.maxMana.value;
    }
}
