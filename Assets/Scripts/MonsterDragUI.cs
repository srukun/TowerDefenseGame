using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDragUI : MonoBehaviour
{
    private bool isDragging;
    private Vector3 initialPosition;
    private GameObject unitSpotObj;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        isDragging = true;
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
                unitSpot.SetOccupied(true);
                FileManager.SetMonsterActive(GetComponent<MonsterController>().monster, true);
            }
        }
        else
        {
            transform.position = initialPosition;
            FileManager.SetMonsterActive(GetComponent<MonsterController>().monster, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging)
        {
            if (collision.CompareTag("UnitSpot"))
            {
            }
            if (collision.CompareTag("UnitSpot") && CompareTag("Monster"))
            {
                UnitSpot unitSpot = collision.GetComponent<UnitSpot>();
                if (!unitSpot.IsOccupied)
                {
                    unitSpotObj = collision.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("UnitSpot") && isDragging && unitSpotObj == collision.gameObject)
        {
            unitSpotObj.GetComponent<UnitSpot>().SetOccupied(false);
            FileManager.SetMonsterActive(GetComponent<MonsterController>().monster, false);
            transform.position = initialPosition;
            unitSpotObj = null;
        }
    }
}
