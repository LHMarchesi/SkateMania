using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickHandler : MonoBehaviour
{
    [SerializeField] private GameObject skate;
    [SerializeField] private float ollieJumpForce;
    [SerializeField] private float highOllieJumpForce;
    [SerializeField] private float flipSpeed;

    private PlayerInput input;
    private Rigidbody rb;
    private Quaternion lastRotation;

    private float ollieRotationSpeed = 170; // Speed of the tilt
    private float tiltOllie = 30;  // Max tilt angle for the ollie
    private float ollieResetSpeed = 3; // Speed to reset rotation after jump


    private bool isTrickInProgress;
    public bool IsTrickInProgress { get => isTrickInProgress; set => isTrickInProgress = value; }

    void Awake()
    {
        rb = skate.GetComponent<Rigidbody>();
        input = skate.GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (input.IsJumping && !isTrickInProgress)
        {
            StartCoroutine(PerformTrick());
        }
    }

    IEnumerator PerformTrick()
    {
        isTrickInProgress = true;  // Mark that a trick is in progress
        input.SetJumping(true);
        SkateController skateController = skate.GetComponent<SkateController>();

        yield return StartCoroutine(DoOllie(skateController));

        input.SetJumping(false);
        isTrickInProgress = false;  // Reset after trick is done

    }

    IEnumerator DoOllie(SkateController skateControler)
    {
        Quaternion initialRotation = rb.rotation;
        float currentRotationX = 0f;

        // Rotar la tabla para hacer un Ollie
        while (currentRotationX < tiltOllie)
        {
            currentRotationX += ollieRotationSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(-currentRotationX, 0, 0));
            yield return null;
        }

       // yield return StartCoroutine(WaitForTrickInput());
    }

    IEnumerator WaitForTrickInput()
    {
        bool trickPerformed = false;

        // Esperar hasta que el jugador presione una tecla para hacer un truco
        while (!trickPerformed)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                // yield return StartCoroutine(DoKickflip());
                trickPerformed = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                //  yield return StartCoroutine(DoHeelflip());
                trickPerformed = true;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                yield return StartCoroutine(DoHighOllie());
                trickPerformed = true;
            }

            yield return null; // Espera un frame
        }
    }

    IEnumerator DoKickflip()
    {
        Debug.Log("Flip");
        // Implementar la lógica para el kickflip (por ejemplo, rotación en el eje Z)
        float currentRotationZ = 0f;
        Quaternion initialRotation = rb.rotation;
        rb.AddForce(Vector3.up * ollieJumpForce, ForceMode.Impulse);
        while (currentRotationZ < 360)
        {
            currentRotationZ += flipSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(0, 0, currentRotationZ));
            yield return null;
        }
    }

    IEnumerator DoHeelflip()
    {
        // Implementar la lógica para el heelflip (rotación en el eje Z inverso)
        float currentRotationZ = 0f;
        Quaternion initialRotation = rb.rotation;

        while (currentRotationZ > -360)
        {
            currentRotationZ -= flipSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(0, 0, currentRotationZ));
            yield return null;
        }
    }

    IEnumerator DoHighOllie()
    {
        rb.AddForce(Vector3.up * highOllieJumpForce, ForceMode.Impulse);
        yield return null;
    }
}
