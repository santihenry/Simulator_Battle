using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public enum EnemyTeam
{
    blue,
    red
}
public enum SpeedState
{
    standing,
    walking,
    runing,
    sprinting,
}
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected int _maxLife = 100;
    [Range(0, 100)]
    public int life;
    [Range(.1f, 5)]
    public float speed;
    [Range(1, 30)]
    public float rangeVision;
    [Range(0, 360)]
    public float angleVision;
    [Range(1, 10)]
    public float rangeToAttack;
    int _maxCourage = 70;
    [Range(0, 50)]
    public int courage;
    [Range(0, 50)]
    public int defense;
    [Range(10, 50)]
    public int damage;
    [Range(0, 10)]
    public float obstacleAvoidance;
    [Range(10, 20)]
    public float closestEnemy;
    [Range(10.2f, 20)]
    public float avoidanceMultiplier;
    public Enemy target;
    public LayerMask walls, node;
    protected LayerMask enemies;
    protected StateMachine<States> _fsm;
    Search<States> _search;
    Attacking<States> _attack;
    Fleeing<States> _fleeing;
    chasing<States> _chasing;
    Stun<States> _stunState;
    Healing<States> _healing;
    Defending<States> _defending;
    Die<States> _die;
    Patrol<States> _patrol;
    GoToTheCenter<States> _goToCenter;
    Hit<States> _hit;
    protected float _distanceToTarget;
    protected float _angleToTarget;
    protected Vector3 _directionToTarget;
    public EnemyTeam team;
    bool _scared;
    public Renderer head, body,shield;
    public Material[] materials = new Material[2];
    public GameObject _healingParticles, stunImg;
    protected Animator _animator;
    bool _blocking;
    public bool dummy;
    public bool _dead;
    bool _isHealing;
    public bool isHealing { get => _isHealing; set => _isHealing = value; }
    public bool dead { get => _dead; set => _dead=value; }
    public bool scared { get => _scared; set => _scared = value; }
    public bool blocking { get => _blocking; set => _blocking = value; }
    public int MaxHealth => _maxLife;
    public int MaxCourage => _maxCourage;
    bool _stun;
    public bool Stunned { get => _stun; set => _stun = value; }
    public bool stuned;

    public Slider barLife;
    bool _canCauseDamage;
    public bool canCauseDamage { get => _canCauseDamage; set => _canCauseDamage = value; }
    
    public void CauseDamage()
    {
        _canCauseDamage = true;
    }

    void Start()
    {
        if (team == EnemyTeam.red)
        {
            enemies.value = (int)Mathf.Pow(2, 11);
            gameObject.layer = 10;
            
            head.material = materials[0];
            body.material = materials[0];
            CheckWinnerTeam.instance.redMembers += 1;
        }
        else
        {
            CheckWinnerTeam.instance.blueMembers += 1;
            enemies.value = (int)Mathf.Pow(2, 10);
            gameObject.layer = 11;
            head.material = materials[1];
            body.material = materials[1];
        }
        if (dummy) return;
        _animator = GetComponent<Animator>();
        _distanceToTarget = Mathf.Infinity;
        SetStateMachine();
    }

    // Update is called once per frame
    void Update()
    {

        if (dummy) return;
       
        barLife.value =life;
        
        _fsm.OnUpdate();

        target = ClosestEnemy(!scared);


        
        if (target)
        {

            _directionToTarget = target.transform.position - transform.position;
            _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            _angleToTarget = Vector3.Angle(transform.forward, _directionToTarget);
        }

    }


    public bool InSight()
    {

        if (!target) return false;
        if ((_angleToTarget < angleVision / 2 || _angleToTarget < -angleVision / 2) && _distanceToTarget < rangeVision)
        {

            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, verticalOffset * height, 0), target.transform.position - (transform.position + new Vector3(0, verticalOffset * height, 0)), out hit,
            Vector3.Distance(target.transform.position, transform.position));

            if (hit.collider)
            {


                if (Mathf.Pow(2, hit.collider.transform.gameObject.layer) == walls)
                {
                    return false;
                }
                else
                {

                    return true;
                }

            }
        }

        return false;

    }
    public bool InRange()
    {
        if (!target) return false;
        if (InSight() && _distanceToTarget <= rangeToAttack)
        {
            return true;
        }
        return false;
    }

    public virtual void Transitionfsm(States action)
    {
        _fsm.Transition(action);
    }
    public Node CurrentNode()
    {

        Node node = new global::Node();
        Collider[] nodeColliders = Physics.OverlapSphere(transform.position, 5, this.node);
        float distance = new float();
        for (int i = 0; i < nodeColliders.Length; i++)
        {
            Vector3 _direction = (nodeColliders[i].transform.position - transform.position).normalized;
            float _distance = Vector3.Distance(transform.position, nodeColliders[i].transform.position);
            if (distance == 0)
            {

                if (!Physics.Raycast(transform.position, _direction, _distance, walls))
                {
                    distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                    node = nodeColliders[i].GetComponent<Node>();
                }
            }
            else
            {
                if (Vector3.Distance(nodeColliders[i].transform.position, transform.position) < distance)
                {

                    if (!Physics.Raycast(transform.position, _direction, _distance, walls))
                    {
                        distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                        node = nodeColliders[i].GetComponent<Node>();
                    }
                }
            }

        }

        return node;
    }
    public Transform ClosestObstacle()
    {

        var obstacles = Physics.OverlapSphere(transform.position, obstacleAvoidance, walls);
        Transform _closest = null;
        if (obstacles.Length > 0)
        {
            foreach (var item in obstacles)
            {
                if (!_closest)
                    _closest = item.transform;
                else if (Vector3.Distance(item.transform.position, transform.position) < Vector3.Distance(_closest.position, transform.position))
                    _closest = item.transform;
            }
        }
        return _closest;
    }
    public Vector3 ClosestPointToTarget(Vector3 myTarget)
    {
        Vector3[] possiblePoints = new Vector3[5];
        Vector3[] directions = new Vector3[5]
        {
            (transform.forward),
            (transform.forward+transform.right),
            (transform.right),
            (transform.right*-1),
            (transform.forward+(transform.right*-1))
        };
        int positionIndex = 0;
        int targetIndex = 0;
        float distance = new float();
        float distanceAux = new float();


        for (int i = 0; i < directions.Length; i++)
        {

            Vector3 targetDir = directions[i].normalized;

            debugIndexBoolColliding[i] = false;
            if (!Physics.Raycast(transform.position + new Vector3(0, verticalOffset, 0), (targetDir + transform.position) - transform.position, obstacleAvoidance, walls))
            {

                possiblePoints[positionIndex] = transform.position + directions[i].normalized * obstacleAvoidance;

                distanceAux = Vector3.Distance(transform.position + directions[i].normalized * obstacleAvoidance,
                                                                     new Vector3(myTarget.x,
                                                                                        transform.position.y,
                                                                               myTarget.z));

                if (distance == 0)
                {
                    distance = distanceAux;
                    targetIndex = positionIndex;


                }
                else
                {
                    if (distanceAux < distance)
                    {
                        distance = distanceAux;

                        targetIndex = positionIndex;


                    }

                }
            }
            else
            {
                debugIndexBoolColliding[i] = true;
            }
            positionIndex++;
        }

        debugIndex = targetIndex;

        return possiblePoints[targetIndex];
    } //el angulo mas cercano al enemigo si tengo que girar al esquivar un obstaculo
    public Enemy ClosestEnemy(bool goToEnemy, float distanceToEscape=10) //goToEnemy determina si debo escapar o ir hacia el enemigo
    {

            var nearEnemies = Physics.OverlapSphere(transform.position, closestEnemy, enemies);
            Enemy _closest=target;
            if (target) if (target.dead) _closest = null;
            float _distance = _distanceToTarget;
            float _distanceAux = 0;
            Enemy _preferedEnemy;
            if (nearEnemies.Length > 0)
            {




                if (goToEnemy)
                {



                    foreach (var enemy in nearEnemies)
                    {



                        _distanceAux = Vector3.Distance(enemy.transform.position, transform.position);
                        _preferedEnemy = enemy.GetComponent<Enemy>();

                        if (!_closest && !_preferedEnemy.dead)
                        {
                            _closest = _preferedEnemy;
                            _distance = _distanceAux;

                        }
                        if (_distanceAux < _distance && !_preferedEnemy.dead)
                        {
                            _closest = _preferedEnemy;
                            _distance = _distanceAux;

                        }
                        else if (_distanceAux > _distance && (_distanceAux - _distance) < 2 && !_preferedEnemy.dead)
                        {
                            if (_preferedEnemy.life < _closest.life)
                            {
                                _distance = _distanceAux;
                                _closest = _preferedEnemy;
                            }
                        }
                    }
                    return _closest;
                }
                else
                {
                    foreach (var enemy in nearEnemies)
                    {
                        _preferedEnemy = enemy.GetComponent<Enemy>();
                        _distanceAux = Vector3.Distance(enemy.transform.position, transform.position);
                        if (!_closest && !_preferedEnemy.dead)
                        {
                            if (_preferedEnemy.target == this)
                            {
                                _closest = _preferedEnemy;
                                _distance = _distanceAux;

                            }
                        }
                        if (_distanceAux < _distance && !_preferedEnemy.dead)
                        {
                            if (_preferedEnemy.target == this)
                            {
                                _closest = _preferedEnemy;
                                _distance = _distanceAux;
                            }
                        }
                        else if (_distanceAux > _distance && (_distanceAux - _distance) < 2 && !_preferedEnemy.dead)
                        {
                            if (_preferedEnemy.target == this)
                            {
                                if (_preferedEnemy.life > _closest.life)
                                {
                                    _distance = _distanceAux;
                                    _closest = _preferedEnemy;
                                }
                            }
                        }
                    }
                }


                    return _closest;
            }
            else return null;

    }

            
  

    public Animator animator { get => _animator; set => _animator = value; }
    public void TakeDamage(int damage,Enemy attacker)
    {
        target = attacker;
        if (_blocking)
        {
            attacker.stuned = true;
            int _damageReceived = Mathf.Clamp( damage - defense,0,200);
            life -= _damageReceived;
            courage= courage<MaxCourage?courage+=10:courage=MaxCourage;
            attacker.Transitionfsm(States.stun);
            attacker.courage= attacker.courage>0?attacker.courage-=5:0;
            if (life <= 0) Transitionfsm(States.dead);
        }
        else
        {
            life -= damage;
            courage= courage>0? courage-= 10: courage=0;
            attacker.courage=attacker.courage<MaxCourage?attacker.courage+=5:0;
            if (life <= 0)
            {
                dead = true;
                Transitionfsm(States.dead);
            }
            else
            {
                animator.SetTrigger("hit");
                Transitionfsm(States.hit);
            }
        }
        
    }
    
    public virtual void SetStateMachine()
    {
        _patrol = new Patrol<States>(this);
        _search = new Search<States>(this);
        _chasing = new chasing<States>(this);
        _fleeing = new Fleeing<States>(this);
        _healing = new Healing<States>(this);
        _attack = new Attacking<States>(this);
        _defending = new Defending<States>(this);
        _hit = new Hit<States>(this);
        _die = new Die<States>(this);
        _stunState = new Stun<States>(this);
        _goToCenter = new GoToTheCenter<States>(this);

        _goToCenter.SetTransition(States.patrol, _patrol);

        _patrol.SetTransition(States.search, _search);
        _patrol.SetTransition(States.chase, _chasing);
        _patrol.SetTransition(States.patrol, _patrol);
        _patrol.SetTransition(States.hit, _hit);
        _patrol.SetTransition(States.dead, _die);

        _search.SetTransition(States.chase, _chasing);
        _search.SetTransition(States.patrol, _patrol);
        _search.SetTransition(States.hit, _hit);
        _search.SetTransition(States.dead, _die);

        _chasing.SetTransition(States.attack, _attack);
        _chasing.SetTransition(States.defend, _defending);
        _chasing.SetTransition(States.search, _search);
        _chasing.SetTransition(States.hit, _hit);
        _chasing.SetTransition(States.dead, _die);
        _chasing.SetTransition(States.patrol, _patrol);

        _fleeing.SetTransition(States.heal, _healing);
        _fleeing.SetTransition(States.hit, _hit);
        _fleeing.SetTransition(States.dead, _die);
        _fleeing.SetTransition(States.patrol, _patrol);

        _healing.SetTransition(States.flee, _fleeing);
        _healing.SetTransition(States.chase, _chasing);
        _healing.SetTransition(States.search, _search);
        _healing.SetTransition(States.hit, _hit);
        _healing.SetTransition(States.dead, _die);
        _healing.SetTransition(States.patrol, _patrol);

        _attack.SetTransition(States.defend, _defending);
        _attack.SetTransition(States.search, _search);
        _attack.SetTransition(States.flee, _fleeing);
        _attack.SetTransition(States.chase, _chasing);
        _attack.SetTransition(States.hit, _hit);
        _attack.SetTransition(States.dead, _die);
        _attack.SetTransition(States.patrol, _patrol);
        _attack.SetTransition(States.stun, _stunState);

        _defending.SetTransition(States.attack, _attack);
        _defending.SetTransition(States.flee, _fleeing);
        _defending.SetTransition(States.chase, _chasing);
        _defending.SetTransition(States.search, _search);
        _defending.SetTransition(States.hit, _hit);
        _defending.SetTransition(States.dead, _die);
        _defending.SetTransition(States.patrol, _patrol);

        _hit.SetTransition(States.attack, _attack);
        _hit.SetTransition(States.chase, _chasing);
        _hit.SetTransition(States.flee, _fleeing);
        _hit.SetTransition(States.defend, _defending);
        _hit.SetTransition(States.search, _search);
        _hit.SetTransition(States.dead, _die);
        _hit.SetTransition(States.patrol, _patrol);

        _stunState.SetTransition(States.attack, _attack);
        _stunState.SetTransition(States.chase, _chasing);
        _stunState.SetTransition(States.flee, _fleeing);
        _stunState.SetTransition(States.search, _search);
        _stunState.SetTransition(States.dead, _die);
        _stunState.SetTransition(States.patrol, _patrol);
        _stunState.SetTransition(States.defend, _defending);



        _fsm = new StateMachine<States>(_goToCenter);
    }


    
    int debugIndex;
    bool[] debugIndexBoolColliding = new bool[8];
    public bool[] DebugIndexBoolColliding { get => debugIndexBoolColliding; set => debugIndexBoolColliding = value; }
    public bool debugBool;
    [Range(0, 20)]
    public float height;
    [Range(-1,1)]
    public float verticalOffset;

    
    


    private void OnDrawGizmos()
    {
        if (dummy) return;
        if (target)
        {
            if (Application.isPlaying)
            {
                if (InSight())
                {

                    Gizmos.color = Color.green;
                   
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position + new Vector3(0, verticalOffset* height, 0), target.transform.position - (transform.position+new Vector3(0,verticalOffset* height, 0)));
            }


        }
        Gizmos.DrawRay(transform.position + new Vector3(0, verticalOffset * height), Quaternion.AngleAxis(angleVision / 2, Vector3.up) * transform.forward * rangeVision);
        Gizmos.DrawRay(transform.position + new Vector3(0, verticalOffset * height), Quaternion.AngleAxis(-angleVision / 2, Vector3.up) * transform.forward * rangeVision);
        Gizmos.DrawWireSphere(transform.position, obstacleAvoidance);
        Vector3[] directions = new Vector3[5] 
        {
            (transform.forward),
            (transform.forward+transform.right),
            (transform.right),
            (transform.right*-1),
            (transform.forward+(transform.right*-1))
        };

        for (int i = 0; i < directions.Length; i ++)
        {
            Gizmos.color = Color.yellow;
            if (debugBool)
            {
                if (debugIndexBoolColliding[i]) Gizmos.color = Color.magenta;
                
                

                Gizmos.DrawRay(transform.position+new Vector3(0,verticalOffset,0),
                    ((directions[i].normalized+transform.position)-transform.position) * obstacleAvoidance);
            }
        }
        if (debugBool)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere((transform.position+new Vector3(0,verticalOffset,0) +
                         directions[debugIndex].normalized * obstacleAvoidance), .5f);

        }
        Gizmos.color = Color.red;

        for (int i = 0; i < angleVision; i+=3)
        {
            Gizmos.DrawRay(transform.position + new Vector3(0, verticalOffset * height), Quaternion.AngleAxis(((-angleVision / 2)+i), Vector3.up) * transform.forward *rangeToAttack);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, closestEnemy);
        if (team == EnemyTeam.red) Gizmos.color = Color.red;
            else Gizmos.color = Color.blue;
        if(target)
            Gizmos.DrawSphere(target.transform.position, 1);

        
    }
    

}

