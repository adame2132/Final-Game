using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    private GameObject player;
    public LayerMask whatIsGorund, whatIsPlayer, whatIsBorder, whatIsObject;
    public Vector3 patrolPoint;
    private bool patrolPointSet;
    public float patrolPointRange = 100f;
    public int attackCoolDown = 2;
    public bool attacking;
    private float sightRange;
    private float attackRange;
    private bool playerInSightRange, playerInAttackRange;
    // im going to assign these depending if its a wizard or skeletin so i need to find the tag of the owner
    public float moveSpeed = 5.0f;
    public float smoothTime = 0.03f;
    public Vector3 velocity = Vector3.zero;
    private Animator anim;
    private bool isWalking; 
    private int isWalkingHash;
    private Vector3 playerPosition;
    private AttackScript attackScript;
    public bool isAlive;
    private PauseMenuController ui;
    private bool notifiedDeath = false;
    

    private void Awake(){
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsMoving");
        attackScript = GetComponent<AttackScript>();
        Debug.Log("im a " + transform.tag);
        if(transform.tag == "Skeleton"){
            attackRange = 0.9f;
            sightRange = 20f;
        }
        else if(transform.tag == "Wizard"){
            attackRange = 20f;
            sightRange = 30f;
        }
        isAlive = true;
        ui = GameObject.FindGameObjectWithTag("PauseControls").GetComponent<PauseMenuController>();
    }
    private void Patroling(){
        if(!patrolPointSet){
            Debug.Log("Point not set");
            createPatrolPoint();
        }
        if(patrolPointSet){
            Debug.Log("Point set moving there!");
            Vector3 direction = (patrolPoint - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, 10f, whatIsBorder| whatIsObject)) // Detect if a border is ahead
            {
                Debug.Log("Border or Object detected in path, picking a new point");
                patrolPointSet = false; // Pick a new patrol point
                return;
            }
            transform.position = Vector3.SmoothDamp(transform.position, patrolPoint, ref velocity, smoothTime, moveSpeed);
            if (direction != Vector3.zero) {
                // if(transform.tag == "Skeleton"){
                //     transform.localScale = new Vector3(0.3, 0.3, 0.3);

                // }
                // else if(transform.tag == "Wizard"){
                //     transform.localScale = new Vector3(1, 1, 1);
                // }
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
            }
            anim.SetBool(isWalkingHash, true);
        }
        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;
        if(distanceToPatrolPoint.magnitude <= 1f){
            patrolPointSet = false;
            Debug.Log("there!");
        }
    }
    private void createPatrolPoint(){
        Debug.Log("Create was called");
        float pointx = Random.Range(-patrolPointRange, patrolPointRange);
        float pointz = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(transform.position.x + pointx, transform.position.y, transform.position.z + pointz);
        if (Physics.Raycast(patrolPoint, -transform.up, 2f, whatIsGorund)){
            Debug.Log("Point looks like: " + patrolPoint);
            patrolPointSet = true;
        }
    }

    private void chasePlayer(){
        Debug.Log("chasing player");
        anim.SetBool(isWalkingHash, true);
        transform.LookAt(player.transform);
        playerPosition = player.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime, moveSpeed);        
    }
    private void AttackPlayer(){
        // anim.SetBool(isWalkingHash, false);
        // Debug.Log("Attacking Player");
        Debug.Log("WizardDebug: wizard is attacking");
        transform.LookAt(player.transform);
        attackScript.Attack(gameObject);
        // StartCoroutine(AttackCooldown());

    }

    private void Update(){
        if(isAlive){
            if(attacking){
                return;
            }
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if(!playerInSightRange & !playerInAttackRange){
                Patroling();
            }
            else if (playerInSightRange & !playerInAttackRange){
                chasePlayer();
            }
            else if(playerInSightRange & playerInAttackRange){
                if(!attacking){
                    attacking = true;
                    AttackPlayer();
                }
                else{
                    Debug.Log("Im cooling down");
                }
            } 

        }
        else if(!isAlive){
            if(notifiedDeath){
                return;
            }
            else if(!notifiedDeath){
                notifiedDeath = true;
                ui.killedEnemy();
                StartCoroutine(HandleDeath());
                return;
            }
            

        }
    }
    public void AttackCooldown(){
        StartCoroutine(Cooldown());
    }
    private IEnumerator Cooldown(){
        Debug.Log("WizardDebug: Cooldown started");
        yield return new WaitForSeconds(attackCoolDown); // Wait for the cooldown period
        attacking = false; // Allow the monster to attack again
        Debug.Log(" WizardDebug: Cooldown ended, ready to attack again");
    }
    
    IEnumerator HandleDeath()
    {
        // anim.SetTrigger("Death");
        yield return new WaitForSeconds(5); // Wait for 2 seconds before pausing the game
        gameObject.SetActive(false);
    }
}