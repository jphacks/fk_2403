using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    //ログインをするためのスクリプト
    private void SingIn(FirebaseAuth auth, string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            // var newUser = task.Result;
            // Debug.Log($"サインインに成功しました。UserId is {newUser.UserId}");
        });
    }
}
