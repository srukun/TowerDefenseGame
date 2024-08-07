using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetAnimationDirection(Transform nearestEnemy)
    {

        Vector2 direction = nearestEnemy.position - transform.position;
        SetAllAnimatorFalse();
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetBool("Rightward", true);
            }
            else
            {
                animator.SetBool("Leftward", true);
            }
        }
        else
        {
            if (direction.y < 0)
            {
                animator.SetBool("Forward", true);
            }
            else
            {
                animator.SetBool("Backward", true);
            }
        }
    }
    public void SetAllAnimatorFalse()
    {
        animator.SetBool("Rightward", false);
        animator.SetBool("Leftward", false);
        animator.SetBool("Forward", false);
        animator.SetBool("Backward", false);

    }
}
