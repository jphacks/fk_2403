using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumePath : MonoBehaviour
{
    public string SpritePath { get; set; }
    public int ID{get; set;}

    public void OnClicked(){
        LoadAvatarImage loadAvatarImage = GameObject.Find("Scripts").gameObject.GetComponent<LoadAvatarImage>();
        loadAvatarImage.tempAvatarStorage.CostumePaths[ID] = SpritePath;
        loadAvatarImage.ChangeCostumeImage();
    }
}
