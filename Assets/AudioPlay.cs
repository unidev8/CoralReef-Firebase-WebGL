using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject btnPause;
    //public GameObject sldVolum;

    [HideInInspector]
    public bool playState = true;

    public static AudioPlay instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = audioClip;        
        playState = true;

        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        //sldVolum.GetComponent<Slider>().value = 0.1f;
        audioSource.Play();
    }


    public void AudioAction()
    {
        //if (!GameControl.instance.isUserlogin) return;
        
        if (playState)
        {
            audioSource.Pause();            
        }
        else
        {
            audioSource.Play();
        }
        playState = !playState;
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void ChangeVolum()
    {
        //audioSource.volume = sldVolum.GetComponent<Slider>().value;
    }

    // Update is called once per frame
    void Update()
    {
        if (playState)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("AudioSource isPlaying!");
            }
            Debug.Log("AudioSource playstate true");                
        }           
        
    }
}
