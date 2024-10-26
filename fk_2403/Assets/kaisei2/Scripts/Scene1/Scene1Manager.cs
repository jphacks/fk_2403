using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // InputField用
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class Scene1Manager : MonoBehaviour
{
    private FirebaseAuth auth;
    private Login login;
    private Register register;

    //チュートリアルの画面遷移で使う値
    private int tutorialnum = 0;

    public string email;
    public string password;

    //利用規約に同意しているか
    public bool check = false;

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

    //インプットフィールド(新規登録)
    [SerializeField] private InputField signinmailField;
    [SerializeField] private InputField signinpassField;
    [SerializeField] private InputField signinrepassField;


    // エラーメッセージを表示するprefab
    [SerializeField] private GameObject Errorobj;

    [SerializeField] GameObject checkboxobj;
    [SerializeField] Sprite checksprite;
    [SerializeField] Sprite checkbasesprite;


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
        if (tutorialnum < TutorialImage.Length - 2)
        {
            tutorialnum++;
            TutorialImage[tutorialnum].SetActive(true);
        }
        else
        {
            TutorialImage[TutorialImage.Length - 1].SetActive(true);
            GameObject.Find("TutorialPanel").transform.Find("FinishButton").gameObject.SetActive(true);
        }
        Debug.Log(tutorialnum);

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

    public async void onClicked_logindone()
    {
        email = loginmailField.text;
        password = loginpassField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowErrorMessage("メールアドレスまたはパスワードが空です。正しい情報を入力してください。");
            return;
        }


        // ログイン処理の実行
        bool loginSuccess = await login.SignIn(auth, email, password);

        // ログインが成功したらシーンを遷移
        if (loginSuccess)
        {
            SceneManager.LoadScene("Scene3");
        }
        else
        {
            ShowErrorMessage("サインインに失敗しました。");
            Debug.LogWarning("ログインに失敗しました。シーン遷移をキャンセルします。");
        }



    }

    // アカウント登録ボタンが押されたときの処理
    public async void onClicked_signindone()
    {
        string signinmail = signinmailField.text;
        string signinpass = signinpassField.text;
        string signinrepass = signinrepassField.text;

        // パスワードが一致するか確認
        if (signinpass != signinrepass)
        {
            ShowErrorMessage("パスワードが一致しません。");
            return;
        }

        // 入力が正しいか確認
        if (string.IsNullOrEmpty(signinmail) || string.IsNullOrEmpty(signinpass))
        {
            ShowErrorMessage("メールアドレスまたはパスワードが空です。正しい情報を入力してください。");
            Debug.LogWarning("メールアドレスまたはパスワードが空です。正しい情報を入力してください。");
            return;
        }

        if (check)
        {
            // 新規登録処理の実行
            bool registerSuccess = await register.CreateUserWithEmailAndPassword(signinmail, signinpass);
            // 新規登録が成功したらシーンを遷移
            if (registerSuccess)
            {
                SceneManager.LoadScene("Scene2"); // 登録完了後のシーンへ遷移
            }
            else
            {
                ShowErrorMessage(register.errorMessage);
                Debug.LogWarning("登録に失敗しました。シーン遷移をキャンセルします。");
            }
        }
        else
        {
            ShowErrorMessage("利用規約に同意していません。");
        }
    }

    // エラーメッセージを表示する関数
    public void ShowErrorMessage(string message)
    {
        if (Errorobj != null)
        {
            // プレハブを生成
            GameObject errorInstance = Instantiate(Errorobj, GameObject.Find("Canvas").transform);
            // 生成したプレハブの中のTextコンポーネントを探す
            Text errorText = errorInstance.GetComponentInChildren<Text>();
            if (errorText != null)
            {
                errorText.text = message;
            }
            errorText.text = message;
            // 一定時間後にエラーメッセージを自動的に削除する (5秒後)
            Destroy(errorInstance, 3f);
        }
    }

    public void onClicked_Backbutton(GameObject storeobj)
    {
        //残したいパネルをアタッチしておく
        // storeobjの親オブジェクトを取得
        Transform parentTransform = storeobj.transform.parent;

        if (parentTransform != null)
        {
            // 親の子オブジェクトをすべてチェック
            foreach (Transform child in parentTransform)
            {
                // storeobj自身を除外して、他のオブジェクトを非表示にする
                if (child.gameObject != storeobj)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("親オブジェクトが見つかりませんでした。");
        }
    }

    public void onClicked_checkbox()
    {
        if (check)
        {
            check = false;
            checkboxobj.GetComponent<UnityEngine.UI.Image>().sprite = checkbasesprite;
        }
        else
        {
            check = true;
            checkboxobj.GetComponent<UnityEngine.UI.Image>().sprite = checksprite;
        }


    }
}
