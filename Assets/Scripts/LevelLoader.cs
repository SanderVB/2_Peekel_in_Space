using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [SerializeField] float loadDelay = 5f;
    [SerializeField] float respawnDelay = 1f;
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
        SetSceneIndex();
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitAndLoadGameLevel(1, loadDelay));
        }
    }

    private void Update()
    {
        if (!sceneUpdated)
            SetSceneIndex();
    }

    IEnumerator WaitAndLoadGameLevel(int loadIndex, float newLoadDelay)
    {
        yield return new WaitForSeconds(newLoadDelay);
        SceneManager.LoadScene(loadIndex);
        FindObjectOfType<MusicPlayer>().MusicChanger(loadIndex);
        sceneUpdated = false;
    }

    public void LoadNextlevel()
    {
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1) //prevents 'loading outside of build-index' error & goes back to splash atm
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex + 1, respawnDelay));
        else
            WaitAndLoadGameLevel(0, loadDelay);
    }

    public void LoadPreviousLevel()
    {
        if (currentSceneIndex < 1)
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex, loadDelay));
        else
            StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex - 1, loadDelay));
    }

    public void RestartLevel()
    {
        StartCoroutine(WaitAndLoadGameLevel(currentSceneIndex, respawnDelay));
    }

    public int GetSceneIndex()
    {
        SetSceneIndex();
        return currentSceneIndex;
    }

    private void SetSceneIndex()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneUpdated = true;
    }
}
