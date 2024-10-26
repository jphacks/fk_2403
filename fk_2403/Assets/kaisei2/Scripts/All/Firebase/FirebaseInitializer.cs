using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System;

public class FirebaseInitializer : MonoBehaviour
{
    public static DatabaseReference DatabaseReference { get; private set; }
    public static event Action OnFirebaseInitialized; // 初期化完了イベント

    public void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebaseのデータベースルート参照を取得
                DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Realtime Databaseに接続しました");

                // 初期化完了イベントを発行
                OnFirebaseInitialized?.Invoke();
            }
            else
            {
                Debug.LogError("Firebaseの依存関係が解決できませんでした: " + task.Result);
            }
        });
    }
}
