using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBar : MonoBehaviour
{
    public static InventoryBar Instance { get; private set; }

    [SerializeField] private float spacing;
    private bool updateNeeded = true;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<Sprite> sprites;

    [SerializeField] private Image selectionImage;
    [SerializeField] private Sprite selectionSprite;

    [SerializeField] private int _selected = 0;

    public int Selected
    {
        get => _selected;

        set
        {
            _selected = value;
            updateNeeded = true;
        }
    }

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        for (int i = 0; i != images.Count; ++i) {
            images[i].sprite = sprites[i];
        }

        for (int i = 1; i != sprites.Count; ++i) {
            images.Add(Instantiate(images[^1]));

            images[^1].name = $"{images[0].name} ({i})";

            images[^1].transform.Translate(
                selectionImage.rectTransform.rect.width + spacing, 0, 0
            );

            images[^1].transform.SetParent(gameObject.transform, false);
        }

        images[0].name = $"{images[0].name} (0)";
    }

    protected void Update()
    {
        if (updateNeeded) {
            selectionImage.transform.position =
                images[Selected].transform.position;

            updateNeeded = false;
        }
    }
}
