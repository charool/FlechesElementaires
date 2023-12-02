using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [field: SerializeField] public AudioClip Clip { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public float Volume { get; set; }
}
