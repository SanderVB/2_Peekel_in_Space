﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [SerializeField] float loadDelay = 3f;
    int currentSceneIndex;
    bool sceneUpdated = false;

    private void Awake()
    {
        int levelManagerCount = FindObjectsOfType<LevelLoader>().Length;
        if (levelManagerCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
    private void Start()
    {
        UpdateSceneIndex();
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitAndLoadGameLevel());
        }
    }

    private void Update()
    {
        if (!sceneUpdated)
            UpdateSceneIndex();
    }

    IEnumerator WaitAndLoadGameLevel()
    {
        yield return new WaitForSeconds(loadDelay);
        LoadNextLevel();
    }

    public void LoadNextLevel() //Loads next level and makes music player load corresponding track
    {
        sceneUpdated = false;
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1) //prevents 'loading outside of build-index' error & goes back to splash atm
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
            FindObjectOfType<MusicPlayer>().MusicChanger(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
            FindObjectOfType<MusicPlayer>().MusicChanger(0);
        }
    }

        public void LoadPreviousLevel()
    {
        sceneUpdated = false;
        if (currentSceneIndex < 1)
            SceneManager.LoadScene(currentSceneIndex);
        else
            SceneManager.LoadScene(currentSceneIndex - 1);
    }

    public void RestartLevel()
    {
        sceneUpdated = false;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void UpdateSceneIndex()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
