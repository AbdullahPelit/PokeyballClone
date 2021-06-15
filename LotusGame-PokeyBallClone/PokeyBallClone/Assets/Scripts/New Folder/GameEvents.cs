using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static UnityEvent m_MyDetectionEvent, m_MyUpdateEvent;

    void Start()
    {
        if (m_MyDetectionEvent == null)
            m_MyDetectionEvent = new UnityEvent();
            
        if (m_MyUpdateEvent == null)
            m_MyUpdateEvent = new UnityEvent();
    }

    void FixedUpdate()
    {
        m_MyDetectionEvent.Invoke();
    }

    void Update()
    {
        m_MyUpdateEvent.Invoke();
    }
}