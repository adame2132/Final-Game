using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    private AudioScript audioManager;
    private HealthBarScript healthbar;
    private Animator anim;
    private int isWalkingHash; 
    private bool isWalking;
    public float speed = 5.0f;
    public float turnSpeed = 3.0f;
    private Vector2 movePlayer;
    public float cameraSpeed = 1.0f;
    private Vector2 moveCamera;
    public GameObject pausePannel, gameOverPannel, healthBarUI, enemyCountInfo;
    private bool gamePaused = false;
    private PlayerInput playerInput;
    public GameObject pause_first;
    public bool isAlive;
    public  GameObject follow;
    private Quaternion cameraRestPos;
    public float resetSpeed = 2.0f;  
    public float playerRotationSpeed ;
    public float minCameraY = -1477.0f;
    public float maxCameraY = -1381.5f;
    private PauseMenuController pauseMenu;
    public bool canPlayerFoward; 

    


    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
        cameraRestPos = follow.transform.rotation;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseControls").GetComponent<PauseMenuController>();
        canPlayerFoward = true;
    }
    private void Start(){
        anim = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsMoving");
        // isAliveHash = Animator.StringToHash("isAlive");
        playerInput = GetComponent<PlayerInput>();
        healthbar= GetComponent<HealthBarScript>();
        isAlive = true;
        Debug.Log("current action map is: " + playerInput.currentActionMap.name);
        // swordMesh = sword.GetComponent<MeshCollider>();
        // at the start it should be off 

    }
    private void OnPause(){
        gamePaused = !gamePaused;
        if (gamePaused == true){
            Debug.Log("player pressed pause");
            healthBarUI.SetActive(false);
            enemyCountInfo.SetActive(false);
            pausePannel.SetActive(true);
            Time.timeScale = 0;
            playerInput.SwitchCurrentActionMap("Menu");
            Debug.Log("current action map is: " + playerInput.currentActionMap.name);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pause_first);

        }
        else{
            Debug.Log("player pressed unpause");
            healthBarUI.SetActive(true);
            enemyCountInfo.SetActive(true);
            pausePannel.SetActive(false);
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap("Player");
            Debug.Log("current action map is: " + playerInput.currentActionMap.name);
            pauseMenu.resetMenu();

        }
        
    }
    private void OnMove(InputValue value){
        movePlayer = value.Get<Vector2>();
        // Debug.Log("player is moving: " + movePlayer);
    }
    private void OnAttack1(){
        // Debug.Log("Player pressed square");
        audioManager.playSFX(audioManager.SwordSwingAudio);
        anim.SetTrigger("Attack1");

    }
    private void OnAttack2(){
        // Debug.Log("Player pressed triangle");
        audioManager.playSFX(audioManager.SwordSwingAudio);
        anim.SetTrigger("Attack2");

    }
    private void OnJump(){
        Debug.Log("Player pressed x");
        anim.SetTrigger("Jump");
        // isAlive = healthbar.TakeDamage(20);
    }
    private void MoveLogic(){
        float xCal = movePlayer.x * speed * Time.fixedDeltaTime;
        float zCal = movePlayer.y * speed * Time.fixedDeltaTime;
        Vector3 new_move = new Vector3 (xCal, 0, zCal);
        if(new_move.x != 0 || new_move.z != 0){
            if(!canPlayerFoward && new_move.z > 0){
                new_move.z = 0;
            }
            transform.Translate(new_move);
            anim.SetBool(isWalkingHash, true);
        }
        else{
            // Debug.Log("Player stopped moving");
            anim.SetBool(isWalkingHash,false);
        }
    }
    private void OnRotateCamera(InputValue value){
        // Debug.Log("Player is attempting to move the camera");
        moveCamera = value.Get<Vector2>();
        // moveCamera = value.Get<Vector2>();
        // float yCal = moveCamera.y * cameraSpeed * Time.fixedDeltaTime;
        // Vector3 new_camera_pos = new Vector3(follow.transform.position.x, yCal, follow.position.z);
        // follow.transform.rotation = new_camera_pos;
    }
   private void cameraLogic()
{
    // Only update if there is input from the player
    if (moveCamera.x != 0.0f || moveCamera.y != 0.0f)
    {
        // If the player is walking, update the camera smoothly
        if (isWalking)
        {
            Debug.Log($"Camera move input looks like X: {moveCamera.x}");
            // Calculate camera's Y-axis rotation change based on input
            float yChange = moveCamera.x; 
            //* playerRotationSpeed * Time.fixedDeltaTime;
            Vector3 currentR = follow.transform.eulerAngles;
            float yCal = currentR.y + yChange;

            // Directly update the rotation without clamping
            Quaternion target = Quaternion.Euler(currentR.x, yCal, currentR.z);
            follow.transform.rotation = target;
            //Quaternion.Slerp(follow.transform.rotation, target, Time.fixedDeltaTime*4);
            cameraRestPos = follow.transform.rotation;

            // Update the player's rotation to match camera's direction
            Vector3 followDirection = follow.transform.forward;
            followDirection.y = 0.0f; // Ensure we only rotate on the XZ plane
            followDirection.Normalize();

            if (followDirection != Vector3.zero)
            {
                // Smoothly rotate the player to face the camera's direction
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(followDirection), Time.fixedDeltaTime * playerRotationSpeed);
            }
        }
        else
        {
            // Debug.Log("Player is not moving, giving camera full 360° rotation control");
            float yRotationChange = moveCamera.x * cameraSpeed * Time.deltaTime;
            Vector3 currentRotation = follow.transform.eulerAngles;
            float newYRotation = currentRotation.y + yRotationChange;
            follow.transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
        }
    }
    else
    {
        follow.transform.rotation = Quaternion.Slerp(follow.transform.rotation, cameraRestPos, Time.fixedDeltaTime * resetSpeed);
    }
}
private void checkIfMoving(){
    isWalking = anim.GetBool(isWalkingHash);
}
    private void FixedUpdate(){
            if (!isAlive)
            {
                StartCoroutine(HandleDeath());
            }
            else
            {
                checkIfMoving();
                MoveLogic();
                cameraLogic();
            }
        }

    IEnumerator HandleDeath()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(2);
        Time.timeScale = 0; 
        Debug.Log("Game paused after death.");
        healthBarUI.SetActive(false);
        enemyCountInfo.SetActive(false);
        gameOverPannel.SetActive(true);
    }
    public void victory(){
        anim.SetTrigger("Victory");
    }
    
    
    
    
    
    
    
    //menu 
    private void OnUp(){
        Debug.Log("pressed up on dpad");
    }
    private void OnDown(){
        Debug.Log("pressed down on dpad");
    }
    private void OnLeft(){
        Debug.Log("pressed left on dpad");
    }
    private void OnRight(){
        Debug.Log("pressed right on dpad");
    }
    private void OnConfrim(){
        Debug.Log("pressed x for menu modal");
    }
    private void OnCancel(){
        Debug.Log("pressed o for menu modal");
        pauseMenu.oPressed();
    }

}
