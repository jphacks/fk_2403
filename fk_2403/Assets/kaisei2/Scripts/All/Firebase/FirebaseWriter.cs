using Firebase.Extensions;
using UnityEngine;

public class FirebaseWriter : MonoBehaviour
{
    public void WriteData(string key, string value)
    {
        if (FirebaseInitializer.DatabaseReference != null)
        {
            FirebaseInitializer.DatabaseReference.Child(key).SetValueAsync(value).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("データベースに書き込みが完了しました: " + key + " = " + value);
                }
                else
                {
                    Debug.LogError("データベースへの書き込みに失敗しました: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Firebase Databaseの参照が初期化されていません");
        }
    }
}
