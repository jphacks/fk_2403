using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class Scene1Manager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    Login login;
    Register register;
    // Start is called before the first frame update
    void Start()
    {
        login = GetComponent<Login>();
        register = GetComponent<Register>();

        var email = "demo.user@normal.com";
        var password = "password";
        register.CreateUserWithEmailAndPassword(email, password);
        // SingIn(email, password);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
