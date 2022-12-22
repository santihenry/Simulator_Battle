using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Hit<T> : State<T>
{
    float _hitCadence = .7f;
    float _hitTimer;
    RoulleteWheel<States> rouletteWheel = new RoulleteWheel<States>();
    Enemy _source;
    public Hit(Enemy outerSource)
    {
        _source = outerSource;
    }
    public override void OnEnter()
    {

        
       
            _hitTimer = 0;
            _source.animator.SetTrigger("hit");
            _source.animator.SetInteger("speed", (int)SpeedState.standing);
        

    }
    public override void OnUpdate()
    {
        if (!_source.target) _source.Transitionfsm(States.patrol);
        if (!_source.InRange()) _source.Transitionfsm(States.chase);
        Vector3 dir = (new Vector3(_source.target.transform.position.x,
                   _source.transform.position.y,
                   _source.target.transform.position.z) - _source.transform.position);
        _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2f * Time.deltaTime);
        if (_hitTimer < _hitCadence)
        {
            _hitTimer += Time.deltaTime;
        }
        else
        {
            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                             new Tuple<int, States>(_source.courage+_source.life, States.attack) ,
                             new Tuple<int, States>(_source.courage+Mathf.Clamp(_source.MaxHealth-_source.life,0,100), States.defend),
                             new Tuple<int, States>(Mathf.Clamp(Mathf.Clamp(_source.MaxHealth-_source.life,0,100)-_source.courage,0,100),States.flee)
            };
            States _nextState = rouletteWheel.ProbabilityCalculator(transitions);
            _source.Transitionfsm(_nextState);

            


        }
    }
}
