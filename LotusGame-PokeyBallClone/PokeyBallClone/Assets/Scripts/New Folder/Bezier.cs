using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] points; // count must be 3.
    private int numPoints = 15;
    private Vector3[] positions = new Vector3[50];
    InputHandler InputHandler;

    void Start()
    {
        lineRenderer.positionCount = numPoints;
        InputHandler = FindObjectOfType<InputHandler>();
    }

    void Update()
    {
        Stretch();
        DrawQuadraticCurve();
    }

    private void DrawQuadraticCurve()
    {
        lineRenderer.enabled = true;
        if (InputHandler.grabbed)
        {
            for (int i = 1; i < numPoints + 1; i++)
            {
                float t = i / (float)numPoints;
                positions[i - 1] = CalculateQuadraticBezierPoint(t, points[0].position, points[1].position, points[2].position);
            }
            lineRenderer.SetPositions(positions);
        }      
        else
        {
            StartCoroutine(NoneDraw());
            //lineRenderer.enabled = false;
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //B(t) = (1-t)2 P0 + 2(1-t)tP1 + t2P2 , 0 < t < 1
        //         uu           u        tt
        //         uu*p0   + 2 *u*t*p1 + tt*p2

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    void Stretch()
    {
        points[0].position = InputHandler.ball.transform.position;
        points[1].position = new Vector3(points[1].position.x, points[2].position.y - 0.2f * (points[2].position.y - points[0].position.y), points[1].position.z); // point[1]in esnekliğini 0.2f veriyo.

        if (!InputHandler.grabbed)
        {
            points[2].position = new Vector3(points[2].position.x, InputHandler.ball.transform.position.y, points[2].position.z);
        }
    }
    IEnumerator NoneDraw()
    {
        
           
            lineRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
        
    }
}
