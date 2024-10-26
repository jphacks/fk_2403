using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // InputField用
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;

public class Scene1Manager : MonoBehaviour
{
    private FirebaseAuth auth;
    private Login login;
    private Register register;

    //チュートリアルの画面遷移で使う値
    private int tutorialnum = 0;

    public string email;
    public string password;



    [SerializeField] GameObject SignInCanvas;
    [SerializeField] GameObject TermsOfUseCanvas;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] GameObject TutorialCanvas;

    [SerializeField]
    GameObject[] TutorialPanel;



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

    public void onClicked_toSignIn()
    {
        //新規登録画面を表示
        SignInCanvas.SetActive(true);
    }
    public void onClicked_toTermsOfUse()
    {
        //利用規約を表示
        TermsOfUseCanvas.SetActive(true);
    }

    public void onClicked_toLogin()
    {
        //ログイン画面を表示
        LoginCanvas.SetActive(true);
    }

    public void onClicked_toTutorial()
    {
        //チュートリアル画面を表示
        TutorialCanvas.SetActive(true);
    }

    public void onClicked_nextTutorial()
    {
        TutorialPanel[tutorialnum].SetActive(true);
        tutorialnum++;


    }

}
