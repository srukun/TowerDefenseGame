using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string buttonType;
    public string provience;
    public string chapterID;
    public GameObject lockedText;
    public GameObject lockedBlockImage;
    public int sceneId;
    void Start()
    {
        SetUp();
    }

    void Update()
    {
        
    }
    public void SetUp()
    {
        if(buttonType == "ChapterSelect")
        {
            bool unlocked = FileManager.ChapterUnlocked(provience, chapterID);
            Debug.Log(unlocked);
            if (unlocked)
            {
                lockedText.SetActive(false);
                lockedBlockImage.SetActive(false);
            }
        }

    }
    public void OnClick()
    {
        SceneManager.LoadScene(sceneId);
    }
}
