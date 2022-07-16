using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class UnitCore : MonoBehaviour
{
    public enum Team {
        Player,
        Enemy
    }
    public enum UNIT_TYPE
    {
        KNIGHT,
        ARCHER,
        WIZARD
    }

    public float _HP;
    public float _Speed;
    public float _AttackDamage;
    public float _AttackSpeed; // In seconds
    bool _attackCooldown = false;
    public float _Range;
    public float _Armour; // percentage of damage blocked
    public float _ArmourPiercing; // amount of armour to ignore
    public bool _isAlive = true;
    public Transform _target;
    public Team team = Team.Player;
    public Material[] teamMaterials;
    public NavMeshAgent agent;
    bool pause;

    public TMP_Text HPText;

    public UnitCore()
    {
        team = Team.Enemy;
        _HP = 100f;
        _Speed = 0.0f;
        _AttackDamage = 0.0f;
        _AttackSpeed = 0.0f;
        _Range = 0.0f;
        _Armour = 0.0f;
        _ArmourPiercing = 0.0f;
    }
    public UnitCore(
        Team assignedTeam,
        float HP,
        float Speed,
        float AttackDamage,
        float AttackSpeed,
        float Range,
        float Armour,
        float ArmourPiercing)
    {
        team = assignedTeam;
        _HP = HP;
        _Speed = Speed;
        _AttackDamage = AttackDamage;
        _AttackSpeed = AttackSpeed;
        _Range = Range;
        _Armour = Armour;
        _ArmourPiercing = ArmourPiercing;
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

    public void TakeDamage(float damage, float armourPiercing)
    {
        float finalArmour = _Armour - armourPiercing;
        float finalDamage = damage -  Mathf.Clamp(finalArmour/100 * damage, 0, 100);

        _HP -= finalDamage;
        
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
    }

    private void Update()
    {
        if (pause)
        {
            return;
        }

        HPText.text = _HP.ToString();

        if (!_isAlive)
        {
            // record death
            
            Destroy(gameObject);
        }

        if (!_target)
        {
            if (!FindTarget())
            {
                pause = true;
            }
        }

        // rotate to face target
        Vector3 direction = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;


        // check if in range of _target
        if (Vector3.Distance(transform.position, _target.position) > _Range)
        {
            agent.isStopped = false;
            agent.SetDestination(_target.position);
        }
        // if too close move away
        if (Vector3.Distance(transform.position, _target.position) < _Range)
        {
            agent.isStopped = false;
            agent.SetDestination(transform.position + (transform.position - _target.position).normalized * _Range);
        }
        if (!_attackCooldown && Vector3.Distance(transform.position, _target.position) <= _Range)
        {
            agent.isStopped = true;
            // attack
            _target.GetComponent<UnitCore>().TakeDamage(_AttackDamage, _ArmourPiercing);
            StartCoroutine(AttackCooldown());
        }
        
    }

}
