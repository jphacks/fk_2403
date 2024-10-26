using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Text;

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
                    rtnStr = "読み込みデータ: \n" + ParseSnapshot(snapshot);
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

    private string ParseSnapshot(DataSnapshot snapshot, int level = 0)
    {
        StringBuilder sb = new StringBuilder();
        string indent = new string(' ', level * 2); // インデント用のスペース

        // 子要素がある場合は再帰的に処理
        if (snapshot.HasChildren)
        {
            foreach (var child in snapshot.Children)
            {
                sb.AppendLine($"{indent}{child.Key}:");
                sb.Append(ParseSnapshot(child, level + 1));
            }
        }
        else
        {
            // 子要素がない場合はその値を追加
            sb.AppendLine($"{indent}{snapshot.Key}: {snapshot.Value}");
        }

        return sb.ToString();
    }
}
