using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Text;
using System;
using System.Threading.Tasks;

public class FirebaseReader : MonoBehaviour
{
    public bool IsCompleted { get; private set; }
    public string rtnStr { get; private set; }

    public async Task ReadNestedDataAsync(string path)
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

            try
            {
                DataSnapshot snapshot = await reference.GetValueAsync();
                //Debug.Log("非同期：" + snapshot.Value);
                rtnStr = snapshot.Value.ToString();
                IsCompleted = true; // 完了フラグを設定
            }
            catch (System.Exception ex)
            {
                Debug.LogError("データの取得に失敗しました: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Firebase Databaseの参照が初期化されていません");
        }
    }
}
