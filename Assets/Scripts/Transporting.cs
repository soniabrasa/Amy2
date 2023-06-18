using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporting : MonoBehaviour
{
    Transform objectTransform;

    float minPickUp, maxPickUp, aire;

    bool isUp;

    void Start()
    {
        isUp = false;

        // Posiciones de carga relativa al Player
        minPickUp = 0.8f;
        maxPickUp = 1.2f;

        // Espacio entre el Player y el objeto
        aire = 0.4f;
    }


    void Update()
    {
        // des/Cargar objectos pickables
        if (Input.GetButtonDown("Interaction") && PlayerCs.instance.GroundedPlayer)
        {
            Debug.Log($"{gameObject.name} Iteraction Button");

            // Alternancia Interaction soltar
            if (objectTransform != null)
            {
                Liberate(true);
            }

            // Alternancia Interaction buscar object y cargar
            else
            {
                Transport();
            }
        }

        // if (Input.GetKey(KeyCode.V))
        if (Input.GetButton("PickUp"))
        {
            Debug.Log($"{gameObject.name} Button PickUp {objectTransform.gameObject.name}");
            PickUp(1);
        }

        // if (Input.GetKey(KeyCode.B))
        if (Input.GetButton("PickDown"))
        {
            Debug.Log($"{gameObject.name} Button PickDown {objectTransform.gameObject.name}");
            PickUp(-1);
        }

        // No podrá caminar ni saltar on el objeto en Up

    }

    void Liberate(bool liberate)
    {
        // Liberamos el objeto
        if (liberate)
        {
            objectTransform.GetComponent<Rigidbody>().isKinematic = false;
            objectTransform.parent = null;
            objectTransform = null;
            isUp = false;
            PlayerCs.instance.ObjectInUp = isUp;
        }

        // Cargamos el objeto al Player
        else
        {
            objectTransform.parent = transform;
            // Para que no le afecten las físicas como la Gravedad
            objectTransform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void PickUp(float yDirection)
    {
        if (objectTransform == null) { return; }

        Vector3 direction = new Vector3(0, yDirection, 0);

        Debug.Log($"{gameObject.name}.PickUp({yDirection}) Local Position {objectTransform.localPosition}");

        // Pick up
        if (
            (yDirection > 0 && objectTransform.localPosition.y <= maxPickUp)
            || (yDirection < 0 && objectTransform.localPosition.y >= minPickUp)
        )
        {
            objectTransform.Translate(direction * Time.deltaTime);
        }

        else
        {
            Debug.Log($"{objectTransform.gameObject.name} No se puede levantar o bajar más");
        }

        // Booleano para enviar al Player si el objeto está elevado
        isUp = (objectTransform.localPosition.y > minPickUp) ? true : false;
        PlayerCs.instance.ObjectInUp = isUp;
    }

    void Transport()
    {
        // Buscar
        // CheckForPickables @return hit.collider.transform;
        objectTransform = CheckForPickables();

        if (objectTransform != null)
        {
            Liberate(false);

            objectTransform.localPosition = new Vector3(0, minPickUp, aire);
            objectTransform.localRotation = Quaternion.identity;
        }

        else
        {
            Debug.Log($"{gameObject.name}.PickUp Not Found");
        }
    }

    // Busca de elementos pickables a 2 alturas
    // @ return Transform hit
    Transform CheckForPickables()
    {
        RaycastHit hit;
        float h;

        // Tampoco se puede empujar si Amy está en el aire
        // if (!groundedPlayer || _displacement == Vector3.zero) { return; }

        // Raycast sobrecargado con out in
        // bool Raycast(Vector3 origin, Vector3 direction,
        // out RaycastHit hitInfo, float maxDistance,
        // int layerMask, QueryTriggerInteraction queryTriggerInteraction);

        // Prioridad 1

        h = 0.7f;

        Vector3 origin = transform.position
            + transform.up * h
            + transform.forward * 0.1f;
        Vector3 direction = transform.forward;
        float maxDistance = 0.5f;
        // int layerMask;


        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            // Debug.Log($"{gameObject.name}.CheckForPushables Hit {hit.collider.gameObject.name}");

            if (hit.collider.gameObject.CompareTag("Pickable"))
            {
                return hit.collider.transform;
            }
        }

        // Para ver el Raycast en modo play en la pestaña Scene (Gizmos)
        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        // Prioridad objeto más bajo
        h = 0.35f;

        origin = transform.position
            + transform.up * h
            + transform.forward * 0.1f;

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            // Debug.Log($"{gameObject.name}.CheckForPushables Hit {hit.collider.gameObject.name}");

            if (hit.collider.gameObject.CompareTag("Pickable"))
            {
                return hit.collider.transform;
            }
        }

        // Para ver el Raycast en modo play en la pestaña Scene (Gizmos)
        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        return null;
    }
}
