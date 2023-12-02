using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBar : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private float spacing;
    private bool UpdateNeeded { get; set; } = true;

    [SerializeField]
    private List<Image> images;

    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private Image selectionImage;

    [SerializeField]
    private Sprite selectionSprite;

    [SerializeField]
    private int _selected = 0;

    public int Selected
    {
        get => _selected;

        set
        {
            _selected = value;
            UpdateNeeded = true;
        }
    }

    protected void Start()
    {
        for (int i = 0; i != images.Count; ++i) {
            images[i].sprite = sprites[i];
        }

        for (int i = 1; i != sprites.Count; ++i) {
            images.Add(Instantiate(images[^1], images[^1].transform.localPosition, images[^1].transform.localRotation));

            images[^1].name = $"{images[0].name} ({i})";

            images[^1].transform.Translate(
                selectionImage.rectTransform.rect.width + spacing, 0, 0
            );

            images[^1].transform.SetParent(canvas.transform, false);
        }

        images[0].name = $"{images[0].name} (0)";
    }

    protected void Update()
    {
        if (UpdateNeeded) {
            selectionImage.transform.position =
                images[Selected].transform.position;

            UpdateNeeded = false;
        }
    }
}
