using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDoor : DoorController
{
    public float amplitude;
    public float period;

    void Awake()
    {
        // base.Start();

        amplitude = 4f;
        period = 2.5f;
    }


    void Update()
    {
        if (doorMoving)
        {
            Vector3 tmpPosition = transform.position;
            tmpPosition.z = DoorPosition(Time.time - timeOffset + period / 4);
            transform.position = tmpPosition;
        }
    }

    private float DoorPosition(float time)
    {
        // Moviendo una puerta con función PingPong
        // PingPong(t,1) * amplitudMovimto - amplitudMovimto/2;
        // Con un período de 2.5s
        // PingPong(2*t/period, 1)

        // return Mathf.PingPong( 2 * time / period, 1 ) * amplitude - amplitude/2;

        // Mathf.SmoothStep(float from, float to, float t);
        float from = -amplitude / 2;
        float to = amplitude / 2;
        float t = Mathf.PingPong(2 * time / period, 1);

        return Mathf.SmoothStep(from, to, t);
    }
}
