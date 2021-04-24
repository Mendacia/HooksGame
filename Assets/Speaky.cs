using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speaky : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private Text text;
    private bool playerInRange = false;
    public SpeakyHolder mySpeaky = null;

    [SerializeField] private string fullText = null;
    [SerializeField] private string displayedText = null;

    private void Start()
    {
        mySpeaky.InitializeMe();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            textbox.SetActive(true);
            fullText = mySpeaky.SetNextText();
            StartCoroutine(TextPopulator());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = false;
            displayedText = null;
            fullText = null;
            textbox.SetActive(false);
        }
    }
    IEnumerator TextPopulator()
    {
        for(int i = 0; i < fullText.Length; i++)
        {
            displayedText = fullText.Substring(0, i+1);
            text.text = displayedText;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
