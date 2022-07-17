using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    const float RotationPerNumber = -36f;
    const float RotationSpeed = 90.0f;

    public Transform TensRotor;
    public Transform UnitRotor;

    int Tens = 0;
    int Units = 0;
    bool TensinMovement = false;
    bool UnitsinMovement = false;
    Quaternion tensRotationTarget;
    Quaternion unitsRotationTarget;

    public void SetHealth(int heath, bool noMovement = false)
    {
        int tens = (int)Mathf.Floor(heath / 10.0f);
        int units = (int)Mathf.Floor(heath % 10.0f);

        if (tens != Tens)
        {
            Tens = tens;
            tensRotationTarget = Quaternion.Euler(Tens * RotationPerNumber + 360, 0.0f, 0.0f);
            if (noMovement)
                TensRotor.localRotation = tensRotationTarget;
            else
                TensinMovement = true;
        }
        if (units != Units)
        {
            Units = units;
            unitsRotationTarget = Quaternion.Euler(Units * RotationPerNumber + 360, 0.0f, 0.0f);
            if (noMovement)
                UnitRotor.localRotation = unitsRotationTarget;
            else
                UnitsinMovement = true;
        }
    }

    void FixedUpdate()
    {
        if (TensinMovement)
        {
            TensRotor.Rotate(Vector3.right * RotationSpeed * Time.deltaTime, Space.Self);

            if (Quaternion.Angle(tensRotationTarget, TensRotor.localRotation) <= 1.0f)
            {
                TensinMovement = false;
                TensRotor.localRotation = tensRotationTarget;
            }
        }
        if (UnitsinMovement)
        {
            UnitRotor.Rotate(Vector3.right * RotationSpeed * Time.deltaTime, Space.Self);

            if (Quaternion.Angle(unitsRotationTarget, UnitRotor.localRotation) <= 1.0f)
            {
                UnitsinMovement = false;
                UnitRotor.localRotation = unitsRotationTarget;
            }
        }

    }
}
