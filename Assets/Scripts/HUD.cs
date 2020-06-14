using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour
{
    public Text timer;
    public TimeSpan gameTime;

    // Update is called once per frame
    void Update()
    {
        gameTime += TimeSpan.FromSeconds(Time.deltaTime);
        timer.text = gameTime.ToString(@"mm\:ss\.ff");
    }
}
