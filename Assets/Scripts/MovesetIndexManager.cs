using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MovesetIndexManager : MonoBehaviour
{
    public MovesetManager moveset;
    public int index;
    void Start()
    {
        if(moveset.monster.moveSet[index] != null)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = moveset.monster.moveSet[index].moveName;

        }
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
    }
    public void OnButtonClick()
    {
        moveset.ChangeAttackIndex(index);
    }
}
