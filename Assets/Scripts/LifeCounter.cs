using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    const float RotationPerNumber = -36f;
    const float RotationSpeed = -90.0f;

    public Transform TensRotor;
    public Transform UnitRotor;

    int Tens = 0;
    int Units = 0;
    bool TensinMovement = false;
    bool UnitsinMovement = false;
    Vector3 tensRotationTarget;
    Vector3 unitsRotationTarget;

    public void SetHealth(int heath, bool noMovement = false)
    {
        int tens = (int)Mathf.Floor(heath / 10.0f);
        int units = (int)Mathf.Floor(heath % 10.0f);

        if (tens != Tens)
        {
            Tens = tens;
            tensRotationTarget = new Vector3(Tens * RotationPerNumber + 360, 0.0f, 0.0f);
            if (noMovement)
                TensRotor.localEulerAngles = tensRotationTarget;
            else
                TensinMovement = true;
        }
        if (units != Units)
        {
            Units = units;
            unitsRotationTarget = new Vector3(Units * RotationPerNumber + 360, 0.0f, 0.0f);
            if (noMovement)
                UnitRotor.localEulerAngles = unitsRotationTarget;
            else
                UnitsinMovement = true;
        }
    }

    void FixedUpdate()
    {
        if (TensinMovement)
        {
            TensRotor.Rotate(Vector3.right * RotationSpeed * Time.deltaTime, Space.Self);

            if (Mathf.Abs(TensRotor.localEulerAngles.x - tensRotationTarget.x) < 0.5f)
            {
                TensinMovement = false;
                TensRotor.localEulerAngles = tensRotationTarget;
            }
        }
        if (UnitsinMovement)
        {
            UnitRotor.Rotate(Vector3.right * RotationSpeed * Time.deltaTime, Space.Self);

            if (Mathf.Abs(UnitRotor.localEulerAngles.x - unitsRotationTarget.x) < 0.5f)
            {
                UnitsinMovement = false;
                UnitRotor.localEulerAngles = unitsRotationTarget;
            }
        }

    }
}
