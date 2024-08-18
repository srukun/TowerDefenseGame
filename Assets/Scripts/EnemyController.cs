using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public GameObject appleCarrypoint;
    public Animator animator;
    public LevelManager levelManager;
    public bool isActive;
    public Monster thisCreature;
    public GameObject[] healthbar;
    void Start()
    {
        
    }

    public void UpdateHealthbar()
    {
        float healthScale = thisCreature.currentHealth / thisCreature.baseHealth;
        healthbar[0].transform.localScale = new Vector3(healthScale, 0.1f, 2);
        healthbar[1].transform.localScale = new Vector3(healthScale, 0.1f, 1);
    }

    public void RemoveEnemyFromScene()
    {
        if(appleCarrypoint.transform.childCount > 0)
        {
            appleCarrypoint.GetComponentInChildren<Apple>().pickedup = false;
            appleCarrypoint.transform.SetParent(null);
            appleCarrypoint.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        if (!levelManager.gameIsPaused)
        {
            UpdateHealthbar();
            Move();
        }
    }
    public void Move()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, thisCreature.speed * Time.deltaTime);



            // Set the animation state
            animator.SetBool("Leftward", direction.x < 0);
            animator.SetBool("Rightward", direction.x > 0);
            animator.SetBool("Forward", direction.y > 0);
            animator.SetBool("Backward", direction.y < 0);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            if(appleCarrypoint.transform.childCount > 0)
            {
                levelManager.numberOfEmerald--;
            }
            Destroy(gameObject);
        }
    }
    void SetAnimationState(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }
    public void TakeDamage(float damage)
    {
        thisCreature.TakeDamage(damage);
        if(thisCreature.currentHealth <= 0)
        {
            RemoveEnemyFromScene();
        }
    }
}
