using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Dialogue intoDialogue;
    void Start()
    {
        var dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.OnEndDialogueForLevelComplete += ReturnToMenu;
        dialogueManager.StartDialogue(intoDialogue, true);
        Monster aquaphion = new Monster(IDGenerator.GenerateID(), "Aquaphion", 5, 0, 20, 3.4f, 5, 75.74f, 1.45f, 4.2f, 1.75f);
        FileManager.UnlockChapter("Windwar Province", "Chapter 1");
        FileManager.TameMonster(aquaphion);
    }

    void Update()
    {
        
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
