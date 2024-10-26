using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene4Manager : MonoBehaviour
{
    [SerializeField] InputField password_input;
    [SerializeField] InputField detatime_input;
    CreateSendProfilePassword createSendProfilePassword;

    void Start()
    {
        createSendProfilePassword = GetComponent<CreateSendProfilePassword>();
        
    }

    public void OnSendProfileButtonClicked(){
        string path = password_input.text;
        createSendProfilePassword.SetPasssword(path, () => {
            Debug.Log("END");
        });
    }
}
