using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBarSlider;
    private int maxHealth = 100;
    private int minHealth = 0;
    private int currentHealth; 
    private bool isAlive;
    public Gradient gradient;
    public Image healthBarImg; 
    void Start(){
        currentHealth = maxHealth;
        healthBarSlider.value = currentHealth;
        healthBarImg.color = gradient.Evaluate(1f);
    }

    public bool  TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= minHealth){
            Debug.Log("ya boi is dead");
            updateHealthBarUI(minHealth);
            isAlive =  false;
        }
        else if(currentHealth > minHealth){
            Debug.Log("Still alive man");
            updateHealthBarUI(currentHealth);
            isAlive =  true;
        }
        return isAlive;
        
    }
    public void updateHealthBarUI(int health){
        healthBarSlider.value = health;
        healthBarImg.color = gradient.Evaluate(healthBarSlider.normalizedValue);
    }

}
