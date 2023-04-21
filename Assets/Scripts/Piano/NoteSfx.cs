using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSfx : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Array of all possible sounds to play")] [SerializeField] private AudioClip[] _audio;
    [Tooltip("The audio source")] private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        AudioClip randomNote = PickRandomNote();
        _audioSource.clip = randomNote;
        _audioSource.Play();
    }

    private AudioClip PickRandomNote()
    {
        return _audio[Random.Range(0, _audio.Length)];
    }
}
