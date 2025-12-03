using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("References")]
    public Image iconImage;
    public Button myButton;
    public TextMeshProUGUI nameText;

    [Header("Read Only")]
    public ItemData currentItem;

    public void UpdateSlot(ItemData newItem)
    {
        currentItem = newItem;

        if (currentItem != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = currentItem.icon;
            iconImage.enabled = true;

            if (nameText != null)
            {
                nameText.text = currentItem.displayName;
                nameText.gameObject.SetActive(true);
            }
            myButton.interactable = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.gameObject.SetActive(false);

        if (nameText != null)
        {
            nameText.text = "";
            nameText.gameObject.SetActive(false);
        }

        myButton.interactable = false;
    }
}
