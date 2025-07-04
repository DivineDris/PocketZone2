using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    [field: SerializeField]
    public bool IsStackable { get; set; }
    public int Id => GetInstanceID();
    [field: SerializeField]
    public int MaxStackSize { get; set; } = 1;
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    public Sprite ItemImage { get; set; }
}
