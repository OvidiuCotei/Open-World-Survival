using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OvyKode.Singletons;
using OvyKode.Multilanguage;
using DG.Tweening;

namespace OvyKode
{
    public class LoadingScreenController : Singleton<LoadingScreenController>
    {
        private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        private float totalSceneProgress;
        private int tipCount;
        public int maxTips = 3;
        private string tipDescription;

        public void StartGame(bool isLoadingSaved, int slotNumber)
        {
            LoadingScreenView.Instance.backgroundImage.sprite = LoadingScreenView.Instance.backgrounds[Random.Range(0, LoadingScreenView.Instance.backgrounds.Length)];
            LoadingScreenView.Instance.loadingScreen.SetActive(true);
            StartCoroutine(GenerateTips());
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.START_MENU));
            scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.GAME, LoadSceneMode.Additive));
            StartCoroutine(GetSceneLoadProgress(isLoadingSaved, slotNumber));
        }

        private IEnumerator GenerateTips()
        {
            tipCount = Random.Range(0, maxTips);

            switch (tipCount)
            {
                case 0:
                    tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_01");
                    break;
                case 1:
                    tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_02");
                    break;
                case 2:
                    tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_03");
                    break;
            }

            LoadingScreenView.Instance.tipsText.text = tipDescription;

            while (LoadingScreenView.Instance.loadingScreen.activeInHierarchy)
            {
                yield return new WaitForSeconds(3f);
                LoadingScreenView.Instance.tipsCanvasgroup.DOFade(0f, 0.5f);
                //LoadingScreenView.Instance.tipsCanvasgroup.alpha = 0f;
                yield return new WaitForSeconds(0.5f);
                tipCount++;

                if (tipCount >= maxTips)
                {
                    tipCount = 0;
                }

                switch (tipCount)
                {
                    case 0:
                        tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_01");
                        break;
                    case 1:
                        tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_02");
                        break;
                    case 2:
                        tipDescription = LocalizationManager.Instance.GetTextForKey("TIPS_03");
                        break;
                }

                LoadingScreenView.Instance.tipsText.text = tipDescription;
                LoadingScreenView.Instance.tipsCanvasgroup.DOFade(1f, 0.5f);
                //LoadingScreenView.Instance.tipsCanvasgroup.alpha = 1f;
            }
        }

        private IEnumerator GetSceneLoadProgress(bool isLoadingSaved, int slotNumber)
        {
            for (int i = 0; i < scenesLoading.Count; i++)
            {
                while (!scenesLoading[i].isDone)
                {
                    totalSceneProgress = 0;

                    foreach (AsyncOperation operation in scenesLoading)
                    {
                        totalSceneProgress += operation.progress;
                    }

                    totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                    LoadingScreenView.Instance.loadingProgressbar.value = Mathf.RoundToInt(totalSceneProgress);
                    LoadingScreenView.Instance.loadingText.text = string.Format("Loading Environment: {0}%", totalSceneProgress);
                    yield return null;
                }
            }

            if (isLoadingSaved)
            {
                // Așteaptă să se termine încărcarea salvarii jocului
                yield return StartCoroutine(DelayedLoading(slotNumber));
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
                LoadingScreenView.Instance.loadingScreen.SetActive(false);
            }
            else
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
                LoadingScreenView.Instance.loadingScreen.SetActive(false);
            }
        }

        private IEnumerator DelayedLoading(int slotNumber)
        {
            yield return new WaitForSeconds(1f);
            SaveManager.Instance.LoadGame(slotNumber);
        }
    }
}