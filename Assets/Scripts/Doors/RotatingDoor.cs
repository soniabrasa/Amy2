using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : DoorController2
{
    [SerializeField] float rotateSpeed;


    // protected override void Start()
    // {
    //     rotateSpeed = 90f; // 90º/s
    // }


    void Update()
    {
        if (doorMoving)
        {
            // public void Rotate(Vector3 eulers, Space relativeTo = Space.Self);
            // transform.Rotate(transform.up * rotateSpeed * Time.deltaTime);

            // public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self);
            transform.Rotate(transform.up, rotateSpeed * Time.deltaTime);
        }
    }
}