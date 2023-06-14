using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private float _horizontal = 1f;
        [SerializeField] private float speed = 8f;
        [SerializeField] private float jumpingPower = 16f;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.2f;

        private float _coyoteTimeCounter;
        private float _jumpBufferTimeCounter;
        private bool _isFacingRight = true;

        private Rigidbody2D _rb;

        [FormerlySerializedAs("_groundCheck")] [SerializeField]
        private Transform groundCheck;

        [SerializeField] private LayerMask groundLayer;

        private InputManager _input;

        private bool _goingRight = true;


        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            groundCheck = transform.GetChild(0);
            _input = InputManager.Instance;
            _input.OnJumpStart += NormalJump;
            _input.OnJumpPerformed += HighJump;
        }

        // Update is called once per frame
        private void Update()
        {
            ChangeDirection(_input.MovementInput);
            Flip();
            if (IsGrounded())
                _coyoteTimeCounter = coyoteTime;
            else
                _coyoteTimeCounter -= Time.deltaTime;

            _jumpBufferTimeCounter -= Time.deltaTime;

            if (_coyoteTimeCounter > 0f && _jumpBufferTimeCounter > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpingPower);
                _jumpBufferTimeCounter = 0f;
                AudioManager.Instance.PlayOneShot("jump");
                GameEvents.Instance.PlayerInput();
            }
        }

        private void ChangeDirection(float input)
        {
            if (_goingRight && input < 0f)
            {
                _horizontal = -1f;
                _goingRight = false;
                GameEvents.Instance.PlayerInput();
            }

            if (!_goingRight && input > 0f)
            {
                _goingRight = true;
                _horizontal = 1f;
                GameEvents.Instance.PlayerInput();
            }
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.GameStart) return;
            if (!_rb.simulated) _rb.simulated = true;
            _rb.velocity = new Vector2(_horizontal * speed, _rb.velocity.y);
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.33f, groundLayer);
        }

        private void Flip()
        {
            if ((_isFacingRight && _horizontal < 0f) || (!_isFacingRight && _horizontal > 0f))
            {
                _isFacingRight = !_isFacingRight;
                var movementTransform = transform;
                var localScale = movementTransform.localScale;
                localScale.x *= -1f;
                movementTransform.localScale = localScale;
            }
        }

        private void NormalJump()
        {
            if (!GameManager.Instance.GameStart) return;
            _jumpBufferTimeCounter = jumpBufferTime;
        }

        private void HighJump()
        {
            if (!GameManager.Instance.GameStart) return;
            if (_rb.velocity.y > 0f)
            {
                var velocity = _rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 1.25f);
                _rb.velocity = velocity;
                _coyoteTimeCounter = 0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ground")) GameManager.Instance.GameOver();
        }

        private void LeftFoot()
        {
            if (!IsGrounded()) return;
            AudioManager.Instance.PlayOneShot("footstepOne");
        }

        private void RightFoot()
        {
            if (!IsGrounded()) return;
            AudioManager.Instance.PlayOneShot("footstepTwo");
        }
    }
}