using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.MainMenu
{
    public class TempButtonFunction : MonoBehaviour
    {
        [SerializeField] string message = " Button is pressed";
        public void MyButtonFunction()
        {
            print($"{message}");
        }
    }
}
