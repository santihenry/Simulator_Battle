using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attacking<T> : State<T>
{
    float _attackCadence=2;
    float _attackTimer;
    Enemy _source;
    RoulleteWheel<States> rouleteWheel = new RoulleteWheel<States>();
    public Attacking(Enemy outerEnemy)
    {
        _source = outerEnemy;
    }
    public override void OnEnter()
    {



        _attackTimer = 0;
        _source.animator.SetInteger("speed", (int)SpeedState.standing);
        _source.animator.SetTrigger("attack");

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
        else
            _source.Transitionfsm(States.patrol);

        if (_attackTimer < _attackCadence)
        {
                _attackTimer += Time.deltaTime;
            if (Mathf.Abs(_attackTimer - (_attackCadence / 2)) < 0.3f)
            {

                if (_source.InRange()) 
                    if (_source.canCauseDamage)
                    {
                        _source.target.canCauseDamage = false;
                        _source.target.TakeDamage(_source.damage, _source);
                        _source.canCauseDamage = false;
                        _attackTimer = 0;
                    }
            }
                
        }
        else
        {
            
                    
                if (!_source.InRange()) _source.Transitionfsm(States.chase);

            var healingCondition = _source.target.isHealing ? _source.courage : 0;

            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                                new Tuple<int, States>(_source.courage+_source.life+healingCondition, States.attack) ,
                                new Tuple<int, States>(_source.courage+Mathf.Clamp(_source.MaxHealth-_source.life,0,100), States.defend),
                                new Tuple<int, States>(Mathf.Clamp(Mathf.Clamp(_source.MaxHealth-_source.life,0,100)-_source.courage,0,100),States.flee)
                };

                States _nextState = rouleteWheel.ProbabilityCalculator(transitions);
                if (_nextState == States.attack)
                {
                    _source.animator.SetTrigger("attack");
                    _attackTimer = 0;
                }
                else
                    _source.Transitionfsm(_nextState);
            
        }
        
    }
    public override void OnSleep()
    {
        
        base.OnSleep();

    }
}
