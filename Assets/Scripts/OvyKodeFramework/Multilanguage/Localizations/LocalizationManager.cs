using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvyKode.Singletons;

namespace OvyKode.Multilanguage
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private const string FILE_NAME_PREFIX = "text_";
        private const string FILE_EXTENSION = ".json";
        private string fullNameTextFile;
        private string fullPathTextFile;
        private string languageChoose = "en";
        private string loadedJsonText;
        [HideInInspector] public bool isReady = false;
        private bool isFileFound = false;
        private bool isTryChangeLangRunTime = false;
        private Dictionary<string, string> localizedDictionary;
        private LocalizationData loadedData;


        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator Start()
        {
            //languageChoose = LocaleHelper.GetSupportedLanguageCode();
            fullNameTextFile = FILE_NAME_PREFIX + languageChoose.ToLower() + FILE_EXTENSION;

#if UNITY_IOS || UNITY_ANDROID
            fullPathTextFile = Path.Combine(Application.persistentDataPath, fullNameTextFile);
#else
            fullPathTextFile = Path.Combine(Application.streamingAssetsPath, fullNameTextFile);
#endif
            yield return StartCoroutine(LoadJsonLanguageData());
            isReady = true;
        }

        private IEnumerator LoadJsonLanguageData()
        {
            CheckFileExist();
            yield return new WaitUntil(() => isFileFound);

            loadedData = JsonUtility.FromJson<LocalizationData>(loadedJsonText);
            localizedDictionary = new Dictionary<string, string>(loadedData.items.Count);

            loadedData.items.ForEach(item =>
            {
                try
                {
                    localizedDictionary.Add(item.key, item.value);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }

        private void CheckFileExist()
        {
            Debug.Log("File is already existing");
            LoadFileContents();
        }

        private void LoadFileContents()
        {
            CopyFileFromResources();
            loadedJsonText = File.ReadAllText(fullPathTextFile);
            isFileFound = true;
            Debug.Log(loadedJsonText);
        }

        private void CopyFileFromResources()
        {
            TextAsset myFile = Resources.Load("Languages/" + FILE_NAME_PREFIX + languageChoose) as TextAsset;

            if (myFile == null)
            {
                Debug.LogError("Make sure the file " + FILE_NAME_PREFIX + languageChoose + " is in Resources folder in Languages folfer.");
                return;
            }

            loadedJsonText = myFile.ToString();
            File.WriteAllText(fullPathTextFile, loadedJsonText);
            Debug.Log("We are copying file from Resources folder.");
            StartCoroutine(WaitCreationFile());
        }

        private IEnumerator WaitCreationFile()
        {
            FileInfo myFile = new FileInfo(fullNameTextFile);
            float timeOut = 0.0f;

            while (timeOut < 5.0f && !IsFileFinishCreated(myFile))
            {
                timeOut += Time.deltaTime;
                yield return null;
            }

            Debug.Log("The creation file is succed.");
        }

        private bool IsFileFinishCreated(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                isFileFound = true;
                Debug.Log("We succed to find file. ");
                return true;

            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }

            }
            isFileFound = false;
            return false;
        }

        public string GetTextForKey(string localizationKey)
        {
            if (localizedDictionary.ContainsKey(localizationKey))
            {
                return localizedDictionary[localizationKey];  
            }
            else
            {
                return "Error! No key matching with: " + localizationKey;
            }
        }

        private IEnumerator SwitchLanguageRuntime(string langChoose)
        {
            if (!isTryChangeLangRunTime)
            {
                isTryChangeLangRunTime = true;
                isFileFound = false;
                isReady = false;
                languageChoose = langChoose;
                fullNameTextFile = FILE_NAME_PREFIX + languageChoose.ToLower() + FILE_EXTENSION;
#if UNITY_IOS || UNITY_ANDROID
                fullPathTextFile = Path.Combine(Application.persistentDataPath, fullNameTextFile);
#else
                fullPathTextFile = Path.Combine(Application.streamingAssetsPath, fullNameTextFile);
#endif
                yield return StartCoroutine(LoadJsonLanguageData());
                isReady = true;
                LocalizedText[] arrayText = FindObjectsOfType<LocalizedText>();

                for (int i = 0; i < arrayText.Length; i++)
                {
                    arrayText[i].AttributionText();
                }

                isTryChangeLangRunTime = false;
            }
        }

        public void ChangeLanguage(string lang)
        {
            StartCoroutine(SwitchLanguageRuntime(lang));
        }
    }
}