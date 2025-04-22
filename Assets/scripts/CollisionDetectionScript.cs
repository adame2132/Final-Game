using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionScript : MonoBehaviour
{
    private  HealthBarScript hp;
    private bool isAlive;
    private Animator anim;
    private PlayerControls playerScript;
    void Start()
    {
        hp = GetComponent<HealthBarScript>();
        anim = GetComponent<Animator>();
        playerScript = GetComponent<PlayerControls>();
        isAlive = true;

        
    }
    void OnTriggerEnter(Collider hitter){
        if(hitter.tag == "Wizard Staff"){
            Debug.Log("Staff hit me");
            isAlive = hp.TakeDamage(10);
            DamageAction(isAlive);


        }
        else if(hitter.tag == "Claws"){
            Debug.Log("Claws hit me");
            isAlive = hp.TakeDamage(10);
            DamageAction(isAlive);
        }
        else if(hitter.tag == "0bject"){
            Debug.Log("Player intally touched an object");
            playerScript.canPlayerFoward = false;
        }
    }
    void OnTriggerExit(Collider hitter){
        if(hitter.tag == "0bject"){
            Debug.Log("player stoped touching object");
            playerScript.canPlayerFoward = true;
        }
    }

    void DamageAction(bool isAlive){
        if(!isAlive){
            playerScript.isAlive = false;
        }
        else if(isAlive){
            anim.SetTrigger("Flinch");
        }
    }
    public void LightningHit(){
        Debug.Log("Lightning hit me");
        isAlive = hp.TakeDamage(10);
        DamageAction(isAlive);

    }

}
