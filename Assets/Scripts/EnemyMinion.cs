using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion : Enemy
{
    // Start is called before the first frame update
     FollowLeader<States> _followLeader;
     chasing<States> _chasing;
     Attacking<States> _attacking;
     Fleeing<States> _fleeing;
     Healing<States> _healing;
     Defending<States> _defending;
     Hit<States> _hit;
     Die<States> _die;
     Stun<States> _stun;
     StateMachine<States> _fsm;
    public bool randomizeAttributes;
    

    void Start()
    {
        if (randomizeAttributes)
        {
            courage = (int)Random.Range(10, 50);
            defense= (int)Random.Range(10, 50);
            damage= (int)Random.Range(1, 10);
        }
        if (team == EnemyTeam.red)
        {
            enemies.value = (int)Mathf.Pow(2, 11);
            gameObject.layer = 10;

            body.material = materials[0];

        }
        else
        {

            enemies.value = (int)Mathf.Pow(2, 10);
            gameObject.layer = 11;
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

        barLife.value = life;

        _fsm.OnUpdate();

        target = ClosestEnemy(!scared);


        
        if (target)
        {

            _directionToTarget = target.transform.position - transform.position;
            _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            _angleToTarget = Vector3.Angle(transform.forward, _directionToTarget);
        }
    }
    
    public override void SetStateMachine()
    {
        _followLeader=new FollowLeader<States>(this);
        _chasing=new chasing<States>(this);
        _attacking= new Attacking<States>(this);
        _fleeing=new Fleeing<States>(this);
        _healing= new Healing<States>(this);
        _defending=new Defending<States>(this);
        _hit = new Hit<States>(this);
        _stun = new Stun<States>(this);
        _die = new Die<States>(this);

        _followLeader.SetTransition(States.chase, _chasing);
        _followLeader.SetTransition(States.hit, _hit);
        _followLeader.SetTransition(States.dead, _die);

        _chasing.SetTransition(States.patrol, _followLeader);
        _chasing.SetTransition(States.attack, _attacking);
        _chasing.SetTransition(States.defend, _defending);
        _chasing.SetTransition(States.hit, _hit);
        _chasing.SetTransition(States.dead, _die);

        _attacking.SetTransition(States.chase, _chasing);
        _attacking.SetTransition(States.defend, _defending);
        _attacking.SetTransition(States.attack, _attacking);
        _attacking.SetTransition(States.flee, _fleeing);
        _attacking.SetTransition(States.patrol, _followLeader);
        _attacking.SetTransition(States.hit, _hit);
        _attacking.SetTransition(States.stun, _stun);
        _attacking.SetTransition(States.dead, _die);

        _fleeing.SetTransition(States.heal, _healing);
        _fleeing.SetTransition(States.defend, _defending);
        _fleeing.SetTransition(States.hit, _hit);
        _fleeing.SetTransition(States.dead, _die);
        _fleeing.SetTransition(States.patrol, _followLeader);

        _healing.SetTransition(States.chase, _chasing);
        _healing.SetTransition(States.flee, _fleeing);
        _healing.SetTransition(States.hit, _hit);
        _healing.SetTransition(States.dead, _die);

        _defending.SetTransition(States.chase, _chasing);
        _defending.SetTransition(States.defend, _defending);
        _defending.SetTransition(States.attack, _attacking);
        _defending.SetTransition(States.flee, _fleeing);
        _defending.SetTransition(States.patrol, _followLeader);
        _defending.SetTransition(States.hit, _hit);
        _defending.SetTransition(States.dead, _die);

        _hit.SetTransition(States.chase, _chasing);
        _hit.SetTransition(States.defend, _defending);
        _hit.SetTransition(States.attack, _attacking);
        _hit.SetTransition(States.flee, _fleeing);
        _hit.SetTransition(States.patrol, _followLeader);
        _hit.SetTransition(States.dead, _die);

        _stun.SetTransition(States.attack, _attacking);
        _stun.SetTransition(States.chase, _chasing);
        _stun.SetTransition(States.flee, _fleeing);
        _stun.SetTransition(States.patrol, _followLeader);
        _stun.SetTransition(States.defend, _defending);
        _stun.SetTransition(States.dead, _die);

        _fsm = new StateMachine<States>(_followLeader);
    }

    public override void Transitionfsm(States action)
    {
        _fsm.Transition(action);
    }

}
