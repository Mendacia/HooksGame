using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Speaky Holder", menuName = "Hooks/Speaky Holder")]

public class SpeakyHolder : ScriptableObject
{
    /*[TextArea(7, 7)]public string[] myLines;
    [SerializeField] private int currentDialogue = 0;
    private int thisLineCharacterCount = 0;

    public void InitializeMe()
    {
        currentDialogue = 0;
    }


    public string SetNextText()
    {
        thisLineCharacterCount = 0;
        foreach (char x in myLines[currentDialogue])
        {
            thisLineCharacterCount++;
        }
        currentDialogue++;
        return (myLines[currentDialogue-1]);
    }*/
    public string characterName;
    public Sprite characterSprite;
    public Color32 textColor;
}
