using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISizeController : MonoBehaviour
{
    [Header("UI Settings")]
    [Range(5f, 15f)]
    public float widthInPrecent = 5;

    // private reference
    // Info UI
    private GameObject _moneyUI;
    private RectTransform _moneyRectTransform;
    private GameObject _garbageUI;
    private RectTransform _garbageRectTransform;
    private GameObject _dayUI;
    private RectTransform _dayRectTransform;

    // Health UI
    private GameObject _energyUI;
    private RectTransform _energyRectTransform;
    private GameObject _toxicUI;
    private RectTransform _toxicRectTransform;


    // Private value variables
    private float _moneyHeightInPercent;
    private float _garbageHeightInPercent;
    private float _dayHeightInPercent;

    private float _energyHeightInPercent;
    private float _toxicHeightInPercent;

    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the UI
        _moneyUI = GameObject.Find("MoneyUIPrefab");
        _garbageUI = GameObject.Find("GarbageUIPrefab");
        _dayUI = GameObject.Find("DayUIPrefab");

        _energyUI = GameObject.Find("EnergyBar");
        _toxicUI = GameObject.Find("ToxicBar");


        // Calculate the UIs heights in precent relative to the width
        _moneyRectTransform = _moneyUI.GetComponent<RectTransform>();
        _moneyHeightInPercent = _moneyRectTransform.sizeDelta.y / _moneyRectTransform.sizeDelta.x;

        _garbageRectTransform = _garbageUI.GetComponent<RectTransform>();
        _garbageHeightInPercent = _garbageRectTransform.sizeDelta.y / _garbageRectTransform.sizeDelta.x;

        _dayRectTransform = _dayUI.GetComponent<RectTransform>();
        _dayHeightInPercent = _dayRectTransform.sizeDelta.y / _dayRectTransform.sizeDelta.x;

        _energyRectTransform = _energyUI.GetComponent<RectTransform>();
        _energyHeightInPercent = _energyRectTransform.sizeDelta.y / _energyRectTransform.sizeDelta.x;

        _toxicRectTransform = _toxicUI.GetComponent<RectTransform>();
        _toxicHeightInPercent = _toxicRectTransform.sizeDelta.y / _toxicRectTransform.sizeDelta.x;
    }



    // Update is called once per frame
    void Update()
    {
        float screenWidth = Screen.width;
        //float ScreeHeight = Screen.height;
        float UIWidth = screenWidth * widthInPrecent / 100;

        // Calculate the size of the UI depending on the size of the screen
        _moneyRectTransform.sizeDelta = new Vector2(UIWidth, UIWidth * _moneyHeightInPercent);
        float UITopMargin = -UIWidth * _moneyHeightInPercent / 2;
        _moneyRectTransform.anchoredPosition = new Vector2(UIWidth / 2, UITopMargin);

        _garbageRectTransform.sizeDelta = new Vector2(UIWidth, UIWidth * _garbageHeightInPercent);
        UITopMargin += -UIWidth * _garbageHeightInPercent / 2;
        _garbageRectTransform.anchoredPosition = new Vector2(UIWidth / 2, UITopMargin);


        _dayRectTransform.sizeDelta = new Vector2(UIWidth, UIWidth * _dayHeightInPercent);
        UITopMargin += -UIWidth * _dayHeightInPercent / 2;
        _dayRectTransform.anchoredPosition = new Vector2(UIWidth / 2, UITopMargin);

        _energyRectTransform.sizeDelta = new Vector2(UIWidth, UIWidth * _energyHeightInPercent);

        _toxicRectTransform.sizeDelta = new Vector2(UIWidth, UIWidth * _toxicHeightInPercent);
    }


    // Private methods
    //private float calcHeightPrecent(GameObject UI, RectTransform UIRectTransform)
    //{
    //    UIRectTransform = 
    //}
}
