using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseWriter writer;
    private FirebaseReader reader;

    private FirebaseInitializer initializer;

    void Start()
    {
        // FirebaseWriterとFirebaseReaderのインスタンスを取得
        writer = GetComponent<FirebaseWriter>();
        reader = GetComponent<FirebaseReader>();
        initializer = GetComponent<FirebaseInitializer>();

        // Firebaseの初期化完了イベントにリスナーを登録
        FirebaseInitializer.OnFirebaseInitialized += CheckConnection;

        //初期化
        initializer.InitializeFirebase();
    }

    // Firebase接続確認（初期化完了時に呼ばれる）
    private void CheckConnection()
    {
        if (FirebaseInitializer.DatabaseReference != null)
        {
            Debug.Log("Firebaseに接続済み");

            // テストデータを書き込む
            WriteSampleData();
        }
        else
        {
            Debug.LogError("Firebaseへの接続が未完了です。");
        }
    }

    //データを書き込む
    public void WriteData(string key, string data){
        if (writer != null)
        {
            writer.WriteData(key, data);
        }
    }

    //データベースからデータを読み込む
    public string ReadTestData(string key)
    {
        string rtn = "";
        if (reader != null)
        {
            rtn = reader.ReadData(key);
        }

        return rtn;
    }

    // サンプルデータを書き込む
    public void WriteSampleData()
    {
        if (writer != null)
        {
            writer.WriteData("sample_data", "This is a test message from FirebaseManager!");
        }
    }

    // データベースからデータを読み込み
    public void ReadTestData()
    {
        if (reader != null)
        {
            reader.ReadData("sample_data");
        }
    }

    void OnDestroy()
    {
        // イベントの解除
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
    }
}
