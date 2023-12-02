using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBar : MonoBehaviour
{
    public static InventoryBar Instance { get; private set; }

    [SerializeField] private float spacing;
    private bool updateNeeded = true;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<InventoryItem> items;

    [SerializeField] private Image selectionImage;
    [SerializeField] private TextMeshProUGUI selectionLabel;

    [SerializeField] private int _selected = 0;

    public ArrowType Selected
    {
        get => (ArrowType) (_selected + 1);

        set
        {
            _selected = (int) value - 1;
            updateNeeded = true;
        }
    }

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
         for (int i = 1; i != items.Count; ++i) {
            images.Add(Instantiate(images[^1]));

            images[^1].name = $"{images[0].name} ({i})";

            images[^1].transform.Translate(
                selectionImage.rectTransform.rect.width + spacing, 0, 0
            );

            images[^1].transform.SetParent(gameObject.transform, false);
        }

        for (int i = 0; i != items.Count; ++i) {
            images[i].sprite = items[i].Sprite;
        }

        images[0].name = $"{images[0].name} (0)";
    }

    protected void Update()
    {
        if (updateNeeded) {
            for (int i = 0; i != items.Count; ++i) {
                images[i]
                    .transform
                    .Find("Count Label")
                    .gameObject
                    .GetComponent<TextMeshProUGUI>()
                    .text
                    = items[i].Count.ToString();
            }

            selectionImage.transform.position =
                images[_selected].transform.position;

            selectionLabel.text = items[_selected].Label;

            updateNeeded = false;
        }
    }
}
