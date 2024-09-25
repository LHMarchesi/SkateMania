using UnityEngine;

public class WinLoose : MonoBehaviour
{
    public bool gameEnded;

    public void WinLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win!");
            // Aqu� puedes agregar l�gica para mostrar un men� de victoria
            Destroy(gameObject);
            gameEnded = true;
        }
    }

    public void LooseLevel(string reason)
    {
        if (!gameEnded)
        {
            Debug.Log(reason);
            // Aqu� puedes agregar l�gica para mostrar un men� de derrota
            Destroy(gameObject);
            gameEnded = true;
        }
    }
}
