using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Healing <T>: State<T>
{
    Enemy _source;
    float _timer;
    float _healCadence=.6f;
    RoulleteWheel<States> roulleteWheel=new RoulleteWheel<States>();
    public Healing(Enemy outerEnemy)
    {
        _source = outerEnemy;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        

        _source.animator.SetInteger("speed", (int)SpeedState.standing);
        _source.animator.SetBool("healing", true);
        _source._healingParticles.SetActive(true);
        _source.isHealing = true;

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_source.life < _source.MaxHealth)
        {

            if (_timer < _healCadence)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _source.courage++;
                _source.life += 5;
                _timer = 0;

            }
        }
        else
        {
            _source.courage += 10;
            _source.Transitionfsm(States.chase);
        }
        if (_source.InSight())
        {
            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                            new Tuple<int, States>(_source.courage, States.chase) ,
                            new Tuple<int, States>(Mathf.Clamp(Mathf.Clamp(_source.MaxHealth-_source.life,0,100)-_source.courage,0,100),States.flee)
            };
            States _nextState = roulleteWheel.ProbabilityCalculator(transitions);
            _source.Transitionfsm(_nextState);
        }
    }
    public override void OnSleep()
    {
        base.OnSleep();
        _source.animator.SetBool("healing", false);
        _source._healingParticles.SetActive(false);
        _source.isHealing = false;
    }
}
