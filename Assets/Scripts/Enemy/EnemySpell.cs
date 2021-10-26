using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    // Reference to RigidBody component of EnemySpell
    private Rigidbody spellRigidBody;
    // Attack power of spell, set by Enemy
    private int attackPower;
    // Lifetime of spell is 15 seconds
    [SerializeField] private int lifeTime = 15;
    // Tag for Player Interaction
    private string PLAYER = "PlayerMesh";

    // Start is called before the first frame update
    void Awake()
    {
        // Get reference to RigidBody component
        spellRigidBody = GetComponent<Rigidbody>();
    }

    // Set direction of Spell movement
    public void FireSpell(Vector3 direction, float spellSpeed, int attack)
    {
        spellRigidBody.velocity = direction * spellSpeed;
        attackPower = attack;
        // EnemySpell lasts for 15 seconds
        StartCoroutine(EnemySpellLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When EnemySpell collides with PLAYER, it deals damage, else it is destroyed
    private void OnTriggerEnter(Collider col)
    {
        // If col has PLAYER tag, then it deals attack damage
        if(col.tag == PLAYER)
        {
            // Player component is found in Parent Player object
            Player player = col.GetComponentInParent<Player>();
            player.TakeDamage(attackPower);
        }
        // Else, Start Coroutine to destroy EnemySpell after 15 seconds
        else
        {
            print(col.tag);
            Destroy(gameObject);
        }
    }

    // EnemySpellLifetime will destroy EnemySpell after 15 seconds
    private IEnumerator EnemySpellLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
