using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> arrowPrefabs;

    [SerializeField]
    private List<byte> arrowInventory;

    private int _currentArrowType;

    public ArrowType currentArrowType
    {
        get => (ArrowType) _currentArrowType;
        set => _currentArrowType = (int) value;
    }

    public bool HasCurrentArrowType => arrowInventory[_currentArrowType] != 0;

    public static bool IsValidArrowType(int __arrowType)
    {
        return Enum.IsDefined(typeof(ArrowType), __arrowType);
    }

    public bool NextArrowType()
    {
        bool is_valid = IsValidArrowType(_currentArrowType + 1);

        if (is_valid) {
            ++_currentArrowType;
        }

        return is_valid;
    }

    public bool PreviousArrowType()
    {
        bool is_valid = IsValidArrowType(_currentArrowType - 1);

        if (is_valid) {
            --_currentArrowType;
        }

        return is_valid;
    }

    public byte GetNumberOfArrow(ArrowType __a)
    {
        return arrowInventory[(int) __a];
    }

    public void SetNumberOfArrow(ArrowType __a, byte __b) 
    {
        arrowInventory[(int) __a] = __b;
    }

    public GameObject FireCurrentArrowType()
    {
        if (HasCurrentArrowType) {
            print($"Firing '{currentArrowType.ToString()}' arrow!");

            --arrowInventory[_currentArrowType];

            return arrowPrefabs[_currentArrowType];
        } else {
            print($"No more '{currentArrowType.ToString()}' arrow in the inventory!");
        }

        return null;
    }
}
