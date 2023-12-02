using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<Sound, AudioSource> AudioSources { get; set; }
    public static AudioManager Instance { get; private set; }

    [field: SerializeField] public List<Sound> Sounds { get; set; }

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        foreach (Sound sound in Sounds) {
            var source = gameObject.AddComponent<AudioSource>();

            source.clip = sound.Clip;
            source.volume = sound.Volume;

            AudioSources[sound] = source;
        }
    }

    public void Play(string __name)
    {
        AudioSources[Sounds.Find(sound => sound.Name == __name)]?.Play();
    }
}
