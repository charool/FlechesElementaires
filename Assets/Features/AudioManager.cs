using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private Dictionary<Sound, AudioSource> AudioSources { get; set; } = new();

    public Sound CurrentLoop { get; private set; }
    [field: SerializeField] public string DefaultLoop { get; private set; }
    [field: SerializeField] public List<Sound> Sounds { get; private set; }

    protected void Awake() => Instance = this;

    protected void Start()
    {
        foreach (Sound sound in Sounds) {
            var source = gameObject.AddComponent<AudioSource>();

            source.clip = sound.Clip;
            source.loop = sound.Loop;
            source.volume = sound.Volume;

            AudioSources.Add(sound, source);
        }

        PlayDefaultLoop();
    }

    public void Play(string __name)
    {
        Sound sound = Sounds.Find(sound => sound.Name == __name);

        if (sound == null) {
            print($"'{__name}' is not a correct audio name!");
        } else {
            AudioSource src = AudioSources[sound];

            if (!src.isPlaying) {
                if (sound.Loop) {
                    if (CurrentLoop != null) {
                        print(
                            $"Changing loop from '{CurrentLoop.Name}' to " +
                            $"'{__name}'!"
                        );

                        AudioSources[CurrentLoop].Stop();
                    }

                    CurrentLoop = sound;
                }

                print($"Play '{__name}'!");
                src.Play();
            } else {
                print($"'{__name}' is already being played!");
            }
        }
    }

    public void PlayDefaultLoop()
    {
        if (DefaultLoop != null) {
            Play(DefaultLoop);
        }
    }
}
