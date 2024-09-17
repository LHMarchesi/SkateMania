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

    private float ollieRotationSpeed = 170; // Speed of the tilt
    private float tiltOllie = 30;  // Max tilt angle for the ollie


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
        rb.AddForce(Vector3.up * ollieJumpForce, ForceMode.Impulse);
        Quaternion initialRotation = rb.rotation;
        float currentRotationX = 0f;
        //Rotar la tabla para hacer un Ollie

        while (currentRotationX < tiltOllie)
        {
            rb.freezeRotation = true;
            currentRotationX += ollieRotationSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(-currentRotationX, 0, 0));
            yield return null;
        }
        rb.freezeRotation = false;

        yield return StartCoroutine(WaitForTrickInput());

    }

    IEnumerator WaitForTrickInput()
    {
        bool trickPerformed = false;

        // Esperar hasta que el jugador presione una tecla para hacer un truco
        while (!trickPerformed)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                yield return StartCoroutine(DoKickflip());
                trickPerformed = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                yield return StartCoroutine(DoHeelflip());
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
        rb.freezeRotation = true;
        Quaternion initialRotation = rb.rotation;

        float currentRotationZ = 0f;
        float currentRotationX = 0f;

        rb.AddForce(Vector3.up * highOllieJumpForce, ForceMode.Impulse);
        while (currentRotationZ > -360f)
        {
            currentRotationZ -= flipSpeed * Time.deltaTime;
            if (currentRotationX < 40f)
            {
                currentRotationX += ollieRotationSpeed * Time.deltaTime;
            }

            rb.MoveRotation(initialRotation * Quaternion.Euler(currentRotationX, 0, currentRotationZ));
            yield return null;
        }
        rb.freezeRotation = false;
        yield return null;
    }

    IEnumerator DoHeelflip()
    {
        Debug.Log("HeelFlip");
        rb.freezeRotation = true;
        Quaternion initialRotation = rb.rotation;

        float currentRotationZ = 0f;
        float currentRotationX = 0f;

        rb.AddForce(Vector3.up * highOllieJumpForce, ForceMode.Impulse);
        while (currentRotationZ < 360f)
        {
            currentRotationZ += flipSpeed * Time.deltaTime;
            if (currentRotationX < 40f)
            {
                currentRotationX += ollieRotationSpeed * Time.deltaTime;
            }

            rb.MoveRotation(initialRotation * Quaternion.Euler(currentRotationX, 0, currentRotationZ));
            yield return null;
        }
        rb.freezeRotation = false;
        yield return null;
    }

    IEnumerator DoHighOllie()
    {
        rb.AddForce(Vector3.up * highOllieJumpForce, ForceMode.Impulse);
        Quaternion initialRotation = rb.rotation;
        Debug.Log("HighOllie");

        float currentRotationX = 0f;
        while (currentRotationX < 40f)
        {
            rb.freezeRotation = true;
            currentRotationX += ollieRotationSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(currentRotationX, 0, 0));
            yield return null;
        }

        rb.freezeRotation = false;
        yield return null;
    }
}
