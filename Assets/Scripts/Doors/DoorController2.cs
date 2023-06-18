using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController2 : MonoBehaviour
{
    [SerializeField]
    PresenceDetector presenceDetector;

    [SerializeField]
    List<string> activeTags;

    protected bool openDoor;

    protected bool doorMoving;

    protected float elapsedTime;


    protected virtual void Start()
    {
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


    void Update()
    {
    }

    protected virtual void OnDetectedEnter(string detectorName, GameObject other)
    {
        if (!activeTags.Contains(other.tag)) { return; }

        doorMoving = true;
        elapsedTime = 0f;
        openDoor = true;
    }

    protected virtual void OnDetectedExit(string detectorName, GameObject other)
    {
        if (activeTags.Contains(other.tag))
        {
            doorMoving = true;
            elapsedTime = 0f;
            openDoor = false;
        }
    }
}
