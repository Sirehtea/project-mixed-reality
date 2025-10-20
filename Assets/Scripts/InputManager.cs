using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public StateManager stateManager;
    public InputAction primaryButton;
    public InputAction secondaryButton;
    
    void Awake()
    {
        primaryButton.Enable();
        primaryButton.performed += OnPrimaryPressed;
        secondaryButton.Enable();
        secondaryButton.performed += OnSecondaryPressed;
    }

    void OnDestroy()
    {
        primaryButton.Disable();
        primaryButton.performed -= OnPrimaryPressed;
        secondaryButton.Disable();
        secondaryButton.performed -= OnSecondaryPressed;
    }

    private void OnPrimaryPressed(InputAction.CallbackContext context)
    {
        stateManager.NextState();
    }
    private void OnSecondaryPressed(InputAction.CallbackContext context)
    {
        stateManager.PreviousState();
    }
}