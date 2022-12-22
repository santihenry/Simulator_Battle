using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Stun<T> : State<T>
{
    
    float _stunDuration = 1f;
    float _stunTimer;
    RoulleteWheel<States> rouletteWheel = new RoulleteWheel<States>();
    Enemy _source;
    public Stun(Enemy outerSource)
    {
        _source = outerSource;
    }
    public override void OnEnter()
    {


            _source.stunImg.SetActive(true);
            _stunTimer = 0;
            _source.animator.SetBool("stun",true);

            _source.animator.SetInteger("speed", (int)SpeedState.standing);
        

    }
    public override void OnUpdate()
    {

       
        if (_stunTimer < _stunDuration)
        {
            _stunTimer += Time.deltaTime;
        }
        else
        {
            if (!_source.target) _source.Transitionfsm(States.patrol);
            _stunTimer = 0;
            _source.stunImg.SetActive(false);
            List<Tuple<int, States>> transitions = new List<Tuple<int, States>>() {
                             new Tuple<int, States>(_source.courage+_source.life, States.attack) ,
                             new Tuple<int, States>(_source.courage+Mathf.Clamp(_source.MaxHealth-_source.life,0,100), States.defend),
                             new Tuple<int, States>(Mathf.Clamp(Mathf.Clamp(_source.MaxHealth-_source.life,0,100)-_source.courage,0,100),States.flee)
            };
            States _nextState = rouletteWheel.ProbabilityCalculator(transitions);
            
                _source.Transitionfsm(_nextState);


        }
    }
    public override void OnSleep()
    {
        base.OnSleep();
        _source.animator.SetBool("stun", false);
        _source.stunImg.SetActive(false);
    }
}
