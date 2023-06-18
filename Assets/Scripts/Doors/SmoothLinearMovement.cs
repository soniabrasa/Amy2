using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLinearMovement : MonoBehaviour
{
    public static SmoothLinearMovement instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }


    // Mediante Vector3.Lerp y SmoothStep, resulta un movimento infinito entre
    // A y B, tardando period segundos en ida y vuelta
    public Vector3 SmoothLerp(Vector3 a, Vector3 b, float period)
    {
        // public static Vector3 Lerp(Vector3 a, Vector3 b, float t);
        // @return Vector3 Interpolated value, equals to a + (b - a) * t.

        // Cuando t= 0, Vector3.Lerp(a, b, t) devuelve a.
        // Cuando t= 1, Vector3.Lerp(a, b, t) devuelve b.
        // Cuando t= 0,5, Vector3.Lerp(a, b, t) devuelve el punto medio entre a y b.

        float t = NormalizedSmoothStep(Time.time / period);

        return Vector3.Lerp(a, b, t);
    }

    // Función SmoothStep con periodo 1 y que devuelve un valor entre 0 y 1
    public float NormalizedSmoothStep(float seconds)
    {
        float value = NormalizedPingPong(seconds);
        float min = 0f;
        float max = 1f;

        // public static float Clamp(float value, float min, float max);
        // @return float entre min y max

        float myClamp = Mathf.Clamp(value, min, max);

        // public static float SmoothStep(float from, float to, float t);
        // Interpolado entre from y to, con suavizado en los límites.

        float from = 0f;
        float to = 1f;
        float t = myClamp;

        return Mathf.SmoothStep(from, to, t);
    }


    // Método propio PingPong normalizado a lenght 1 (período 1) 
    // @return float [0 - 1]
    public float NormalizedPingPong(float seconds)
    {
        // PingPong precisa que t sea un valor autoincremental, p.ej., Time.time o Time.unscaledTime.
        // @return float [0 - lenght]

        // public static float PingPong(float t, float length = 2f);

        float t = seconds * 2;
        float lenght = 1f;

        return Mathf.PingPong(t, lenght);
    }
}
