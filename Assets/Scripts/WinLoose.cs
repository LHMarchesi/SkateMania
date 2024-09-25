using UnityEngine;

public class WinLoose : MonoBehaviour
{
    public bool gameEnded;

    public void WinLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win!");
            // Aquí puedes agregar lógica para mostrar un menú de victoria
            Destroy(gameObject);
            gameEnded = true;
        }
    }

    public void LooseLevel(string reason)
    {
        if (!gameEnded)
        {
            Debug.Log(reason);
            // Aquí puedes agregar lógica para mostrar un menú de derrota
            Destroy(gameObject);
            gameEnded = true;
        }
    }
}
