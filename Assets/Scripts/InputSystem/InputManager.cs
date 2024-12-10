using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private InputMap _inputMap;
    public event Action onMovementPerformed;
    
    private void Awake()
    {
        Instance = this;

        _inputMap = new InputMap();
        _inputMap.Player.Enable();
        _inputMap.Player.Movement.performed += MovementPerformed;
    }

    private void MovementPerformed(InputAction.CallbackContext obj)
    {
        onMovementPerformed?.Invoke();
    }


    public Vector2 GetMovementInputDirection_Normalized()
    {
        return _inputMap.Player.Movement.ReadValue<Vector2>().normalized;
    }
}
