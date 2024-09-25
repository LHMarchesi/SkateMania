using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private TextMeshProUGUI totalPoints;
    [SerializeField] private TextMeshProUGUI timerText;
    public float startTime = 90f;

    private float currentTime;

    private void Awake()
    {
        LoseScreen.SetActive(false);
        }
    void Start()
    {
        currentTime = startTime;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (currentTime > 0)
        {
            timerText.text = currentTime.ToString("F2");

            yield return null;
            currentTime -= Time.deltaTime; 
        }
        currentTime = 0;
        timerText.text = currentTime.ToString("F2");

        SkateController skateController = FindObjectsOfType<SkateController>()[0];
        totalPoints.text = skateController.TotalPoints.ToString();

        LoseScreen.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoseScreen.SetActive(false);
            currentTime = startTime;
        }
    }
}
