using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Profiling;
using UnityEngine;

public class CreateSendProfilePassphrase : MonoBehaviour
{
    public void SetPassphrase(string path, System.Action action)
    {
        FirebaseManager.instance.GetChildrenNum(path, (num) => {
            Debug.Log(num);
            if(num > 1){
                Debug.Log("aaaaaaa");
            }else{
                action();
            }
        });

        // FirebaseManager.instance.ReadData(path, (value) => {
        //     if(!value.Equals("NoData")){
                
        //     }
        // });
    }
}
