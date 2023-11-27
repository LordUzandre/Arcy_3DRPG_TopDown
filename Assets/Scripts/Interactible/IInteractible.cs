using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Interaction
{
    public interface IInteractible
    {
        Transform transform { get; }
        float setDistanceToPlayer(float distanceToPlayer);
        void Interact();
    }
}
