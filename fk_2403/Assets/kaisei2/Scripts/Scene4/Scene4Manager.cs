using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene4Manager : MonoBehaviour
{
    [SerializeField] InputField passphrase_input;
    [SerializeField] InputField datetime_input;
    CreateSendProfilePassphrase createSendProfilePassphrase;

    int profIndex = 0;

    void Start()
    {
        createSendProfilePassphrase = GetComponent<CreateSendProfilePassphrase>();

    }

    public void OnSendProfileButtonClicked()
    {
        string passphrase = passphrase_input.text;
        string datetime = datetime_input.text;
        createSendProfilePassphrase.SetPassphrase("ExchangeProf/" + passphrase, () =>
        {
            FirebaseManager.instance.AddAutoID("ExchangeProf/" + passphrase, (value) =>
            {
                string oppomentUserId = "";
                FirebaseManager.instance.GetAllChildKeys("ExchangeProf/" + passphrase, (keys) => {
                    foreach(var key in keys){
                        if(key.Equals(UserDataManager.instance.uid)){
                            continue;
                        }
                        oppomentUserId = key;
                    }
                });

                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/oppomentUserId", oppomentUserId);//アバターを表示する方
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/datetimeMemo", datetime);

                //ユーザIDにプロフIDを紐付け
                FirebaseManager.instance.WriteData($"{UserDataManager.instance.uid}/{value}", "");
            });
            Debug.Log("END");
        });
    }

    public void OnProfButtonClicked(int value)
    {
        profIndex += value;
        if (profIndex > 4)
        {
            profIndex = 0;
        }

        if (profIndex < 0)
        {
            profIndex = 4;
        }
    }

    public void onClicked_backbutton()
    {
        SceneManager.LoadScene("Scene3");
    }
}
