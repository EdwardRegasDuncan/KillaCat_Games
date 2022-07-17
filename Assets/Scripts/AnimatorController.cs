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
        HIT = 6,
        COUNT = 7
    }

    public Animator animatior;
    public UnitCore unitCore;

    UNIT_STATE previus_state = UNIT_STATE.ANY;

    private void Update()
    { 
        if (unitCore)
        {
            ChangeAnim();
        }
    }

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
                animatior.Play("Attack");
                break;
            case UNIT_STATE.ATTACK_CROSSBOW:
                animatior.Play("Attack");
                break;
            case UNIT_STATE.HIT:
                animatior.Play("Hit");
                break;
            case UNIT_STATE.ATTACK_SPELL:
                animatior.Play("Attack");
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
