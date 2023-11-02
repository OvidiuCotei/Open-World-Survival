using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class ConstructionManager : MonoBehaviour
    {
        public static ConstructionManager Instance { get; set; }
        public bool inConstructionMode = false;
        public bool isValidPlacement;
        public bool selectingAGhost;
        public GameObject itemToBeConstructed;
        public GameObject constructionHoldingSpot;
        public GameObject selectedGhost;
        public Material ghostSelectedMat;
        public Material ghostSemiTransparentMat;
        public Material ghostFullTransparentMat;
        public List<GameObject> allGhostsInExistence = new List<GameObject>();
        public GameObject itemToBeDestroyed;
        public GameObject constructionUI;
        public GameObject player;

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

        private void Update()
        {
            constructionUI.SetActive(inConstructionMode);

            if (itemToBeConstructed != null && inConstructionMode)
            {
                if (itemToBeConstructed.name == "FoundationModel")
                {
                    if (CheckValidConstructionPosition())
                    {
                        isValidPlacement = true;
                        itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                    }
                    else
                    {
                        isValidPlacement = false;
                        itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10f))
                {
                    var selectionTransform = hit.transform;

                    if (selectionTransform.gameObject.CompareTag("Ghost") && itemToBeConstructed.name == "FoundationModel")
                    {
                        itemToBeConstructed.SetActive(false);
                        selectingAGhost = true;
                        selectedGhost = selectionTransform.gameObject;

                        // Adăugați un mic offset în sus pentru a preveni intrarea în pământ
                        Vector3 placementPosition = hit.point + new Vector3(0f, 0.05f, 0f);
                        itemToBeConstructed.transform.position = placementPosition;
                    }
                    else if (selectionTransform.gameObject.CompareTag("WallGhost") && itemToBeConstructed.name == "WallModel")
                    {
                        itemToBeConstructed.SetActive(false);
                        selectingAGhost = true;
                        selectedGhost = selectionTransform.gameObject;

                        // Adăugați un mic offset în sus pentru a preveni intrarea în pământ
                        Vector3 placementPosition = hit.point;
                        itemToBeConstructed.transform.position = placementPosition;
                    }
                    else
                    {
                        itemToBeConstructed.SetActive(true);
                        selectedGhost = null;
                        selectingAGhost = false;

                        // Dacă nu selectați un ghost, poziționați obiectul direct la locul hit-ului
                        itemToBeConstructed.transform.position = hit.point;
                    }

                }
            }

            // Left Mouse Click to Place item
            if (Input.GetMouseButtonDown(0) && inConstructionMode)
            {
                // We don't want the freestyle to be triggered when we select a ghost.
                if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "FoundationModel")
                {
                    PlaceItemFreeStyle();
                    DestroyItem(itemToBeDestroyed);
                }

                if (selectingAGhost)
                {
                    PlaceItemInGhostPosition(selectedGhost);
                    DestroyItem(itemToBeDestroyed);
                }
            }
            // Right Mouse Click to Cancel                      
            //TODO - don't destroy the ui item until you actually placed it.
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (itemToBeDestroyed != null)
                {
                    itemToBeDestroyed.SetActive(true);
                    itemToBeDestroyed = null;
                    DestroyItem(itemToBeConstructed);
                    itemToBeConstructed = null;
                    inConstructionMode = false;
                }
            }
        }

        public void ActivateConstructionPlacement(string itemToConstruct)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));
            //change the name of the gameobject so it will not be (clone)
            item.name = itemToConstruct;
            //item.transform.SetParent(constructionHoldingSpot.transform, false);
            itemToBeConstructed = item;
            itemToBeConstructed.gameObject.tag = "ActiveConstructable";
            // Disabling the non-trigger collider so our mouse can cast a ray
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;
            // Actiavting Construction mode
            inConstructionMode = true;
        }

        private void GetAllGhosts(GameObject itemToBeConstructed)
        {
            List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

            foreach (GameObject ghost in ghostlist)
            {
                Debug.Log(ghost);
                allGhostsInExistence.Add(ghost);
            }
        }

        private void PerformGhostDeletionScan()
        {
            foreach (GameObject ghost in allGhostsInExistence)
            {
                if (ghost != null)
                {
                    if (ghost.GetComponent<GhostItem>().hasSamePosition == false) // if we did not already add a flag
                    {
                        foreach (GameObject ghostX in allGhostsInExistence)
                        {
                            // First we check that it is not the same object
                            if (ghost.gameObject != ghostX.gameObject)
                            {
                                // If its not the same object but they have the same position
                                if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                                {
                                    if (ghost != null && ghostX != null)
                                    {
                                        // setting the flag
                                        ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (GameObject ghost in allGhostsInExistence)
            {
                if (ghost != null)
                {
                    if (ghost.GetComponent<GhostItem>().hasSamePosition)
                    {
                        DestroyImmediate(ghost);
                    }
                }
            }
        }

        private float XPositionToAccurateFloat(GameObject ghost)
        {
            if (ghost != null)
            {
                // Turning the position to a 2 decimal rounded float
                Vector3 targetPosition = ghost.gameObject.transform.position;
                float pos = targetPosition.x;
                float xFloat = Mathf.Round(pos * 100f) / 100f;
                return xFloat;
            }
            return 0;
        }

        private float ZPositionToAccurateFloat(GameObject ghost)
        {

            if (ghost != null)
            {
                // Turning the position to a 2 decimal rounded float
                Vector3 targetPosition = ghost.gameObject.transform.position;
                float pos = targetPosition.z;
                float zFloat = Mathf.Round(pos * 100f) / 100f;
                return zFloat;

            }
            return 0;
        }

        private void PlaceItemInGhostPosition(GameObject copyOfGhost)
        {
            Vector3 ghostPosition = copyOfGhost.transform.position;
            Quaternion ghostRotation = copyOfGhost.transform.rotation;

            selectedGhost.gameObject.SetActive(false);

            // Setting the item to be active again (after we disabled it in the ray cast)
            itemToBeConstructed.gameObject.SetActive(true);
            // Setting the parent to be the root of our scene
            itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

            var randomOffset = Random.Range(0.01f, 0.03f);

            itemToBeConstructed.transform.position = new Vector3(ghostPosition.x, ghostPosition.y, ghostPosition.z + randomOffset);
            itemToBeConstructed.transform.rotation = ghostRotation;

            // Enabling back the solider collider that we disabled earlier
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;
            // Setting the default color/material
            itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();

            if (itemToBeConstructed.name == "FoundationModel")
            {
                // making the Ghost to no longer be children of this item
                itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
                itemToBeConstructed.tag = "PlacedFoundation";
                //Adding all the ghosts of this item into the manager's ghost bank
                GetAllGhosts(itemToBeConstructed);
                PerformGhostDeletionScan();
            }
            else
            {
                itemToBeConstructed.tag = "PlacedWall";
                DestroyImmediate(selectedGhost);
            }

            itemToBeConstructed = null;
            inConstructionMode = false;
        }

        private void PlaceItemFreeStyle()
        {
            // Setting the parent to be the root of our scene
            itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);

            // Making the Ghost Children to no longer be children of this item
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            // Setting the default color/material
            itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
            itemToBeConstructed.tag = "PlacedFoundation";
            itemToBeConstructed.GetComponent<Constructable>().enabled = false;
            // Enabling back the solider collider that we disabled earlier
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

            //Adding all the ghosts of this item into the manager's ghost bank
            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();

            itemToBeConstructed = null;

            inConstructionMode = false;
        }

        private bool CheckValidConstructionPosition()
        {
            if (itemToBeConstructed != null)
            {
                return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
            }

            return false;
        }

        private void DestroyItem(GameObject item)
        {
            DestroyImmediate(item);
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }
    }
}