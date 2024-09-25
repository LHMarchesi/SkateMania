using UnityEngine;

public class Goal : MonoBehaviour
{
    public WinLoose winLooseScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winLooseScript.WinLevel();
        }

    }
}
