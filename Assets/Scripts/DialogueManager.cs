using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Animator animator;
    private Queue<DialogueLine> sentences;
    public LevelManager levelManager;
    public bool isTyping;
    public event Action OnEndDialogueForLevelComplete;
    public bool endScene;
    void Start()
    {
        sentences = new Queue<DialogueLine>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, bool endScene)
    {
        this.endScene = endScene;
        animator.SetBool("isOpen", true);
        sentences.Clear();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            sentences.Enqueue(line);
        }
        if (levelManager != null)
        {
            levelManager.PauseGame();
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = sentences.Dequeue();
        string nameTextWithColor = "<color=#2a8ff5>" + line.speakerName + "</color>";

        nameText.text = nameTextWithColor;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
/*            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.text = sentence;
                break;
            }*/
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
        isTyping = false;
        OnSentenceComplete();
    }
    private void OnSentenceComplete()
    {
        string continueText = "<color=#2a8ff5> <<Press Space To Continue>> </color>"; 

        dialogueText.text += $"\n{continueText}";

    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        if(levelManager != null)
        {
            levelManager.ResumeGame();
        }
        if (endScene)
        {
            EndDialogueForLevelComplete();
        }

    }

    public void EndDialogueForLevelComplete()
    {
        OnEndDialogueForLevelComplete?.Invoke(); 
    }

}
