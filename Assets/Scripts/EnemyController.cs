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
    public bool carryingEmerald;
    public GameObject emerald;
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
        if(carryingEmerald)
        {
            GetComponent<LevelManager>().numberOfEmerald--;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        if (!levelManager.gameIsPaused)
        {
            UpdateHealthbar();
            Move();
            CarryEmerald();
        }
    }
    public void Move()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, thisCreature.speed * Time.deltaTime);




            animator.SetBool("Leftward", false);
            animator.SetBool("Rightward", false);
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", false);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    animator.SetBool("Rightward", true);
                }
                else if (direction.x < 0)
                {
                    animator.SetBool("Leftward", true);
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    animator.SetBool("Backward", true);
                }
                else if (direction.y < 0)
                {
                    animator.SetBool("Forward", true);
                }
            }

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
    public void PickUpEmerald(GameObject emerald)
    {
        if (!carryingEmerald)
        {
            carryingEmerald = true;
            this.emerald = emerald;
            this.emerald.transform.SetParent(appleCarrypoint.transform, true);
            this.emerald.transform.position = appleCarrypoint.transform.position;
        }
    }
    public void CarryEmerald()
    {
        if (carryingEmerald)
        {
            this.emerald.transform.position = appleCarrypoint.transform.position;
        }
    }
}
