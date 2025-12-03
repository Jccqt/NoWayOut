using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    public void Interact()
    {
        bool success = InventoryManager.Instance.AddItem(itemData);

        if (success)
        {
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
