using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    //ログインをするためのスクリプト
    public void SignIn(FirebaseAuth auth, string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("アカウント情報が違います" + task.Exception);
                return;
            }

            var user = task.Result?.User; // サインインしたユーザー情報を取得
            if (user != null && user.Email == email)
            {
                Debug.Log($"サインインに成功しました。ユーザーID: {user.UserId}");
            }
            else
            {
                Debug.LogWarning("サインイン情報が一致しませんでした。");
            }
        });
    }

}
