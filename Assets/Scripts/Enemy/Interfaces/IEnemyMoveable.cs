using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody rB { get; set; }
    bool IsFacingRight { get; set; }
    void MoveEnemy(Vector3 velocity);
    void CheckForLeftOrRightFacing(Vector3 velocity);
}
