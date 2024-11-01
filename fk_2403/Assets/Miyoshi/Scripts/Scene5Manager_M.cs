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
    [SerializeField] GameObject allert;


    public bool isReceivePanelDisplay = false;
    public bool isDisplayDirectionsEnd = false;
    public bool isLocationServiceStart = false;

    private bool isSeatch = false;

    private string passPhrase = null;

    public double distance = double.MaxValue;

    public float threshold = 200/*0.0004f*/;
    string opponentId = "";

    [SerializeField]private string receivedOpponentProfileID = null;
    [SerializeField]private string receivedMyProfileID = null;
    List<string> profIds = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(panel1 != null && panel2 != null && inputField != null && locationServiceScript_M != null);
        instance = this;
        panel2.SetActive(false);
        allert.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        if(isDisplayDirectionsEnd)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
        }

        if(distance <= threshold)
        {
            allert.SetActive(true);
        }
        else
        {
            allert.SetActive(false);
        }
    }

    public void OnClickSearch()
    {
        if(passPhrase == null) return;//壊れたら怖いのでチェック.挙動に問題がないなら消していい.
        Debug.Log($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passPhrase}");
        FirebaseManager.instance.ReadData($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passPhrase}", (value) => {
            if(!value.Equals("NoData"))//合言葉が存在したら.//仮置きaaa.
            {
                //opponentIdがプロフIdになってる
                //相手のIDがほしい
                
                GetProfID();
                Invoke("GetMyRecieveProfID", 2f);
                Invoke("GetOpponentID",4f);

                //Debug.Log("aaaaaaaaa");

                


            }
            else
            {
                Debug.Log("あいことばが存在しません");
            }
            
        });
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

            /*//受け取り処理よろしくお願いします！.
            //FirebaseManager.instance.WriteData();
            FirebaseManager.instance.ReadData($"users/{UserDataManager.instance.uid}/heldProfiles/", (value) =>
            {
                receivedOpponentProfileID = (value);
            });


            */

            FirebaseManager.instance.Remove($"users/{UserDataManager.instance.uid}/heldProfiles/{receivedOpponentProfileID}");
            //FirebaseManager.instance.Remove($"users/{opponentId}/heldProfiles/{receivedMyProfileID}");

            //FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/receivedProfiles/{receivedMyProfileID}", "");
            FirebaseManager.instance.WriteData($"users/{opponentId}/receivedProfiles/{receivedOpponentProfileID}", "");


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
        allert.SetActive(false);
        Debug.Log("close");
    }

    public void GetProfID()
    {
        FirebaseManager.instance.GetAllChildKeys($"ExchangeProf/{passPhrase}", (value) =>
        {
            profIds.AddRange(value);
            //GetMyRecieveProfID();
        });
    }

    public void GetMyRecieveProfID()
    {
        FirebaseManager.instance.GetAllChildKeys($"users/{UserDataManager.instance.uid}/heldProfiles", (value) =>
        {
            Debug.Log(value.Length);
            foreach (string str in value)
            {
                Debug.Log(str);
                if (str.Equals(profIds[0]))
                {
                    receivedOpponentProfileID = profIds[0];
                    receivedMyProfileID = profIds[1];
                }
                else if (str.Equals(profIds[1]))
                {
                    receivedOpponentProfileID = profIds[1];
                    receivedMyProfileID = profIds[0];
                }
            }
            Debug.Log("receivedMyProfileID:"+receivedMyProfileID);
            Debug.Log("receivedOpponentProfileID:"+receivedOpponentProfileID);
            //GetOpponentID();
            
        });
    }

    public void GetOpponentID()
    {
        FirebaseManager.instance.ReadData($"ProfInfo/{receivedOpponentProfileID}/ownerId", (value) =>
        {
            opponentId = value;
            Debug.Log("opponentId:" + value);
            locationServiceScript_M.SetInfo(passPhrase, opponentId);
            StartCoroutine(locationServiceScript_M.StartLocationSystem());
            StartCoroutine(locationServiceScript_M.DisplayDirections());
            isSeatch = true;
            
        });
    }

    public void OnEndEdit(string s)
    {
        passPhrase = s;
    }
}
