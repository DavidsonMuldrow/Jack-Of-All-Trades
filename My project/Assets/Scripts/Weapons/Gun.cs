using UnityEngine;

public class Gun : Weapons
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float timer = 1f;
    private float time = 0f;

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    public override void WeaponAction()
    {
        if (time <= 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            time = timer;
        }
    }
}
