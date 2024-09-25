using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI totalPoints;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 90f;

    private float currentTime;

    public GameObject WinScreen { get => winScreen; set => winScreen = value; }
    public GameObject LoseScreen { get => loseScreen; set => loseScreen = value; }

    private bool isGameOver;
    private void Awake()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    void Start()
    {
        currentTime = startTime;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (currentTime > 0 && !isGameOver)
        {
            timerText.text = currentTime.ToString("F2");

            yield return null;
            currentTime -= Time.deltaTime;
        }
        if (!isGameOver) 
        {
            currentTime = 0;
            timerText.text = currentTime.ToString("F2");

            SkateController skateController = FindObjectsOfType<SkateController>()[0];
            totalPoints.text = skateController.TotalPoints.ToString();

            LoseScreen.SetActive(true);
            isGameOver = true;
        }
    }

    public void TriggerWin()
    {
        isGameOver = true;
        StopCoroutine(StartTimer()); // Detenemos la corrutina del timer

        SkateController skateController = FindObjectsOfType<SkateController>()[0];
        totalPoints.text = skateController.TotalPoints.ToString();

        WinScreen.SetActive(true); // Activamos la pantalla de victoria
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkateController skateController = FindObjectsOfType<SkateController>()[0];

            if (isGameOver)
            {
                skateController.TotalPoints = 0;

                LoseScreen.SetActive(false);
                WinScreen.SetActive(false);

                currentTime = startTime;
                isGameOver = false; // Reiniciamos el estado de la partida
                StartCoroutine(StartTimer()); // Volvemos a iniciar el temporizador

                skateController.Restart();
            }
            else
            {
                skateController.Restart();
            }
        }
    }
}
