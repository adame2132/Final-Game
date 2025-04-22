using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterColliderScript : MonoBehaviour
{
    private HealthBarScript hp;
    private bool isAlive;
    private Animator anim;
    private MonsterScript monster;
    void Start()
    {
        hp = GetComponent<HealthBarScript>();
        anim = GetComponent<Animator>();
        isAlive = true;
        monster = GetComponent<MonsterScript>();

        
    }
    void OnTriggerEnter(Collider hitter){
        if(hitter.tag == "Sword"){
            isAlive = hp.TakeDamage(20);
            DamageAction(isAlive);
        }
    }

    void DamageAction(bool isAlive){
        if(!isAlive){
            Debug.Log("WizardDebug: playing death trigger rn");
            anim.SetTrigger("Death");
            monster.isAlive = false;
        }
        else if(isAlive){
            anim.SetTrigger("Flinch");
        }
    }

    
}
