using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

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
        Debug.Log("データを取得中...");

        string result = await reader.GetDataFromServer(path);

        Debug.Log("データの取得完了: " + result);
        
        // データを取得後に追加の処理を実行
        action(result);
    }


    // public void testa()
    // {
    //     WriteData("test/id/aaa", "teststr");
    // }

    // public void testb()
    // {
    //     ReadData("test/id/aaa", (value) =>
    //     {
    //         Debug.Log(value);
    //     });
    // }

    void OnDestroy()
    {
        // イベントの解除
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
    }
}
