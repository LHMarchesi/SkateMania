using TMPro;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            LevelManager levelManager = FindObjectsOfType<LevelManager>()[0];
            levelManager.TriggerWin();

            SkateController player = other.GetComponent<SkateController>();
            levelManager.totalPoints.text = "Total points: " + player.TotalPoints.ToString();
        }
    }
}
