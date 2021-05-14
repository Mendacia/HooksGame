using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speaky : MonoBehaviour
{
    [Header("Set this to the appropriate character")]
    public SpeakyHolder mySpeaky = null;

    [Header("This character's Lines in this instance")]
    [TextArea(7, 7)] public string[] myLines;
    [SerializeField] private int currentDialogue = 0;

    [Header("Dialogue UI locations")]
    [SerializeField] private GameObject textbox;
    [SerializeField] private Text text;
    [SerializeField] private Image portrait;
    [SerializeField] private Text namePlate;


    private bool playerInRange = false;
    [Header("Visible for debug purposes")]
    [SerializeField] private string fullText = null;
    [SerializeField] private string displayedText = null;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            textbox.SetActive(true);
            portrait.sprite = mySpeaky.characterSprite;
            namePlate.text = mySpeaky.characterName;
            text.color = mySpeaky.textColor;
            fullText = myLines[currentDialogue];
            currentDialogue++;
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
