using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 将这个枚举定义在类的外面，这样其他脚本（比如PlayerInteraction）也能轻松访问它
public enum ItemType 
{ 
    Hammer, 
    Phone, 
    PhoneBattery, 
    KeyChain 
}

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType; 
    public string pickupNotification = "Acquired: ";

    public void Pickup()
    {
        Destroy(gameObject);
        
    }
}
