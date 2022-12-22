using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader<T> : State<T>
{
    EnemyMinion _source;
    Flocking _flocking;
    public FollowLeader(EnemyMinion outerEnemy)
    {
        _source = outerEnemy;
    }
    public override void OnEnter()
    {
        _source.animator.SetInteger("speed", (int)SpeedState.walking);
        _flocking = _source.GetComponent<Flocking>();
        
    }
    public override void OnUpdate()
    {
        
        _flocking.SetLeader();
        var dir = _flocking.GetDir();
        if (_source.ClosestObstacle())
        {


            dir += (_source.ClosestPointToTarget(_flocking.leaderToFollow.transform.position) - _source.transform.position)
                 * Vector3.Distance(_source.ClosestObstacle().position, _source.transform.position)
                 * _source.avoidanceMultiplier;

        }
        if (Vector3.Distance(_flocking.leaderToFollow.position, _source.transform.position) < 5)
        {
            _flocking.ClearTarget();
        }
        _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f * Time.deltaTime);
        _source.transform.position += _source.transform.forward * _source.speed * Time.deltaTime;

        if (_source.InSight()) _source.Transitionfsm(States.chase);

    }

    
}

