using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public int audioState;
    private int lastAudioState;

    public List<AudioClip> audioClips;
    private AudioSource audioSource;


    IEnumerator PlayIntroMusic() {
        //play the intro music once through, then continue with looping music starting from the "normal" music
        audioSource.PlayOneShot(audioClips[1]);
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioState = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioState = 1;
        lastAudioState = 1;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroMusic());
    }

    // Update is called once per frame
    void Update()
    {

        if (audioState != lastAudioState) { // audioState can be changed from elsewhere depending on the circumstances
            audioSource.Stop();
            audioSource.clip = audioClips[audioState];
            audioSource.Play();
            lastAudioState = audioState;
        }

    }
}
