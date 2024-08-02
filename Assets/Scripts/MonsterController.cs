using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : MonoBehaviour
{
    public Monster monster;
    public GameObject levelManager;
    public GameObject healthbar;
    public GameObject movesetUI;
    public GameObject attackPoint;
    public LayerMask enemyLayer;

    public string monsterState;

    //attack
    public float attackRate;
    public GameObject[] moves;


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
    public void CreateMoveset(Vector3 spawnPoint)
    {
        GameObject moveset = Instantiate(movesetUI, new Vector3(spawnPoint.x, -4.33f, spawnPoint.z), Quaternion.identity);
        moveset.GetComponent<Moveset>().monsterController = this;
    }
    public void UpdateHealthbar()
    {
        healthbar.transform.localScale = new Vector3(monster.currentHealth / monster.baseHealth, 0.1f, 1);
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
                    GameObject move = Instantiate(moves[0], attackPoint.transform.position, Quaternion.identity);
                    move.GetComponent<MoveController>().target = nearestEnemy;
                    move.GetComponent<MoveController>().monster = monster;
                    break;
            }
        }

    }

}
