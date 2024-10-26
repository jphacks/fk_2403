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

    public string passPhrase = null;

    [SerializeField] 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(panel1 != null && panel2 != null && inputField != null);
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

    public void OnClickReceive()
    {
        Debug.Log("Searchclicked:"+passPhrase);
        if(passPhrase == null) return;
        if(true)//正しい合言葉が入力されていたら.
        {

        }
        isReceivePanelDisplay = true;
        //Debug.Log("uketoru");
        /*
        panel1.SetActive(false);
        panel2.SetActive(true);
        */
    }

    public void OnClickaaa()
    {
        if(true)//相手が近くにいたら.
        {
            
        }
        Debug.Log("receive");
        isReceivePanelDisplay = false;
        isDisplayDirectionsEnd = false;
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    public void OnEndEdit(string s)
    {
        passPhrase = s;
        Debug.Log("passPhraseSet"+passPhrase);
    }
}
