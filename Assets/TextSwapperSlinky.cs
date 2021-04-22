using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSwapperSlinky : MonoBehaviour
{

    private Text myText;
    [SerializeField] private bool currentlyLoves = false;
    private void Awake()
    {
        myText = gameObject.GetComponent<Text>();
        StartCoroutine(textTimer());
    }
    IEnumerator textTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentlyLoves)
        {
            myText.text = "Lamp Hater";
        }
        else
        {
            myText.text = "Lamp Lover";
        }
        currentlyLoves = !currentlyLoves;

        StartCoroutine(textTimer());
    }
}
