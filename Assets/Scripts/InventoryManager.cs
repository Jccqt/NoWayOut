using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public InventorySlot[] slots;

    [Header("Player Reference")]
    public MonoBehaviour playerController;

    private bool isInventoryOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isInventoryOpen = false;
        inventoryPanel.SetActive(false);
        UpdateCursorState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            UpdateCursorState();
        }
    }

    void UpdateCursorState()
    {
        if (isInventoryOpen)
        {
            // Show mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (playerController != null) playerController.enabled = false;
        }
        else
        {
            // Hide mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (playerController != null) playerController.enabled = true;
        }
    }

    public bool AddItem(ItemData itemToAdd)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentItem == null)
            {
                slots[i].UpdateSlot(itemToAdd);
                return true;
            }
        }
        return false;
    }
}
