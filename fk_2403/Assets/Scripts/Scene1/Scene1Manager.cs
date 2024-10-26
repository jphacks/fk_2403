using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // InputField用
using Firebase;
using Firebase.Auth;

public class Scene1Manager : MonoBehaviour
{
    private FirebaseAuth auth;
    private Login login;
    private Register register;

    public string email;
    public string password;


    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        login = GetComponent<Login>();
        register = GetComponent<Register>();

        // 新規登録
        //register.CreateUserWithEmailAndPassword(email, password);
    }

    // サインインをボタンから実行するメソッド
    public void OnSignInButtonPressed()
    {

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Email or password is empty. Please enter valid credentials.");
            return;
        }

        // ログイン処理の実行
        login.SignIn(auth, email, password);
    }
}
