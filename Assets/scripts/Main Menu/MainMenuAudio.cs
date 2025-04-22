using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioClip music; 
    public AudioClip confirmSound;

    public void Start(){
        musicSource.clip = music;
        musicSource.Play();
    }
    public void playSFX(AudioClip sound){
        SFXSource.PlayOneShot(sound);
    }
}
