using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<Sound, AudioSource> AudioSources { get; set; } = new();
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
            source.loop = sound.Loop;
            source.volume = sound.Volume;

            AudioSources.Add(sound, source);
        }
    }

    public void Play(string __name)
    {
        AudioSource src = AudioSources[Sounds.Find(sound => sound.Name == __name)];

        if (src == null) {
            print($"'{__name}' is not a correct audio name!");
        } else if (!src.isPlaying) {
            print($"Play '{__name}'");
            src.Play();
        } else {
            print($"'{__name}' is already being played!");
        }
    }
}
