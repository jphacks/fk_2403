using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance = null;
    public string uid = "";

    private void Awake()
    {
        //他のスクリプトを弄りたいときはコメントアウトを外して
        PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (PlayerPrefs.HasKey("uid"))
        {
            UserDataManager.instance.uid = PlayerPrefs.GetString("uid", "");
            if (uid != "")
            {
                SceneManager.LoadScene("Scene3");
            }

        }
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        //アプリがバックグラウンドに移行した時
        if (pauseStatus)
        {
            PlayerPrefs.SetString("uid", UserDataManager.instance.uid);
            PlayerPrefs.Save();
        }
    }

    public void SetUsernameToServer(string username)
    {
        FirebaseManager.instance.WriteData(uid + "/username", username);
    }

    public void SetAvatarDataToServer(string[] paths)
    {
        FirebaseManager.instance.WriteData(uid + "/eyes", paths[0]);
        FirebaseManager.instance.WriteData(uid + "/mouth", paths[1]);
        FirebaseManager.instance.WriteData(uid + "/eyebrow", paths[2]);
        FirebaseManager.instance.WriteData(uid + "/hair", paths[3]);
        FirebaseManager.instance.WriteData(uid + "/accessories", paths[4]);
    }

    public void SetRecieveProf(string profId)
    {
        FirebaseManager.instance.WriteData(uid + "/RecieveProf/" + profId, "");
    }

    public void SetResultProf(string profId)
    {
        FirebaseManager.instance.WriteData(uid + "/ResultProf/" + profId, "");
    }
}
