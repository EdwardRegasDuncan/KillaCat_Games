using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    public enum Sound 
    { 
        TropMove,
        TropAttack,
        TropReciveHit,
        TropShooting,
        TropSpell,
        TropDead,
        TropPlacing,
        UISound,
        BackGroundMusic
    }

    public static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static GameObject musicGameObject;
    private static AudioSource oneShotAudioSource;
    private static AudioSource musicAudioSource;

    public static void Initilize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.TropMove] = 0f;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound, float volume = 0.5f)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.volume = volume;
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlayMusic(Sound sound, float volume = 0.2f)
    {
        if (CanPlaySound(sound))
        {
            if (musicGameObject == null)
            {
                musicGameObject = new GameObject("Sound");
                musicAudioSource = musicGameObject.AddComponent<AudioSource>();
            }
            musicAudioSource.volume = volume;
            musicAudioSource.loop = true;
            musicAudioSource.clip = GetAudioClip(sound);
            musicAudioSource.Play();
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.TropMove:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }

        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        return null;
    }
}
