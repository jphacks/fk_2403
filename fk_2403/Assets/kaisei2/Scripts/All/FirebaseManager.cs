using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using System.Collections.Generic;

// DictionaryをJSONに変換するためのヘルパークラス
[System.Serializable]
public class Serialization<TKey, TValue>
{
    public TKey[] keys;
    public TValue[] values;

    public Serialization(Dictionary<TKey, TValue> dictionary)
    {
        keys = new TKey[dictionary.Count];
        values = new TValue[dictionary.Count];
        int i = 0;
        foreach (var pair in dictionary)
        {
            keys[i] = pair.Key;
            values[i] = pair.Value;
            i++;
        }
    }
}

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance = null;
    private FirebaseWriter writer;
    private FirebaseReader reader;

    private FirebaseInitializer initializer;
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

        // Firebaseの初期化完了イベントにリスナーを登録
        FirebaseInitializer.OnFirebaseInitialized += CheckConnection;

    }

    void Start()
    {
        // FirebaseWriterとFirebaseReaderのインスタンスを取得
        writer = GetComponent<FirebaseWriter>();
        reader = GetComponent<FirebaseReader>();
        initializer = GetComponent<FirebaseInitializer>();


        if (reader == null)
        {
            Debug.LogError("FirebaseReaderが正しく初期化されていません。スクリプトがアタッチされているか確認してください。");
        }
        else
        {
            Debug.Log("FirebaseReaderが正常に初期化されました。");
        }


        //初期化
        initializer.InitializeFirebase();

    }

    // Firebase接続確認（初期化完了時に呼ばれる）
    private void CheckConnection()
    {
        if (FirebaseInitializer.DatabaseReference != null)
        {
            Debug.Log("Firebaseに接続済み");
        }
        else
        {
            Debug.LogError("Firebaseへの接続が未完了です。");
        }
    }

    //データを書き込む
    public void WriteData(string key, string data)
    {
        if (writer != null)
        {
            writer.WriteData(key, data);
        }
    }

    //データベースからデータを読み込む
    /// <summary>
    /// 非同期なので、値を使って何かしたいときはその後の処理全部actionにいれてください
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    public async void ReadData(string path, System.Action<string> action)
    {
        if (reader == null)
        {
            Debug.LogError("FirebaseReaderがnullです。正しく初期化されているか確認してください。");
            return;
        }
        Debug.Log("データを取得中...");

        var result = await reader.GetDataFromServer(path);

        Debug.Log("データの取得完了: " + result);

        // データを取得後に追加の処理を実行
        action(result);
    }

    public void AddAutoID(string key, System.Action<string> action)
    {
        // 新しいノードの参照を取得
        DatabaseReference newRef = FirebaseInitializer.DatabaseReference.Child(key).Push();
        // 自動生成IDでデータを生成
        newRef.SetValueAsync("").ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                action(newRef.Key);
                Debug.Log("Empty node added successfully at path: " + newRef.Key);
            }
            else
            {
                // エラー詳細をログに表示
                Debug.LogError("Error adding empty node: " + task.Exception);
            }
        });
    }

    public void AddDictionaryToFirebase(string path, Dictionary<string, object> data)
    {
        // DictionaryをJSONに変換
        string jsonData = JsonUtility.ToJson(new Serialization<string, object>(data));
        FirebaseInitializer.DatabaseReference.Child(path).SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Dictionary added successfully at path: " + path);
            }
            else
            {
                Debug.LogError("Error adding dictionary: " + task.Exception);
            }
        });
    }

    void OnDestroy()
    {
        // イベントの解除
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
    }
}
