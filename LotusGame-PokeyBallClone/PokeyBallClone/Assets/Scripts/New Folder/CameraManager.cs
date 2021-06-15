using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    public Transform target;
    InputHandler InputHandler;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;

    
    private void FixedUpdate()
    {

         //transform.position = target.position;
         Vector3 desiredPos = target.position + offset;
         Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
         transform.position = smoothedPos;

         transform.LookAt(target);
       
    }
}
