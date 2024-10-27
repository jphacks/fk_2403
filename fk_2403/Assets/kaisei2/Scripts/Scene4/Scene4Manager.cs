using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene4Manager : MonoBehaviour
{
    [SerializeField] InputField passphrase_input;
    [SerializeField] InputField detatime_input;
    CreateSendProfilePassphrase createSendProfilePassphrase;

    int profIndex = 0;

    void Start()
    {
        createSendProfilePassphrase = GetComponent<CreateSendProfilePassphrase>();

    }

    public void OnSendProfileButtonClicked()
    {
        string passphrase = passphrase_input.text;
        createSendProfilePassphrase.SetPassphrase("ExchangeProf/" + passphrase, () =>
        {
            FirebaseManager.instance.AddAutoID("ExchangeProf/" + passphrase, (value) =>
            {
                // Dictionary<string, object> data = new Dictionary<string, object>
                // {
                //     { "profbase", profIndex },
                //     { "seal", 0 },
                //     { "originalUserId", "myID"},
                //     { "currentUserId", "yourId"}
                // };
                // FirebaseManager.instance.AddDictionaryToFirebase("ProfInfo/"+value, data);
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/profbase", profIndex.ToString());
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/seal", 0.ToString());
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/originalUserId", "myID");
                FirebaseManager.instance.WriteData("ProfInfo/" + value + "/currentUserId", "yourId");
            });
            Debug.Log("END");
        }, () =>
        {
            //error
            Debug.Log("すでに合言葉が存在してます");
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
