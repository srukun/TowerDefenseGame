using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesetManager : MonoBehaviour
{
    public GameObject[] moves;
    public Monster monster;

    public Transform attackPoint;
    public float attackRate;
    public GameObject target;
    public MonsterController monsterController;
    public Outline[] outlines;
    void Start()
    {

    }

    void Update()
    {

    }
    public void ChangeAttackIndex(int activeMoveIndex)
    {


        if (monster.moveSet[activeMoveIndex] != null)
        {

            monster.activeMoveIndex = activeMoveIndex;
            foreach (var outline in outlines)
            {
                outline.effectDistance = new Vector2(0, 0);
            }
            outlines[activeMoveIndex].effectDistance = new Vector2(3, 3);
        }

    }

}
