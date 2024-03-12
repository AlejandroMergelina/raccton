using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudio", menuName = "DialogueAudio")]
public class DialogueAudioInfoSO : ScriptableObject
{

    [SerializeField]
    private string iD;

    [SerializeField]
    private AudioClip[] clips;

    [SerializeField, Range(1, 5)]
    private int frecuencyLevel;

    [SerializeField, Range(-3, 3)]
    private float minPitch;

    [SerializeField, Range(-3, 3)]
    private float maxPitch;

    [SerializeField]
    private bool stopAudioSource;

    public AudioClip[] Clips { get => clips; set => clips = value; }
    public int FrecuencyLevel { get => frecuencyLevel; set => frecuencyLevel = value; }
    public float MinPitch { get => minPitch; set => minPitch = value; }
    public float MaxPitch { get => maxPitch; set => maxPitch = value; }
    public bool StopAudioSource { get => stopAudioSource; set => stopAudioSource = value; }
    public string ID { get => iD; set => iD = value; }
}
