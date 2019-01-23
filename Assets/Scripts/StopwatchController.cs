using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StopwatchController : MonoBehaviour {
    TextMeshProUGUI timerText;
    float timer;
    int minutes;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        Stopwatch();
    }

    private void Stopwatch() //not the most elegant solution but works for now
    {
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            minutes += 1;
            timer = 0;
        }
        timerText.text = String.Format("{0:00}:{1:00.00}", minutes, timer);
    }
}
