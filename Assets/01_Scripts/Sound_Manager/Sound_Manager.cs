using System.Collections;
using UnityEngine;

namespace HashTableAudioManager
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioListener))]
    public class Sound_Manager : MonoBehaviour
    {
        //public members
        public static Sound_Manager instance = null;

        //private menbers
        private string path = "Audio/";//If you put all audios in Resourse/Audio/
        private Hashtable audioTable = new Hashtable();
        private AudioSource audioSource;
        private float currentVolume;
        private float nextVolume;
        private AudioClip cache;
        private bool clipSwitching;
        private string currentClip;
        private string nextClip;
        private float fadeScale = 2f;

        #region Unity Methods
        private void Awake()
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        #endregion

        #region Public Methods
        public void AddAudio(string key)
        {
            if (audioTable.ContainsKey(key))
            {
                return;
            }
            AudioClip clip = LoadClip(key);
            if (clip != null)
            {
                audioTable.Add(key, clip);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("No audio with the key!");
#endif
            }
        }

        public void RemoveAudio(string key)
        {
            audioTable.Remove(key);
        }

        public void ClearAudio()
        {
            audioTable.Clear();
        }

        public void OnPlayClip(string key, int _volume = 1)
        {
            if (key == currentClip)
            {
                return;
            }

            if (currentClip == "")
            {
                currentClip = key;
                currentVolume = _volume;
                audioSource.volume = currentVolume;
                audioSource.clip = (AudioClip)audioTable[key];
                audioSource.Play();
                return;
            }

            nextClip = key;
            cache = (AudioClip)audioTable[nextClip];
            nextVolume = _volume;
            clipSwitching = true;
            StartCoroutine(ClipSwitch());

        }

        public IEnumerator ClipSwitch()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                currentVolume -= Time.deltaTime * fadeScale;
                audioSource.volume = currentVolume;
                if (currentVolume <= 0)
                {
                    currentClip = nextClip;
                    audioSource.clip = cache;
                    currentVolume = nextVolume;
                    audioSource.Stop();
                    audioSource.volume = currentVolume;
                    clipSwitching = false;
                    audioSource.Play();
                }
            } while (clipSwitching);
        }

        public void OnStop()
        {
            audioSource.Stop();
            currentClip = "";
            audioSource.clip = null;
        }

        public void OnPlayOneShot(string key, float volume = 1)
        {
            audioSource.PlayOneShot((AudioClip)audioTable[key], volume);
        }
        #endregion


        #region Private Methods
        private AudioClip LoadClip(string key)
        {
            AudioClip clip = (AudioClip)Resources.Load(path + key);
            return clip;
            //You can use Resources or any other things to put your audios
        }
        #endregion
    }
}