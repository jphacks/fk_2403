using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class Register : MonoBehaviour
{
    //新規登録をするためのスクリプト
    private void CreateUserWithEmailAndPassword(string email, string password)
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
        });
    }
}
