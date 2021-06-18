using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable] public class MoveInputEvent : UnityEvent<Vector2> { }
[Serializable] public class AimMouseInputEvent : UnityEvent<Vector2> { }
[Serializable] public class AimStickInputEvent : UnityEvent<Vector2> { }
[Serializable] public class JumpInputEvent : UnityEvent{ }
[Serializable] public class HookInputEvent : UnityEvent { }
[Serializable] public class SwingInputEvent : UnityEvent { }

public class InputController : MonoBehaviour
{
    Controls controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public AimMouseInputEvent aimMouseInputEvent;
    public AimStickInputEvent aimStickInputEvent;
    public HookInputEvent hookInputEvent;
    public SwingInputEvent swingInputEvent;

    void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();

        //Moving
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMovePerformed;

        //Mouse Aiming
        controls.Gameplay.AimPointer.performed += OnAimPointerPerformed;

        //Stick Aiming
        controls.Gameplay.AimStick.performed += OnAimStickPerformed;

        //Jumping
        controls.Gameplay.Jump.performed += OnJumpPerformed;

        //Firing Hook Through
        controls.Gameplay.HookThrough.performed += OnHookPerformed;

        //Firing Hook Swing
        controls.Gameplay.HookSwing.performed += OnSwingPerformed;
    }

    private void OnSwingPerformed(InputAction.CallbackContext context)
    {
        swingInputEvent.Invoke();
    }

    private void OnHookPerformed(InputAction.CallbackContext context)
    {
        hookInputEvent.Invoke();
    }

    private void OnAimPointerPerformed(InputAction.CallbackContext context)
    {
        aimMouseInputEvent.Invoke(context.ReadValue<Vector2>());
    }
    private void OnAimStickPerformed(InputAction.CallbackContext context)
    {
        aimStickInputEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpInputEvent.Invoke();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInputEvent.Invoke(context.ReadValue<Vector2>());
    }
}
