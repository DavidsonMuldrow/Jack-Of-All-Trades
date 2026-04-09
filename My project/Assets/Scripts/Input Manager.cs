using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool RunIsHeld;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        if (PlayerInput != null)
        {
            _moveAction = PlayerInput.actions["Move"];
            _jumpAction = PlayerInput.actions["Jump"];
            _runAction = PlayerInput.actions["Run"];

            if (_moveAction == null) Debug.LogWarning("InputManager: 'Move' action not found!");
            if (_jumpAction == null) Debug.LogWarning("InputManager: 'Jump' action not found!");
            if (_runAction == null) Debug.LogWarning("InputManager: 'Run' action not found!");
        }
        else
        {
            Debug.LogError("InputManager: PlayerInput component missing from this GameObject!");
        }
    }

    private void Update()
    {
        if (_moveAction != null)
        {
            Movement = _moveAction.ReadValue<Vector2>();

            if (Movement != Vector2.zero) Debug.Log("Movement detected: " + Movement);
        }

        if (_jumpAction != null)
        {
            JumpWasPressed = _jumpAction.WasPressedThisFrame();
            JumpIsHeld = _jumpAction.IsPressed();
            JumpWasReleased = _jumpAction.WasReleasedThisFrame();
        }

        if (_runAction != null)
        {
            RunIsHeld = _runAction.IsPressed();
        }
    }
}
