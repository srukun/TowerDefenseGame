using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSlotManager : MonoBehaviour
{
    public string slotType;
    public Monster monster;
    public CollectionManager collectionManager;
    public int index;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
    public void ClearSlot()
    {
        monster = null;
    }
}
