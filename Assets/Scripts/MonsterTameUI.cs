using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTameUI : MonoBehaviour
{
    private bool isDragging;
    private Vector3 initialPosition;
    private GameObject unitSpotObj;
    public LevelManager levelManager;
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
        transform.position = initialPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging)
        {
            if (collision.CompareTag("Enemy") && CompareTag("Card"))
            {
                Monster monster = collision.GetComponent<EnemyController>().thisCreature;
                if (monster.currentHealth < monster.baseHealth/2 && monster.baseHealth < 100)
                {
                    FileManager.TameMonster(monster.GetMonster());
                    collision.GetComponent<EnemyController>().RemoveEnemyFromScene();

                    levelManager.DeleteTeam();
                    levelManager.SpawnTeam();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
