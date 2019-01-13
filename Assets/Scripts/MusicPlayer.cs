using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip bossMusic;
    private AudioSource myAudioSource;

    // Use this for initialization
    private void Awake()
    {
        int musicPlayerCount = FindObjectsOfType<MusicPlayer>().Length;
        if (musicPlayerCount > 1)
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
        myAudioSource = GetComponent<AudioSource>();
        MusicChanger(FindObjectOfType<LevelLoader>().GetSceneIndex());
    }

    public void MusicChanger(int levelNumber) //changes music based on scene# being loaded
    {
        switch (levelNumber)
        {
            case 0:
                myAudioSource.clip = menuMusic;
                break;
            case 1:
                myAudioSource.clip = levelMusic;
                break;
            case 2:
                myAudioSource.clip = bossMusic;
                break;
            default:
                myAudioSource.clip = levelMusic;
                break;
        }
        myAudioSource.Play();
    }
}
