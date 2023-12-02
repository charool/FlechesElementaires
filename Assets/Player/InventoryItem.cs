using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    [field: SerializeField] public byte Count { get; set; }
    [field: SerializeField] public string Label { get; set; }
    [field: SerializeField] public Sprite Sprite { get; set; }
}
