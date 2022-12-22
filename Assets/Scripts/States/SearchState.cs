using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search<T> : State<T>
{
    Enemy _source;
    AgentIA _agentIA;
    List<Node> _waypoints=new List<Node>();
    int _currentWaypoint;
    Node init, final;
    float _speed = 2;
    float _timer;
    float _timeToPatrol=3;
    public Search(Enemy source)
    {
        _source = source;
        _agentIA = source.GetComponent<AgentIA>();
    }
    public override void OnEnter() 
    {
        _source.scared = false;
        _source.animator.SetInteger("speed", (int)SpeedState.runing);
        _currentWaypoint = 0;
        init = _source.CurrentNode();
        final = _source.target.CurrentNode();
        _agentIA.SetInit(init).SetFinal(final);
        _waypoints = _agentIA.ThetaPath();
        _timer = 0;
    }

    public override void OnUpdate() {


        if (_source.InSight()) { _source.Transitionfsm(States.chase); }
        if (_timer > _timeToPatrol) _source.Transitionfsm(States.patrol);
        if (_waypoints != null)
        {
            if (_waypoints.Count > 0)
            {
                if (Vector3.Distance(_waypoints[_waypoints.Count - 1].transform.position, _source.transform.position) <= 1)
                {
                    _timer += Time.deltaTime;
                }
              
                if (Vector3.Distance(_waypoints[_currentWaypoint].transform.position, _source.transform.position) <= 2)
                {
                    if (_currentWaypoint + 1 <= _waypoints.Count - 1)
                        _currentWaypoint++;
                }

                Vector3 dir = (new Vector3(_waypoints[_currentWaypoint].transform.position.x,
                    _source.transform.position.y,
                    _waypoints[_currentWaypoint].transform.position.z) - _source.transform.position);
                
                if (_source.ClosestObstacle())
                {
                    _source.debugBool = true;

                    dir += (_source.ClosestPointToTarget(_waypoints[_currentWaypoint].transform.position) - _source.transform.position)
                         * Vector3.Distance(_source.ClosestObstacle().position, _source.transform.position)
                         * _source.avoidanceMultiplier;

                }

                _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2 * Time.deltaTime);
                _source.transform.position += _source.transform.forward* _source.speed*1.5f * Time.deltaTime;
            }
        }
    }
    public override void OnSleep() { }
}
