using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUserIcon : MonoBehaviour
{
    public string profId = "";
    public void OnSet(string name, string memo, string passphrase)
    {
        transform.Find("name").gameObject.GetComponent<Text>().text = name;
        transform.Find("memo").gameObject.GetComponent<Text>().text = memo;
        transform.Find("passphrase").gameObject.GetComponent<Text>().text = passphrase;

    }
}
