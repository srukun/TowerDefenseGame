using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; }
    private GameObject occupiedMonster;

    public void SetOccupied(GameObject monster, bool occupied)
    {
        IsOccupied = occupied;
        if (occupied)
        {
            occupiedMonster = monster;
        }
        else
        {
            occupiedMonster = null;
        }
    }

    public GameObject GetOccupiedMonster()
    {
        return occupiedMonster;
    }
}
