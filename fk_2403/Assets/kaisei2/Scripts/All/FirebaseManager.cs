using Unity.VisualScripting;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseWriter writer;
    private FirebaseReader reader;

    private FirebaseInitializer initializer;
    private bool IsReadWaiting = true;
    private string ReadStr = "";
    float time = 0f;
    float interval = 0.5f;
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

    private void Update() {
        // if(IsReadWaiting){
        //     time += Time.deltaTime;
        //     if(time >= interval)
        //     {
        //         time = 0;
        //         if(reader.GetIsCompleted())
        //         {
        //             ReadStr = reader.GetReadStr();
        //             IsReadWaiting = false;
        //         }

        //     }
        // }
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
    public void WriteData(string key, string data){
        if (writer != null)
        {
            writer.WriteData(key, data);
        }
    }

    //データベースからデータを読み込む
    public void ReadData(string key)
    {
        reader.ReadNestedData(key);
        IsReadWaiting = true;
    }

    public void testa()
    {
        WriteData("test/id/aaa", "teststr");
    }

    public void testb()
    {
        ReadData("test/id/aaa");
        Debug.Log(reader.GetReadStr());

    }

    void OnDestroy()
    {
        // イベントの解除
        FirebaseInitializer.OnFirebaseInitialized -= CheckConnection;
    }
}
