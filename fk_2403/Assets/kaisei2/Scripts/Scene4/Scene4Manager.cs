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
            FirebaseManager.instance.AddAutoID("ExchangeProf/" + passphrase, (value) =>
            {
                if (isNewCreate)
                {
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/datetimeMemo", datetime);
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/ownerId", UserDataManager.instance.uid);

                    //ユーザIDにプロフIDを紐付け
                    FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/heldProfiles/{value}", "");
                    FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passphrase}", "");
                }
                else
                {
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/oppomentUserId", UserDataManager.instance.uid);//アバターを表示する方
                    FirebaseManager.instance.WriteData("ProfInfo/" + value + "/datetimeMemo", datetime);

                    //ユーザIDにプロフIDを紐付け
                    FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/heldProfiles/{value}", "");
                    FirebaseManager.instance.WriteData($"users/{UserDataManager.instance.uid}/linkedPassphrase/{passphrase}", "");

                }

                
            });
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
