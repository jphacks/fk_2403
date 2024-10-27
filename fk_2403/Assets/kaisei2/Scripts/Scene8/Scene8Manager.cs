using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class Scene8Manager : MonoBehaviour
{
    [SerializeField] InputField Inputpassphrases; // 入力フィールド
    private FirebaseManager firebaseManager;

    public String[] profid = new String[2];
    public string userUid; // 自分のUIDをここに設定

    [SerializeField] GameObject writepanel;

    // このデータを保存したい
    [SerializeField] InputField nameinput;
    [SerializeField] InputField Nickname;

    // Start is called before the first frame update
    void Start()
    {
        // FirebaseManagerのインスタンスを取得
        firebaseManager = FirebaseManager.instance;
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが初期化されていません。");
        }

        userUid = UserDataManager.instance.uid;
    }

    // 検索ボタンが押されたときの処理
    public void onClicked_searchbutton()
    {
        string passphraseToSearch = Inputpassphrases.text;
        if (string.IsNullOrEmpty(passphraseToSearch))
        {
            Debug.LogWarning("検索するフレーズを入力してください。");
            return;
        }

        // Firebaseから入力されたパスフレーズを検索
        CheckIfNodeExistsInFirebase(passphraseToSearch);
    }

    // Firebaseで特定のノードが存在するかを確認し、子オブジェクトのキーを取得
    private void CheckIfNodeExistsInFirebase(string nodeName)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、検索を実行できません。");
            return;
        }

        string parentPath = "ExchangeProf";

        FirebaseDatabase.DefaultInstance.GetReference(parentPath).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChild(nodeName))
                {
                    Debug.Log($"ノード '{nodeName}' が見つかりました。");

                    DataSnapshot nodeSnapshot = snapshot.Child(nodeName);
                    List<string> childKeys = new List<string>();
                    foreach (DataSnapshot child in nodeSnapshot.Children)
                    {
                        childKeys.Add(child.Key);
                    }

                    if (childKeys.Count > 0)
                    {
                        int i = 0;
                        foreach (string key in childKeys)
                        {
                            profid[i] = key;
                            i++;
                            Debug.Log($"キー: {key}");
                        }

                        CheckProfInfoNodes(profid);
                    }
                    else
                    {
                        Debug.Log($"'{nodeName}' には子オブジェクトがありません。");
                    }

                    if (!string.IsNullOrEmpty(profid[0]))
                    {
                        Debug.Log("プロフ帳を表示します。");
                        writepanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning($"ノード '{nodeName}' は存在しません。");
                }
            }
            else
            {
                Debug.LogError("データの取得に失敗しました。");
            }
        });
    }

    // ProfInfoノードでprofidをチェックし、oppomentUserIdが自分のUIDと一致するか確認
    private void CheckProfInfoNodes(string[] profids)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、検索を実行できません。");
            return;
        }

        string profInfoPath = "ProfInfo";

        foreach (string id in profids)
        {
            if (string.IsNullOrEmpty(id)) continue;

            FirebaseDatabase.DefaultInstance.GetReference(profInfoPath).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string oppomentUserId = snapshot.Child("oppomentUserId").Value?.ToString();
                        if (oppomentUserId == userUid)
                        {
                            Debug.Log($"ProfInfoに '{id}' が見つかり、UIDが一致しました: {oppomentUserId}");
                            DisplayMatchingProfile(id);
                        }
                        else
                        {
                            Debug.LogWarning($"ProfInfoに '{id}' が見つかりましたが、UIDが一致しません。");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"ProfInfoに '{id}' は見つかりませんでした。");
                    }
                }
                else
                {
                    Debug.LogError("ProfInfoのデータの取得に失敗しました。");
                }
            });
        }
    }

    // 一致するプロフIDの処理
    private void DisplayMatchingProfile(string profId)
    {
        Debug.Log($"一致するプロフID: {profId}");

        // 一致するprofIdを保存処理で利用するために一時的に保存
        this.profid[0] = profId;
    }

    // 保存ボタンが押されたときの処理
    public void onClicked_savebutton()
    {
        if (string.IsNullOrEmpty(profid[0]))
        {
            Debug.LogError("保存するプロフIDが設定されていません。");
            return;
        }

        // 入力フィールドのデータを保存
        SaveProfileData(profid[0]);
    }

    // 入力フィールドのデータをFirebaseに保存
    private void SaveProfileData(string profId)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、データ保存を実行できません。");
            return;
        }

        // 入力された名前とニックネームを取得
        string name = nameinput.text;
        string nickname = Nickname.text;

        // 入力フィールドのオブジェクト名をキーとして使用
        string nameKey = nameinput.gameObject.name;
        string nicknameKey = Nickname.gameObject.name;

        // Firebaseにデータを保存
        DatabaseReference profRef = FirebaseDatabase.DefaultInstance.GetReference("ProfInfo").Child(profId);
        profRef.Child(nameKey).SetValueAsync(name).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"名前 '{name}' が '{nameKey}' キーとして保存されました。");
            }
            else
            {
                Debug.LogError("名前の保存に失敗しました。");
            }
        });

        profRef.Child(nicknameKey).SetValueAsync(nickname).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"ニックネーム '{nickname}' が '{nicknameKey}' キーとして保存されました。");
            }
            else
            {
                Debug.LogError("ニックネームの保存に失敗しました。");
            }
        });
    }
}
