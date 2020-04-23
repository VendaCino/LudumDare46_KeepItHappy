using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioConfig config;
    private float originPitch;
    private float randomPitchScale = 1;

    public AudioConfig Config => config;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        originPitch = audioSource.pitch;
    }


    public CharacterAudioManager ReSet()
    {
        randomPitchScale = 1;
        return this;
    }

    public CharacterAudioManager SetRandomPitchScale(float scale)
    {
        randomPitchScale = scale;
        return this;
    }
    public CharacterAudioManager AtRandomHighPitch()
    {
        audioSource.pitch = Random.Range(0f,0.5f)* randomPitchScale + originPitch;
        return this;
    }
    public CharacterAudioManager AtRandomLowPitch()
    {
        audioSource.pitch = originPitch - Random.Range(0f, 0.5f)* randomPitchScale;
        return this;
    }
    public CharacterAudioManager SayHi()
    {
        audioSource.clip = config.Hi;
        audioSource.Play();
        return this;
    }
    public CharacterAudioManager SaySad()
    {
        audioSource.clip = config.Sad;
        audioSource.Play();
        return this;
    }
    public CharacterAudioManager SayHappy()
    {
        audioSource.clip = config.Happy;
        audioSource.Play();
        return this;
    }

    public CharacterAudioManager SetPitchDelta(float pitch)
    {
        audioSource.pitch = pitch + originPitch;
        return this;
    }
    public CharacterAudioManager AtRandomPitch()
    {
        audioSource.pitch = Random.Range(-0.5f, 0.5f) * randomPitchScale + originPitch;
        return this;
    }

    [Serializable]
    public struct AudioConfig
    {
        public AudioClip Hi;
        public AudioClip Sad;
        public AudioClip Happy;
    }
}
