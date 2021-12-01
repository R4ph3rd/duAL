using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class InteractableButton : MonoBehaviour
    {
        public void ButtonHasBeenTouched()
        {
            if (transform.parent)
            {
                transform.GetComponentInParent<InteractableKeyboard_MiniGame>().ButtonPushed(gameObject);
            }
        }

        public void OnButtonUp(Hand fromHand)
        {

        }

        private void ColorSelf(Color newColor)
        {
            //Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
            //for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
            //{
            //    renderers[rendererIndex].material.color = newColor;
            //}
        }
    }
}