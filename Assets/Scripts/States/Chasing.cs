using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class chasing<T> : State<T>
{
    Enemy _source;
    RoulleteWheel<States> rouletteWheel=new RoulleteWheel<States>();
   

    public chasing(Enemy source)
    {
        _source = source;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _source.animator.SetInteger("speed", (int)SpeedState.runing);
        _source.scared = false;

    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!_source.target) { _source.Transitionfsm(States.patrol); 
            return; 
        }
        
        Vector3 dir = (new Vector3(_source.target.transform.position.x,
                   _source.transform.position.y,
                   _source.target.transform.position.z) - _source.transform.position);

        if (_source.ClosestObstacle())
        {
            _source.debugBool = true;

            dir+=(_source.ClosestPointToTarget(_source.target.transform.position)-_source.transform.position)
                 *Vector3.Distance(_source.ClosestObstacle().position,_source.transform.position)
                 *_source.avoidanceMultiplier;
            
        }
        else
        {
            _source.debugBool = false;
            for (int i = 0; i < _source.DebugIndexBoolColliding.Length; i++)
            {
                _source.DebugIndexBoolColliding[i] = false;
            }
            
        }

        _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f*Time.deltaTime);

        _source.transform.position += _source.transform.forward * _source.speed*2f * Time.deltaTime;

        if (_source.InRange())
        {
            var healingCondition = _source.target.isHealing ? _source.courage : 0;
            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>()
            {
                            new Tuple<int, States>(_source.courage+_source.life+healingCondition, States.attack) ,
                            new Tuple<int, States>(_source.courage+Mathf.Clamp(_source.MaxHealth-_source.life,0,100), States.defend),
                            
            };
            
            _source.Transitionfsm(rouletteWheel.ProbabilityCalculator(transitions));

          
        }
        else
            _source.Transitionfsm(States.search);




    }
    
}
