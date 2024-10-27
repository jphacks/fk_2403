using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LocationServiceScript_M : MonoBehaviour
{
    [SerializeField] GameObject indicatorPrefab;//相手のポイント.
    [SerializeField] Canvas targetCanvas;
    [SerializeField] GameObject centerPosition;
    [SerializeField] GameObject tmp;
    //List<GameObject> indicators = new List<GameObject>();

    //debug
    [SerializeField] Text myTxt;
    [SerializeField] Text opoTxt;
    [SerializeField] Text disTxt;
    //

    private float myLatitude;
    private float myLongitude;
    private float opponentLatitude = 33.67161f;
    private float opponentLongitude = 130.4463f;
    // Start is called before the first frame update
    private string passPhrase = "";

    private string opponentID = "";

    private int maxSearchCount = 30;
    void Start()
    {
        Debug.Assert(indicatorPrefab != null && targetCanvas != null && centerPosition != null);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator StartLocationSystem()
    {
        //位置情報が拒否されていたら終了.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("位置情報を許可してください");
            yield break;
        }

        //開始.
        Input.location.Start();
        
        //開始されるまでに少し時間がかかる,20秒までは待つ.
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //タイムアウトの処理.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }
        Debug.Log("start");
        Scene5Manager_M.instance.isLocationServiceStart = true;
        yield break;
    }

    public IEnumerator GetLocation()
    {
        if(!Scene5Manager_M.instance.isLocationServiceStart)
        {
            yield return new WaitForSeconds(.1f);
        }
        // 位置情報が取れたかどうか.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // ここで位置情報が取れる.
            myLatitude = Input.location.lastData.latitude;
            myLongitude = Input.location.lastData.longitude;
            FirebaseManager.instance.WriteData($"Communication/{passPhrase}/{UserDataManager.instance.uid}/latitude", myLatitude.ToString());
            FirebaseManager.instance.WriteData($"Communication/{passPhrase}/{UserDataManager.instance.uid}/longitude", myLongitude.ToString());
            Debug.Log("Location: " + myLatitude + " " + myLongitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        //位置情報の取得を終了.
        //Input.location.Stop();//他で終了させる.
    }

    private double GetLocationDistance(float latitude1, float longtitude1, float latitude2, float longtitude2)
    {
        return Mathf.Sqrt((latitude2 - latitude1) * (latitude2 - latitude1) + (longtitude2 - longtitude1) * (longtitude2 - longtitude1));
    }

    public IEnumerator DisplayDirections(/*float myLatitude, float myLongitude, *//*float opponentLatitude, float opponentLongitude*/)
    {
        while (true)
        {
            StartCoroutine(GetLocation());
            //ここで相手の座標を取りたい.
            FirebaseManager.instance.ReadData($"Communication/{passPhrase}/{opponentID}/latitude", (value) => {
                opponentLatitude = float.Parse(value);
            });
            FirebaseManager.instance.ReadData($"Communication/{passPhrase}/{opponentID}/longitude", (value) => {
                opponentLongitude = float.Parse(value);
            });
            Vector3 targetPosition = CalculatePosition(opponentLatitude, opponentLongitude);
            Vector3 myPosition = CalculatePosition(myLatitude, myLongitude);
            myTxt.GetComponent<Text>().text = "m:"+myLatitude+", "+myLongitude;
            opoTxt.GetComponent<Text>().text = "o:"+opponentLatitude+", "+opponentLongitude;
            //Debug.Log(Mathf.Sqrt((targetPosition.x-myPosition.x)*(targetPosition.x-myPosition.x)+(targetPosition.y-myPosition.y)*(targetPosition.y-myPosition.y)));
            Scene5Manager_M.instance.distance = Mathf.Sqrt((targetPosition.x-myPosition.x)*(targetPosition.x-myPosition.x)+(targetPosition.y-myPosition.y)*(targetPosition.y-myPosition.y));
            disTxt.GetComponent<Text>().text = "d:"+Scene5Manager_M.instance.distance;
            Vector3 direction = (targetPosition - myPosition).normalized;
            direction *= 1.5f;//mapのサイズに合わせて調整.
            Destroy(tmp);
            tmp = Instantiate(indicatorPrefab);
            tmp.transform.SetParent(targetCanvas.transform, false);
            if(Scene5Manager_M.instance.distance <= Scene5Manager_M.instance.threshold)
            {
                tmp.transform.position = centerPosition.transform.position;
            }
            else
            {
                tmp.transform.position = centerPosition.transform.position + direction;
            }
            //indicators.Add(tmp);
            //tmp.transform.LookAt(targetPosition);
            Debug.Log("dir");
            GetLocation();
            if (Scene5Manager_M.instance.isReceivePanelDisplay)
            {
                Destroy(tmp);
                Debug.Log("tomeru");
                Scene5Manager_M.instance.isDisplayDirectionsEnd = true;
                yield break;
            }
            maxSearchCount--;
            if(maxSearchCount < 0)
            {
                Destroy(tmp);
                Scene5Manager_M.instance.OnClickClose();
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private Vector3 CalculatePosition(float latitude, float longitude)
    {
        return new Vector3(longitude, latitude, 0);
    }

    public void SetInfo(string passPhrase, string opponentID){
        this.passPhrase = passPhrase;
        this.opponentID = opponentID;
    }
}
