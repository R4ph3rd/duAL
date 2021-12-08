using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    //MiniGame sounds
    public AudioClip IAWinMiniGameSound, IALooseMiniGameSound, HumanWinMiniGameSound, HumanLooseMiniGameSound;
    public AudioClip SuccessStepSound, FailureStepSound;

    //Intro sounds
    public AudioClip introVoice, instruTPVoice, instruHackVoice, infoDiskVoice, goodLuckVoice;

    //Super power sounds
    public AudioClip gravitySound, smokeSound, tpSound, tazzerSound, stunSound;

    [Header("End game sounds")]
    public AudioClip aiWinVoice, humanWinVoice;

    //Hack sounds
    public AudioClip iaHackedSound, humanHackedSound;

    private static SoundManager _this = null;
    public static SoundManager GetSoundManager()
    {
        if (_this == null)
        {
            _this = FindObjectOfType<SoundManager>(); ;
        }
        return _this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioSource sound)
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
