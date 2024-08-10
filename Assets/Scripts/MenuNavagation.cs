using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavagation : MonoBehaviour
{

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadChapterSelect()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCollection()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadChallengeMode()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadMysteryGift()
    {
        SceneManager.LoadScene(4);
    }
}
