using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScreenHandler : MonoBehaviour
{
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    public void HasWon(bool hasWon)
    {
        FindObjectOfType<MusicPlayer>().ResultSound(hasWon);
        if (hasWon)
        {
            //AudioSource.PlayClipAtPoint(winSound, transform.position, soundVolume);
            winScreen.SetActive(true);
            FindObjectOfType<LevelLoader>().LoadNextlevel();
        }
        else
        {

            //AudioSource.PlayClipAtPoint(loseSound, transform.position, soundVolume);
            loseScreen.SetActive(true);
            FindObjectOfType<LevelLoader>().RestartLevel();

        }
    }
}
