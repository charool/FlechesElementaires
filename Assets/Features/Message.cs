using System;
using UnityEngine;

[Serializable]
public class Message
{
    [field: SerializeField] public string Content { get; set; }
    [field: SerializeField] public string Name { get; set; }
}
