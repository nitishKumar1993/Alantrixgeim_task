using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchingGame
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void PlaySfx(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}