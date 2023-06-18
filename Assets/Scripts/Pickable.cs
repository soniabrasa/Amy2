using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    float pickedObjectSpeed = 1.5f;
    float pickedObjectLowPosition = 0.8f;
    float pickedObjectHighPosition = 1.2f;
    bool pickedObjectTooHigh = false;

    Transform pickedObject;


    void Start()
    {
    }


    void Update()
    {
        if (Input.GetButtonDown("Interaction") && PlayerCs.instance.GroundedPlayer)
        {
            PickObject();
        }

        if (Input.GetKey(KeyCode.R))
        // if (Input.GetButton("Explosion"))
        {
            MovePickedObject(Vector3.up);
        }

        if (Input.GetKey(KeyCode.F))
        // if (Input.GetButton("Force"))
        {
            MovePickedObject(Vector3.down);
        }


    }

    private void MovePickedObject(Vector3 direction)
    {
        if (pickedObject == null)
        {
            return;
        }
        if (direction.y < 0 && pickedObject.localPosition.y > pickedObjectLowPosition)
        {
            pickedObject.Translate(direction * pickedObjectSpeed * Time.deltaTime);
        }
        else if (direction.y > 0 && pickedObject.localPosition.y < pickedObjectHighPosition)
        {
            pickedObject.Translate(direction * pickedObjectSpeed * Time.deltaTime);
        }
        if (pickedObject.localPosition.y > pickedObjectLowPosition)
        {
            pickedObjectTooHigh = true;
        }
        else
        {
            pickedObjectTooHigh = false;
        }
    }

    private void PickObject()
    {
        if (pickedObject != null)
        {
            //Liberamos el objeto
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.parent = null;
            pickedObject = null;
            pickedObjectTooHigh = false;
        }
        else
        {
            // Miramos se temos un obxecto Pickeable ó alcance
            // SearchPickeabloObjet @return hit.collider.transform;

            pickedObject = SearchPickeableObject();
            //Si encontramos algo, lo cogemos
            if (pickedObject != null)
            {
                Debug.Log($"{gameObject.name}.PickObject Found {pickedObject.gameObject.name}");

                pickedObject.parent = transform;
                pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                pickedObject.localPosition = new Vector3(0, pickedObjectLowPosition, 0.4f);
                pickedObject.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.Log($"{gameObject.name}.PickObject Not Found");
            }
        }
    }

    private Transform SearchPickeableObject()
    {
        RaycastHit hit;

        // Altura del objeto
        float objectH = 0.7f;

        Vector3 origin = transform.position + 0.1f * transform.forward + objectH * transform.up;
        Vector3 direction = transform.forward;
        float maxDistance = 0.5f;

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            // Comprobamos si el objeto es Pickable
            if (hit.collider.gameObject.CompareTag("Pickable"))
            {
                return hit.collider.transform;
            }
        }

        // Objetos más abajo
        objectH = 0.35f;
        origin = transform.position + 0.1f * transform.forward + objectH * transform.up;

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            // Comprobamos si el objeto es Pickable
            if (hit.collider.gameObject.CompareTag("Pickable"))
            {
                return hit.collider.transform;
            }
        }

        return null;
    }

    public bool IsPickableObjectUp()
    {
        return pickedObjectTooHigh;
    }
}
