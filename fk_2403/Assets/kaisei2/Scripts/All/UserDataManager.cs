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
        FirebaseManager.instance.WriteData("users/" + uid + "/username", username);
    }

    public void SetAvatarDataToServer(string[] paths)
    {
        string path = "users/" + uid + "/avatar";
        FirebaseManager.instance.WriteData($"{path}/eyes", paths[0]);
        FirebaseManager.instance.WriteData($"{path}/mouth", paths[1]);
        FirebaseManager.instance.WriteData($"{path}/eyebrow", paths[2]);
        FirebaseManager.instance.WriteData($"{path}/hair", paths[3]);
        FirebaseManager.instance.WriteData($"{path}/accessories", paths[4]);
    }

    public void SetRecieveProf(string profId)
    {
        FirebaseManager.instance.WriteData("users/" + uid + "/RecieveProf/" + profId, "");
    }

    public void SetResultProf(string profId)
    {
        FirebaseManager.instance.WriteData("users/" + uid + "/ResultProf/" + profId, "");
    }
}
