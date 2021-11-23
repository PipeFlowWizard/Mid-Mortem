using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFallAttack : MonoBehaviour
{
    // Reference to Rigidbody of Meteor
    private Rigidbody _rigidbody;
    public float height;
    public float speed;
    public int damage;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // MeteorCrash instantiates the Meteor above the passed position and drops it using gravity and speed
    public void MeteorCrash()
    {
        transform.position += (Vector3.up * height);
        _rigidbody.velocity = (Vector3.down * speed);
        
    }

    // When Meteor collides with object, it is destroyed and deals damage if Player
    private void OnTriggerEnter(Collider col)
    {
        // If col is Player, then deal damage
        if(col.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
