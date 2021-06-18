using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputDeviceChangeHandler : MonoBehaviour
{
    [SerializeField] private PlayerAiming playerAiming;
    [SerializeField] private SpriteRenderer spriteToChange;
    [SerializeField] private Sprite mKSprite;
    [SerializeField] private Sprite cSprite;

    void Awake()
    {
        PlayerInput input = gameObject.GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        InputUser.onChange += OnInputDeviceChange;
    }
    void OnDisable()
    {
        InputUser.onChange -= OnInputDeviceChange;
    }

    void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if(change == InputUserChange.ControlSchemeChanged)
        {
            Debug.Log("Changing Scheme to " + user.controlScheme.Value.name);
            ChangeGameWorld(user.controlScheme.Value.name);
        }
    }

    void ChangeGameWorld(string schemeName)
    {
        if (schemeName.Equals("Gamepad"))
        {
            playerAiming.ChangeControls(true);
            spriteToChange.sprite = cSprite;
        }
        else
        {
            playerAiming.ChangeControls(false);
            spriteToChange.sprite = mKSprite;
        }
    }
}
