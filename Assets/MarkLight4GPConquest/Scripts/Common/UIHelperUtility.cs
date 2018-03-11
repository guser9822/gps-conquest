using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using MarkLight.Views.UI;
using System;

namespace TC.GPConquest.MarkLight4GPConquest.Common
{
    public static class UIHelperUtility
    {
        /*
         * Function that validate a string for
         * username,password and email field
         * **/
        public static bool ValidateString(string _string)
        {
            return _string != null &&
                _string.Length > 0 &&
                Regex.IsMatch(_string, @"^[a-zA-Z0-9@_]+$");
        }

        /*
         * Execute an action on a button only when all arguments
         * matches with the accepting criteria. T is the type of
         * the arguments that is passed in.
         * **/
        public static bool ExecuteActionOnButton<T>(Button _button,
            List<T> argsForPredicate,
            Predicate<List<T>> p,
            Func<bool, Button, bool> actionToExecute)
        {
            return actionToExecute(p(argsForPredicate), _button);
        }

    }
}
