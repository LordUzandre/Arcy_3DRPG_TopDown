using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JE.helpers
{
    public class JEActivator : MonoBehaviour
    {
        public static void ActivateOrDeactivate(bool shouldBeActive, GameObject myObject)
        {
            if (myObject.activeInHierarchy == true)
            {
                switch (shouldBeActive)
                {
                    case true:
                        break;
                    case false:
                        myObject.SetActive(true);
                        break;
                }
            }
            else if (myObject.activeInHierarchy == false)
            {
                switch (shouldBeActive)
                {
                    case true:
                        myObject.SetActive(false);
                        break;
                    case false:
                        break;
                }
            }
        }
    }
}
