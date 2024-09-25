using TMPro;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] TextMeshProUGUI totalPoints;

    private void Awake()
    {
        WinScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WinScreen.SetActive(true);
            SkateController player = other.GetComponent<SkateController>();
            totalPoints.text = "Total points: " + player.TotalPoints.ToString();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            WinScreen.SetActive(false);
        }
    }
}
