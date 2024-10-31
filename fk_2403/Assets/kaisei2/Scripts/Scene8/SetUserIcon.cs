using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUserIcon : MonoBehaviour
{
    public string namestr;
    public string memo;
    public string passphrase;
    public int index;
    public void OnSet(string name, string memo, string passphrase, int index)
    {
        namestr = name;
        this.memo = memo;
        this.passphrase = passphrase;
        this.index = index;
        transform.Find("name").gameObject.GetComponent<Text>().text = name;
        transform.Find("memo").gameObject.GetComponent<Text>().text = memo;
        transform.Find("passphrase").gameObject.GetComponent<Text>().text = passphrase;

    }
}
