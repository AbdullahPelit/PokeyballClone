using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool canGrab = false, grabbed = false;
    public float mousePos_Y, initPos_Y;
    float force = 0, forceMultiplier = 5;
    Vector3 initTrPos;
    public LineRenderer lr;
    public Vector3 TutNokta;
    public SpringJoint joint;
    public LayerMask Tutunulabilir;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    void FixedUpdate()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100, Tutunulabilir))
            {
                TutNokta = hit.point;
                
                Debug.DrawRay(transform.position, Vector3.forward * 10f, Color.yellow);
                if (hit.collider.name == "Kule")
                {
                    canGrab = true;





                }
                else canGrab = false;
            }
        }
       
    }

    void Update()
    {
        mousePos_Y = Input.mousePosition.y;
        if (canGrab && Input.GetMouseButtonDown(0) && !grabbed)
        {
            grabbed = true;
            initPos_Y = Input.mousePosition.y;
            initTrPos = transform.position;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, TutNokta);
            transform.LookAt(TutNokta);
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = TutNokta;
            float TutMesafe = Vector3.Distance(transform.position, TutNokta);
            joint.maxDistance = TutMesafe * 2f;
            joint.minDistance = 0;
            joint.spring = 6f;
            joint.damper = 8f;
            joint.massScale = 4f;
            lr.positionCount = 2;
            Debug.Log("tuttum");
            
        }
        if (Input.GetMouseButton(0))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            if (initTrPos.y - transform.position.y < 1)
            {
                force = initPos_Y - mousePos_Y;
                transform.position = new Vector3(transform.position.x, transform.position.y - force / 10000, transform.position.z);
            }
        }
        else GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        if (!canGrab)
        {

        }

        if (grabbed && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Bıraktım");
            GetComponent<Rigidbody>().AddForce(Vector3.up * force * forceMultiplier);
            force = 0;
            grabbed = false;
        }
    }
}
