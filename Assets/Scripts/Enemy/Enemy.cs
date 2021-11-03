using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// General Enemy class for Enemies
public class Enemy : Damageable
{
    private State currentState;                             // Current State of Enemy

    // Enemy Movement
    private Rigidbody eRigidBody;                           // Reference to RigidBody of Enemy
    public bool isMoving;                                   // Bool to determine if Player is moving

    // Reference to Player
    public Transform target;                                // Enemy target (Player)

    // Enemy Attack
    [SerializeField] private GameObject enemySpell;         // Reference to EnemySpell GameObject
    [SerializeField] private float pushBackForce = 15.0f;   // Push Enemy back after attacking Player with melee
    public bool meleeAttack;                                // Bool to determine when to Melee attack 
    public bool rangeAttack;                                // Bool to determine when to Ranged attack
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual

    // Enemy Material
    private Material eMaterial;                             // Reference to Enemy Material
    private Color enemyColor;                               // Original Enemy Color
    private bool flash;                                     // Bool to determine when to change fadeAmount
    [SerializeField] private float colorChange = 0.5f;      // Change color every 0.5 seconds
    
    // Reap quantities of Enemy
    public bool canReap;                                    // Enemy is now weakend enough and can be reaped
    public bool waitingForReap;                             // Whether enemy is waiting to be Reaped

    // Tags for possible Enemy interaction
    private const string PLAYER = "Player";
    private const string ENEMY = "Enemy";
    private const string SCYTHE = "PlayerHurtBox";

