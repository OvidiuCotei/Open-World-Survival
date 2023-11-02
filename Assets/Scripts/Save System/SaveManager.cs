using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace OvyKode
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; set; }
        public bool isSavingToJson = true;
        public bool isLoading;

        // Binary Save Path
        private string binaryPath;
        // Json Project Save path
        private string jsonPathProject;
        // json External/Real Save Path
        private string jsonPathPersistent;
        string fileName = "SaveGame";

        private void Start()
        {
            jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
            jsonPathPersistent = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
            binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void SaveGame(int slotNumber)
        {
            AllGameData data = new AllGameData();
            data.playerData = GetPlayerData();

            data.environmentData = GetEnvironmentData();

            SavingTypeSwitch(data, slotNumber);
        }

        public void LoadGame(int slotNumber)
        {
            isLoading = true;
            // Player Data
            SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);
            // Environment Data
            SetEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);

            isLoading = false;
        }

        public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
        {
            if (isSavingToJson)
            {
                SaveGameDataToJsonFile(gameData, slotNumber);
            }
            else
            {
                SaveGameDataToBinaryFile(gameData, slotNumber);
            }
        }

        public AllGameData LoadingTypeSwitch(int slotNumber)
        {
            if (isSavingToJson)
            {
                AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
                return gameData;
            }
            else
            {
                AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
                return gameData;
            }
        }

        public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);
            formatter.Serialize(stream, gameData);
            Debug.Log("Data saved to" + binaryPath + fileName + slotNumber + ".bin");
            stream.Close();
        }

        public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
        {
            if (File.Exists(binaryPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);
                AllGameData data = formatter.Deserialize(stream) as AllGameData;
                stream.Close();
                Debug.Log("Data loaded from" + binaryPath + fileName + slotNumber + ".bin");
                return data;
            }
            else
            {
                return null;
            }
        }

        public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
        {
            string json = JsonUtility.ToJson(gameData, true);
            // string encrypted = EncryptionDecryption(json);

            using (StreamWriter writer = new StreamWriter(jsonPathPersistent + fileName + slotNumber + ".json"))
            {
                writer.Write(json);
                Debug.Log("Saved Game to Json file at: " + jsonPathPersistent + fileName + slotNumber + ".json");
            };
        }

        public AllGameData LoadGameDataFromJsonFile(int slotNumber)
        {
            using(StreamReader reader = new StreamReader(jsonPathPersistent + fileName + slotNumber + ".json"))
            {
                string json = reader.ReadToEnd();
                //string decrypted = EncryptionDecryption(json);
                AllGameData data = JsonUtility.FromJson<AllGameData>(json);
                Debug.Log("Load Game from Json file at: " + jsonPathPersistent + fileName + slotNumber + ".json");
                return data;
            };
        }

        public string EncryptionDecryption(string jsonString)
        {
            string keyword = "2061996";
            string result = "";

            for (int i = 0; i < jsonString.Length; i++)
            {
                result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
            }

            return result;
        }

        public bool DoesFileExists(int slotNumber)
        {
            if (isSavingToJson)
            {
                if (File.Exists(jsonPathPersistent + fileName + slotNumber + ".json"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSlotEmpty(int numberSlot)
        {
            if (DoesFileExists(numberSlot))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DeselectButton()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }

        private PlayerData GetPlayerData()
        {
            // Getting Player Stats
            float[] playerStats = new float[3];
            playerStats[0] = PlayerState.Instance.currenthealth;
            playerStats[1] = PlayerState.Instance.currentCalories;
            playerStats[2] = PlayerState.Instance.currentHydrationPercent;

            // getting Player Position
            float[] playerPosAndRot = new float[6];
            playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
            playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
            playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

            // Getting Player Rotation
            playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
            playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
            playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

            // Getting Inventory Content
            string[] inventory = InventorySystem.Instance.itemList.ToArray();

            /// Getting Quick Slots Content
            string[] quickSlots = GetQuickSlotsContent();

            return new PlayerData(playerStats, playerPosAndRot, inventory, quickSlots);
        }

        private void SetPlayerData(PlayerData playerData)
        {
            // Setting Player Stats
            PlayerState.Instance.currenthealth = playerData.playerStats[0];
            PlayerState.Instance.currentCalories = playerData.playerStats[1];
            PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

            // Setting Player Position
            Vector3 loadedPosition;
            loadedPosition.x = playerData.playerPositionAndRotation[0];
            loadedPosition.y = playerData.playerPositionAndRotation[1];
            loadedPosition.z = playerData.playerPositionAndRotation[2];
            PlayerState.Instance.playerBody.transform.position = loadedPosition;

            // Setting Player Rotation
            Vector3 loadedRotation;
            loadedRotation.x = playerData.playerPositionAndRotation[3];
            loadedRotation.y = playerData.playerPositionAndRotation[4];
            loadedRotation.z = playerData.playerPositionAndRotation[5];
            PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

            // Setting the inventory content
            foreach(string item in playerData.inventoryContent)
            {
                InventorySystem.Instance.AddToInventory(item, "");
            }

            // Setting Quick Slots Content
            foreach(string item in playerData.quickSlotsContent)
            {
                // Find next free quick slot
                GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
                var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
                itemToAdd.transform.SetParent(availableSlot.transform, false);
            }
        }

        private string[] GetQuickSlotsContent()
        {
            List<string> temp = new List<string>();

            foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
            {
                if(slot.transform.childCount != 0)
                {
                    string name = slot.transform.GetChild(0).name;
                    string str2 = "(Clone)";
                    string cleanName = name.Replace(str2, "");
                    temp.Add(cleanName);
                }
            }

            return temp.ToArray();
        }

        private EnvironmentData GetEnvironmentData()
        {
            List<string> itemsPickedUp = InventorySystem.Instance.itemsPickedup;
            return new EnvironmentData(itemsPickedUp);
        }

        private void SetEnvironmentData(EnvironmentData environmentData)
        {
            foreach(Transform itemType in EnvironmentManager.Instance.allItems.transform)
            {
                foreach(Transform item in itemType.transform)
                {
                    if(environmentData.pickedupItems.Contains(item.name))
                    {
                        Destroy(item.gameObject);
                    }
                }
            }

            InventorySystem.Instance.itemsPickedup = environmentData.pickedupItems;
        }
    }
}