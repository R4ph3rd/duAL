using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class VignettePostProcess : MonoBehaviour
{

    private PostProcessVolume m_PpVolume;
    private Vignette m_Vignette;
    private float m_VignetteIntensityValue = 0.63f;
    public bool VignetteOn = false;



    void Start()
    {
        // PostProcess Initialisation
        m_PpVolume = gameObject.GetComponent<PostProcessVolume>();
        m_PpVolume.profile.TryGetSettings(out m_Vignette);
        m_Vignette.enabled.value = false;
    }

    
    void Update()
    {
        // Vignette enabled
        if(VignetteOn)
        {
            m_Vignette.enabled.value = true;
            m_Vignette.intensity.value = m_VignetteIntensityValue;
        }

        // Vignette disabled
        else
        {
            m_Vignette.enabled.value = false;

        }

    }
}
