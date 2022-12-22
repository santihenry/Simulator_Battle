using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defending<T>:State<T>
{
    float _defendCadence=2f;
    float _defendTimer;
    RoulleteWheel<States> rouletteWheel = new RoulleteWheel<States>();
    Enemy _source;
    public Defending(Enemy outerSource)
    {
        _source = outerSource;
    }
    public override void OnEnter()
    {
       
        _defendTimer = 0;

        _source.shield.enabled = true;
        _source.blocking = true;
        _source.animator.SetBool("block", true);
        _source.animator.SetInteger("speed", (int)SpeedState.standing);
        
    }
    public override void OnUpdate()
    {
        if (_source.target)
        {

            Vector3 dir = (new Vector3(_source.target.transform.position.x,
                       _source.transform.position.y,
                       _source.target.transform.position.z) - _source.transform.position);
            _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f * Time.deltaTime);
        }
        else _source.Transitionfsm(States.patrol);
        if (!_source.InRange()) _source.Transitionfsm(States.chase);
       
        if (_defendTimer < _defendCadence)
        {
            _defendTimer += Time.deltaTime;
        }
        else
        {
            var healingCondition = _source.target.isHealing ? _source.courage : 0;

            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                            new Tuple<int, States>(_source.courage+_source.life+healingCondition, States.attack) ,
                            new Tuple<int, States>(_source.courage+Mathf.Clamp(_source.MaxHealth-_source.life,0,100), States.defend),
                            new Tuple<int, States>(Mathf.Clamp(Mathf.Clamp(_source.MaxHealth-_source.life,0,100)-_source.courage,0,100),States.flee)
            };
            States _nextState = rouletteWheel.ProbabilityCalculator(transitions);
            _source.blocking = false;
            _source.shield.enabled = false;
            _source.animator.SetBool("block", false);
            if (_nextState == States.defend)
            {
                _source.shield.enabled = true;
                _source.blocking = true;
                _source.animator.SetBool("block", true);
                _defendTimer = 0;
            }
            else
            {
                
                _source.Transitionfsm(_nextState);

            }
            
            
        }
    }
    public override void OnSleep()
    {
        _source.shield.enabled = false;
        _source.animator.SetBool("block", false);
        _source.blocking = false;
    }

}
