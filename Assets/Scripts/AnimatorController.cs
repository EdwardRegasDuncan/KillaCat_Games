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
                animatior.Play("Melee");
                break;
            case UNIT_STATE.ATTACK_CROSSBOW:
                animatior.Play("Crossbow");
                break;
            case UNIT_STATE.ATTACK_SPELL:
                animatior.Play("Spell");
                break;
            case UNIT_STATE.DIE:
                animatior.Play("Death");
                break;
            case UNIT_STATE.MOVMENT:
                animatior.Play("Move");
                break;
            case UNIT_STATE.ANY:
            default:
                break;
        }
    }
}
