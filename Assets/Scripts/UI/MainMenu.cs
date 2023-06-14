using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject levelsMenu;

        [SerializeField] private Slider master, sfx, music;
        [SerializeField] private Toggle shakeToggle;

        public LevelMenu levelMenu;

        private void Awake()
        {
            levelMenu = transform.GetChild(2).GetComponent<LevelMenu>();
        }

        private void Start()
        {
            master.value = AppManager.Instance.masterSlider;
            sfx.value = AppManager.Instance.sfxSlider;
            music.value = AppManager.Instance.musicSlider;
            shakeToggle.isOn = AppManager.Instance.allowCameraShake;
        }

        public void LevelMenuClick()
        {
            AudioManager.Instance.PlayOneShot("uiSelect");
            levelsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void ExitButtonClick()
        {
            Application.Quit();
        }


        public void OptionsMenuClick()
        {
            AudioManager.Instance.PlayOneShot("uiSelect");
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void BackClick(string currentMenu)
        {
            if (currentMenu == "levelMenu") levelsMenu.SetActive(false);

            if (currentMenu == "optionsMenu") optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
            AudioManager.Instance.PlayOneShot("uiSelect");
        }

        public void ShakeToggle(bool value)
        {
            AppManager.Instance.allowCameraShake = value;
        }

        public void SetMasterVol(float value)
        {
            AppManager.Instance.SetMasterVol(value);
        }

        public void SetSfxVol(float value)
        {
            AppManager.Instance.SetSFXVol(value);
        }

        public void SetMusicVol(float value)
        {
            AppManager.Instance.SetMusicVol(value);
        }

        public void ButtonHover()
        {
            AudioManager.Instance.PlayOneShot("uiHover");
        }
    }
}