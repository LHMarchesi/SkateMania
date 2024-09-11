using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ollie : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject sktObj;
    [SerializeField] private float ollieRotationSpeed = 90; // Speed of the tilt
    [SerializeField] private float maxOllieRotation = 30f;  // Max tilt angle for the ollie
    [SerializeField] private float ollieJumpForce = 1f;    // Jump force for the ollie
    [SerializeField] private float ollieResetSpeed = 80f;    // Speed to reset rotation after jump

    private Quaternion lastRotation;
    private Rigidbody rb;



    void Start()
    {
        rb = sktObj.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (input.IsJumping) // Replace with your trick input
        {
            lastRotation = rb.rotation;
            StartCoroutine(PerformOllie());
        }
    }

    IEnumerator PerformOllie()
    {
        input.SetJumping(true);
        SkateMovement skateMovement = sktObj.GetComponent<SkateMovement>();
        skateMovement.gravity = Vector3.zero;

        while (-rb.rotation.eulerAngles.x < -maxOllieRotation) // Rotate the skateboard for the ollie 
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(ollieRotationSpeed * Time.deltaTime, 0, 0));
            yield return null;
        }
        
        rb.AddForce(Vector3.up * ollieJumpForce, ForceMode.Impulse); // Apply the jump force after reaching the max tilt

        // Wait until the skateboard reaches the peak of the jump
        yield return new WaitForSeconds(0.2f); // Adjust timing based on jump height

        skateMovement.gravity = new(0, -300f, 0);

        while (Quaternion.Angle(rb.rotation, lastRotation) > 0.1f)  // Restore the rotation to make the skateboard land flat
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lastRotation, ollieResetSpeed * Time.deltaTime));
            yield return null;
        }

        input.SetJumping(false);
    }
}
