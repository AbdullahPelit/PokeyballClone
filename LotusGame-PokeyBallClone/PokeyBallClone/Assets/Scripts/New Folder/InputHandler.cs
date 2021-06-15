using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour, IEndGameObserver
{
    public GameObject ball;
    public bool canGrab = false, grabbed = false, isBlock = false;
    float mousePos_Y, initPos_Y, amountOfStretch = 2f;
    float force = 0, forceMultiplier = 15f;
    Vector3 initTrPos;
    private GameManager gameManager;
    [SerializeField] ParticleSystem confettiParticle;
    ParticleSystem confettiP;
    RaycastHit hit;


    AudioSource audioSource;
    [SerializeField] AudioClip impactSound;

   
    
    public void GameStart()
    {
        ball.GetComponent<Rigidbody>().useGravity = true;
        audioSource = GetComponent<AudioSource>();
        GameEvents.m_MyDetectionEvent.AddListener(Detect);        
        GameEvents.m_MyUpdateEvent.AddListener(BallBehaviour);
    }

   

    public void Detect()
    {
         
        //RaycastHit hit;

        if (Physics.Raycast(ball.transform.position + new Vector3(0, 0, 1), new Vector3(0, 0, 10), out hit, 10f))
        {
            Debug.DrawRay(ball.transform.position + new Vector3(0, 0, 1), new Vector3(0, 0, 10), Color.yellow);
            if (hit.collider.CompareTag("Tower"))
            {
                canGrab = true;
                isBlock = false;
            }
            if (hit.collider.CompareTag("Block"))
            {
                canGrab = false;
                isBlock = true;
            }
            //else { canGrab = false;
            //       isBlock = false;
            //}
        }
        else canGrab = false;
        
    }
   

    void BallBehaviour()
    {
        if (canGrab && Input.GetMouseButtonDown(0) && !grabbed)
        {
            grabbed = true;
            initPos_Y = Input.mousePosition.y;
            initTrPos = ball.transform.position;
            
            audioSource.PlayOneShot(impactSound, 1);                        
        }
        if (grabbed && Input.GetMouseButton(0))
        {
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            ball.GetComponent<Rigidbody>().useGravity = false;

            float delta_Y = initTrPos.y - ball.transform.position.y;
            if (delta_Y < amountOfStretch && delta_Y >= 0)
            {
                audioSource.volume = 1;
                mousePos_Y = Input.mousePosition.y;
                force = initPos_Y - mousePos_Y;
                ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y - force / 10000, ball.transform.position.z);
                ball.transform.Rotate(force * -0.5f * Time.deltaTime, 0, 0);

            }
            if (delta_Y >= amountOfStretch) { delta_Y = amountOfStretch; audioSource.volume = 0; }
            if (delta_Y <= -amountOfStretch) { delta_Y = amountOfStretch; audioSource.volume = 0; }
        }
        else ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                
        

        if (!canGrab)
        {
            // Debug.Log("Loser");
            //line rebder enable false yap
        }

        if (grabbed && Input.GetMouseButtonUp(0) && initPos_Y != mousePos_Y)
        {
            audioSource.volume = 0;
            
            ForceBall();
        }

        if (!grabbed) // Serbest
        {
            ball.transform.Rotate(ball.GetComponent<Rigidbody>().velocity.y * 360 * Time.deltaTime, 0, 0);
        }
       
    }
    public void Notify()
    {
        // When the game ends..
        ball.GetComponent<Rigidbody>().useGravity = false;
        confettiP = Instantiate(confettiParticle, ball.transform.position, Quaternion.Euler(-90, 0, 0));
        StartCoroutine(Confetties());
    }
    IEnumerator Confetties()
    {
        while (true)
        {
            confettiP.transform.position = ball.transform.position;
            confettiP.Play();
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void ForceBall()
    {
        
            ball.GetComponent<Rigidbody>().AddForce(Vector3.up * force * forceMultiplier);
            force = 0;
            ball.GetComponent<Rigidbody>().useGravity = true;
            grabbed = false;
        
        
    }
   
    
}
