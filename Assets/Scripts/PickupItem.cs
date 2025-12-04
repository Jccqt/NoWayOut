using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    [Header("Interaction Events")]
    public UnityEvent onPickup;

    public void Interact()
    {
        bool success = InventoryManager.Instance.AddItem(itemData);

        if (success)
        {
            onPickup?.Invoke();

            if (itemData.id == "flashlight")
            {
                GameObject handLight = GameObject.Find("FlashlightHolder");
                if (handLight != null)
                {
                    handLight.transform.GetChild(0).gameObject.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
    }
}
