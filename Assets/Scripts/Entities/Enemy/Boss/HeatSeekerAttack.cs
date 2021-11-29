using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekerAttack : MonoBehaviour
{
    // Reference to Rigidbody of HeatSeeker attackDamage
    private Rigidbody _rigidbody;
    // Reference to Player Transform
    private Transform player;
    public float speed;
    public int damage;
    public int lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to RigidBody
        _rigidbody = GetComponent<Rigidbody>();
        // Get reference to Player transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Start lifetime of spell
        StartCoroutine(SpellLifetime());
    }

    // Update is called once per frame
    void Update()
    { 
        // If Player still in scene, continue to follow Player with attackDamage
        if(player != null)
        {
            TurnSpell();
            MoveSpell();
        }
        // Else destroy it
        else
        {
            Destroy(this);
        }
    }

    // TurnSpell makes Spell turn towards Player
    private void TurnSpell()
    {
        // Get vector pointing towards Player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        // Rotate Enemy is direction is not zero
        if (direction != Vector3.zero)
        {
            // Get Quaternion to rotate towards Player
            Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
            // Rotate Enemy, use Slerp to make Rotation gradual
            transform.rotation = rotate;
        }
    }

    // MoveSpell moves Spell towards Player
    private void MoveSpell()
    {
        // Move Enemy in direction they are facing using RigidBody
        _rigidbody.MovePosition(transform.position + (transform.forward * speed) * Time.deltaTime);
    }

    // HeatSeeker attackDamage only lasts for a certain amount of time
    private IEnumerator SpellLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    // When HeatSeeker collides with object, it is destroyed and deals damage if Player
    private void OnTriggerEnter(Collider col)
    {
        // If col is Player, then deal damage
        if (col.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            print("Destroy");
            Destroy(gameObject);
        }
        else if(!col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
