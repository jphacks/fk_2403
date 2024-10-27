using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using System.Collections.Generic;

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

            // Firebaseの初期化完了イベントにリスナーを登録
            FirebaseInitializer.OnFirebaseInitialized += CheckConnection;
            FirebaseInitializer.OnFirebaseInitialized += InitializeManagerAfterFirebase;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitializeManagerAfterFirebase()
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

    // データを書き込む
    public void WriteData(string key, string data)
    {
        if (writer != null)
        {
            writer.WriteData(key, data);
        }
    }

    // データベースからデータを読み込む
    public async void ReadData(string path, System.Action<string> action)
    {
        // Firebaseが初期化されるのを待つ
        await WaitForFirebaseInitialization();

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

    private async Task WaitForFirebaseInitialization()
    {
        while (FirebaseInitializer.DatabaseReference == null)
        {
            Debug.Log("Firebaseが初期化されるのを待っています...");
            await Task.Delay(100); // 100ms 待機
        }
    }

    public void AddAutoID(string key, System.Action<string> action)
    {
        DatabaseReference newRef = FirebaseInitializer.DatabaseReference.Child(key).Push();
        newRef.SetValueAsync("").ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                action(newRef.Key);
                Debug.Log("Empty node added successfully at path: " + newRef.Key);
            }
            else
            {
                Debug.LogError("Error adding empty node: " + task.Exception);
            }
        });
    }

    public void AddDictionaryToFirebase(string path, Dictionary<string, object> data)
    {
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
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
        FirebaseInitializer.OnFirebaseInitialized -= InitializeManagerAfterFirebase;
    }
}
