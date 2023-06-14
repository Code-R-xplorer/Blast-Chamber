using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        private float _timeLeft;
        private bool _startTimer;

        [SerializeField] private TextMeshProUGUI timerText;

        private GameManager _gameManager;

        private int _time;

        public void SetTimer(float time, GameManager manager)
        {
            _timeLeft = time;
            _startTimer = true;
            _gameManager = manager;
        }

        private void Update()
        {
            if (_startTimer)
            {
                _timeLeft -= Time.deltaTime;

                _time = (int)_timeLeft;

                timerText.text = _time.ToString();
                if (_time <= 0)
                {
                    timerText.text = "0";
                    _gameManager.GameOver();
                    _startTimer = false;
                }
            }
        }

        public int StopTimer()
        {
            _startTimer = false;
            return _time;
        }
    }
}