using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fleeing<T> :State<T>
{
    Enemy _source;
    RoulleteWheel<States> roulleteWheel;
    public Fleeing (Enemy outerEnemy)
    {
        _source = outerEnemy;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        _source.animator.SetInteger("speed",(int)SpeedState.sprinting);
        
        
        _source.scared = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!_source.target)
        {
            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                            new Tuple<int, States>(Mathf.Clamp(_source.MaxCourage-_source.courage,0,100), States.heal) ,
                            new Tuple<int, States>(_source.life, States.patrol),
            };
            States _nextState = roulleteWheel.ProbabilityCalculator(transitions);
        }

            if (Vector3.Distance(_source.target.transform.position, _source.transform.position) < 7)
            {

                Vector3 dir = _source.transform.position - (new Vector3(_source.target.transform.position.x,
                           _source.transform.position.y,
                           _source.target.transform.position.z));
                
                if (_source.InRange()) _source.Transitionfsm(States.defend);

                if (_source.ClosestObstacle())
                {
                    _source.debugBool = true;

                    dir += (_source.ClosestPointToTarget(_source.transform.position - _source.target.transform.position) - _source.transform.position)
                         * Vector3.Distance(_source.ClosestObstacle().position, _source.transform.position)
                         * _source.avoidanceMultiplier * 0.5f;

                }
                else
                {
                    _source.debugBool = false;
                    for (int i = 0; i < _source.DebugIndexBoolColliding.Length; i++)
                    {
                        _source.DebugIndexBoolColliding[i] = false;
                    }

                }

                _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 3f * Time.deltaTime);

                _source.transform.position += _source.transform.forward * (_source.speed*3) * Time.deltaTime;
            }
            else
                _source.Transitionfsm(States.heal);





    }
}
