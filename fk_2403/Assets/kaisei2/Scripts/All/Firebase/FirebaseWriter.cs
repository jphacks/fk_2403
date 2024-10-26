using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Text;

public class FirebaseWriter : MonoBehaviour
{
    /// <summary>
    /// ネストが深い場合、スラッシュで挟む
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
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
