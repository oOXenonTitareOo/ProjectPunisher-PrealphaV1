using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        StartCoroutine(PlaySFXCoroutine(audioClip, position, volume));
    }

    IEnumerator PlaySFXCoroutine(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        GameObject tempAudioHost = new GameObject("TempAudio_" + audioClip.name);
        tempAudioHost.transform.position = position;

        AudioSource audioSource = tempAudioHost.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 2f;
        audioSource.maxDistance = 30;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.Play();

        yield return new WaitForSeconds(audioClip.length);

        Destroy(tempAudioHost);
    }
}
