using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"{gameObject.name}.OnTriggerEnter({other.gameObject.name})");

        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log($"\t Position {transform.position})");

            ManagerScene.instance.GoNewLevel(transform.position);
        }
    }
}
