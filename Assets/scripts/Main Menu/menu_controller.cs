using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class menu_controller : MonoBehaviour
{
    public string _GameSceen;
    public PlayerInput playerInput;
    private MainMenuAudio mainMenuSound;
    public void Start(){
        playerInput = GetComponent<PlayerInput>(); 
        mainMenuSound = GameObject.FindGameObjectWithTag("Audio").GetComponent<MainMenuAudio>();      

    }
    public void startGame(){
        SceneManager.LoadScene(_GameSceen);
    }
    private void OnConfrim()
    {
        Debug.Log("Player pressed X and wants to play");
        mainMenuSound.playSFX(mainMenuSound.confirmSound);
        StartCoroutine(WaitForSoundAndStart(mainMenuSound.confirmSound));
    }

    private IEnumerator WaitForSoundAndStart(AudioClip sound)
    {
        if (sound != null)
        {
            yield return new WaitForSeconds(sound.length);
        }
        startGame();
    }
}
