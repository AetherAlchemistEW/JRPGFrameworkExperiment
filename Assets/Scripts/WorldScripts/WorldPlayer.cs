using UnityEngine;
using System.Collections;

public class WorldPlayer : WorldAgent
{
    //Movement variables
    Vector2 axisMove;
    public float moveSpeed;
    public LayerMask terrainLayer;
    public LayerMask playerLayer;

    void Update()
    {
        //update axis values
        axisMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //run Movement controls if sqrMagnitude > 0
        if(axisMove.sqrMagnitude > 0)
        {
            Debug.Log("Moving");
            Movement();
        }
    }

    void Movement()
    {
        Vector3 projectionPoint = transform.position + new Vector3(axisMove.x, 0, axisMove.y);
        Vector3 movePoint = transform.position;
        //cast ray from 10 units above player over calcPosition to get y for movePoint 
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(projectionPoint + new Vector3(0, 10, 0), Vector3.down * 10, Color.green);
        if(Physics.Raycast(projectionPoint + new Vector3(0,10,0), Vector3.down, out hit, 20f, terrainLayer))
        {
            Debug.DrawLine(hit.point + new Vector3(0,1,0), transform.position, Color.red);
            Vector3 fromToVector = (hit.point - transform.position).normalized;
            Vector3 fromToFlat = (new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position).normalized;
            if (!Physics.Linecast(hit.point + new Vector3(0, 1, 0), transform.position, ~playerLayer) && Vector3.Dot(fromToVector, fromToFlat) > 0.3f)
            {
                Debug.Log("No blockage");
                //check line between player and that point for collisions, if there's not, initiate movement to that point
                movePoint = hit.point + new Vector3(0, 1, 0);
            }
        }

        transform.LookAt(new Vector3(movePoint.x, transform.position.y, movePoint.z));
        transform.Translate((movePoint - transform.position).normalized * moveSpeed * Time.smoothDeltaTime, Space.World);
    }
}
