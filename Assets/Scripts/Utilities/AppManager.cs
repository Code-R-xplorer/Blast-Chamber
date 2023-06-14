using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Utilities
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; private set; }

        public AudioMixer AudioMixer;
        public bool allowCameraShake = true;
        public float masterVol, sfxVol, musicVol;
        public float masterSlider = 1, sfxSlider = 1, musicSlider = 1;

        public Dictionary<int, bool> levels;
        private List<int> _levelIDs;
        public string levelPrefix;

        private int _currentSceneID;
        // public LevelMenu levelMenu;

        private void Awake()
        {
            // Check if there is already an AppManager instance and if it's different from this instance.
            if (Instance != null && Instance != this)
            {
                // Destroy this game object if there's already an AppManager instance.
                Destroy(gameObject);
                return;
            }

            // Set the AppManager instance to this instance.
            Instance = this;
            // Make sure the AppManager instance is not destroyed when loading a new scene.
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            levels = new Dictionary<int, bool>();
            _levelIDs = new List<int>();
            for (var i = 1; i < SceneManager.sceneCountInBuildSettings; i++) _levelIDs.Add(i);

            foreach (var id in _levelIDs) levels.Add(id, false);

            levels[1] = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
                GameObject.Find("Canvas").GetComponent<MainMenu>().levelMenu.CreateLevelItems(_levelIDs);
        }

        public void SetMasterVol(float value)
        {
            masterSlider = value;
            masterVol = Mathf.Log10(value) * 20;
            AudioMixer.SetFloat("MasterVol", masterVol);
        }

        public void SetSFXVol(float value)
        {
            sfxSlider = value;
            sfxVol = Mathf.Log10(value) * 20;
            AudioMixer.SetFloat("SFXVol", sfxVol);
        }

        public void SetMusicVol(float value)
        {
            musicSlider = value;
            musicVol = Mathf.Log10(value) * 20;
            AudioMixer.SetFloat("MusicVol", musicVol);
        }

        public bool LevelUnlocked(int levelID)
        {
            return levels[levelID];
        }

        public void LoadLevel(int levelID)
        {
            _currentSceneID = levelID;
            SceneManager.LoadScene($"{levelPrefix}{levelID}");
        }

        public void LoadNextLevel()
        {
            if (_currentSceneID < SceneManager.sceneCountInBuildSettings)
            {
                _currentSceneID++;
                LoadLevel(_currentSceneID);
            }
        }

        public bool FinalLevel()
        {
            // Debug.Log(SceneManager.sceneCountInBuildSettings);
            Debug.Log(_currentSceneID);
            return _currentSceneID == SceneManager.sceneCountInBuildSettings - 1;
        }

        public void LevelCompleted()
        {
            levels[_currentSceneID + 1] = true;
        }
    }
}