using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : MonoBehaviour
{
    public Monster monster;
    public GameObject levelManager;
    public GameObject[] healthbar;
    public GameObject movesetUI;
    public GameObject attackPoint;
    public LayerMask enemyLayer;

    public string monsterState;

    //attack
    public float attackRate;
    public GameObject[] moves;
    public GameObject canvas;


    void Start()
    {
        UpdateHealthbar();
        attackRate = monster.moveSet[monster.activeMoveIndex].attackRate;

    }

    void Update()
    {
        if (attackRate <= 0f)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, 5f, enemyLayer);

            if (enemiesInRange.Length > 0)
            {
                Transform nearestEnemy = GetNearestEnemy(enemiesInRange);
                if (nearestEnemy != null)
                {
                    Attack(nearestEnemy.gameObject);
                    attackRate = monster.moveSet[monster.activeMoveIndex].attackRate;
                }
            }
        }
        else
        {
            attackRate -= Time.deltaTime;
        }
    }
    public void CreateMoveset(GameObject canvas, Vector3 movesetPosition)
    {
        this.canvas = canvas;
        GameObject moveset = Instantiate(movesetUI, new Vector3(0, 0, 0), Quaternion.identity);
        moveset.transform.SetParent(canvas.transform, false);
        moveset.transform.position = movesetPosition;
        moveset.GetComponent<MovesetManager>().monster = monster;
        moveset.GetComponent<MovesetManager>().monsterController = this;

    }
    public void UpdateHealthbar()
    {
        healthbar[0].transform.localScale = new Vector3(monster.currentHealth / monster.baseHealth, 0.1f, 1);
        healthbar[1].transform.localScale = new Vector3(monster.currentHealth / monster.baseHealth, 0.1f, 1);

    }
    private Transform GetNearestEnemy(Collider2D[] enemies)
    {
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                nearestEnemy = enemy.transform;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }

    public void Attack(GameObject nearestEnemy)
    {
        if (FileManager.TeamMemberIsActive(monster))
        {
            switch (monster.moveSet[monster.activeMoveIndex].moveName)
            {
                case "Punch":
                    GameObject punch = Instantiate(moves[0], attackPoint.transform.position, Quaternion.identity);
                    punch.GetComponent<MoveController>().target = nearestEnemy;
                    punch.GetComponent<MoveController>().monster = monster;
                    break;
                case "Thunder Punch":
                    GameObject thunderPunch = Instantiate(moves[1], attackPoint.transform.position, Quaternion.identity);
                    thunderPunch.GetComponent<MoveController>().target = nearestEnemy;
                    thunderPunch.GetComponent<MoveController>().monster = monster;
                    break;
            }
        }

    }

}
