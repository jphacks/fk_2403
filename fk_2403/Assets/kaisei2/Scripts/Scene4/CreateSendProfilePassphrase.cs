using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Profiling;
using UnityEngine;

public class CreateSendProfilePassphrase : MonoBehaviour
{
    public void SetPassphrase(string path, System.Action action, System.Action error){
        
        FirebaseManager.instance.ReadData(path, (value) => {
            if(value.Equals("NoData")){
                action();
            }else{
                error();
            }
        });
    }
}
