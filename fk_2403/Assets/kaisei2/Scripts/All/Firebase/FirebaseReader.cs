using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Text;
using System;

public class FirebaseReader : MonoBehaviour
{
    bool IsCompleted = false;
    string rtnStr = "";
    public void ReadNestedData(string path)
    {
        IsCompleted = false;

        if (FirebaseInitializer.DatabaseReference != null)
        {
            string[] keys = path.Split('/');
            var reference = FirebaseInitializer.DatabaseReference;

            // パスを分割して`Child`メソッドをチェーン
            foreach (string key in keys)
            {
                Debug.Log(key);
                reference = reference.Child(key);
            }

            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("非同期："+snapshot.Value);
                    rtnStr = "取得したデータ: " + snapshot.Value;
                    IsCompleted = true;
                }
                else
                {
                    Debug.LogError("データの取得に失敗しました: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Firebase Databaseの参照が初期化されていません");
        }
    }

    public bool GetIsCompleted(){
        return IsCompleted;
    }

    public string GetReadStr(){
        return rtnStr;
    }
}
