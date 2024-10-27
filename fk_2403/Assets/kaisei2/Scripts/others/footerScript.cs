using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class footerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClicked_homebutton()
    {
        SceneManager.LoadScene("Scene3");
    }
    public void onClicked_writebutton()
    {
        SceneManager.LoadScene("Scene8");
    }

    public void onClicked_friendbutton()
    {
        SceneManager.LoadScene("Scene7");
    }

    public void onClicked_settingbutton()
    {
        SceneManager.LoadScene("Scene6");
    }

}
