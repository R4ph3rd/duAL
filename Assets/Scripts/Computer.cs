using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Computer : MonoBehaviour
    {
        public GameManager.Owner status = GameManager.Owner.None;

        // FX
        public Material IAScreen, HumanScreen;
        public AudioSource IAWinMiniGame, IALooseMiniGame, HumanWinMiniGame, HumanLooseMiniGame;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CaptureComputer(GameManager.Owner owner)
        {
            status = owner;
            switch (owner)
            {
                case GameManager.Owner.IA:
                    PlaySound(IAWinMiniGame);
                    break;
                case GameManager.Owner.Human:
                    PlaySound(HumanWinMiniGame);
                    break;
                default:
                    break;
            }
        }

        public void FailedMiniGame(GameManager.Owner owner)
        {
            switch (owner)
            {
                case GameManager.Owner.IA:
                    PlaySound(IALooseMiniGame);
                    break;
                case GameManager.Owner.Human:
                    PlaySound(HumanLooseMiniGame);
                    break;
                default:
                    break;
            }
        }

        private void PlaySound(AudioSource sound)
        {
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