using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class UnitCore : MonoBehaviour
{
    public enum Team {
        Player,
        Enemy,
        Neutral
    }
    public enum UNIT_TYPE
    {
        KNIGHT = 0,
        ARCHER = 1,
        WIZARD = 2,
        COUNT = 3
    }
    public enum Behavior_State
    {
        IDLE = 0,
        APPROACHING = 1,
        RETREATING = 2,
        ATTACKING = 3,
        DEAD = 4,
        PAUSE = 5
    }

    public AnimatorController.UNIT_STATE current_state = AnimatorController.UNIT_STATE.ANY;
    public Team team = Team.Player;
    public Behavior_State _b_state = Behavior_State.IDLE;

    public UNIT_TYPE _type = UNIT_TYPE.KNIGHT;

    public int _HP;
    public float _Speed;
    public float _AttackDamage;
    public float _AttackSpeed; // In seconds
    bool _attackCooldown = false;
    public float _Range;
    public float _Armour; // percentage of damage blocked
    public float _ArmourPiercing; // amount of armour to ignore
    public bool _isAlive = true;
    public Transform _target;
    
    public Material[] teamMaterials;
    public NavMeshAgent agent;
    public bool pause;

    public int maxHealth = 100;

    public event EventHandler attack_event;

    public HealthBar healthBar;

    public AnimatorController.UNIT_STATE GetState()
    {
        return current_state;
    }

    public UnitCore()
    {
        team = Team.Neutral;
        _Speed = 0.0f;
        _AttackDamage = 0.0f;
        _AttackSpeed = 0.0f;
        _Range = 0.0f;
        _Armour = 0.0f;
        _ArmourPiercing = 0.0f;

        pause = true;
    }
    public UnitCore(
        Team assignedTeam,
        int HP,
        float Speed,
        float AttackDamage,
        float AttackSpeed,
        float Range,
        float Armour,
        float ArmourPiercing,
        UNIT_TYPE type)
    {
        team = assignedTeam;
        _Speed = Speed;
        _AttackDamage = AttackDamage;
        _AttackSpeed = AttackSpeed;
        _Range = Range;
        _Armour = Armour;
        _ArmourPiercing = ArmourPiercing;
        _HP = HP;
        _type = type;
    }

    public bool FindTarget()
    {
        
        Team targetTag = team == Team.Player ? Team.Enemy : Team.Player;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag.ToString());
        if (enemies.Length == 0)
        {
            return false;
        }
        // get closest gameobject with tag
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < distance)
            {
                distance = dist;
                closest = enemy;
            }
        }
        _target = closest.transform;
        return true;
    }
    
    public IEnumerator AttackCooldown()
    {
        _attackCooldown = true;
        yield return new WaitForSeconds(_AttackSpeed);
        _attackCooldown = false;
    }
    public IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, float armourPiercing)
    {
        float finalArmour = _Armour - armourPiercing;
        float finalDamage = damage -  Mathf.Clamp(finalArmour/100 * damage, 0, 100);

        _HP -= (int)finalDamage;

        healthBar.SetHealth(_HP);
        SoundManager.PlaySound(SoundManager.Sound.TropReciveHit);

        if (_HP <= 0)
        {
            _isAlive = false;
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameObject.tag = team.ToString();
        agent.speed = _Speed;
        agent.stoppingDistance = _Range;
        GetComponentInChildren<Renderer>().material = teamMaterials[(int)team];

        maxHealth = _HP;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        if (pause)
        {
            return;
        }

        if (!_isAlive)
        {
            _b_state = Behavior_State.DEAD;
        }
        else if (!_target || !_target.gameObject.GetComponent<UnitCore>()._isAlive)
        {
            if (!FindTarget())
            {
                agent.SetDestination(transform.position);
                _b_state = Behavior_State.IDLE;
            }
        }
        else
        {
            // check if in range of _target
            if (Vector3.Distance(transform.position, _target.position) > _Range)
            {
                _b_state = Behavior_State.APPROACHING;
            }
            // if too close move away
            if (Vector3.Distance(transform.position, _target.position) < _Range)
            {
                _b_state = Behavior_State.RETREATING;
            }
            if (!_attackCooldown && Vector3.Distance(transform.position, _target.position) <= _Range)
            {
                _b_state = Behavior_State.ATTACKING;
            }

            // rotate to face target
            Vector3 direction = _target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;

        }

        

        switch (_b_state)
        {
            case Behavior_State.IDLE:
                break;
            case Behavior_State.APPROACHING:
                agent.SetDestination(_target.position);
                //SoundManager.PlaySound(SoundManager.Sound.TropMove, transform.localPosition);
                current_state = AnimatorController.UNIT_STATE.MOVMENT;
                break;
            case Behavior_State.RETREATING:
                //agent.isStopped = false;
                agent.SetDestination(transform.position + (transform.position - _target.position).normalized * _Range);
                //SoundManager.PlaySound(SoundManager.Sound.TropMove, transform.localPosition);
                current_state = AnimatorController.UNIT_STATE.MOVMENT;
                break;
            case Behavior_State.ATTACKING:
                //agent.isStopped = true;
                if (!_attackCooldown)
                {
                    _target.GetComponent<UnitCore>().TakeDamage(_AttackDamage, _ArmourPiercing);
                    StartCoroutine(AttackCooldown());
                    attack_event?.Invoke(this, EventArgs.Empty);
                }
                break;
            case Behavior_State.DEAD:
                SoundManager.PlaySound(SoundManager.Sound.TropDead);
                current_state = AnimatorController.UNIT_STATE.DIE;
                team = Team.Neutral;
                StartCoroutine(DeathTimer());
                break;
        }
    }
}
