using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject weapon;

    private void FixedUpdate()
    {
        Vector3 diff = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
