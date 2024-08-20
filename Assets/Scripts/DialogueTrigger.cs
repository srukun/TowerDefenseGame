using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [Header("Dialogue Lines")]
    public Dialogue winDialogue;
    public Dialogue loseDialogue;

    public void TriggerDialogue(bool endScene)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, endScene);
    }
}
