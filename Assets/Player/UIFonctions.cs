using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFonctions : MonoBehaviour
{
    [SerializeField]
    private GameObject topRightMenu;
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private TextMeshProUGUI textFPS;

    private const int defaultFPS = 90;
    private void Awake()
    {
        FPStarget(defaultFPS);
    }
    public void TopRightButton()
    {
        topRightMenu.SetActive(!topRightMenu.activeSelf);
    }

    public void Spawn()
    {
        Player.instance.UnableDeplacement();
        spawn.SetActive(true);
        Map.type = MapType.Spawn;
        Player.instance.transform.position = new Vector3(0f,2f,0f);
        Map.instance.Destroy();
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void FPStarget(float value)
    {
        Application.targetFrameRate = (int)value;
        textFPS.text = "FPS : " + value.ToString();
    }
}
