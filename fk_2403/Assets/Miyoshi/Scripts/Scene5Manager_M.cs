using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene5Manager_M : MonoBehaviour
{
    public static Scene5Manager_M instance;
    [SerializeField] GameObject panel1;
    [SerializeField] GameObject panel2;
    [SerializeField] GameObject inputField;

    [SerializeField] LocationServiceScript_M locationServiceScript_M;


    public bool isReceivePanelDisplay = false;
    public bool isDisplayDirectionsEnd = false;
    public bool isLocationServiceStart = false;

    private bool isSeatch = false;

    public string passPhrase = null;

    /*
    private float myLatitude;
    private float myLongitude;
    private float opponentLatitude;
    private float opponentLongitude;
    */

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(panel1 != null && panel2 != null && inputField != null && locationServiceScript_M != null);
        instance = this;
        panel2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDisplayDirectionsEnd)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }

    public void OnClickSearch()
    {
        Debug.Log("Searchclicked:"+passPhrase);
        if(passPhrase == null) return;//壊れたら怖いのでチェック.挙動に問題がないなら消していい.
        if(passPhrase.Equals("aaa"))//合言葉が存在したら.
        {
            Debug.Log("aaadayo");
            StartCoroutine(locationServiceScript_M.StartLocationSystem());
            StartCoroutine(locationServiceScript_M.DisplayDirections());
            isSeatch = true;
            return;
        }
        /*
        Debug.Log("aaaaaaaaaaaa");
        StartCoroutine(locationServiceScript_M.StartLocationSystem());
        StartCoroutine(locationServiceScript_M.DisplayDirections());
        isSeatch = true;
        */
        //StartCoroutine(locationServiceScript_M.GetLocation());
    }

    public void OnClickReceive()
    {
        Debug.Log("Searchclicked:"+passPhrase);
        if(passPhrase == null) return;
        if(!isSeatch) return;
        //locationServiceScript_M.
        /*
        if(true)//正しい合言葉が入力されていたら.
        {

        }
        else{
            Debug.Log("合言葉が正しくありません");
        }
        //相手との距離を取りたい
        if(true)
        {
            Debug.Log("相手との距離が遠いです");
        }
        else
        {
            Debug.Log("受け取り推移");
            isReceivePanelDisplay = true;
        }
        */
        isReceivePanelDisplay = true; 
        //Debug.Log("uketoru");
        /*
        panel1.SetActive(false);
        panel2.SetActive(true);
        */
    }

    public void OnClickClose()
    {
        isReceivePanelDisplay = false;
        isDisplayDirectionsEnd = false;
        isSeatch = false;
        panel1.SetActive(true);
        panel2.SetActive(false);
        Debug.Log("clode");
    }

    public void OnEndEdit(string s)
    {
        passPhrase = s;
        Debug.Log("passPhraseSet"+passPhrase);
    }
}
