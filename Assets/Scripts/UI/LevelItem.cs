using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private GameObject levelLocked, levelUnlocked, levelSelected;
        [SerializeField] private TextMeshProUGUI levelNumberText;

        private int _levelNumber;
        private bool _unlocked;

        public void Initialize(int levelNumber)
        {
            _levelNumber = levelNumber;
            levelNumberText.text = _levelNumber.ToString();
            _unlocked = AppManager.Instance.LevelUnlocked(_levelNumber);
            if (_unlocked)
            {
                levelLocked.SetActive(false);
                levelUnlocked.SetActive(true);
            }
            else
            {
                levelUnlocked.SetActive(false);
            }
        }

        public void PointerEnter()
        {
            levelSelected.SetActive(true);
            AudioManager.Instance.PlayOneShot("uiHover");
        }

        public void PointerExit()
        {
            levelSelected.SetActive(false);
        }

        public void PointerClick()
        {
            if (!_unlocked) return;
            AudioManager.Instance.PlayOneShot("uiSelect");
            AppManager.Instance.LoadLevel(_levelNumber);
        }
    }
}