using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI trickText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI speedText;

    private int totalPoints;

    public void UpdateTrickText(string trickName)
    {
        trickText.text = "Trick: " + trickName;
    }

    public void UpdatePoints(int points)
    {
        totalPoints += points;
        pointsText.text = "Points: " + totalPoints.ToString();
    }

    public void UpdateSpeed(float speed)
    {
        speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
    }
}
