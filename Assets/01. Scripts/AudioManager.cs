using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();


    }
}
