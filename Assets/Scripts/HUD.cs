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

    // Update is called once per frame
    void Update()
    {
        gameTime += TimeSpan.FromSeconds(Time.deltaTime);
        timer.text = gameTime.ToString(@"mm\:ss\.ff");

        coinDisplay.text = coinCount.ToString();
    }
}
