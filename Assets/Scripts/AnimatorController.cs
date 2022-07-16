using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public enum UNIT_STATE
    {
        ANY = 0,
        ATTACK_MELEE = 1,
        ATTACK_SPELL = 2,
        ATTACK_CROSSBOW = 3,
        DIE = 4,
        MOVMENT = 5,
        COUNT = 6
    }

    public Animator animatior;
    public UnitCore unitCore;

    UNIT_STATE previus_state = UNIT_STATE.ANY;

    public void ChangeAnim()
    {
        UNIT_STATE current_state = unitCore.GetState();
        bool state_changed = current_state != previus_state;

        if (!state_changed)
        {
            return;
        }

        previus_state = current_state;

        switch (current_state)
        {
            case UNIT_STATE.ATTACK_MELEE:
                // animatior.Pla
                break;
            case UNIT_STATE.ATTACK_CROSSBOW:
                break;
            case UNIT_STATE.ATTACK_SPELL:
                break;
            case UNIT_STATE.DIE:
                break;
            case UNIT_STATE.MOVMENT:
                break;
            case UNIT_STATE.ANY:
            default:
                break;
        }
    }
}
