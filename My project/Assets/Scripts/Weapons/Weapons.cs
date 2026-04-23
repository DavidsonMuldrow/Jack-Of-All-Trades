using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{
    public virtual void WeaponAction()
    {
        Debug.Log("Weapon Action");
    }

    private void FixedUpdate()
    {
        if (InputManager.Clicked)
        {
            WeaponAction();
        }
    }

    public void GetLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            WeaponAction();
        }
    }
}
