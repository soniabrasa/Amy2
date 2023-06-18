using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : DoorController2
{
    [Header("Axis")]
    [SerializeField]
    public DoorAxis doorAxis = DoorAxis.Up;

    [SerializeField]
    float amplitude;

    [SerializeField]
    float period;

    float t;

    Vector3 startPoint, endPoint;

    protected override void Start()
    {
        base.Start();

        amplitude = 2f;
        period = 1.8f;

        startPoint = transform.position;
        endPoint = startPoint + GetDoorAxisVector() * amplitude;
    }


    void Update()
    {
        if (doorMoving)
        {
            elapsedTime += Time.deltaTime;

            t = Mathf.SmoothStep(0f, 1f, elapsedTime / period);

            if (t >= 1f) { doorMoving = false; }

            if (openDoor)
            {
                transform.position = Vector3.Lerp(startPoint, endPoint, t);
            }

            else
            {
                transform.position = Vector3.Lerp(endPoint, startPoint, t);
            }
        }
    }

    Vector3 GetDoorAxisVector()
    {
        switch (doorAxis)
        {
            case DoorAxis.Up:
                return Vector3.up;

            case DoorAxis.Down:
                return Vector3.down;

            case DoorAxis.Left:
                return Vector3.left;

            case DoorAxis.Right:
                return Vector3.right;

            case DoorAxis.Forward:
                return Vector3.forward;

            case DoorAxis.Back:
                return Vector3.back;

            default:
                return Vector3.up;
        }
    }
}

public enum DoorAxis
{
    Up,
    Down,
    Left,
    Right,
    Forward,
    Back
}
