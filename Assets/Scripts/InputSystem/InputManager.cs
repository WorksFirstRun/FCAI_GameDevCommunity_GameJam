using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private InputMap _inputMap;
    public event Action onMovementPerformed;
    public event Action onAttackPerformed;
    public event Action onDashPerformed;
    public event Action onCastPerformed;

    public event Action onCastReleased;

    public event Action onEscapeAction;
    
    private void Awake()
    {
        Instance = this;

        _inputMap = new InputMap();
        _inputMap.Player.Enable();
        _inputMap.Player.Movement.performed += MovementPerformed;
        _inputMap.Player.Attack.performed += AttackPerformed;
        _inputMap.Player.Dash.performed += DashPerformed;
        _inputMap.Player.Cast.performed += CastPerformed;
        _inputMap.Player.Cast.canceled += CastReleased;
        _inputMap.Player.Esc.performed += GamePause;
    }

    private void GamePause(InputAction.CallbackContext obj)
    {
        onEscapeAction?.Invoke();
    }

    private void CastReleased(InputAction.CallbackContext obj)
    {
        onCastReleased?.Invoke();
    }

    private void CastPerformed(InputAction.CallbackContext obj)
    {
        onCastPerformed?.Invoke();
    }

    private void DashPerformed(InputAction.CallbackContext obj)
    {
        onDashPerformed?.Invoke();
    }

    private void AttackPerformed(InputAction.CallbackContext obj)
    {
        onAttackPerformed?.Invoke();
    }

    private void MovementPerformed(InputAction.CallbackContext obj)
    {
        onMovementPerformed?.Invoke();
    }


    public Vector2 GetMovementInputDirection_Normalized()
    {
        return _inputMap.Player.Movement.ReadValue<Vector2>().normalized;
    }

    public void DisableInput()
    {
        _inputMap.Player.Disable();
    }

    private void OnDestroy()
    {
        onMovementPerformed = null;
        onCastReleased = null;
        onAttackPerformed = null;
        onCastPerformed = null;
        onDashPerformed = null;
        onEscapeAction = null;
    }
}
