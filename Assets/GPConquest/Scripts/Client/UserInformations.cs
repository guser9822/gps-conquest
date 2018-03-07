using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used to pass user informations between scenes
 * **/
public class UserInformations : MonoBehaviour {

    public string username;
    public string password;
    public string email;
    public string faction;
    public string selectedUma;

    private static UserInformations _instance;

    private void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    public void SetUserInformations(string _username, 
        string _password,
        string _email,
        string _faction,
        string _selectedUma)
    {
        username = _username;
        password = _password;
        email = _email;
        faction = _faction;
        selectedUma = _selectedUma;
    }

}
