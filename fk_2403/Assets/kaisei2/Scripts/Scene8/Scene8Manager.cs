using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Scene8Manager : MonoBehaviour
{
    [SerializeField] InputField Inputpassphrases; // 合言葉入力フィールド
    private FirebaseManager firebaseManager;

    public String[] profid = new string[2];
    public string userUid; // 自分のUID

    [SerializeField] GameObject writepanel;

    // 保存するデータ
    [SerializeField] InputField nameinput;
    [SerializeField] InputField Nickname;

    [SerializeField] Transform parent;
    [SerializeField] GameObject userIconPrefab;

    List <Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    void Start()
    {
        // FirebaseManagerのインスタンスを取得
        firebaseManager = FirebaseManager.instance;
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが初期化されていません。");
        }

        userUid = UserDataManager.instance.uid;

        Invoke("DisplayheldProfiles", 0.5f);
        Invoke("SetHeldProfiles", 1f);
    }

    //自分が書き込むプロフを一覧表示する
    public void DisplayheldProfiles()
    {

        SetUserIcon setUserIcon;
        FirebaseManager.instance.GetAllChildKeys($"users/{userUid}/heldProfiles", (keys) =>
        {
            foreach (string profId in keys)
            {
                //ボタン生成
                GameObject ins = Instantiate(userIconPrefab, parent);
                setUserIcon = ins.GetComponent<SetUserIcon>();
                ins.transform.Find("Button (Legacy)").GetComponent<Button>().onClick.AddListener(() =>
                {
                    DisplayMatchingProfile(profId, setUserIcon.index);
                    profid[0] = profId;
                });

            }
        });
    }

    void SetHeldProfiles()
    {
        FirebaseManager.instance.GetAllChildKeys($"users/{userUid}/heldProfiles", (keys) =>
        {
            foreach (string profId in keys)
            {
                
                FirebaseManager.instance.GetAllDataFromServer($"ProfInfo/{profId}", (dic) =>
                {
                    dicList.Add(dic);
                    FirebaseManager.instance.ReadData($"users/{dic["ownerId"].ToString()}/dispname", (value) =>
                    {
                        parent.transform.GetChild(0).gameObject.GetComponent<SetUserIcon>().OnSet(
                        value, dic["datetimeMemo"].ToString(), dic["passphrase"].ToString(), dicList.IndexOf(dic));
                    });

                });
                

            }
        });
    }

    public void onClicked_searchbutton()
    {
        string passphraseToSearch = Inputpassphrases.text;
        if (string.IsNullOrEmpty(passphraseToSearch))
        {
            Debug.LogWarning("検索するフレーズを入力してください。");
            return;
        }

        // Firebaseから入力された合言葉を検索
        CheckIfPassphraseExists(passphraseToSearch);
    }

    // 合言葉がExchangeProfに存在するか確認
    private void CheckIfPassphraseExists(string passphrase)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、検索を実行できません。");
            return;
        }

        string parentPath = "ExchangeProf";

        // FirebaseManagerを使用してデータを取得
        firebaseManager.ReadData($"{parentPath}/{passphrase}", (result) =>
        {
            if (!string.IsNullOrEmpty(result) && result != "NoData")
            {
                // 合言葉が存在した場合
                Debug.Log($"合言葉 '{passphrase}' が見つかりました。");

                // writepanelを表示
                writepanel.SetActive(true);

                // 取得したプロフIDを確認
                firebaseManager.GetAllChildKeys($"{parentPath}/{passphrase}", (keys) =>
                {
                    if (keys.Length == 2)
                    {
                        profid[0] = keys[0];
                        profid[1] = keys[1];

                        // 次のステップ：ProfInfoノードを確認
                        CheckProfInfoNodes(profid);
                    }
                    else
                    {
                        Debug.LogWarning("2つのプロフIDが見つかりませんでした。");
                    }
                });
            }
            else
            {
                Debug.LogWarning($"合言葉 '{passphrase}' が存在しません。");
            }
        });
    }

    private void CheckProfInfoNodes(string[] profids)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、検索を実行できません。");
            return;
        }

        foreach (string id in profids)
        {
            if (string.IsNullOrEmpty(id)) continue;

            string path = $"ProfInfo/{id}";
            firebaseManager.ReadData(path, (result) =>
            {
                // UIDが一致するかを確認
                string oppomentUserId = ExtractOppomentUserId(result);
                if (oppomentUserId == userUid)
                {
                    Debug.Log($"自分のUIDと一致するプロフIDが見つかりました: {id}");
                    DisplayMatchingProfile(id);
                }
            });
        }
    }

    private string ExtractOppomentUserId(string jsonResult)
    {
        return jsonResult;
    }

    private void DisplayMatchingProfile(string profId, int index = -1)
    {
        Debug.Log($"一致するプロフID: {profId}");

        // 一致するprofIdを保存処理で利用するために一時的に保存
        this.profid[0] = profId;

        // 書き込みパネルを表示
        writepanel.SetActive(true);

        if (index != -1)
        {
            Debug.Log(index);
            nameinput.text = dicList[index]["Name"].ToString();
            Nickname.text = dicList[index]["Nickname"].ToString();
        }
        
    }

    public void onClicked_savebutton()
    {
        if (string.IsNullOrEmpty(profid[0]))
        {
            Debug.LogError("保存するプロフIDが設定されていません。");
            return;
        }

        // 入力データを保存
        SaveProfileData(profid[0]);
        SceneManager.LoadScene("Scene3");
    }

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

        // FirebaseManager経由でデータを保存
        firebaseManager.WriteData($"ProfInfo/{profId}/Name", name);
        firebaseManager.WriteData($"ProfInfo/{profId}/Nickname", nickname);
    }
}
