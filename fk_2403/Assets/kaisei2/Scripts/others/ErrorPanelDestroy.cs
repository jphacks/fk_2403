using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPanelDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 5f);
    }

    private void Destroy()
    {
        Destroy(this);
    }
}
