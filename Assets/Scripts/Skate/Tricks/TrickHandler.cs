using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickHandler : MonoBehaviour
{
    [SerializeField] private GameObject skate;
    [SerializeField] private float ollieJumpForce;
    [SerializeField] private float highOllieJumpForce;
    [SerializeField] private float flipSpeed;
    private AudioSource audioSource;
     public AudioClip jumpSound;
     public AudioClip trickSound;

    [SerializeField] private CheckWheelCollision checkWheelCollision;

    private PlayerInput input;

    private Rigidbody rb;

    private SkateController skateController;

    private float ollieRotationSpeed = 170;
    private float tiltOllie = 30; 

    private bool isTrickInProgress;
    public bool IsTrickInProgress { get => isTrickInProgress; set => isTrickInProgress = value; }

    private UiManager uiManager;

    void Awake()
    {

        audioSource = skate.GetComponent<AudioSource>();
        rb = skate.GetComponent<Rigidbody>();
        input = skate.GetComponent<PlayerInput>();

        skateController = skate.GetComponent<SkateController>();

        uiManager = FindObjectOfType<UiManager>();
    }

    void Update()
    {
        if (input.IsJumping && !isTrickInProgress)
        {
            audioSource.clip = jumpSound;
            audioSource.Play();
            StartCoroutine(PerformTrick());
        }
    }

    IEnumerator PerformTrick()
    {
        isTrickInProgress = true;  // Mark that a trick is in progress
       
        SkateController skateController = skate.GetComponent<SkateController>();

        yield return StartCoroutine(DoOllie(skateController));

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
        while (!trickPerformed && !checkWheelCollision.IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                audioSource.clip = trickSound;
                audioSource.Play();
                yield return StartCoroutine(DoKickflip());
                skateController.AddPoints(200, "Kickflip");
                uiManager.UpdateTrickText("Kickflip");
                
                trickPerformed = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                audioSource.clip = trickSound;
                audioSource.Play();
                yield return StartCoroutine(DoHeelflip());
                skateController.AddPoints(250, "Heelflip");
                uiManager.UpdateTrickText("Heelflip");
    
                trickPerformed = true;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                audioSource.clip = trickSound;
                audioSource.Play();
                yield return StartCoroutine(DoHighOllie());
                skateController.AddPoints(150, "High Ollie");
                uiManager.UpdateTrickText("High Ollie");

                trickPerformed = true;
            }

            yield return null; 
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
