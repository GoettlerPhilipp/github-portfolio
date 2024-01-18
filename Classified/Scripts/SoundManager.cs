using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{
    
    public enum Sound
    {
        loseFight,
        winFight,
        
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        //audioSource.PlayOneShot(GetAudioClip(sound));
    }

    /*private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAsset.SoundAudioClip soundAudioClip in GameAsset.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
            Debug.LogError("Sound" + sound + "not found!");
            return null;
        }*/
    }


