using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TC.GPConquest.Player
{
    /*
 * This class is used to pass user informations between scenes.
 * For the moment we will mock the database, faking the registrtions.
 * This class will store the List of registered accounts.
 * **/
    public class UsersContainer : MonoBehaviour
    {
        private HashSet<UserInformations> registeredUsers = new HashSet<UserInformations>();
        private static UsersContainer _instance;
        public UserInformations UserInfos;//template for all registered users

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

        public bool AddNewUser(string _username,
                                string _password,
                                string _email,
                                string _faction,
                                string _selectedUma)
        {
            //Clones the UserInformations prefab
            UserInformations newUserInfos = Instantiate<UserInformations>(UserInfos);

            //Initializes the new user
            newUserInfos.SetUserInformations(_username,
                        _password,
                        _email,
                        _faction,
                        _selectedUma);

            //Adds the new user to the list
            if (registeredUsers.Add(newUserInfos))
            {
                //Makes the new object not destroyable through scenes switching
                DontDestroyOnLoad(newUserInfos);

                //Adds the new object to the User contaner hieararchy
                newUserInfos.transform.parent = this.transform;
            }
            else //The user already exists, destroy the last created
            {
                Destroy(newUserInfos.gameObject);
                return false;
            }

            return true;
        }

        public UserInformations GetUserByUsernameAndPassword(string _username, string _password)
        {
            return registeredUsers.FirstOrDefault(x=> x.username.Equals(_username) && x.password.Equals(_password));
        }

    }

}