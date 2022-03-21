using UnityEngine;
using UnityEngine.UI;

namespace HashTableAudioManager
{
    public class Audio_Object : MonoBehaviour
    {

        public string audioKey;
        public bool isBG;
        private Button button;


        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => PlayAudio());
            if (Sound_Manager.instance != null)
            {
                Sound_Manager.instance.AddAudio(audioKey);
                //Register and load audio on sound manager
            }
        }

        public void PlayAudio()
        {
            if (isBG)
            {
                Sound_Manager.instance.OnPlayClip(audioKey);
            }
            else
            {
                Sound_Manager.instance.OnPlayOneShot(audioKey);
            }
        }
    }
}