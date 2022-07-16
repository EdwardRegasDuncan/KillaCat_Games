using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    private void Awake()
    {
        _HP = 100;
        _Speed = 10f;
        _AttackDamage = 10f;
        _AttackSpeed = 1.5f;
        _Range = 2f;
        _Armour = 20f;
        _ArmourPiercing = 10f;
    }

}
