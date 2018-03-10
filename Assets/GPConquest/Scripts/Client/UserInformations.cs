using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.Player
{
    public class UserInformations : MonoBehaviour
    {

        public string username;
        public string password;
        public string email;
        public string faction;
        public string selectedUma;

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

        public override bool Equals(System.Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            UserInformations p = (UserInformations)obj;
            return username.Equals(p.username) &&
                        password.Equals(p.password) &&
                        email.Equals(p.email) &&
                        faction.Equals(p.faction) &&
                        selectedUma.Equals(p.selectedUma);
        }

        public static bool operator ==(UserInformations x, UserInformations y)
        {

            if (object.ReferenceEquals(x, null))
            {
                return object.ReferenceEquals(y, null);
            }

            return x.Equals(y);
        }

        public static bool operator !=(UserInformations x, UserInformations y)
        {
            if (object.ReferenceEquals(x, null))
            {
                return object.ReferenceEquals(y, null);
            }

            return !x.Equals(y);
        }


        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((username == null) ? 0 : username.GetHashCode());
            result = prime * result + ((password == null) ? 0 : password.GetHashCode());
            result = prime * result + ((email == null) ? 0 : email.GetHashCode());
            result = prime * result + ((faction == null) ? 0 : faction.GetHashCode());
            result = prime * result + ((selectedUma == null) ? 0 : selectedUma.GetHashCode());
            //result = prime * result + (int)(id ^ (id >>> 32));
            return result;
        }

    }

}