using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{
    public void WeaponAction()
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
