using UnityEngine;
using System.Collections;

//Functional but requires some balancing and tweaking
public class BattleCamera : MonoBehaviour
{
    #region Variables
    //VARIABLE
    public Transform activeAgent;
    public Transform targetAgent;

    [Range(0.1f, 25)]
    public float defaultDistance;
    [Range(0.1f, 10)]
    public float trackingDistance;

    [Range(0.1f, 1f)]
    public float rotateSpeed;
    [Range(0.1f, 0.5f)]
    public float trackSpeed;

    Vector3 aimPoint;
    Vector3 intendedPosition;

    Vector3 lDebug;
    Vector3 rDebug; 
    #endregion

    void Start()
    {
        //StartCoroutine("StandardConfig");
    }

    public IEnumerator FollowConfig()
    {
        if (activeAgent != null && targetAgent != null)
        {
            //move behind active agent facing the point between active and target agents, move in closer
            aimPoint = DetermineAimPoint(activeAgent.position, targetAgent.position);
            int trackingSide = activeAgent.GetComponent<BattleAgent>().character.isPlayer ? 1 : -1;
            intendedPosition = activeAgent.position + new Vector3(trackingDistance, 1, trackingSide);
        }
        StartCoroutine("Track");
        yield return null;
    }

    public IEnumerator StandardConfig()
    {
        //reset
        Vector3 leftSide = new Vector3();
        Vector3 rightSide = new Vector3();
        int players = 0;
        int enemies = 0;
        foreach (BattleAgent b in FindObjectsOfType<BattleAgent>())
        {
            if (b.character.isPlayer)
            {
                leftSide += b.transform.position;
                players++;
            }
            else
            {
                rightSide += b.transform.position;
                enemies++;
            }
        }
        //get all players, average their positions
        //get all non players, average their positions
        leftSide = leftSide / players;
        rightSide = rightSide / enemies;
        lDebug = leftSide;
        rDebug = rightSide;
        //aimpoint
        aimPoint = DetermineAimPoint(leftSide, rightSide);
        intendedPosition = aimPoint + new Vector3(0, -1, defaultDistance);

        //Debug.Log(aimPoint);
        //Debug.Log(intendedPosition);
        StartCoroutine("Track");
        yield return null;
    }

    IEnumerator Track()
    {
        Vector3 targOrientation = (aimPoint - transform.position).normalized;
        while (Vector3.Distance(transform.position, intendedPosition) > 0.1f || Vector3.Angle(transform.rotation.eulerAngles, targOrientation) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position, intendedPosition, Time.smoothDeltaTime * trackSpeed);
            targOrientation = (aimPoint - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.TransformDirection(Vector3.forward), targOrientation, Time.smoothDeltaTime * rotateSpeed), Vector3.up);
            yield return null;
        }
        Debug.Log("Ended");
        yield return null;
    }

    Vector3 DetermineAimPoint(Vector3 a, Vector3 b)
    {
        Vector3 result = new Vector3();
        float dist = Vector3.Distance(a, b);
        Vector3 vec = (b-a).normalized;
        result = a+vec * (dist /2);
        return result;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aimPoint, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lDebug, 1f);
        Gizmos.DrawWireSphere(rDebug, 1f);
    }
}
