using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    PresenceDetector presenceDetector;

    [SerializeField]
    List<string> activeTags;

    protected bool doorMoving;

    protected float timeOffset;

    float timeOut;

    protected virtual void Start()
    {
        doorMoving = false;
        timeOffset = 0;
        timeOut = 0;

        if (presenceDetector == null) { return; }

        else
        {
            // script.delegate de detección de entrada.
            // Cuando salta el aviso, el delegate envía sus parámetros
            presenceDetector.OnGameObjectDetectedEnter += OnDetectedEnter;

            // script.delegate de detección de salida.
            // Cuando salta el aviso, el delegate envía sus parámetros
            presenceDetector.OnGameObjectDetectedExit += OnDetectedExit;
        }
    }


    protected virtual void OnDetectedEnter(string detectorName, GameObject other)
    {
        if (!activeTags.Contains(other.tag)) { return; }

        // print($"DoorControllerCs.OnObjectEntered in '{zoneDetectorTag}'");

        doorMoving = true;
        timeOffset += Time.time - timeOut;
    }

    protected virtual void OnDetectedExit(string detectorName, GameObject other)
    {
        if (activeTags.Contains(other.tag))
        {
            // print($"DoorControllerCs.OnObjectExited of '{zoneDetectorTag}'");

            doorMoving = false;
            timeOut = Time.time;
        }
    }
}
