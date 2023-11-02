using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OvyKode
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }
        public bool onTarget;
        public GameObject interaction_Info_UI;
        public GameObject selectedObject;
        private Text interaction_text;
        public Image centerDotIcon;
        public Image handIcon;
        [HideInInspector] public bool handIsVisible;
        public GameObject selectedTree;
        public GameObject chopHolder;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            onTarget = false;
            interaction_text = interaction_Info_UI.GetComponent<Text>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var selectionTransform = hit.transform;

                if(selectionTransform.GetComponent<NPC>() is var npc && npc != null && npc.playerInRange)
                {
                    interaction_text.text = npc.GetItemName();
                    interaction_Info_UI.SetActive(true);

                    if(Input.GetMouseButtonDown(0) &&npc.isTaklkingWithPlayer == false)
                    {
                        npc.StartConversation();
                    }

                    if(DialogSystem.Instance.dialogUIActive)
                    {
                        interaction_Info_UI.SetActive(false);
                        centerDotIcon.gameObject.SetActive(false);
                    }
                }
                else
                {
                    interaction_text.text = string.Empty;
                    interaction_Info_UI.SetActive(false);
                }

                if (selectionTransform.GetComponent<ChoppableTree>() is var choppableTree && choppableTree != null && choppableTree.playerInRange)
                {
                    choppableTree.canBeChopped = true;
                    selectedTree = choppableTree.gameObject;
                    chopHolder.gameObject.SetActive(true);
                }
                else
                {
                    if(selectedTree != null)
                    {
                        selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                        selectedTree = null;
                        chopHolder.gameObject.SetActive(false);
                    }
                }

                if (selectionTransform.GetComponent<InteractableObject>() is var selection && selection != null && selection.playerInRange)
                {
                    onTarget = true;
                    selectedObject = selection.gameObject;
                    interaction_text.text = selection.GetItemName();
                    interaction_Info_UI.SetActive(true);

                    if (selection.tags == TAG.PICKABLE)
                    {
                        centerDotIcon.gameObject.SetActive(false);
                        handIcon.gameObject.SetActive(true);
                        handIsVisible = true;
                    }
                    else
                    {
                        centerDotIcon.gameObject.SetActive(true);
                        handIcon.gameObject.SetActive(false);
                        handIsVisible = false;
                    }
                }
                else
                {
                    onTarget = false;
                    //interaction_Info_UI.SetActive(false);
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
                handIsVisible = false;
            }
        }

        public void EnableSelection()
        {
            handIcon.enabled = true;
            centerDotIcon.enabled = true;
            interaction_Info_UI.SetActive(true);
        }

        public void DisableSelection()
        {
            handIcon.enabled = false;
            centerDotIcon.enabled = false;
            interaction_Info_UI.SetActive(false);
            selectedObject = null;
        }
    }
}