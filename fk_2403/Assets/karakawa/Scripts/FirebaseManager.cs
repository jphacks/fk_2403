using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseWriter writer;
    private FirebaseReader reader;

    void Start()
    {
        // FirebaseWriterとFirebaseReaderのインスタンスを取得
        writer = FindObjectOfType<FirebaseWriter>();
        reader = FindObjectOfType<FirebaseReader>();

        // Firebaseの初期化完了イベントにリスナーを登録
        FirebaseInitializer.OnFirebaseInitialized += CheckConnection;
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
