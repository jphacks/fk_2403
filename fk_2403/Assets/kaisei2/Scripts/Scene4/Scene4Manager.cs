using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene4Manager : MonoBehaviour
{
    [SerializeField] InputField passphrase_input;
    [SerializeField] InputField datetime_input;

    int profIndex = 0;
    bool isNewCreate = true;

    [SerializeField] GameObject errorPrefab;
    [SerializeField] Transform parent;

    public void OnSendProfileButtonClicked()
    {
        string passphrase = passphrase_input.text;
        string datetime = datetime_input.text;
        SetPassphrase("ExchangeProf/" + passphrase, passphrase, () =>
        {
            if (isNewCreate)
            {
                //ユーザIDに合言葉を紐付け
                FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passphrase}", "");

                FirebaseManager.instance.AddAutoID("ExchangeProf/" + passphrase, (value) =>
                {
                    //まずは自分側のプロフをつくる
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/datetimeMemo", datetime);
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/ownerId", UserDataManager.instance.uid);
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/passphrase", passphrase);
                    //相手のIDがわからないので自分のIDを自分のプロフに書いて終わり

                });
            }
            else
            {
                //ユーザIDに合言葉を紐付け
                FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passphrase}", "");

                FirebaseManager.instance.GetAllChildKeys("ExchangeProf/" + passphrase, (str) =>
                {
                    //相手のプロフIDを受け取る　＝＞　str

                    //相手側のプロフに自分のIDを書き込む
                    FirebaseManager.instance.WriteData("ProfInfo/" + str[0] + "/oppomentId", UserDataManager.instance.uid);
                    //ユーザIDに相手のプロフIDを紐付け
                    FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/heldProfiles/{str[0]}", "");

                    //相手側のプロフ経由で相手のユーザIDを取得
                    FirebaseManager.instance.ReadData("ProfInfo/" + str[0] + "/ownerId", (oppomentId) =>
                    {
                        FirebaseManager.instance.AddAutoID("ExchangeProf/" + passphrase, (value) =>
                        {
                            //自分側のプロフをつくる
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/ownerId", UserDataManager.instance.uid);
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/passphrase", passphrase);
                            //自分のプロフに相手のIDを書き込み
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/oppomentId", oppomentId);
                            FirebaseManager.instance.WriteData("ProfInfo/" + value + "/datetimeMemo", datetime);

                            //相手のユーザIDに自分のプロフIDを紐付け
                            FirebaseManager.instance.WriteData($"users/{oppomentId}/heldProfiles/{value}", "");
                        });
                        
                    });
                });

                
            }
            Debug.Log("END");

            onClicked_backbutton();
        }, (value) =>
        {
            GameObject tmp = Instantiate(errorPrefab, parent);
            tmp.transform.GetChild(0).GetComponent<Text>().text = value;
        }
        );
    }

    public void SetPassphrase(string path, string passphrase, System.Action action, System.Action<string> error)
    {
        if (isNewCreate)
        {
            FirebaseManager.instance.GetAllChildKeys("ExchangeProf", (value) =>
            {
                bool b = true;
                foreach (string str in value)
                {
                    if (str.Equals(passphrase))
                    {
                        b = false;
                    }
                }

                if (b)
                {
                    action();
                }
                else
                {
                    error("すでに合言葉が存在しています");
                }

            });
            
        }
        else
        {
            FirebaseManager.instance.GetAllChildKeys(path, (num) =>
            {
                bool b = true;
                foreach (string str in num)
                {
                    if (str.Equals(UserDataManager.instance.uid))
                    {
                        b = false;
                    }
                }

                if (b)
                {
                    if (num.Length > 1)
                    {
                        error("すでに二人分登録されてます");
                    }
                    else
                    {
                        action();
                    }
                }
                else
                {
                    error("すでに自分の情報が登録されています");
                }
                
            });

        }
        

    }

    public void OnProfButtonClicked(int value)
    {
        profIndex = value;
    }

    public void CheckBox(bool flag)
    {
        isNewCreate = flag;
    }

    public void onClicked_backbutton()
    {
        SceneManager.LoadScene("Scene3");
    }
}
