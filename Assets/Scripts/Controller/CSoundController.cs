using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CSoundController : MonoBehaviour
{
   

    public AudioClip[] clips;
    private static AudioSource audioSource;

    private void OnEnable()
    {
        CKnigthBehaviour.OnPlaySound += Play;
        CCannonBehaviour.OnPlaySound += Play;
        CMushroomBehaviour.OnPlaySound += Play;
        CTreasureBehaviour.OnPlaySound += Play;
    }

    private void OnDisable()
    {
        CKnigthBehaviour.OnPlaySound -= Play;
        CCannonBehaviour.OnPlaySound -= Play;
        CMushroomBehaviour.OnPlaySound -= Play;
        CTreasureBehaviour.OnPlaySound -= Play;
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Play(int i,float pitch)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clips[i]);
       
    }

   

}
