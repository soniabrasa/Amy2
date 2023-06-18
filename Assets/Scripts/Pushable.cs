using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    void Start()
    {
    }


    void Update()
    {
    }

    public void Push(Vector3 displacement)
    {
        // Debug.Log($"{gameObject.name}.Pushable({displacement})");

        // public void Translate(Vector3 translation, Space relativeTo = Space.Self);
        transform.Translate(displacement, Space.World);
    }
}
