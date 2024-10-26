using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSendProfilePassword : MonoBehaviour
{
    public void SetPasssword(string password, System.Action action){
        string path = "ExchangeProf/Password";
        FirebaseManager.instance.ReadData(path, (value) => {
            Debug.Log(value);
            FirebaseManager.instance.WriteData(path, password);
            FirebaseManager.instance.WriteData(path+"/ID", "MyProfID");
            action();
        });
    }
}
