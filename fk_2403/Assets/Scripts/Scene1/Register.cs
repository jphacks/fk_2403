using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class Register : MonoBehaviour
{
    //新規登録をするためのスクリプト
    public void CreateUserWithEmailAndPassword(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogWarning($"SignInWithCredentialAsync was fault. {task.Exception}");
                return;
            }

            // var newUser = task.Result;
            // Debug.Log($"Firebase上でメールアドレス認証によるユーザ作成に成功しました。 UserId is {newUser.UserId}");
        });
    }
}
