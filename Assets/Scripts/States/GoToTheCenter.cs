using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTheCenter<T> : State<T>
{
    
    Enemy _source;
    Vector3 center=new Vector3(22,0,24);
    public GoToTheCenter(Enemy outerEnemy)
    {
        _source = outerEnemy;
    }
    public override void OnEnter()
    {
        
        
        _source.animator.SetInteger("speed", (int)SpeedState.walking);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 dir = (new Vector3(center.x,
                  _source.transform.position.y,
                  center.z) - _source.transform.position);
        if (_source.ClosestObstacle())
        {
            _source.debugBool = true;

            dir += (_source.ClosestPointToTarget(center) - _source.transform.position)
                 * Vector3.Distance(_source.ClosestObstacle().position, _source.transform.position)
                 * _source.avoidanceMultiplier;

        }
        _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f * Time.deltaTime);

        _source.transform.position += _source.transform.forward * _source.speed * Time.deltaTime;

        if (Vector3.Distance(center, _source.transform.position) < 20) _source.Transitionfsm(States.patrol);

    }
}
