using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCs : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;
    bool groundedPlayer, objectInUp;
    float playerSpeed, jumpHeight, gravityValue;

    Animator animator;
    PlayerState state;

    // Transform Main Camera
    Transform cameraTrasform;

    // Telepor new Scene
    Vector3 teleportPoint;
    bool teleport;

    public static PlayerCs instance;

    public bool GroundedPlayer { get { return groundedPlayer; } }
    public bool ObjectInUp
    {
        get { return objectInUp; }
        set { objectInUp = value; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }


    void Start()
    {
        playerSpeed = 2.0f;
        jumpHeight = 1.0f;
        gravityValue = -9.81f;
        playerVelocity = Vector3.zero;

        // gameObject.AddComponent<CharacterController>();
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        // El Animator se inicia con la animación Idle
        SetState(PlayerState.Idle);

        cameraTrasform = Camera.main.transform;
    }

    void Update()
    {
        // Go to New Scene
        if (teleport) { Teleport(); }
        if (ManagerScene.instance.FullLevel) { return; }

        // CharacterController.isGrounded
        groundedPlayer = controller.isGrounded;

        // CharacterController.Move Vector3 move example
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        // Transformamos el movimiento en la direccion de la cámara
        move = cameraTrasform.forward * move.z + cameraTrasform.right * move.x;
        move.y = 0;
        move = move.normalized;

        // CharacterController.Move
        controller.Move(move * Time.deltaTime * playerSpeed);

        // CharacterController.Move Vector3 move example
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Con un objeto elevado no se puede mover ni saltar
        if (objectInUp)
        {
            move = Vector3.zero;
            SetState(PlayerState.Idle);
        }
        else
        {
            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                // playerVelocity.y += Mathf.Sqrt( jumpHeight * -3.0f * gravityValue );
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

                // Pasamos a la animación de salto
                SetState(PlayerState.Jump);
            }

            // Si, después de saltar, tocamos suelo
            // Decidimos si pasamos a Idle o Run
            else if (groundedPlayer)
            {
                if (move == Vector3.zero)
                {
                    SetState(PlayerState.Idle);
                }
                else
                {
                    SetState(PlayerState.Run);
                }
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        // Comenzamos a caer después del punto máximo de subida del Jump
        if (playerVelocity.y < 0 && !groundedPlayer)
        {
            SetState(PlayerState.Fall);
        }

        // Vector en la misma dirección y distancia del propio Player
        Vector3 displacement = move * playerSpeed * Time.deltaTime;

        // Enviamos el vector de desplazamiento a los GO pushables
        CheckForPushables(displacement);

        // CharacterController.Move
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // // Busca de elementos pushables y envío del vector desplazamiento
    void CheckForPushables(Vector3 _displacement)
    {
        // Tampoco se puede empujar si Amy está en el aire
        if (!groundedPlayer || _displacement == Vector3.zero) { return; }

        // Raycast sobrecargado con out in
        // bool Raycast(Vector3 origin, Vector3 direction,
        // out RaycastHit hitInfo, float maxDistance,
        // int layerMask, QueryTriggerInteraction queryTriggerInteraction);

        Vector3 origin = transform.position
            + transform.up * 1.1f
            + transform.forward * 0.1f;
        Vector3 direction = transform.forward;
        float maxDistance = 0.5f;
        // int layerMask;

        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            // Debug.Log($"{gameObject.name}.CheckForPushables Hit {hit.collider.gameObject.name}");

            if (hit.collider.gameObject.CompareTag("Pushable"))
            {
                hit.collider.GetComponent<Pushable>().Push(_displacement);
                SetState(PlayerState.Push);
            }
        }

        // Para ver el Raycast en modo play en la pestaña Scene (Gizmos)
        Debug.DrawRay(origin, direction * maxDistance, Color.red);
    }

    void Teleport()
    {
        float distance = Vector3.Distance(teleportPoint, transform.position);

        Debug.Log($"{gameObject.name}.Teleport. Distance = {distance}");

        Vector3 move = (teleportPoint - transform.position) / distance;

        Vector3 displacement = move * playerSpeed * Time.deltaTime;

        controller.Move(displacement);

        if (distance < 0.08f)
        {
            teleport = false;
            SetState(PlayerState.Idle);
        }
    }

    public void ActivateTeleport(Vector3 toPoint)
    {
        Debug.Log($"{gameObject.name}.ActivateTeleport({toPoint})");

        teleportPoint = toPoint;
        teleport = true;
    }


    public void SetState(PlayerState newState)
    {
        if (state != newState)
        {
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Fall");
            animator.ResetTrigger("Push");

            state = newState;
            animator.SetTrigger($"{state}");
        }
    }

}

public enum PlayerState { Idle, Run, Jump, Fall, Push }
