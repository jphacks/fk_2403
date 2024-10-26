using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocationServiceScript_M : MonoBehaviour
{
    [SerializeField] GameObject indicatorPrefab;//相手のポイント.
    [SerializeField] Canvas targetCanvs;
    [SerializeField] GameObject centerPosition;
    [SerializeField] GameObject tmp;
    //List<GameObject> indicators = new List<GameObject>();

    private float latitude;
    private float longitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetLocation()
    {
        //位置情報が拒否されていたら終了.
        if (!Input.location.isEnabledByUser)
            yield break;

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
            print("Timed out");
            yield break;
        }

        // 位置情報が取れたかどうか.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // ここで位置情報が取れる.
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            print("Location: " + latitude + " " + longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        //位置情報の取得を終了.
        Input.location.Stop();
    }

    public double GetLocationDistance(float latitude1, float longtitude1, float latitude2, float longtitude2)
    {
        return Mathf.Sqrt((latitude2-latitude1)*(latitude2-latitude1)+(longtitude2-longtitude1)*(longtitude2-longtitude1));
    }

    public void OnClickGetLocation()//自身座標と取ってくる.
    {
        StartCoroutine(GetLocation());
    }

    public void OnClickDisplayDirections()
    {
        //合言葉が同じ人の中から検索.
        //
        StartCoroutine(DisplayDirections(33.67335f, 130.4413f, 33.67372f, 130.4412f));
    }

    public IEnumerator DisplayDirections(float myLatitude, float myLongitude, float targetLatitude, float targetLongitude)
    {
        while(true)
        {
            Vector3 targetPosition = CalculatePosition(targetLatitude, targetLongitude);
            Vector3 myPosition = CalculatePosition(myLatitude, myLongitude);
            //Debug.Log(Mathf.Sqrt((targetPosition.x-myPosition.x)*(targetPosition.x-myPosition.x)+(targetPosition.y-myPosition.y)*(targetPosition.y-myPosition.y)));
            Vector3 direction = (targetPosition - myPosition).normalized;
            //Debug.Log(direction);
            direction *= 2f;//mapのサイズに合わせて調整.
            Destroy(tmp);
            tmp = Instantiate(indicatorPrefab);
            tmp.transform.SetParent(targetCanvs.transform, false);
            tmp.transform.position = centerPosition.transform.position + direction;
            //indicators.Add(tmp);
            //tmp.transform.LookAt(targetPosition);
            Debug.Log("dir");
            if(Scene5Manager_M.instance.isReceivePanelDisplay)
            {
                Destroy(tmp);
                Debug.Log("tomeru");
                Scene5Manager_M.instance.isDisplayDirectionsEnd = true; 
                yield break;
            }
            yield return new WaitForSeconds(1);
        }       
    }

    private Vector3 CalculatePosition(float latitude, float longitude)
    {
        return new Vector3(longitude, latitude, 0);
    }
}
