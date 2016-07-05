using UnityEngine;
using System.Collections;

public class SpellEffectBase : MonoBehaviour
{
    Camera mainCam;
	// Use this for initialization
    void Start()
    {
        StartCoroutine("Run");
    }

	public void Begin ()
    {
        mainCam = Camera.main;
        mainCam.enabled = false;
	}

    public IEnumerator Run ()
    {
        Begin();
        //in between here would run all the necessary particles and animations as well as all necessary actions
        yield return null;
        End();
    }
	
	public void End()
    {
        mainCam.enabled = true;
        Destroy(this.gameObject);
    }
}
