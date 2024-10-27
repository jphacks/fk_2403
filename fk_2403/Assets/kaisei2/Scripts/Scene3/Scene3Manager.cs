using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene3Manager : MonoBehaviour
{
    [SerializeField] GameObject dispnametext;  // 表示するテキストオブジェクト
    [SerializeField] InputField dispnameinput; // 入力フィールド

    // Firebaseの初期化を管理するための参照
    private FirebaseManager firebaseManager;
    private string userId = "0Yg2ntXPZ50afumfzUMTQf4D3p73"; // 実際のユーザーIDを設定してください

    // Start is called before the first frame update
    void Start()
    {
        // FirebaseManagerのインスタンスを取得
        firebaseManager = FirebaseManager.instance;
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが初期化されていません。");
            return;
        }

        // Firebaseから表示名を読み取って表示
        LoadDisplayNameFromFirebase();
    }

    // 表示名を変更するボタンが押されたときの処理
    public void onClicked_changedispname()
    {
        dispnameinput.gameObject.SetActive(true);
    }

    // 入力が完了したときに呼び出される処理
    public void OnInputEnd()
    {
        string newDisplayName = dispnameinput.text;

        // コンソールに入力内容を表示し、テキストに反映
        Debug.Log("ユーザーが入力したテキスト: " + newDisplayName);
        dispnametext.GetComponent<Text>().text = newDisplayName;
        dispnameinput.gameObject.SetActive(false);

        // Firebaseに保存
        SaveDisplayNameToFirebase(newDisplayName);
    }

    // Firebaseに表示名を保存する処理
    private void SaveDisplayNameToFirebase(string displayName)
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、データを保存できません。");
            return;
        }

        // 保存するパス: ユーザーID/dispname
        string path = $"{userId}/dispname";

        // Firebaseにデータを書き込む
        firebaseManager.WriteData(path, displayName);
        Debug.Log($"Firebaseにデータを保存しました: {path} = {displayName}");
    }

    // Firebaseから表示名を読み取る処理
    private void LoadDisplayNameFromFirebase()
    {
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManagerが正しく初期化されていないため、データを読み取れません。");
            return;
        }

        // 読み取るパス: ユーザーID/dispname
        string path = $"{userId}/dispname";

        // Firebaseからデータを取得
        firebaseManager.ReadData(path, (result) =>
        {
            if (result != "error" && result != "NoData")
            {
                // 取得した表示名をテキストに設定
                dispnametext.GetComponent<Text>().text = result;
                Debug.Log($"Firebaseから表示名を取得しました: {result}");
            }
            else
            {
                Debug.LogWarning("表示名の取得に失敗しました。デフォルト名を使用します。");
                dispnametext.GetComponent<Text>().text = "ななし";
            }
        });
    }
}
