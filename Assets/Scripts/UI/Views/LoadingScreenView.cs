using UnityEngine;
using UnityEngine.UI;
using OvyKode.Singletons;

public class LoadingScreenView : Singleton<LoadingScreenView>
{
    public GameObject loadingScreen;
    public Image backgroundImage;
    public CanvasGroup tipsCanvasgroup;
    public Slider loadingProgressbar;
    public Text loadingText;
    public Text tipsText;
    public Sprite[] backgrounds;
}