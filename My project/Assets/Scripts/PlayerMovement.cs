using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats MoveStats;
    [SerializeField] private Collider2D collision;

    private Rigidbody2D rb;

    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool bumpedHead;

    private void Awake()
    {
        _isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MoveSpeed;

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(_moveVelocity.x, rb.linearVelocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(_moveVelocity.x, rb.linearVelocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 100f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -100f, 0f);
        }
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(collision.bounds.center.x, collision.bounds.min.y);
        Vector2 boxCastSize = new Vector2(collision.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        //put gizmos here for grounded raycast debugging
    }

    private void CollisionChecks()
    {
        IsGrounded();
    }
}
