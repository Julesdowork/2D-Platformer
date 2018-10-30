using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1f)] public float volume = 0.7f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    [Range(0, 0.5f)] public float volumeVariance = 0.1f;
    [Range(0, 0.5f)] public float pitchVariance = 0.1f;

    AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
        source.pitch = pitch * (1 + Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
        source.Play();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager was found.");
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            go.transform.SetParent(transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }

        // no sound with name
        Debug.LogWarning("AudioManager: Sound-" + name + " not found in array.");
    }
}
