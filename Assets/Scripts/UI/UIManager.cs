using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("UI Scripts")] public Timer timer;
        public Counter cards;
        public Counter money;

        [Header("UI Elements")] [SerializeField]
        private GameObject hud;

        [SerializeField] private GameObject gameOver;
        [SerializeField] private GameObject win;
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject nextLevelButton;

        [SerializeField] private List<TextMeshProUGUI> gameOverTexts;
        [SerializeField] private List<TextMeshProUGUI> winTexts;


        private void Awake()
        {
            Instance = this;
            gameOver.SetActive(false);
            win.SetActive(false);
            pause.SetActive(false);
        }

        private void Start()
        {
            if (AppManager.Instance.FinalLevel()) nextLevelButton.SetActive(false);
        }


        public void DisplayGameOverScreen(int cardsCollected, int moneyCollected, int score)
        {
            gameOverTexts[0].text = cardsCollected.ToString();
            gameOverTexts[1].text = moneyCollected.ToString();
            gameOverTexts[2].text = score.ToString();
            GameManager.Instance.ToggleBlur(true);
            hud.SetActive(false);
            gameOver.SetActive(true);
        }

        public void DisplayWinScreen(int cardsCollected, int moneyCollected, int finalScore)
        {
            winTexts[0].text = cardsCollected.ToString();
            winTexts[1].text = moneyCollected.ToString();
            winTexts[2].text = finalScore.ToString();
            GameManager.Instance.ToggleBlur(true);
            hud.SetActive(false);
            win.SetActive(true);
        }

        public void MenuButtonClick()
        {
            AudioManager.Instance.PlayOneShot("uiSelect");
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartButtonClick()
        {
            AudioManager.Instance.PlayOneShot("uiSelect");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ButtonHover()
        {
            AudioManager.Instance.PlayOneShot("uiHover");
        }

        public void ResumeButtonClick()
        {
            GameManager.Instance.TogglePause();
        }

        public void NextLevelButton()
        {
            AppManager.Instance.LoadNextLevel();
        }

        public void TogglePauseMenu(bool show)
        {
            pause.SetActive(show);
            hud.SetActive(!show);
        }
    }
}