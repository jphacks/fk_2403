using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FirebaseReader : MonoBehaviour
{
    public async Task<string> GetDataFromServer(string path)
    {
        string rtnStr = "error";
        if (FirebaseInitializer.DatabaseReference != null)
        {
            string[] keys = path.Split('/');
            var reference = FirebaseInitializer.DatabaseReference;

            foreach (string key in keys)
            {
                reference = reference.Child(key);
            }

            DataSnapshot snapshot = await reference.GetValueAsync();
            if (snapshot.Exists)
            {
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
        return rtnStr;
    }

}
