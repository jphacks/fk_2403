using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseReader : MonoBehaviour
{
    public string ReadData(string key)
    {
        string rtnStr = "error";

        if (FirebaseInitializer.DatabaseReference != null)
        {
            FirebaseInitializer.DatabaseReference.Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    rtnStr = "読み込みデータ: " + key + " = " + snapshot.Value;
                    Debug.Log(rtnStr);
                }
                else
                {
                    Debug.LogError("データベースからの読み込みに失敗しました: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Firebase Databaseの参照が初期化されていません");
        }

        return rtnStr;
    }
}
