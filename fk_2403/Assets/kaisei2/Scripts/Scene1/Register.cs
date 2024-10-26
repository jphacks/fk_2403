using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System;

public class Register : MonoBehaviour
{
    public String errorMessage = "";
    // 新規登録をするためのスクリプト
    public async Task<bool> CreateUserWithEmailAndPassword(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;

        try
        {
            //var newUser = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            //Debug.Log($"Firebase上でメールアドレス認証によるユーザ作成に成功しました。 UserId is {newUser.UserId}");
            var userCredential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = userCredential.User;
            UserDataManager.instance.uid = newUser.UserId;

            UserDataManager.instance.uid = newUser.UserId;
            return true; // 成功した場合
        }


        catch (System.Exception ex)
        {
            if (ex.Message.Contains("The email address is badly formatted"))
            {
                errorMessage = "メールアドレスが正しくありません。";

                Debug.Log("メールアドレスが正しくありません。");
            }
            else if (ex.Message.Contains("The given password is invalid"))
            {
                errorMessage = "パスワードが使用できません。";
                Debug.Log("パスワードが使用できません。");
            }
            else if (ex.Message.Contains("The email address is already in use by another account"))
            {
                errorMessage = "メールアドレスが既に登録されています。";
                Debug.Log("メールアドレスが既に登録されています。");
            }
            //Debug.LogWarning($"ユーザー作成に失敗しました: {ex.Message}" + ex.ToString());
            return false; // 失敗した場合
        }
    }
}
