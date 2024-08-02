using System.Collections;
using System.Collections.Generic;
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
    public GameObject healthbar;
    void Start()
    {
        
    }

    public void UpdateHealthbar()
    {
        healthbar.transform.localScale = new Vector3(thisCreature.currentHealth/thisCreature.baseHealth, 0.1f, 1);
    }

    public void KillSelf()
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

            if (direction.x < 0)
            {

                SetAnimationState("IsWalkingLeft", true);
                SetAnimationState("IsWalkingRight", false);


            }
            else if (direction.x > 0)
            {
                SetAnimationState("IsWalkingLeft", false);
                SetAnimationState("IsWalkingRight", true);
            }
            else
            {
                SetAnimationState("IsWalkingLeft", false);
                SetAnimationState("IsWalkingRight", false);
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
                levelManager.numberOfApples--;
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
}
