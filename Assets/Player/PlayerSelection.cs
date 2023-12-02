using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private List<byte> arrowInventory;
    [SerializeField] private List<GameObject> arrowPrefabs;

    private int _currentArrowType = 0;

    public ArrowType CurrentArrowType
    {
        get => (ArrowType) (_currentArrowType + 1);
        set => _currentArrowType = (int) value - 1;
    }

    public bool HasCurrentArrowType => arrowInventory[_currentArrowType] != 0;

    protected void Start()
    {
        for (int i = 0; i != arrowInventory.Count; ++i) {
            InventoryBar.Instance.setItemCount(
                (ArrowType)(i + 1),
                arrowInventory[i]
            );
        }
    }

    public static bool IsValidArrowType(ArrowType __arrowType)
    {
        return
            Enum.IsDefined(typeof(ArrowType), __arrowType)
            && (__arrowType != ArrowType.None);
    }

    public bool NextArrowType()
    {
        bool is_valid = IsValidArrowType(CurrentArrowType + 1);

        if (is_valid) {
            ++_currentArrowType;
        } else {
            _currentArrowType = 0;
        }

        InventoryBar.Instance.Selected = CurrentArrowType;

        return is_valid;
    }

    public bool PreviousArrowType()
    {
        bool is_valid = IsValidArrowType(CurrentArrowType - 1);

        if (is_valid) {
            --_currentArrowType;
        } else {
            _currentArrowType = (arrowPrefabs.Count - 1);
        }

        InventoryBar.Instance.Selected = CurrentArrowType;

        return is_valid;
    }

    public byte GetNumberOfArrow(ArrowType __a) => arrowInventory[(int) __a - 1];

    public void AddNumberOfArrow(ArrowType __a, byte __b)
    {
        SetNumberOfArrow(__a, (byte) (GetNumberOfArrow(__a) + __b));
    }

    public void SetNumberOfArrow(ArrowType __a, byte __b)
    {
        arrowInventory[(int) __a - 1] = __b;
        InventoryBar.Instance.setItemCount(__a, arrowInventory[(int)__a - 1]);
    }

    public GameObject FireCurrentArrowType()
    {
        if (HasCurrentArrowType) {
            print($"Firing '{CurrentArrowType.ToString()}' arrow!");

            InventoryBar.Instance.dropItemCount(CurrentArrowType);
            --arrowInventory[_currentArrowType];

            return arrowPrefabs[_currentArrowType];
        } else {
            print($"No more '{CurrentArrowType.ToString()}' arrow in the inventory!");
        }

        return null;
    }
}
