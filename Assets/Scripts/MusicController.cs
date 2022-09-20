using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public int audioState = 0;
    private int lastAudioState = 0;

    public List<AudioClip> audioClips;
    private AudioSource audioSource;


    IEnumerator PlayIntroMusic() {
        // play the intro music once through, then set the music to start looping and continue
        audioSource.PlayOneShot(audioClips[0]);
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioState = 1;
        audioSource.loop = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroMusic());
    }

    // Update is called once per frame
    void Update()
    {
        if (audioState != lastAudioState) { // music state has changed: stop the old music and start playing the new one
            audioSource.Stop();
            audioSource.PlayOneShot(audioClips[audioState]);
            lastAudioState = audioState;
        }
    }
}
