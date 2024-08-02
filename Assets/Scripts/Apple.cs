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
        if (!pickedup && collision.transform.tag == "Enemy" && collision.gameObject.GetComponent<EnemyController>().appleCarrypoint != null)
        {
            pickedup = true;
            gameObject.transform.SetParent(collision.gameObject.GetComponent<EnemyController>().appleCarrypoint.transform);
            transform.position = collision.gameObject.GetComponent<EnemyController>().appleCarrypoint.transform.position;
        }
    }


}
