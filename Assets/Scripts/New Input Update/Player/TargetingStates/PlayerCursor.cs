using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public CursorState currentState {get; private set;}


    public PlayerInputHandler InputHandler { get; private set; }
}
