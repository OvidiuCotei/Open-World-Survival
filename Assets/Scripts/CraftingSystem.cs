using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class CraftingSystem : MonoBehaviour
    {
        public static CraftingSystem Instance { get; set; }
        public GameObject craftingScreenUI;
        public GameObject toolsScreenUI;
        public GameObject survivalScreenUI;
        public GameObject refineScreenUI;
        public GameObject constructionScreenUI;
        public Transform craftingContainer;
        public Transform toolsContainer;
        public Transform survivalContainer;
        public Transform refineContainer;
        public Transform constructionContainer;
        public List<string> inventoryItemList = new List<string>();

        // Category Buttons
        private Button toolsButton;
        private Button survivalButton;
        private Button refineButton;
        private Button constructionButton;

        // Craft Buttons
        private Button craftAxeButton;
        private Button craftPlankButton;
        private Button craftFoundationButton;
        private Button craftWallButton;

        // Requirements Text
        private Text axeReq_1, axeReq_2, plankReq1, foundationReq_1, wallReq_1;

        public bool isOpen;

        // All Blueprints
        public BluePrint axeBluePrint = new BluePrint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        public BluePrint plankBluePrint = new BluePrint("Plank", 2, 1, "Log", 1, "", 0);
        public BluePrint foundationBluePrint = new BluePrint("Foundation", 1, 1, "Plank", 4, "", 0);
        public BluePrint wallBluePrint = new BluePrint("Wall", 1, 1, "Plank", 2, "", 0);

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
        }

        private void Start()
        {
            isOpen = false;
            toolsButton = craftingContainer.Find("Tools Button").GetComponent<Button>();
            toolsButton.onClick.AddListener(delegate { OpenToolsCategory(); });

            survivalButton = craftingContainer.Find("Survival Button").GetComponent<Button>();
            survivalButton.onClick.AddListener(delegate { OpenSurvivalCategory(); });

            refineButton = craftingContainer.Find("Refine Button").GetComponent<Button>();
            refineButton.onClick.AddListener(delegate { OpenRefineCategory(); });

            constructionButton = craftingContainer.Find("Construction Button").GetComponent<Button>();
            constructionButton.onClick.AddListener(delegate { OpenConstructionCategory(); });

            // Axe
            axeReq_1 = toolsContainer.Find("Axe").transform.Find("Requirement_1").GetComponent<Text>();
            axeReq_2 = toolsContainer.Find("Axe").transform.Find("Requirement_2").GetComponent<Text>();
            craftAxeButton = toolsContainer.Find("Axe").transform.Find("Craft Button").GetComponent<Button>();
            craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(axeBluePrint); });

            // Plank
            plankReq1 = refineContainer.Find("Plank").transform.Find("Requirement_1").GetComponent<Text>();
            craftPlankButton = refineContainer.Find("Plank").transform.Find("Craft Button").GetComponent<Button>();
            craftPlankButton.onClick.AddListener(delegate { CraftAnyItem(plankBluePrint); });

            // Foundation
            foundationReq_1 = constructionContainer.Find("Foundation").transform.Find("Requirement_1").GetComponent<Text>();
            craftFoundationButton = constructionContainer.Find("Foundation").transform.Find("Craft Button").GetComponent<Button>();
            craftFoundationButton.onClick.AddListener(delegate { CraftAnyItem(foundationBluePrint); });

            // Wall
            wallReq_1 = constructionContainer.Find("Wall").transform.Find("Requirement_1").GetComponent<Text>();
            craftWallButton = constructionContainer.Find("Wall").transform.Find("Craft Button").GetComponent<Button>();
            craftWallButton.onClick.AddListener(delegate { CraftAnyItem(wallBluePrint); });
        }

        private void Update()
        {
            if (Input.GetKeyDown(InputManager.Instance.CraftingKey) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
            {
                craftingScreenUI.SetActive(true);
                CursorIsVisible(true);
                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
                isOpen = true;

            }
            else if (Input.GetKeyDown(InputManager.Instance.CraftingKey) && isOpen)
            {
                craftingScreenUI.SetActive(false);
                //toolsScreenUI.SetActive(false);
                DisableAllScreens();

                if (!InventorySystem.Instance.isOpen)
                {
                    CursorIsVisible(false);
                    SelectionManager.Instance.EnableSelection();
                    SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                }

                isOpen = false;
            }
        }

        public void CursorIsVisible(bool isVisible)
        {
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void DisableAllScreens()
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);
        }

        private void OpenToolsCategory()
        {
            DisableAllScreens();
            toolsScreenUI.SetActive(true);
        }

        private void OpenSurvivalCategory()
        {
            DisableAllScreens();
            survivalScreenUI.SetActive(true);
        }

        private void OpenRefineCategory()
        {
            DisableAllScreens();
            refineScreenUI.SetActive(true);
        }

        private void OpenConstructionCategory()
        {
            DisableAllScreens();
            constructionScreenUI.SetActive(true);
        }

        private void CraftAnyItem(BluePrint bluePrintToCraft)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.craftindSound);
            StartCoroutine(CraftedDelayForSound(bluePrintToCraft));

            // Remove resources from inventory
            if (bluePrintToCraft.numOfRequirements == 1)
            {
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req1, bluePrintToCraft.req1Amount);
            }
            else if (bluePrintToCraft.numOfRequirements == 2)
            {
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req1, bluePrintToCraft.req1Amount);
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req2, bluePrintToCraft.req2Amount);
            }
            else if(bluePrintToCraft.numOfRequirements == 3)
            {
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req1, bluePrintToCraft.req1Amount);
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req2, bluePrintToCraft.req2Amount);
                InventorySystem.Instance.RemoveItem(bluePrintToCraft.req3, bluePrintToCraft.req3Amount);
            }
            // Refresh list
            StartCoroutine(Calculate());
        }

        private IEnumerator CraftedDelayForSound(BluePrint bluePrintToCraft)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < bluePrintToCraft.numberOfItemsToProduce; i++)
            {
                // Add item into inventory
                InventorySystem.Instance.AddToInventory(bluePrintToCraft.itemName, "");
            }
        }

        public IEnumerator Calculate()
        {
            yield return 0;
            InventorySystem.Instance.ReCalculateList();
            RefreshNeededItems();
        }

        public void RefreshNeededItems()
        {
            int stone_count = 0;
            int stick_count = 0;
            int log_count = 0;
            int plank_count = 0;

            inventoryItemList = InventorySystem.Instance.itemList;

            foreach (string itemName in inventoryItemList)
            {
                switch (itemName)
                {
                    case "Stone":
                        stone_count += 1;
                        break;
                    case "Stick":
                        stick_count += 1;
                        break;
                    case "Log":
                        log_count += 1;
                        break;
                    case "Plank":
                        plank_count += 1;
                        break;
                }
            }

            // ===== AXE =====
            axeReq_1.text = "3 Stone[" + stone_count + "]";
            axeReq_2.text = "3 Stick[" + stick_count + "]";

            if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.ChechSlotsAvailable(1))
            {
                craftAxeButton.interactable = true;
            }
            else
            {
                craftAxeButton.interactable = false;
            }

            // ===== PLANK =====
            plankReq1.text = "1 Log[" + log_count + "]";

            if (log_count >= 1 && InventorySystem.Instance.ChechSlotsAvailable(2))
            {
                craftPlankButton.interactable = true;
            }
            else
            {
                craftPlankButton.interactable = false;
            }

            // ===== FOUNDATION ====
            foundationReq_1.text = "4 Plank[" + plank_count + "]";

            if (plank_count >= 4 && InventorySystem.Instance.ChechSlotsAvailable(2))
            {
                craftFoundationButton.interactable = true;
            }
            else
            {
                craftFoundationButton.interactable = false;
            }

            // ===== WALL ====
            wallReq_1.text = "2 Plank[" + plank_count + "]";

            if (plank_count >= 2 && InventorySystem.Instance.ChechSlotsAvailable(2))
            {
                craftWallButton.interactable = true;
            }
            else
            {
                craftWallButton.interactable = false;
            }
        }
    }
}