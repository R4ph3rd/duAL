using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class InteractableButton : MonoBehaviour
    {
        public void OnButtonDown(Hand fromHand)
        {
            print("button down !");
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