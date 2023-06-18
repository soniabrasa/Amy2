using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoor : DoorController
{
    public float amplitude;
    public float period;

    Vector3 starPosition, endPosition;


    protected override void Start()
    {
        base.Start();

        amplitude = 2f;
        period = 1.8f;

        starPosition = transform.position;
        endPosition = starPosition + Vector3.up * amplitude;
    }

    void Update()
    {
        if (doorMoving)
        {
            // Vector3 tmpPosition = transform.position;
            // tmpPosition.y = DoorPosition(Time.time - timeOffset);
            // transform.position = tmpPosition;

            transform.position = SmoothLinearMovement.instance.SmoothLerp(
                starPosition,
                endPosition,
                period
            );
        }
    }



    float DoorPosition(float time)
    {
        // Mathf.SmoothStep(float from, float to, float t);
        float from = 0;
        float to = amplitude;
        float t = Mathf.PingPong(2 * time / period, 1);

        return Mathf.SmoothStep(from, to, t) + 1;
    }
}
