using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiyoshiTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Quit");
    }

    public void OnApplicationPause(bool pause)//ios//アプリを閉じたとき:true//
    {
        if(pause)
        {
            Debug.Log("pause");
        }
    }
}
