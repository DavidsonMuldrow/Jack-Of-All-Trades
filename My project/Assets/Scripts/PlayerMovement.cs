using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerStats MoveStats;
    [SerializeField] private Collider2D collision;
    private Rigidbody2D rb;

    [Header("State Tracking")]
    public float VerticalVelocity { get; private set; }
    public bool IsDashing;

    private Vector2 _moveVelocity;
    private bool _isFacingRight = true;
    private bool _isGrounded;
    private bool _bumpedHead;

    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;

    private int _numberOfJumpsUsed;
    private float _jumpBufferTimer;
    private float _coyoteTimer;
    private float _apexPoint;

    private bool _isOnWall;
    private bool _isWallSliding;
    private int _lastWallDir;
    private int _lastJumpedWallDir;
    private float _wallJumpUnlockTimer;
    private float _wallStickTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _numberOfJumpsUsed = 0;
        VerticalVelocity = 0f;
        _isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();

        // TEST TOOL: Press T during game to see if cards actually changed the asset
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Current Stats -> Speed: {MoveStats.MoveSpeed} | Max Jumps: {MoveStats.NumberOfJumpsAllowed}");
        }
    }

    private void FixedUpdate()
    {
        CollisionChecks();

        if (_wallJumpUnlockTimer > 0) _wallJumpUnlockTimer -= Time.fixedDeltaTime;

        bool isPushingAgainstWall = (InputManager.Movement.x > 0 && _lastWallDir == 1) ||
                                    (InputManager.Movement.x < 0 && _lastWallDir == -1);

        if (isPushingAgainstWall)
        {
            _wallStickTimer = 0.25f;
        }
        else
        {
            _wallStickTimer -= Time.fixedDeltaTime;
        }

        if (_isOnWall && !_isGrounded && rb.linearVelocity.y < 0 && _wallStickTimer > 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }

        Jump();

        if (IsDashing) return;

        if (_wallJumpUnlockTimer <= 0)
        {
            if (_isGrounded)
            {
                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
            }
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
            }
        }
    }

    private void JumpChecks()
    {
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
        }

        if (InputManager.JumpWasReleased && _isJumping && VerticalVelocity > 0f)
        {
            _isFastFalling = true;
        }

        if (_jumpBufferTimer > 0f)
        {
            if (_jumpBufferTimer > 0f && _isWallSliding && _lastWallDir != _lastJumpedWallDir)
            {
                InitiateWallJump();
            }
            else if (_isGrounded || _coyoteTimer > 0f)
            {
                InitiateJump(1);
            }
            else if (_numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
            {
                if (InputManager.JumpWasPressed)
                {
                    InitiateJump(1);
                }
            }
        }

        if (_isGrounded)
        {
            _lastJumpedWallDir = 0;

            if (VerticalVelocity <= 0.1f)
            {
                _isJumping = false;
                _isFalling = false;
                _isFastFalling = false;
                _numberOfJumpsUsed = 0;
            }
        }
    }

    private void InitiateJump(int numberOfJumpsToType)
    {
        _isJumping = true;
        _isFalling = false;
        _isFastFalling = false;
        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsToType;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void InitiateWallJump()
    {
        _isJumping = true;
        _isWallSliding = false;
        _jumpBufferTimer = 0f;
        _isFastFalling = false;
        _lastJumpedWallDir = _lastWallDir;

        _wallJumpUnlockTimer = 0.2f;

        Vector2 jumpDir = new Vector2(-_lastWallDir * MoveStats.WallJumpForce.x, MoveStats.WallJumpForce.y);

        rb.linearVelocity = jumpDir;
        VerticalVelocity = jumpDir.y;
        _moveVelocity.x = jumpDir.x;
    }

    private void Jump()
    {
        if (_isWallSliding)
        {
            VerticalVelocity = Mathf.MoveTowards(rb.linearVelocity.y, -MoveStats.WallSlideSpeed, 50f * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, VerticalVelocity);
            return;
        }

        if (_isJumping)
        {
            if (_bumpedHead)
            {
                _isJumping = false;
                _isFastFalling = true;
                VerticalVelocity = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                return;
            }

            if (VerticalVelocity > 0f)
            {
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > MoveStats.ApexThreshold)
                {
                    VerticalVelocity += MoveStats.Gravity * 0.5f * Time.fixedDeltaTime;
                }
                else
                {
                    float gravMult = _isFastFalling ? MoveStats.GravityOnReleaseMultiplier : 1f;
                    VerticalVelocity += MoveStats.Gravity * gravMult * Time.fixedDeltaTime;
                }
            }
            else
            {
                if (!_isFalling) _isFalling = true;
                float fallMult = _isFastFalling ? MoveStats.GravityOnReleaseMultiplier : 1f;
                VerticalVelocity += MoveStats.Gravity * fallMult * Time.fixedDeltaTime;
            }
        }

        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling) _isFalling = true;
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, VerticalVelocity);
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MoveSpeed;
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector2(_moveVelocity.x, rb.linearVelocity.y);
    }

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        if (!_isGrounded) _coyoteTimer -= Time.deltaTime;
        else _coyoteTimer = MoveStats.JumpCoyoteTime;
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0) Turn(false);
        else if (!_isFacingRight && moveInput.x > 0) Turn(true);
    }

    private void Turn(bool turnRight)
    {
        _isFacingRight = turnRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void CollisionChecks()
    {
        Vector2 groundOrigin = new Vector2(collision.bounds.center.x, collision.bounds.min.y);
        Vector2 groundSize = new Vector2(collision.bounds.size.x, MoveStats.GroundDetectionRayLength);
        _isGrounded = Physics2D.BoxCast(groundOrigin, groundSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

        Vector2 headOrigin = new Vector2(collision.bounds.center.x, collision.bounds.max.y);
        Vector2 headSize = new Vector2(collision.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);
        _bumpedHead = Physics2D.BoxCast(headOrigin, headSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

        float wallCheckDist = 0.2f;
        float playerWidth = collision.bounds.extents.x;
        Vector2 wallSize = new Vector2(0.1f, collision.bounds.size.y * 0.9f);

        Vector2 rightSideOffset = (Vector2)collision.bounds.center + new Vector2(playerWidth, 0);
        Vector2 leftSideOffset = (Vector2)collision.bounds.center + new Vector2(-playerWidth, 0);

        RaycastHit2D hitRight = Physics2D.BoxCast(rightSideOffset, wallSize, 0f, Vector2.right, wallCheckDist, MoveStats.GroundLayer);
        RaycastHit2D hitLeft = Physics2D.BoxCast(leftSideOffset, wallSize, 0f, Vector2.left, wallCheckDist, MoveStats.GroundLayer);

        if (hitRight)
        {
            _isOnWall = true;
            _lastWallDir = 1;
        }
        else if (hitLeft)
        {
            _isOnWall = true;
            _lastWallDir = -1;
        }
        else
        {
            _isOnWall = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (collision == null) return;

        Gizmos.color = _isOnWall ? Color.green : Color.red;

        float wallCheckDist = 0.2f;
        float playerWidth = collision.bounds.extents.x;
        Vector2 wallSize = new Vector2(0.1f, collision.bounds.size.y * 0.9f);

        Vector2 rightPos = (Vector2)collision.bounds.center + new Vector2(playerWidth + (wallCheckDist / 2), 0);
        Vector2 leftPos = (Vector2)collision.bounds.center + new Vector2(-playerWidth - (wallCheckDist / 2), 0);

        Gizmos.DrawWireCube(rightPos, wallSize);
        Gizmos.DrawWireCube(leftPos, wallSize);
    }
}
