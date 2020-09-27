using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController
{
    Dictionary<AudioRef, AudioClip> l_audioDictionary;
    AudioSource c_audioSource;

    public AudioController(List<AudioRef> statesIn, List<AudioClip> audioIn, ref AudioSource sourceIn)
    {
        if (statesIn.Count != audioIn.Count)
        {
            // error
            return;
        }

        for (int i = 0; i < statesIn.Count; i++)
        {
            if (l_audioDictionary.ContainsKey(statesIn[i]))
            {
                continue;
            }

            l_audioDictionary[statesIn[i]] = audioIn[i];
        }

        c_audioSource = sourceIn;
    }

    /// <summary>
    /// State-based audio play. Should be used for sounds that are associated with
    /// a given state (riding on different types of terrain, etc.) and should cleanly
    /// loop
    /// </summary>
    /// <param name="refIn">The State associated with the desired clip.</param>
    /// <returns>false if the audio could not be found</returns>
    public bool PlayAudio(AudioRef refIn)
    {
        if (l_audioDictionary.ContainsKey(refIn))
        {
            c_audioSource.Stop();
            return false;
        }
        AudioClip activeClip;
        l_audioDictionary.TryGetValue(refIn, out activeClip);

        c_audioSource.clip = activeClip;
        c_audioSource.Play();

        return true;
    }

    /// <summary>
    /// Transition audio play. Should be used to play context-sensitive sounds
    /// (such as changing state or vocalizations) that are expected to play once
    /// in their entirety.
    /// </summary>
    /// <param name="refIn">The State associated with the desired clip.</param>
    /// <returns>false if the audio could not be found</returns>
    public bool PlayOneShot(AudioRef refIn)
    {
        if (l_audioDictionary.ContainsKey(refIn))
        {
            return false;
        }
        AudioClip activeClip;
        l_audioDictionary.TryGetValue(refIn, out activeClip);

        c_audioSource.PlayOneShot(activeClip);

        return true;
    }

    /// <summary>
    /// Stop the currently playing audio
    /// </summary>
    public void StopAudio()
    {
        c_audioSource.Stop();
    }
}
