using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour
{
    public int coinCount;
    public Text timer;
    public Text coinDisplay;
    public TimeSpan gameTime;
    public bool updateTimePlease = true;

    private void Start()
    {
        updateTimePlease = true;
    }
    void Update()
    {
        gameTime += TimeSpan.FromSeconds(Time.deltaTime);

        if (updateTimePlease)
        {
            timer.text = gameTime.ToString(@"mm\:ss\.ff");
        }

        coinDisplay.text = coinCount.ToString();
    }
}
