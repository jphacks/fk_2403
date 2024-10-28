using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene2Manager : MonoBehaviour
{
    [SerializeField] Transform costumeView;
    LoadAvatarImage loadAvatarImage;
    // Start is called before the first frame update
    void Start()
    {
        loadAvatarImage = GetComponent<LoadAvatarImage>();
        //loadAvatarImage.GetImage();
        loadAvatarImage.CreateCostumeIcon();

    }

    public void OnSwitchButtonClicked(int value)
    {
        foreach (Transform obj in costumeView)
        {
            obj.gameObject.SetActive(false);
        }
        costumeView.GetChild(value).gameObject.SetActive(true);
    }

    public void OnEndButtonClicked()
    {
        //終わった時データ送る
        loadAvatarImage.tempAvatarStorage.SetUserData();
        SceneManager.LoadScene("Scene3");
    }

    public void OnBackButtonClicked(){
        SceneManager.LoadScene("Scene3");
    }
}
