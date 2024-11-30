using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZoneMovement : MonoBehaviour
{
    public enum MovementType { Linear, Bezier }
    public MovementType movementType;

    // Shared points for all movement types
    public Transform pointA;
    public Transform pointB;

    // Control point for Bezier movement
    public Transform controlPoint;

    // Movement properties
    public float speed = 2f;        // For linear movement
    public float duration = 2f;     // For Bezier

    private bool movingToB = true;  // Used for linear
    private float t = 0f;           // For Bezier movement
    private bool movingForward = true;


    private void Update()
    {
        switch (movementType)
        {
            case MovementType.Linear:
                HandleLinearMovement();
                break;
            case MovementType.Bezier:
                HandleBezierMovement();
                break;
        }
    }

    private void HandleLinearMovement()
    {
        if (movingToB)
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.1f) movingToB = false;
        if (Vector3.Distance(transform.position, pointA.position) < 0.1f) movingToB = true;
    }

    private void HandleBezierMovement()
    {
        if (controlPoint == null)
        {
            Debug.LogError("Control Point is not assigned for Bezier movement.");
            return;
        }

        t += (movingForward ? 1 : -1) * Time.deltaTime / duration;
        t = Mathf.Clamp01(t);

        // Calculate Bezier curve points
        Vector3 p0 = Vector3.Lerp(pointA.position, controlPoint.position, t);
        Vector3 p1 = Vector3.Lerp(controlPoint.position, pointB.position, t);
        transform.position = Vector3.Lerp(p0, p1, t);

        if (t == 1f) movingForward = false;
        if (t == 0f) movingForward = true;
    }
}