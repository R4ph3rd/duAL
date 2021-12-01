using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Computer : MonoBehaviour
    {
        public GameManager.Owner status = GameManager.Owner.None;

        // FX
        public GameObject ComputerScreen;
        public Material IAScreenMat, HumanScreenMat;
        public AudioClip IAWinMiniGameSound, IALooseMiniGameSound, HumanWinMiniGameSound, HumanLooseMiniGameSound;

        public void CaptureComputer(GameManager.Owner owner)
        {
            status = owner;
            switch (owner)
            {
                case GameManager.Owner.IA:
                    if (IAScreenMat) ComputerScreen.GetComponent<Renderer>().material = IAScreenMat;
                    PlaySound(IAWinMiniGameSound);
                    break;
                case GameManager.Owner.Human:
                    if (HumanScreenMat) ComputerScreen.GetComponent<Renderer>().material = HumanScreenMat;
                    PlaySound(HumanWinMiniGameSound);
                    break;
                default:
                    break;
            }

            GetComponentInChildren<TextMesh>().text = owner.ToString();
        }

        public void CaptureComputer()
        {
            GameManager.Owner owner = GameManager.Owner.Human;

            if (IAScreenMat) ComputerScreen.GetComponent<Renderer>().material = IAScreenMat;
            PlaySound(IAWinMiniGameSound);
            status = owner;

            GetComponentInChildren<TextMesh>().text = owner.ToString();
            print(status.ToString());
        }

        public void FailedMiniGame(GameManager.Owner owner)
        {
            switch (owner)
            {
                case GameManager.Owner.IA:
                    PlaySound(IALooseMiniGameSound);
                    break;
                case GameManager.Owner.Human:
                    PlaySound(HumanLooseMiniGameSound);
                    break;
                default:
                    break;
            }
        }

        private void PlaySound(AudioClip clip)
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.clip = clip;

            if (sound.isPlaying)
            {
                sound.time = 0;
                sound.Play();
            }
            else
            {
                sound.Play();
            }
        }
    }
}