using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions
{

    public static void Play(this AudioSource audio, AudioClip clip) {

        audio.clip = clip;
        audio.Play();

    }

    public static void Switch(this AudioSource audioSource, AudioClip b, float rate, float? threshold = 0) {
        
        //do the switch
        IEnumerator s = Switch();

        IEnumerator Switch() {

            IEnumerator e = FadeOut();
            while (e.MoveNext())
                yield return null;
            
            audioSource.clip = b;
            audioSource.Play();

            e = FadeIn();
            while (e.MoveNext())
                yield return null;

        }
        
        IEnumerator FadeOut() {
            while (audioSource.volume > threshold) {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * rate);
                yield return null;
            }
        }
        
        IEnumerator FadeIn() {
            while (audioSource.volume < 1) {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 1, Time.deltaTime * rate);
                yield return null;
            }
        }


    }

}
