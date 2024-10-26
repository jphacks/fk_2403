using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Manager_M : MonoBehaviour
{
    [SerializeField] Canvas titleCanvas;
    [SerializeField] Canvas iDEntryCanvas;
    [SerializeField] Canvas termsOfUseCanvs;
    // Start is called before the first frame update
    void Start()
    {
        iDEntryCanvas.gameObject.SetActive(false);
        termsOfUseCanvs.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
