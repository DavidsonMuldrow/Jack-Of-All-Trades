using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Refusal Settings")]
    [SerializeField] private bool canDashInAir = true;

    private PlayerMovement _moveScript;
    private Rigidbody2D _rb;
    private bool _canDash = true;
    private bool _isDashing;

    private void Awake()
    {
        _moveScript = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            if (!canDashInAir && !CheckIfGrounded()) return;
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        _canDash = false;
        _moveScript.IsDashing = true;

        float dashDir = InputManager.Movement.x != 0 ? Mathf.Sign(InputManager.Movement.x) : (transform.eulerAngles.y > 90 ? -1 : 1);

        _rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        _moveScript.IsDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private bool CheckIfGrounded()
    {
        return _rb.linearVelocity.y == 0;
    }
}