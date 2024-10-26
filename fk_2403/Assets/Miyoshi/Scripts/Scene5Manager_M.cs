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

    public float distance = float.MaxValue;

    private const float threshold = 0.0004f;

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
        if(passPhrase.Equals("aaa"))//合言葉が存在したら.//仮置きaaa.
        {
            Debug.Log("aaadayo");
            StartCoroutine(locationServiceScript_M.StartLocationSystem());
            StartCoroutine(locationServiceScript_M.DisplayDirections());
            isSeatch = true;
        }
        else
        {
            Debug.Log("あいことばが存在しません");
        }
    }

    public void OnClickReceive()
    {
        Debug.Log("Searchclicked:"+passPhrase);
        if(passPhrase == null) return;
        if(!isSeatch) return;
        //相手との距離が閾値以下だったら.
        if(distance <= threshold)
        {
            //ここで受け取り処理をする.
            isReceivePanelDisplay = true;
        }
        else
        {
            Debug.Log("相手との距離が遠いです");
        }
         
    }

    public void OnClickClose()
    {
        isReceivePanelDisplay = false;
        isDisplayDirectionsEnd = false;
        isSeatch = false;
        inputField = null;
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
