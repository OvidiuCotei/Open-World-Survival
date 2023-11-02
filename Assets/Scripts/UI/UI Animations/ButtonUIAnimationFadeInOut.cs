using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace OvyKode
{
    public class ButtonUIAnimationFadeInOut : MonoBehaviour
    {
        [SerializeField] private Button buttonUI;
        [SerializeField] private Text textUI;
        [SerializeField] private float duration = 1f;
        [Header("Button Default Color")]
        public Color buttonDefaultColor = Color.black;
        [Header("Button Target Color")]
        public Color buttonTargetColor = Color.white;
        [Header("Text Default Color")]
        public Color textDefaultColor = Color.white;
        [Header("Text Target Color")]
        public Color textTargetColor = Color.black;

        public void ButtonAnimationFadeIn()
        {
            buttonUI.image.DOColor(buttonTargetColor, duration);
            textUI.DOColor(textTargetColor, duration);
        }

        public void ButtonAnimationFadeOut()
        {
            buttonUI.image.DOColor(buttonDefaultColor, duration);
            textUI.DOColor(textDefaultColor, duration);
        }
    }
}