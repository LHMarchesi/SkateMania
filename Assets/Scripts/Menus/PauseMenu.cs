using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPause;
    public bool Pause = false;
    [SerializeField] private GameObject SpawnPos;
    private int totalPoints = 0;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI trickText;
    public GameObject audioPanel;
    public GameObject pauseMenu;
    public Slider volumeMaster;
    public AudioMixer mixer;

    public void Awake()
    {
        volumeMaster.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float volume)
    {
        mixer.SetFloat("VolMaster", volume);
    }

    public void OpenPanel(GameObject panel)
    {
        audioPanel.SetActive(false);
        pauseMenu.SetActive(false);

        panel.SetActive(true);
    }

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Pause == false)
            {
                menuPause.SetActive(true);
                Pause = true;

                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                AudioSource[] sounds = FindObjectsOfType <AudioSource>();

                for(int i = 0; i < sounds.Length; i++)
                {
                    sounds[i].Pause();
                }
            }
            else if(Pause == true) 
            {
                Resume();
                audioPanel.SetActive(false);
            }
        }
}

    public void Resume()
    {
        menuPause.SetActive(false);
        Pause = false;

        Time.timeScale = 1f;
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioSource[] sounds = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Play();
        }
    }

    public void GoToMenu(string menu)
    {
        SceneManager.LoadScene(menu);
        Time.timeScale = 1f;
        menuPause.SetActive(false);
        Pause = false;

        AudioSource[] sounds = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Play();
        }
    }

    public void Restart()
    {
            this.transform.position = SpawnPos.transform.position;
            this.transform.rotation = SpawnPos.transform.rotation;

            totalPoints = 0; // Update points
            pointsText.text = "Points: " + totalPoints; // Update point text
            totalPoints = 0; // Reiniciar puntaje

            pointsText.text = "Points: " + totalPoints; // Actualizar el texto de puntos
            trickText.text = "Trick: ";

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }
}