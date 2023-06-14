using UnityEngine;
using Player;

namespace Utilities
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : MonoBehaviour
    {
        private PlayerControls _playerControls;
        public static InputManager Instance;

        public float MovementInput { get; private set; }

        public delegate void BaseAction();

        public event BaseAction OnJumpStart;
        public event BaseAction OnJumpPerformed;
        public event BaseAction OnPauseStart;

        private void StartJumpPrimary()
        {
            OnJumpStart?.Invoke();
        }

        private void PerformedJumpPrimary()
        {
            OnJumpPerformed?.Invoke();
        }

        private void StartPausePrimary()
        {
            OnPauseStart?.Invoke();
        }

        private void Awake()
        {
            Instance = this;
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void Start()
        {
            _playerControls.Controls.Jump.started += _ => StartJumpPrimary();
            _playerControls.Controls.Jump.performed += _ => PerformedJumpPrimary();
            _playerControls.Controls.Pause.started += _ => StartPausePrimary();
        }

        private void Update()
        {
            MovementInput = _playerControls.Controls.Movement.ReadValue<float>();
        }
    }
}