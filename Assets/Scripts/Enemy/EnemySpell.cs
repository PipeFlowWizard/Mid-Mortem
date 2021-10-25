using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    // Reference to RigidBody component of EnemySpell
    private Rigidbody spellRigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        // Get reference to RigidBody component
        spellRigidBody = GetComponent<Rigidbody>();
    }

    // Set direction of Spell movement
    public void SetDirection(Vector3 direction, float spellSpeed)
    {
        spellRigidBody.velocity = direction * spellSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroy GameObject when it collides with something
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
