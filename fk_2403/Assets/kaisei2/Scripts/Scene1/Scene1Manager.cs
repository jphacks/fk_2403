using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // InputField用
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Scene1Manager : MonoBehaviour
{
    private FirebaseAuth auth;
    private Login login;
    private Register register;

    //チュートリアルの画面遷移で使う値
    private int tutorialnum = 0;

    public string email;
    public string password;


    //パネル
    [SerializeField] GameObject SignInPanel;
    [SerializeField] GameObject TermsOfUsePanel;
    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject TutorialPanel;

    //チュートリアルの画像群
    [SerializeField] GameObject[] TutorialImage;

    //インプットフィールド(ログイン画面)
    [SerializeField] private InputField loginmailField;
    [SerializeField] private InputField loginpassField;


    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        login = GetComponent<Login>();
        register = GetComponent<Register>();

        // 新規登録
        //register.CreateUserWithEmailAndPassword(email, password);
    }


    public void onClicked_toSignIn()
    {
        //新規登録画面を表示
        SignInPanel.SetActive(true);
    }
    public void onClicked_toTermsOfUse()
    {
        //利用規約を表示
        TermsOfUsePanel.SetActive(true);
    }

    public void onClicked_toLogin()
    {

        //ログイン画面を表示
        LoginPanel.SetActive(true);
    }

    public void onClicked_toTutorial()
    {
        //チュートリアル画面を表示
        TutorialPanel.SetActive(true);
    }

    public void onClicked_nextTutorial()
    {
        //チュートリアルの画面で進む
        if (tutorialnum < TutorialImage.Length - 1)
        {
            tutorialnum++;
        }
        else
        {
            GameObject.Find("TutorialPanel").transform.Find("FinishButton").gameObject.SetActive(true);
        }
        TutorialImage[tutorialnum].SetActive(true);
    }

    public void onClicked_backTutorial()
    {
        //チュートリアルの画面で戻る
        if (tutorialnum != 0)
        {
            TutorialImage[tutorialnum].SetActive(false);
            tutorialnum--;
        }
    }

    public void onClicked_logindone()
    {
        email = loginmailField.text;
        password = loginpassField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Email or password is empty. Please enter valid credentials.");
            return;
        }

        // ログイン処理の実行
        login.SignIn(auth, email, password);


        //ここで処理の完了を待ちたい
        SceneManager.LoadScene("Scene2");

    }

}
