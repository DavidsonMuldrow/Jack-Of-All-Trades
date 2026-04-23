using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{

    
    private void FixedUpdate()
    {
        if (InputManager.Clicked)
        {
            WeaponAction();
        }
    }

    public virtual void WeaponAction()
    {
        Debug.Log("Weapon Action");
    }

    public void GetLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            WeaponAction();
        }
    }
}
