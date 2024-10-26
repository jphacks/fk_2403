using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermsOfUse : MonoBehaviour
{
    bool isCheck = false;
    //利用規約確認用スクリプト
    public bool GetIsCheck(){
        return isCheck;
    }

    public void OnTggleValueChanged(bool value){
        isCheck = value;
    }
}
