using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class InteractableButton : MonoBehaviour
    {
        public InteractableKeyboard_MiniGame computerParent;
        public void ButtonHasBeenTouched()
        {
            if (computerParent.isMiniGamePlaying)
            {
                computerParent.ButtonPushed(gameObject);
            } else
            {
                print("Sorry, but mini game hasn't been launched");
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