using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string moveName;
    public string moveType;
    public float moveDamage;
    public float attackRate;
    public float moveSpeed;
    public Move(string moveName, string moveType, float moveDamage, float attackRate, float moveSpeed)
    {
        this.moveName = moveName;
        this.moveType = moveType;
        this.moveDamage = moveDamage;
        this.attackRate = attackRate;
        this.moveSpeed = moveSpeed;
    }

}
