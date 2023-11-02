using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public KeyCode MouseLeft = KeyCode.Mouse0;
    public KeyCode InventoryKey = KeyCode.Tab;
    public KeyCode CraftingKey = KeyCode.C;

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
}
