using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public int audioState = 0;
    private int lastAudioState = 0;

    public List<AudioClip> audioClips;

    [SerializeField]
    private AudioSource audioSource;


    public IEnumerator PlayIntroMusic() {
        //play the intro music once through, then continue with looping music starting from the "normal" music

        audioState = 1;
        lastAudioState = 1;
        audioSource.Stop();

        audioSource.PlayOneShot(audioClips[1]);
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioState = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //StartCoroutine(PlayIntroMusic());
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
