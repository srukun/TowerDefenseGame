using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string moveName;
    public string moveType;
    public float damage;
    public float attackRate;
    public float moveSpeed;
    public Move(string moveName, string moveType, float moveDamage, float attackRate, float moveSpeed)
    {
        this.moveName = moveName;
        this.moveType = moveType;
        this.damage = moveDamage;
        this.attackRate = attackRate;
        this.moveSpeed = moveSpeed;
    }

}
