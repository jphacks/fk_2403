using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempAvatarStorage
{
    public string[] CostumePaths = {"Costumes/0/eye 1", "Costumes/1/kuti 1", "Costumes/2/brow 1", "Costumes/3/hear 1", "Costumes/4/banso-ko-"};

    public void SetUserData(){
        UserDataManager.instance.SetAvatarDataToServer(CostumePaths);
    }
    
}
