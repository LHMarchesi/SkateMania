using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ollie : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject sktObj;
    [SerializeField] private float ollieRotationSpeed; // Speed of the tilt
    [SerializeField] private float maxOllieRotation;  // Max tilt angle for the ollie
    [SerializeField] private float ollieJumpForce;    // Jump force for the ollie
    [SerializeField] private float ollieResetSpeed;    // Speed to reset rotation after jump
    [SerializeField] private float flipSpeed;    // Speed to reset rotation after jump
    [SerializeField] private float highOllieJumpForce;    // Speed to reset rotation after jump

    private Quaternion lastRotation;
    private Rigidbody rb;
    void Start()
    {
        rb = sktObj.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (input.IsJumping)
        {
            StartCoroutine(PerformTrick());
        }
    }

    IEnumerator PerformTrick()
    {
        input.SetJumping(true);
        SkateController skateController = sktObj.GetComponent<SkateController>();

        yield return StartCoroutine(DoOllie(skateController));

        input.SetJumping(false);
    }

    IEnumerator DoOllie(SkateController skateControler)
    {
        skateControler.gravity = Vector3.zero;

        Quaternion initialRotation = rb.rotation;
        float currentRotationX = 0f;

        // Rotar la tabla para hacer un Ollie
        while (currentRotationX < maxOllieRotation)
        {
            currentRotationX += ollieRotationSpeed * Time.deltaTime;
            rb.MoveRotation(initialRotation * Quaternion.Euler(-currentRotationX, 0, 0));
            yield return null;
        }
        rb.AddForce(Vector3.up * ollieJumpForce, ForceMode.Impulse); // Apply the jump force after reaching the max tilt

        skateControler.gravity = new(0, -300f, 0);

        yield return StartCoroutine(WaitForTrickInput(initialRotation));

    }

    IEnumerator WaitForTrickInput(Quaternion initialRotation)
    {
        bool trickPerformed = false;

        // Esperar hasta que el jugador presione una tecla para hacer un truco
        while (!trickPerformed)
        {
            if (Input.GetKey(KeyCode.D))
            {
               // yield return StartCoroutine(DoKickflip());
                trickPerformed = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
              //  yield return StartCoroutine(DoHeelflip());
                trickPerformed = true;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                yield return StartCoroutine(DoHighOllie(initialRotation));
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

    IEnumerator DoHighOllie(Quaternion initialRotation)
    {
        Debug.Log("High Ollie");

        // Incrementar la fuerza del salto para un ollie más alto
        rb.AddForce(Vector3.up * highOllieJumpForce, ForceMode.Impulse);

        while (Quaternion.Angle(rb.rotation, initialRotation) > 0.1f)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, initialRotation, ollieResetSpeed * Time.deltaTime));
            yield return null;
        }
        yield return null;
    }


}
