using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance = null;
    public string uid = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetUsernameToServer(string username){
        FirebaseManager.instance.WriteData(uid+"/username", username);
    }

    public void SetAvatarDataToServer(string[] paths){
        FirebaseManager.instance.WriteData(uid+"/eyes", paths[0]);
        FirebaseManager.instance.WriteData(uid+"/mouth", paths[1]);
        FirebaseManager.instance.WriteData(uid+"/eyebrow", paths[2]);
        FirebaseManager.instance.WriteData(uid+"/hair", paths[3]);
        FirebaseManager.instance.WriteData(uid+"/accessories", paths[4]);
    }

    public void SetRecieveProf(string profId){
        FirebaseManager.instance.WriteData(uid+"/RecieveProf/"+profId, "");
    }

    public void SetResultProf(string profId){
        FirebaseManager.instance.WriteData(uid+"/ResultProf/"+profId, "");
    }
}
