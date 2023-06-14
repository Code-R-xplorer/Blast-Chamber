using Level;
using UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Utilities
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private Vector2 _screenBounds;
        private GameObject _player;

        [SerializeField] private float margin = 1;
        [SerializeField] private float timeToComplete = 30;

        private int _cardsCollected;

        private int _moneyCollected;

        private int _totalCards;
        private bool _paused;

        [SerializeField] private PostProcessVolume postProcessVolume;

        private bool _finished;

        public bool GameStart { get; private set; }

        private bool _jumped;

        private Animator _playerAnimator;
        private static readonly int Speed = Animator.StringToHash("speed");

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            Time.timeScale = 1;
            _screenBounds =
                Camera.main!.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    Camera.main.transform.position.z));
            _player = GameObject.FindWithTag("Player");
            _playerAnimator = _player.GetComponent<Animator>();
            GameEvents.Instance.OnPickupCollected += UpdateCounter;

            InputManager.Instance.OnPauseStart += TogglePause;
            InputManager.Instance.OnJumpStart += () => { _jumped = true; };
        }

        public void IncreaseTotalCards()
        {
            _totalCards++;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!GameStart)
                if (InputManager.Instance.MovementInput > 0 || InputManager.Instance.MovementInput < 0 || _jumped)
                    StartGame();

            Vector2 playerPos = _player.transform.position;

            if (playerPos.x < _screenBounds.x * -1 - margin) playerPos.x = _screenBounds.x + margin;

            if (playerPos.x > _screenBounds.x + margin) playerPos.x = _screenBounds.x * -1 - margin;

            _player.transform.position = playerPos;
        }

        private void UpdateCounter(Pickup.Type type)
        {
            switch (type)
            {
                case Pickup.Type.Card:
                    _cardsCollected++;
                    UIManager.Instance.cards.SetCount(_cardsCollected);
                    if (_cardsCollected == _totalCards) GameWin();
                    break;
                case Pickup.Type.Money:
                    _moneyCollected++;
                    UIManager.Instance.money.SetCount(_moneyCollected);
                    break;
                default:
                    return;
            }
        }

        private void GameWin()
        {
            _finished = true;
            var timeLeft = UIManager.Instance.timer.StopTimer();
            var finalScore = _cardsCollected * timeLeft * _moneyCollected;
            UIManager.Instance.DisplayWinScreen(_cardsCollected, _moneyCollected, finalScore);
            AppManager.Instance.LevelCompleted();
            _player.SetActive(false);
            Time.timeScale = 0;
        }

        public void GameOver()
        {
            _finished = true;
            var score = _cardsCollected * _moneyCollected;
            UIManager.Instance.DisplayGameOverScreen(_cardsCollected, _moneyCollected, score);
            _player.SetActive(false);
            Time.timeScale = 0;
        }


        public void TogglePause()
        {
            if (_finished) return;
            _paused = !_paused;
            Time.timeScale = _paused ? 0 : 1;
            ToggleBlur(_paused);
            UIManager.Instance.TogglePauseMenu(_paused);
        }

        public void ToggleBlur(bool show)
        {
            postProcessVolume.weight = show ? 1 : 0;
        }

        private void StartGame()
        {
            if (GameStart) return;
            GameStart = true;
            UIManager.Instance.timer.SetTimer(timeToComplete, this);
            _playerAnimator.SetFloat(Speed, 1);
        }
    }
}