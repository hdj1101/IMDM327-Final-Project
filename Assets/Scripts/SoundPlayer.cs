using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;
    public List<AudioClip> audioClips; // List of audio clips that correspond to button IDs

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundsBasedOnPressedButtons(List<int> buttonIds)
    {
        // Special sound
        if (buttonIds.Contains(1) && buttonIds.Contains(7) && buttonIds.Contains(8) && buttonIds.Contains(15))
        {
            SkyEffectManager.Instance.SUPERFUNTIME();
            AudioSource.PlayClipAtPoint(audioClips[16], transform.position);
        }
        else
        {
            // Play the sounds for the listed IDs
            foreach (int id in buttonIds)
            {
                Debug.Log(id);
                if (id >= 0 && id < audioClips.Count)
                {
                    AudioSource.PlayClipAtPoint(audioClips[id], transform.position);
                }
            }
        }
    }
}
