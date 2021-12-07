using System.Collections;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts
{
    public class Computer : MonoBehaviour
    {
        public GameManager.Owner status = GameManager.Owner.None;
        public float ComputerValue;
        public bool isBigComputer = false;
        public Room RoomID;

        // FX
        public GameObject[] ComputerScreens;
        public Material IAScreenMat, HumanScreenMat, NoneScreenMat;

        private Score score;
        private SoundManager sm;
        private AudioSource source;

        public void Start()
        {
            score = Score.GetScore();
            sm = SoundManager.GetSoundManager();
            source = GetComponent<AudioSource>();
            ComputerValue = isBigComputer ? score.BigComputerValue : score.TinyComputerValue;

            CaptureComputer(status);
        }

        private void Update()
        {
            Outline outline;

            /*Handle Outline color*/
            if((status == GameManager.Owner.Human) && TryGetComponent<Outline>(out outline))
            {
                outline.OutlineColor = Color.green;
            }
            else if (status != GameManager.Owner.Human && TryGetComponent<Outline>(out outline))
            {
                outline.OutlineColor = Color.red;
            }
        }

        public void CaptureComputer(GameManager.Owner owner)
        {
            status = owner;
            switch (owner)
            {
                case GameManager.Owner.IA:
                    if (IAScreenMat)
                    {
                        foreach (GameObject screen in ComputerScreens)
                        {
                            screen.GetComponent<Renderer>().material = IAScreenMat;
                        }
                    }
                    source.clip = sm.IAWinMiniGameSound;
                    break;
                case GameManager.Owner.Human:
                    if (HumanScreenMat)
                    {
                        foreach (GameObject screen in ComputerScreens)
                        {
                            screen.GetComponent<Renderer>().material = HumanScreenMat;
                        }
                    }
                    break;
                case GameManager.Owner.None:
                    if (NoneScreenMat)
                    {
                        foreach (GameObject screen in ComputerScreens)
                        {
                            screen.GetComponent<Renderer>().material = NoneScreenMat;
                        }
                    }
                    break;
                default:
                    break;
            }

            sm.PlaySound(source);
            print("this computer is now owned by : " + status.ToString());
        }

        public void CaptureComputer()
        {
            GameManager.Owner owner = GameManager.Owner.Human;

            if (IAScreenMat) {
                foreach (GameObject screen in ComputerScreens)
                {
                    screen.GetComponent<Renderer>().material = HumanScreenMat;
                }
            }
            source.clip = sm.IAWinMiniGameSound;
            sm.PlaySound(source);
            status = owner;

            print("this computer is now owned by : " + status.ToString());
        }

        public void FailedMiniGame(GameManager.Owner owner)
        {
            switch (owner)
            {
                case GameManager.Owner.IA:
                    source.clip = sm.IALooseMiniGameSound;
                    break;
                case GameManager.Owner.Human:
                    source.clip = sm.HumanLooseMiniGameSound;
                    break;
                default:
                    break;
            }
            sm.PlaySound(source);
        }
    }
}