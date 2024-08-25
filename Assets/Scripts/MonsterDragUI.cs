using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDragUI : MonoBehaviour
{
    private bool isDragging;
    private Vector3 initialPosition;
    private GameObject unitSpotObj;
    private UnitSpot originalUnitSpot;

    //slot to box
    //slot to empty slot
    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;

        if (unitSpotObj != null)
        {
            originalUnitSpot = unitSpotObj.GetComponent<UnitSpot>();
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, -9);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (unitSpotObj != null)
        {
            UnitSpot unitSpot = unitSpotObj.GetComponent<UnitSpot>();

            if (!unitSpot.IsOccupied)
            {
                transform.position = new Vector3(unitSpotObj.transform.position.x, unitSpotObj.transform.position.y, -2);
                unitSpot.SetOccupied(gameObject, true);
                FileManager.SetMonsterActive(GetComponent<MonsterController>().monster, true);
                if (originalUnitSpot != null)
                {
                    originalUnitSpot.SetOccupied(null, false);
                }
            }
        }
        else
        {
            transform.position = initialPosition;
            FileManager.SetMonsterActive(GetComponent<MonsterController>().monster, false);

            if (originalUnitSpot != null)
            {
                originalUnitSpot.SetOccupied(null, false);
            }
        }

        originalUnitSpot = null; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging && collision.CompareTag("UnitSpot"))
        {
            unitSpotObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("UnitSpot") && isDragging && unitSpotObj == collision.gameObject)
        {
            unitSpotObj = null;
        }
    }
}
