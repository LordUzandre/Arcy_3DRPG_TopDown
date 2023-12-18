using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsWithStrikingDistance { get; set; }
    void SetAggroStatus(bool IsAggroed);
    void SetStrikingDistanceBool(bool isWithStrikingDistance);
}
