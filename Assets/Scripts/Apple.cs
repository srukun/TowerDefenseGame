using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public bool pickedup;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pickedup && collision.transform.tag == "Enemy" && !collision.gameObject.GetComponent<EnemyController>().carryingEmerald)
        {
            pickedup = true;
            collision.gameObject.GetComponent<EnemyController>().PickUpEmerald(gameObject);
        }
    }

}
