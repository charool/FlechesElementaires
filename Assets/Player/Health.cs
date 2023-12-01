using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private List<Image> images;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    
    public Canvas canvas;
    public float spacing;

    [SerializeField]
    private byte _health;

    public byte HealthValue
    { 
        get => _health;
        
        set
        {
            _health = value;
            UpdateNeeded = true;
        }
    }

    public bool IsDead => HealthValue != 0;
    public byte maxHealth;

    public bool UpdateNeeded { get; private set; } = true;

    public void Damage(byte __b)
    {
        if (__b >= HealthValue) {
            HealthValue = 0;
        } else {
            HealthValue -= __b;
        }
    }

    public bool Kill()
    {
        if (IsDead) {
            return false;
        } else {
            HealthValue = 0;

            return true;
        }
    }

    protected void Start()
    {
        for (int i = 1; i != maxHealth; ++i) {
            images.Add(Instantiate(images[0], canvas.transform));

            images[^1].transform.Translate(
                (images[^2].transform.position.x - images[^1].transform.position.x) 
                    + images[0].rectTransform.rect.width 
                    + spacing,

                0, 
                0
            );
        }
    }

    protected void Update()
    {
        if (UpdateNeeded) {
            for (int i = 0; i != images.Count; ++i) {
                images[i].sprite = (i < HealthValue) ? fullHeart : emptyHeart;
            }

            UpdateNeeded = false;
        }
    }
}
