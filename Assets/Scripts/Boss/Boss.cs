using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Damageable
{
    private State currentState;                             // Current State of Boss

    // Boss Movement
    private Rigidbody eRigidBody;                           // Reference to RigidBody of Boss
    public bool isMoving;                                   // Bool to determine if Player is moving

    // Reference to Player
    public Transform target;                                // Boss target (Player)

    // Boss Attack
    [SerializeField] private GameObject BossSpell;          // Reference to BossSpell GameObject
    [SerializeField] private float zOffset = 10.0f;         // zOffset for Boss Ranged Attacks
    [SerializeField] private float yOffset = 10.0f;         // yOffset for Boss Ranged Attacks
    private Vector3 attackDir;                              // Direction of Player Attack            
    [SerializeField] private float pushBackForce = 15.0f;   // Push Boss back after attacking Player with melee
    public bool meleeAttack;                                // Bool to determine when to Melee attack 
    public bool rangeAttack;                                // Bool to determine when to Ranged attack
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual

    // Boss Material
    private Material eMaterial;                             // Reference to Boss Material
    private Color BossColor;                                // Original Boss Color
    private bool flash;                                     // Bool to determine when to change fadeAmount
    [SerializeField] private float colorChange = 0.5f;      // Change color every 0.5 seconds

    // Reap quantities of Boss
    public bool canReap;                                    // Boss is now weakend enough and can be reaped
    public bool waitingForReap;                             // Whether Boss is waiting to be Reaped

    // Tags for possible Boss interaction
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
        // Get reference to Boss RigidBody
        eRigidBody = GetComponent<Rigidbody>();
        // // Get reference to Boss Material and color
        // eMaterial = GetComponent<MeshRenderer>().material;
        // BossColor = eMaterial.color;
        // canReap, rangeAttack and meleeAttack start as true
        canReap = true;
        meleeAttack = true;
        rangeAttack = true;
        // Set current state to IDLE
        SetState(new IdleBossState(this));
        // Take Damage
        // StartCoroutine(LoseHealth());
        // Change color of Boss
        // StartCoroutine(HealthChange());
    }


    // Why is this in fixed update?
    void FixedUpdate()
    {
        // Get Health of Boss to determine color
        SetBossHealthState();
        // Call Action() for currentState 
        if (target)
        {
            currentState.Action();
        }
    }

    // Rotate Boss toward Player
    public void TurnBoss()
    {
        // Get vector pointing towards Player
        Vector3 direction = target.position - transform.position;
        // Set attackDir to direction
        attackDir = direction.normalized;
        direction.y = 0;
        // Get Quaternion to rotate towards Player
        Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
        // Rotate Boss, use Slerp to make Rotation gradual
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationDamp);
    }

    // Move Boss toward Player
    public void MoveBoss()
    {
        // Set isMoving to true
        isMoving = true;
        // Move Boss in direction they are facing using RigidBody
        eRigidBody.MovePosition(transform.position + transform.forward * characterStats.speed * Time.deltaTime);
    }

    // Stop Boss Movement
    public void StopBoss()
    {
        // Set isMoving to false
        isMoving = false;
        // Set velocity of Boss
        eRigidBody.velocity = Vector3.zero;
    }

    // ReapBoss Destroys the Boss Game Object and returns the type of Boss and modifier boost
    // Can only get a modifier boost if Boss is waiting for reap, being in ReapBossState
    public (string BossType, int mod) ReapBoss()
    {
        if (waitingForReap)
        {
            // If Boss is ATTACK type, return "Attack" and modStat
            if (characterStats.characterType == CharacterStats.CharacterType.ATTACK)
            {
                return ("Attack", characterStats.modStatBoost);
            }
            // Else if Boss is DEFENSE type, return "Defense" and modStat
            else if (characterStats.characterType == CharacterStats.CharacterType.DEFENSE)
            {
                return ("Defense", characterStats.modStatBoost);
            }
            // Else if Boss is SPEED type, return "Speed" and modStat
            else if (characterStats.characterType == CharacterStats.CharacterType.SPEED)
            {
                return ("Speed", characterStats.modStatBoost);
            }
            // Else, if Boss is none of those types, return Health and modStat
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

    // Used to create Boss Ranged Attack
    public void RangedAttack()
    {
        // Offset is transform.forward
        // Instantiate Spell
        GameObject spellObj = Instantiate(BossSpell, transform.position + (transform.forward * zOffset) - (transform.up * yOffset), Quaternion.identity);
        // Get Reference to BossSpell Component
        EnemySpell spell = spellObj.GetComponent<EnemySpell>();
        // Set direction and speed of spell
        spell.FireSpell(attackDir, characterStats.attack_speed, characterStats.attack);
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
    }

    // GetPlayer returns reference to Player transform
    public void GetPlayer()
    {
        // Else find GameObject with Player tag
        if (GameObject.FindGameObjectWithTag(PLAYER))
        {
            target = GameObject.FindGameObjectWithTag(PLAYER).transform;
        }
        // If Player not in scene, target is null
        else
        {
            target = null;
        }
    }

    // SetState sets the current State of Boss
    public void SetState(State state)
    {
        // If currentState is already assigned, then call OnStateExit for that State
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        // Set currentState to new state
        currentState = state;
        // If currentState is now not null, call OnStateEnter for that State
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    // Change Boss Material based on health
    private void SetBossHealthState()
    {
        // If health is less than 50 but greater than 25, the Boss turns yellow
        // Call CurrentHealthState
        if (CurrentHealthState() == 1)
        {
            if (flash)
            {
                // eMaterial.color = BossColor;
            }
            else
            {
                Color yellow = Color.yellow;
                yellow.a = 0.5f;
                // eMaterial.color = yellow;
            }
        }
        // If health is less than 25, Boss turns red
        else if (CurrentHealthState() == 2)
        {
            if (flash)
            {
                eMaterial.color = BossColor;
            }
            else
            {
                Color red = Color.red;
                red.a = 0.5f;
                // eMaterial.color = red;
            }
        }
    }

    // ReapBossTimer starts ReapTimer so Player has 10 seconds to reap Boss
    public void ReapBossTimer()
    {
        // Start ReapTimer Coroutine
        StartCoroutine(ReapTimer());
    }

    // KillBoss knocks the Boss down and Stops all Coroutines and sets attack to false
    public void KillBoss()
    {
        meleeAttack = false;
        rangeAttack = false;
        eRigidBody.constraints = RigidbodyConstraints.None;
        eRigidBody.AddForce(-pushBackForce * rotationDamp * transform.forward, ForceMode.Impulse);
        eRigidBody.velocity = Vector3.zero;
        StartCoroutine(BossKilled());
    }

    private void OnCollisionEnter(Collision col)
    {
        // If collide with a Player, they take damage and then they move back
        if (col.transform.CompareTag(PLAYER))
        {
            meleeAttack = false;
            eRigidBody.AddForce(-pushBackForce * transform.forward, ForceMode.Impulse);
            StartCoroutine(MeleeAttackTimer());
        }
        // If collide with another Boss, then move to left or right
        if (col.transform.CompareTag(ENEMY))
        {
            int index = Random.Range(1, 3);
            if (index == 1)
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
        // After Boss collides with Player, they stop moving, and Call AttackTimer for 2 seconds
        if (col.transform.CompareTag(PLAYER))
        {
            // Debug.Log("Player collision");
            var player = col.gameObject.GetComponent<Player>();
            player.TakeDamage(characterStats.attack);
            StopBoss();
        }
        // After Boss collides with Another Boss, they stop moving
        if (col.transform.CompareTag(ENEMY))
        {
            StopBoss();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If Boss is hit by Player Scythe, they take damage or can be reaped if waiting for Reap
        if (other.CompareTag(SCYTHE))
        {
            var player = other.GetComponentInParent<Player>();

            // If Boss is waitingForReap, then they can call the ReapBoss Function
            // TODO: Add in Reap Animation and adding modifier 
            if (waitingForReap)
            {
                Debug.Log("I T S  R E A P I N'  T I M E");
                reapedEvent.Raise();
                StopBoss();
                KillBoss();
            }
            // Else, the Boss just takes normal damage
            else
            {
                if (player)
                {
                    TakeDamage(player.characterStats.attack);
                }

            }
        }
    }

    // Timer between ranged attacks for Boss
    private IEnumerator RangeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(characterStats.rangedSpawn);
        rangeAttack = true;
    }

    // Timer between attacks for Boss
    private IEnumerator MeleeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(characterStats.meleeSpawn);
        print("Attack");
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

    // Change color of Boss every half-second seconds based on current health
    private IEnumerator HealthChange()
    {
        while (GetHealth() != 0)
        {
            // Every half-second set flash to true to change color
            yield return new WaitForSeconds(colorChange);
            flash = !flash;
        }
    }

    // Make Boss wait 10 seconds in Reaped State
    private IEnumerator ReapTimer()
    {
        // After 10 seconds, Boss can no longer be reaped and returns to previous state
        yield return new WaitForSeconds(characterStats.reapTime);
        canReap = false;
        waitingForReap = false;
        //StartCoroutine(LoseHealth());
    }

    // Destroy Boss after 3 seconds
    private IEnumerator BossKilled()
    {
        deathEvent.Raise();

        // After 3 seconds, destroy Boss Game Object
        yield return new WaitForSeconds(characterStats.rangedSpawn);
        Destroy(gameObject);
    }
}
