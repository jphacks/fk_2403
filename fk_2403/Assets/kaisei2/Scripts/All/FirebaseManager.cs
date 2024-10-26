using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseWriter writer;
    private FirebaseReader reader;

    private FirebaseInitializer initializer;
    private void Awake()
    {
        // Firebaseの初期化完了イベントにリスナーを登録
        FirebaseInitializer.OnFirebaseInitialized += CheckConnection;
    }

    void Start()
    {
        // FirebaseWriterとFirebaseReaderのインスタンスを取得
        writer = GetComponent<FirebaseWriter>();
        reader = GetComponent<FirebaseReader>();
        initializer = GetComponent<FirebaseInitializer>();

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
    public async void ReadData(string path, System.Action<string> action)
    {
        await reader.ReadNestedDataAsync(path);

        if (reader.IsCompleted)
        {
            // データ取得後の処理
            //Debug.Log("取得したデータを使って処理します: " + reader.rtnStr);
            // ここにさらに処理を追加できます
            action(reader.rtnStr);
        }
        else
        {
            Debug.LogWarning("データが取得できませんでした");
        }
    }


    public void testa()
    {
        WriteData("test/id/aaa", "teststr");
    }

    public void testb()
    {
        ReadData("test/id/aaa", (value) =>
        {
            Debug.Log(value);
        });
    }

    void OnDestroy()
    {
        // イベントの解除
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
    }
}
