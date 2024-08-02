using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
}

