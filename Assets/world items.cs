using UnityEngine;

public class worlditems : MonoBehaviour
{
    public enum ItemType
    {
        None,
        Water,
        Food,
        Ball,
        Sleep
    }
    public ItemType itemType;
    public GameObject handPrefab;

    public void Interact(PlayerInteract player)
    {
        if (player.heldItem != ItemType.None) return;

        player.HoldItem(itemType, handPrefab);
        Destroy(gameObject);
    }
}
