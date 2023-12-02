using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public static LifeBar Instance { get; private set; }

    [SerializeField] public float spacing;
    private bool updateNeeded = true;

    [SerializeField] private List<Image> images;

    [SerializeField] public Sprite emptyHeart;
    [SerializeField] public Sprite fullHeart;

    [SerializeField] private byte _health;

    public byte HealthValue
    {
        get => _health;

        set
        {
            _health = value;
            updateNeeded = true;
        }
    }

    [field: SerializeField] public byte MaxHealth { get; set; }

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        for (int i = 1; i != MaxHealth; ++i) {
            images.Add(Instantiate(images[^1], images[^1].transform));

            images[^1].name = $"{images[0].name} ({i})";

            images[^1].transform.Translate(
                images[0].rectTransform.rect.width + spacing,
                0,
                0
            );

            images[^1].transform.SetParent(gameObject.transform, false);
        }

        images[0].name = $"{images[0].name} (0)";
    }

    protected void Update()
    {
        if (updateNeeded) {
            for (int i = 0; i != images.Count; ++i) {
                images[i].sprite = (i < HealthValue) ? fullHeart : emptyHeart;
            }

            updateNeeded = false;
        }
    }
}
