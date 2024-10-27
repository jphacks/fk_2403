using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class Scene7Manager : MonoBehaviour
{
    private string userUid; // 自分のUIDをここに設定
    private FirebaseManager firebaseManager;
    [SerializeField] Text nameText;
    [SerializeField] Text nicknameText;

    // Start is called before the first frame update
    void Start()
    {
        // FirebaseManagerのインスタンスを取得
        firebaseManager = FirebaseManager.instance;
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが初期化されていません。");
        }

        // 自分のUIDを取得
        userUid = UserDataManager.instance.uid;

        // 自分のUIDの下にあるプロフIDを取得してコンソールに表示
        FetchAndDisplayProfileIds();

        //書いてもらったプロフを表示
        LoadResultProf();
    }

    // 自分のUIDのノードの下にあるプロフIDを取得して表示するメソッド
    private void FetchAndDisplayProfileIds()
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、プロフIDを取得できません。");
            return;
        }

        // UIDノードのパスを指定
        string userProfilePath = $"UserProfiles/{userUid}";

        // Firebaseから自分のUIDの下にあるデータを取得
        FirebaseDatabase.DefaultInstance.GetReference(userProfilePath).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // プロフIDを取得してコンソールに表示
                    List<string> profileIds = new List<string>();
                    foreach (DataSnapshot child in snapshot.Children)
                    {
                        string profileId = child.Key;
                        profileIds.Add(profileId);
                        Debug.Log($"取得したプロフID: {profileId}");

                        // プロフIDに基づいてさらに情報を取得したい場合はここに追加
                        FetchProfileData(profileId);
                    }

                    if (profileIds.Count == 0)
                    {
                        Debug.LogWarning("プロフIDが見つかりませんでした。");
                    }
                }
                else
                {
                    Debug.LogWarning("指定したユーザーのプロフデータが存在しません。");
                }
            }
            else
            {
                Debug.LogError("プロフデータの取得に失敗しました。");
            }
        });
    }

    // プロフIDに基づいて追加情報を取得するメソッド（例）
    private void FetchProfileData(string profileId)
    {
        string profilePath = $"ProfInfo/{profileId}";

        FirebaseDatabase.DefaultInstance.GetReference(profilePath).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string nickname = snapshot.Child("nickname").Value?.ToString();
                    string name = snapshot.Child("name").Value?.ToString();

                    Debug.Log($"プロフID: {profileId} の情報 - 名前: {name}, ニックネーム: {nickname}");
                }
                else
                {
                    Debug.LogWarning($"プロフID {profileId} に関連するデータが見つかりませんでした。");
                }
            }
            else
            {
                Debug.LogError("プロフデータの取得に失敗しました。");
            }
        });
    }

    void LoadResultProf(){
        FirebaseManager.instance.GetAllChildKeys("ProfInfo", (keys) => {
            foreach(string key in keys){
                //読み込み
                ReadName(key);
            }
        });
    }

    void ReadName(string key){
        FirebaseManager.instance.ReadData($"ProfInfo/{key}/Name", (value) => {
            if(!value.Equals("NoData")){
                nameText.text = value;
            }
            
        });
        FirebaseManager.instance.ReadData($"ProfInfo/{key}/Nickname", (value) => {
            if(value.Equals("NoData")){
                nicknameText.text = value;
            }
            
        });
    }
}
