using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationServiceScript_M : MonoBehaviour
{
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

        //開始されるまでに少し時間がかかる,20までは待つ.
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
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            
        }

        //位置情報の取得を終了.
        Input.location.Stop();
    }

    public double GetLocationDistance(float latitude1, float longtitude1, float latitude2, float longtitude2)
    {
        return Mathf.Sqrt((latitude2-latitude1)*(latitude2-latitude1)+(longtitude2-longtitude1)*(longtitude2-longtitude1));
    }
}
