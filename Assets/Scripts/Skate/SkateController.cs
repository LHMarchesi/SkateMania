using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkateController : MonoBehaviour
{
    [SerializeField] private Vector3 gravity = new(0, -300f, 0);

    [SerializeField] private GameObject SpawnPos;

    [SerializeField] private float pushForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turn_speed = 15f;

    private float kickturn_thresh = 2f;
    private float kickturn_speed = 150f;
    private float sidewaysFriction = 15f;

    private Rigidbody rb;

    private PlayerInput playerInput;

    private TrickHandler trickHandler;

    private bool isGrinding;

    private string currentGrind;

    private int totalPoints = 0;

    public TextMeshProUGUI trickText;
    public TextMeshProUGUI pointsText;

    public int TotalPoints { get => totalPoints; set => totalPoints = value; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

        playerInput = GetComponent<PlayerInput>();

        trickHandler = GetComponentInChildren<TrickHandler>();

    }

    private void Update()
    {
        
        if (isGrinding)
        {
            HandleGrindInput();
        }

        float speed = rb.velocity.magnitude;

        FindObjectOfType<UiManager>().UpdateSpeed(speed);
    }

    public void AddPoints(int points, string trickName)
    {
        TotalPoints += points;
        trickText.text = "You Made: " + trickName + "!";
        pointsText.text = "Points: " + TotalPoints;
    }


    private void FixedUpdate()
    {
        Vector3 local_velocity = transform.InverseTransformDirection(rb.velocity);

        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        if (!playerInput.IsJumping)
        {
            Physics(local_velocity);

            Inputs(h_input, v_input, local_velocity);
        }
    }

    private void Physics(Vector3 local_velocity)
    {
        rb.AddForce(gravity * Time.fixedDeltaTime, ForceMode.Acceleration);

        float sideways_velocity = local_velocity.x;

        if (sideways_velocity > 0.3)
        {
            sideways_velocity -= sidewaysFriction * Time.fixedDeltaTime;
        }
        else if (sideways_velocity < -0.3)
        {
            sideways_velocity += sidewaysFriction * Time.fixedDeltaTime;
        }
        else
        {
            sideways_velocity = 0;
        }

        local_velocity.x = sideways_velocity;

        rb.velocity = transform.TransformDirection(local_velocity);
    }

    private void Inputs(float h_input, float v_input, Vector3 local_velocity)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * v_input * Time.fixedDeltaTime * 400, ForceMode.Acceleration);
        }

        Quaternion delta_rotation;

        if (Math.Abs(local_velocity.z) <= kickturn_thresh && Math.Abs(v_input) == 0)
        {
            delta_rotation = Quaternion.Euler(new Vector3(0, h_input, 0) * Time.fixedDeltaTime * kickturn_speed);
        }
        else
        {
            delta_rotation = Quaternion.Euler(new Vector3(0, h_input, 0) * Time.fixedDeltaTime * local_velocity.z * turn_speed);
        }

        rb.MoveRotation(rb.rotation * delta_rotation);
    }

    private void HandleGrindInput()
    {
        if (Input.GetButton("GrindLeft"))
        {
            currentGrind = "left";

            rb.velocity = new Vector3(-pushForce, rb.velocity.y, rb.velocity.z); // Move to the left

            Debug.Log("grind left");
        }
        else if (Input.GetButton("GrindRight"))
        {
            currentGrind = "right";

            rb.velocity = new Vector3(pushForce, rb.velocity.y, rb.velocity.z); // Move to the right

            Debug.Log("grind right");
        }

        if (Input.GetButtonUp("GrindLeft") || Input.GetButtonUp("GrindRight"))
        {
            ExitGrind();
        }

        if (playerInput.IsJumping)
        {
            ExitGrind();
        }

        Debug.Log("Grinding: " + currentGrind);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Grindable")
        {
            isGrinding = true;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Grindable")
        {
            ExitGrind();
        }
    }

    private void ExitGrind()
    {
        isGrinding = false;
        currentGrind = null;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
    }

    public void Restart()
    {
            this.transform.position = SpawnPos.transform.position;
            this.transform.rotation = SpawnPos.transform.rotation;

            rb.velocity = Vector3.zero;

            pointsText.text = "Points: " + TotalPoints; // Update point text

            pointsText.text = "Points: " + TotalPoints; // Actualizar el texto de puntos
            trickText.text = "Trick: ";
    }
}