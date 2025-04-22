using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource rainSFX;
    public AudioClip rainAudio;
    public AudioClip backgroundAudioTheme;
    public AudioClip SwordSwingAudio;
    public AudioClip confirmSound;
    public AudioClip lightninSound;


    public void Start(){
        musicSource.clip = backgroundAudioTheme;
        musicSource.Play();
        rainSFX.clip = rainAudio;
        rainSFX.Play();
    }
    public void playSFX(AudioClip sound){
        SFXSource.PlayOneShot(sound);
    }
}
