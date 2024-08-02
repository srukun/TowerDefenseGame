using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Monster monster;
    public MonsterController monsterController;
    public LayerMask enemyLayer;
    private Transform parentTransform;
    public float attackRate;
    private void Start()
    {
        parentTransform = transform.parent;
        monster = monsterController.monster;
        attackRate = monster.moveSet[monster.activeMoveIndex].attackRate;
    }

    private void Update()
    {
        attackRate -= Time.deltaTime;

        if (attackRate <= 0f)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(parentTransform.position, 4f, enemyLayer);

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
    }

    private Transform GetNearestEnemy(Collider2D[] enemies)
    {
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(parentTransform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                nearestEnemy = enemy.transform;
                minDistance = distance;
            }
        }

        return nearestEnemy;
    }

    private void Attack(GameObject enemy)
    {

    }


}
