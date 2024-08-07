using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using static FileManager;

public class DragAndDropUnit : MonoBehaviour
{
    private bool isDragging;
    private Vector3 initialPosition;
    private GameObject currentUnitSpot;
    public GameObject levelManager;
    private void Start()
    {
        initialPosition = transform.position;
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

        if (currentUnitSpot != null)
        {

            UnitSpot unitSpot = currentUnitSpot.GetComponent<UnitSpot>();

            if (!unitSpot.IsOccupied)
            {

                transform.position = new Vector3(currentUnitSpot.transform.position.x, currentUnitSpot.transform.position.y, -2);
                unitSpot.SetOccupied(true);
                if (CompareTag("Monster"))
                {
                    SetMonsterActive(GetComponent<MonsterController>().monster, true);
                }
            }
        }
        else
        {
            transform.position = initialPosition;
            if (CompareTag("Monster"))
            {
                SetMonsterActive(GetComponent<MonsterController>().monster, false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("UnitSpot") && CompareTag("Monster"))
        {
            UnitSpot unitSpot = collision.GetComponent<UnitSpot>();
            if (!unitSpot.IsOccupied)
            {
                currentUnitSpot = collision.gameObject; 
            }
        }
        else if (collision.CompareTag("Enemy") && CompareTag("Card"))
        {
            Monster monster = collision.GetComponent<EnemyController>().thisCreature;
            if(monster.currentHealth < 5)
            {
                //catch
                FileManager.TameMonster(monster.GetMonster());
                collision.GetComponent<EnemyController>().RemoveEnemyFromScene();

                levelManager.GetComponent<LevelManager>().DeleteTeam();
                levelManager.GetComponent<LevelManager>().SpawnTeam();
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("UnitSpot"))
        {
            if (currentUnitSpot == collision.gameObject)
            {
                UnitSpot unitSpot = currentUnitSpot.GetComponent<UnitSpot>();
                unitSpot.SetOccupied(false);
                if (CompareTag("Monster"))
                {

                    SetMonsterActive(GetComponent<MonsterController>().monster, false);
                    transform.position = initialPosition;
                }
                currentUnitSpot = null;

            }
        }
    }
}
