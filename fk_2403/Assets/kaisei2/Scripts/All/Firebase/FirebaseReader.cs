using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FirebaseReader : MonoBehaviour
{

    // サーバーからデータを取得する非同期メソッド
    public async Task<string> GetDataFromServer(string path)
    {
        string rtnStr = "error";
        if (FirebaseInitializer.DatabaseReference != null)
        {
            string[] keys = path.Split('/');
            var reference = FirebaseInitializer.DatabaseReference;

            // パスを分割して`Child`メソッドをチェーン
            foreach (string key in keys)
            {
                //Debug.Log(key);
                reference = reference.Child(key);
            }

            DataSnapshot snapshot = await reference.GetValueAsync();
            if (snapshot.Exists)
            {
                //Debug.Log("非同期：" + snapshot.Value);
                rtnStr = snapshot.Value.ToString();
            }
            else
            {
                Debug.Log("データの取得に失敗しました");
                rtnStr = "NoData";
            }
        }
        else
        {
            Debug.LogError("Firebase Databaseの参照が初期化されていません");
        }

        // サーバーから取得したデータを返す（例として固定値）
        return rtnStr;
    }
}
