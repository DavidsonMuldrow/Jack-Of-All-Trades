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

        // TEST TOOL: Press T in-game to see if your cards actually changed the asset
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Current Stats -> Speed: {MoveStats.MoveSpeed} | Max Jumps: {MoveStats.NumberOfJumpsAllowed}");
        }
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (IsDashing) return;

        if (_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
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
            if (_isGrounded || _coyoteTimer > 0f)
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

        if (_isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _numberOfJumpsUsed = 0;
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

    private void Jump()
    {
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

        // Head Detection
        Vector2 headOrigin = new Vector2(collision.bounds.center.x, collision.bounds.max.y);
        Vector2 headSize = new Vector2(collision.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);
        _bumpedHead = Physics2D.BoxCast(headOrigin, headSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
    }
}
