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
        //PlayerPrefs.DeleteAll();
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
            Debug.Log("動いた");
            PlayerPrefs.SetString("uid", UserDataManager.instance.uid);
            PlayerPrefs.Save();
        }
    }
}
