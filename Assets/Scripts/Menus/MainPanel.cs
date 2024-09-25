using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gameModesPanel;
    public GameObject levelsPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject audioPanel;
    public Slider volumeMaster;
    public AudioMixer mixer;

    public void Awake()
    {
        volumeMaster.onValueChanged.AddListener(ChangeVolume);
    }

    public void OpenPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        gameModesPanel.SetActive(false);
        levelsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        audioPanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ChangeVolume(float volume)
    {
        mixer.SetFloat("VolMaster", volume);
    }

    public void PlayLevel(string levelName) 
    {
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}