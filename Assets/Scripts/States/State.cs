using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> 
{
    
    Dictionary<T, State<T>> _actions = new Dictionary<T, State<T>>();


    public virtual void OnEnter() {  }

    public virtual void OnUpdate() { }
    public virtual void OnSleep() { }
    
    

    public void SetTransition(T input, State<T> state)
    {
        if (!_actions.ContainsKey(input))
            _actions.Add(input, state);
        
    }

    
    
    public State<T> GetState(T input)
    {
        if (_actions.ContainsKey(input))
        {
            return _actions[input];

        }

        else return null;
    }
}
public enum States
{
    patrol,
    search,
    chase,
    flee,
    attack,
    defend,
    heal,
    hit,
    dead,
    stun
}