    [Header("Events")] 
    public GameEvent deathEvent;
    public GameEvent reapedEvent;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Get Transform of Player, Target
        GetPlayer();
        // Get reference to Enemy RigidBody
        eRigidBody = GetComponent<Rigidbody>();
        // Get reference to Enemy Material and color
        eMaterial = GetComponent<MeshRenderer>().material;
        enemyColor = eMaterial.color;
        // canReap, rangeAttack and meleeAttack start as true
        canReap = true;
        meleeAttack = true;
        rangeAttack = true;
        // Set current state to IDLE
        SetState(new IdleEnemyState(this));
        // Take Damage
        // StartCoroutine(LoseHealth());
        // Change color of Enemy
        // StartCoroutine(HealthChange());
    }

    
    // Why is this in fixed update?
    void FixedUpdate()
    {
        // Get Health of Enemy to determine color
        SetEnemyHealthState();
        // Call Action() for currentState 
        if (target)
        {
            currentState.Action();
        }
    }

    // Rotate Enemy toward Player
    public void TurnEnemy()
    {
        // Get vector pointing towards Player
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        // Get Quaternion to rotate towards Player
        Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
        // Rotate Enemy, use Slerp to make Rotation gradual
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationDamp);
    }

    // Move Enemy toward Player
    public void MoveEnemy()
    {
        // Set isMoving to true
        isMoving = true;
        // Move Enemy in direction they are facing using RigidBody
        eRigidBody.MovePosition(transform.position + transform.forward * characterStats.speed * Time.deltaTime);
    }

    // Stop Enemy Movement
    public void StopEnemy()
    {
        // Set isMoving to false
        isMoving = false;
        // Set velocity of Enemy
        eRigidBody.velocity = Vector3.zero;
    }

    // ReapEnemy Destroys the Enemy Game Object and returns the type of Enemy and modifier boost
    // Can only get a modifier boost if Enemy is waiting for reap, being in ReapEnemyState
    public (string enemyType, int mod) ReapEnemy()
    {
        if (waitingForReap)
        {
            // If Enemy is ATTACK type, return "Attack" and modStat
            if (characterStats.characterType == CharacterStats.CharacterType.ATTACK)
            {
                return ("Attack", characterStats.modStatBoost);
            }
            // Else if Enemy is DEFENSE type, return "Defense" and modStat
            else if (characterStats.characterType == CharacterStats.CharacterType.DEFENSE)
            {
                return ("Defense", characterStats.modStatBoost);
            }
            // Else if Enemy is SPEED type, return "Speed" and modStat
            else if (characterStats.characterType == CharacterStats.CharacterType.SPEED)
            {
                return ("Speed", characterStats.modStatBoost);
            }
            // Else, if Enemy is none of those types, return Health and modStat
            else
            {
                return ("Health", characterStats.modStatBoost);
            }
        }
        else
        {
            return ("Null", 0);
        }
    }

    // Used to create Enemy Ranged Attack
    public void RangedAttack()
    {
        // Offset is transform.forward
        // Instantiate Spell
        GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward, Quaternion.identity);
        // Get Reference to EnemySpell Component
        EnemySpell spell = spellObj.GetComponent<EnemySpell>();
        // Set direction and speed of spell
        spell.FireSpell(transform.forward, characterStats.attack_speed, characterStats.attack);
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
    }

    // GetPlayer returns reference to Player transform
    public void GetPlayer()
    {
        // Else find GameObject with Player tag
        if(GameObject.FindGameObjectWithTag(PLAYER))
        {
            target = GameObject.FindGameObjectWithTag(PLAYER).transform;
        }
        // If Player not in scene, target is null
        else
        {
            target = null;
        }
    } 

    // SetState sets the current State of Enemy
    public void SetState(State state)
    {
        // If currentState is already assigned, then call OnStateExit for that State
        if(currentState != null)
        {
            currentState.OnStateExit();
        }
        // Set currentState to new state
        currentState = state;
        // If currentState is now not null, call OnStateEnter for that State
        if(currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    // Change Enemy Material based on health
    private void SetEnemyHealthState()
    {
        // If health is less than 50 but greater than 25, the Enemy turns yellow
        // Call CurrentHealthState
        if(CurrentHealthState() == 1)
        {
            if (flash)
            {
                eMaterial.color = enemyColor;
            }
            else
            {
                Color yellow = Color.yellow;
                yellow.a = 0.5f;
                eMaterial.color = yellow;
            }
        }
        // If health is less than 25, Enemy turns red
        else if(CurrentHealthState() == 2)
        {
            if (flash)
            {
                eMaterial.color = enemyColor;
            }
            else
            {
                Color red = Color.red;
                red.a = 0.5f;
                eMaterial.color = red;
            }
        }
    }

    // ReapEnemyTimer starts ReapTimer so Player has 10 seconds to reap Enemy
    public void ReapEnemyTimer()
    {
        // Start ReapTimer Coroutine
        StartCoroutine(ReapTimer());
    }

    // KillEnemy knocks the enemy down and Stops all Coroutines and sets attack to false
    public void KillEnemy()
    {
        meleeAttack = false;
        rangeAttack = false;
        eRigidBody.constraints = RigidbodyConstraints.None;
        eRigidBody.AddForce(-pushBackForce * rotationDamp * transform.forward, ForceMode.Impulse);
        eRigidBody.velocity = Vector3.zero;
        StartCoroutine(EnemyKilled());
    }

    private void OnCollisionEnter(Collision col)
    {
        // If collide with a Player, they take damage and then they move back
        if (col.transform.CompareTag(PLAYER))
        {
            meleeAttack = false;
            eRigidBody.AddForce(-pushBackForce * transform.forward, ForceMode.Impulse);
        }
        // If collide with another Enemy, then move to left or right
        if(col.transform.CompareTag(ENEMY))
        {
            int index = Random.Range(1, 3);
            if(index == 1)
            {
                // Move right
                eRigidBody.AddForce(pushBackForce * transform.right, ForceMode.Impulse);
            }
            else
            {
                // Move left
                eRigidBody.AddForce(-pushBackForce * transform.right, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        // After Enemy collides with Player, they stop moving, and Call AttackTimer for 2 seconds
        if(col.transform.CompareTag(PLAYER))
        {
            // Debug.Log("Player collision");
            var player = col.gameObject.GetComponent<Player>();
            player.TakeDamage(characterStats.attack);
            StopEnemy();
            StartCoroutine(MeleeAttackTimer());
        }
        // After Enemy collides with Another Enemy, they stop moving
        if(col.transform.CompareTag(ENEMY))
        {
            StopEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If Enemy is hit by Player Scythe, they take damage or can be reaped if waiting for Reap
        if (other.CompareTag(SCYTHE))
        {
            var player = other.GetComponentInParent<Player>();
            
            // If Enemy is waitingForReap, then they can call the ReapEnemy Function
            // TODO: Add in Reap Animation and adding modifier 
            if (waitingForReap)
            {
                Debug.Log("I T S  R E A P I N'  T I M E");
                reapedEvent.Raise();
                StopEnemy();
                KillEnemy();
            }
            // Else, the Enemy just takes normal damage
            else
            {
                if (player)
                {
                    TakeDamage(player.characterStats.attack);
                }

            }
        }
    }

    // Timer between ranged attacks for Enemy
    private IEnumerator RangeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(characterStats.rangedSpawn);
        rangeAttack = true;
    }

    // Timer between attacks for Enemy
    private IEnumerator MeleeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(characterStats.meleeSpawn);
        meleeAttack = true;
    }

    // Take 10 damage every 2 seconds
    public IEnumerator LoseHealth()
    {
        while (!waitingForReap)
        {
            // Every 2 seconds take damage
            yield return new WaitForSeconds(characterStats.meleeSpawn);
            TakeDamage(10);
        }
    }

    // Change color of Enemy every half-second seconds based on current health
    private IEnumerator HealthChange()
    {
        while (GetHealth() != 0)
        {
            // Every half-second set flash to true to change color
            yield return new WaitForSeconds(colorChange);
            flash = !flash;
        }
    }

    // Make Enemy wait 10 seconds in Reaped State
    private IEnumerator ReapTimer()
    {
        // After 10 seconds, Enemy can no longer be reaped and returns to previous state
        yield return new WaitForSeconds(characterStats.reapTime);
        canReap = false;
        waitingForReap = false;
        //StartCoroutine(LoseHealth());
    }

    // Destroy Enemy after 3 seconds
    private IEnumerator EnemyKilled()
    { 
        deathEvent.Raise();
            
        // After 3 seconds, destroy Enemy Game Object
        yield return new WaitForSeconds(characterStats.rangedSpawn);
        Destroy(gameObject);
    }
}
