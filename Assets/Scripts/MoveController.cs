using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public GameObject target;
    public Monster monster;
    void Start()
    {
        
    }

    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, monster.moveSet[monster.activeMoveIndex].moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(monster.GetDamage());
            Destroy(gameObject);

        }
    }


}
