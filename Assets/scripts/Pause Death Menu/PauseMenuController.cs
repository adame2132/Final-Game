using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseModal, optionsBtn, controlsBtn, quitBtn, gameControlsOutline, controllerImg,quitContainer, yesBtn, noBtn, winModal, healthbar, enemybar;
    private GameObject pause_first, quit_first, prevOptionPressed;
    private GameObject current;
    public TextMeshProUGUI enemyCountUI;
    private AudioScript audioManager;
    private PlayerControls player;
    private int menuDepth;
    private int enemyCount = 5;
    private void  Awake(){
        current = optionsBtn;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
        menuDepth = 0;
        enemyCountUI.text = enemyCount.ToString();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();

    }
    private string _MainMenu ="Main Menu";
    
    public void controlBtnClicked(){
        audioManager.playSFX(audioManager.confirmSound);
        StartCoroutine(WaitForSoundAndStart(audioManager.confirmSound, "show controls"));
    }
    public void showControls(){
        current = controlsBtn;
        optionsBtn.SetActive(false);
        controlsBtn.SetActive(false);
        quitBtn.SetActive(false);
        gameControlsOutline.SetActive(true);
        controllerImg.SetActive(true);
    }

    public void quitBtnClicked(){
        audioManager.playSFX(audioManager.confirmSound);
        StartCoroutine(WaitForSoundAndStart(audioManager.confirmSound, "show quit"));
    }
    private void showQuit(){
        current = quitBtn;
        optionsBtn.SetActive(false);
        controlsBtn.SetActive(false);
        quitBtn.SetActive(false);
        gameControlsOutline.SetActive(true);
        quitContainer.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(yesBtn);
    }
    public void yesQuitBtnClicked(){
        audioManager.playSFX(audioManager.confirmSound);
        StartCoroutine(WaitForSoundAndStart(audioManager.confirmSound, "quit Game"));
    }
    public void quitGame(){
        SceneManager.LoadScene(_MainMenu);
    }

    public void noQuitBtnClicked(){
        audioManager.playSFX(audioManager.confirmSound);
        StartCoroutine(WaitForSoundAndStart(audioManager.confirmSound, "cancel quit"));
    }
    
    private void cancelQuitMenu(){
        quitContainer.SetActive(false);
        gameControlsOutline.SetActive(false);
        quitBtn.SetActive(true);
        controlsBtn.SetActive(true);
        optionsBtn.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(current);
    }
    
    private IEnumerator WaitForSoundAndStart(AudioClip sound, string funct)
    {
        Debug.Log("checking if sound exists");
        if (sound != null)
        {
            Debug.Log("got to wait");
            yield return new WaitForSecondsRealtime(sound.length);
        }
        Debug.Log("audio clip is done");
        if(funct == "show controls"){
            showControls();
        }
        else if (funct == "show quit"){
            showQuit();
        }
        else if(funct == "quit Game"){
            quitGame();
        }
        else if(funct == "cancel quit"){
            cancelQuitMenu();
        }
        else if (funct == "o pressed"){
            backMenuDepth();
        }
        else if(funct == "back"){
            backControllerInput();
        }
        
    }
    public void oPressed(){
        Debug.Log("oPressed was called by controller");
        backMenuDepth();
    }
    private void backMenuDepth(){
        if(EventSystem.current.currentSelectedGameObject== yesBtn || EventSystem.current.currentSelectedGameObject == noBtn){
            Debug.Log("going back to from yes or no");
            noQuitBtnClicked();
        }
        else if(EventSystem.current.currentSelectedGameObject == controlsBtn){
            Debug.Log("going back from controls option o pressed");
            audioManager.playSFX(audioManager.confirmSound);
            StartCoroutine(WaitForSoundAndStart(audioManager.confirmSound, "back"));
        }
    }
    private void  backControllerInput(){
        controllerImg.SetActive(false);
        gameControlsOutline.SetActive(false);
        optionsBtn.SetActive(true);
        controlsBtn.SetActive(true);
        quitBtn.SetActive(true);
    }
    public void resetMenu(){
        pauseModal.SetActive(false);
        optionsBtn.SetActive(true);
        controlsBtn.SetActive(true);
        quitBtn.SetActive(true);
        gameControlsOutline.SetActive(false);
        controllerImg.SetActive(false);
        quitContainer.SetActive(false);
    }
    public void killedEnemy(){
        enemyCount -= 1;
        if(enemyCount != 0){
            enemyCountUI.text = enemyCount.ToString();
        }
        else if(enemyCount == 0){
            enemyCountUI.text = enemyCount.ToString();
            player.victory();
            StartCoroutine(HandleWin());
        }

    }
    IEnumerator HandleWin()
    {
        // anim.SetTrigger("Victory");
        yield return new WaitForSeconds(8); // Wait for 2 seconds before pausing the game
        Time.timeScale = 0; // Pause the game
        Debug.Log("Game paused after win .");
        healthbar.SetActive(false);
        enemybar.SetActive(false);
        winModal.SetActive(true);
    }
}
