using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavagation : MonoBehaviour
{
    int sceneNumber;

    [Header("UI References")]
    public TextMeshProUGUI pauseText;
    public GameObject tutorialUI;
    public GameObject dialogueUI;

    public void Start()
    {

    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadChapterSelect()
    {
        bool unlocked = FileManager.ChapterUnlocked("Windwar Province", "Chapter 1");
        if (!unlocked)
        {
            SceneManager.LoadScene(6);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
    public void LoadCollection()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadChallengeMode()
    {
        SceneManager.LoadScene(10);
    }
    public void LoadMysteryGift()
    {
        SceneManager.LoadScene(10);
    }
    public void GoToScene()
    {

    }
    public void PauseGame()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager.isDialoguePlaying)
        {
            return;
        }

        if (!levelManager.gameIsPaused && !dialogueManager.isDialoguePlaying)
        {
            levelManager.PauseGame();
            pauseText.text = "Unpause";
        }
        else
        {
            levelManager.ResumeGame();
            pauseText.text = "Pause";
        }

    }
    public void HowToPlay()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        LevelManager levelManager = FindObjectOfType<LevelManager>();


        if (dialogueManager.isDialoguePlaying)
        {
            return;
        }

        if (!tutorialUI.activeInHierarchy)
        {
            tutorialUI.SetActive(true);
        }

    }
    public void CloseHowToPlay()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        tutorialUI.SetActive(false);
    }
    private bool DialogueIsActive()
    {
        if(dialogueUI.transform.position.x < -700)
        {
            return true;
        }
        return false;
    }
}
