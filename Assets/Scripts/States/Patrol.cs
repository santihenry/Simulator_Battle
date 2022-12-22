using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol<T> : State<T>
{
    Enemy _source;
    Node _destiny, init, final;
    AgentIA _agentIA = new AgentIA();
    List<Node> _waypoints = new List<Node>();
    int _currentWaypoint;
    public Patrol(Enemy source)
    {
        _source = source;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _source.scared = false;
        _source.animator.SetInteger("speed", (int)SpeedState.walking);
        var tempNodes = NodeManager._instance.nodes;
        _destiny = tempNodes[Random.Range(0, tempNodes.Count-1)];
        _currentWaypoint = 0;
        init = _source.CurrentNode();
        final = _destiny;
        _agentIA.SetInit(init).SetFinal(final);
        _waypoints = _agentIA.ThetaPath();

    }
    public override void OnUpdate()
    {

        if (_source.InSight()) { _source.Transitionfsm(States.chase); }
        if (_waypoints != null)
        {
            if (_waypoints.Count > 0)
            {
                if (Vector3.Distance(_waypoints[_waypoints.Count - 1].transform.position, _source.transform.position) <= 5)
                {
                    _source.Transitionfsm(States.patrol);
                }

                if (Vector3.Distance(_waypoints[_currentWaypoint].transform.position, _source.transform.position) <= 2)
                {
                    if (_currentWaypoint + 1 <= _waypoints.Count - 1)
                        _currentWaypoint++;
                }

                
                Vector3 dir= new Vector3(_waypoints[_currentWaypoint].transform.position.x,
                                         _source.transform.position.y,
                                         _waypoints[_currentWaypoint].transform.position.z) - _source.transform.position;
                if (_source.ClosestObstacle())
                {
                    _source.debugBool = true;

                    dir += (_source.ClosestPointToTarget(_waypoints[_currentWaypoint].transform.position) - _source.transform.position)
                         * Vector3.Distance(_source.ClosestObstacle().position, _source.transform.position)
                         * _source.avoidanceMultiplier;

                }

                _source.transform.forward = Vector3.Slerp(_source.transform.forward, dir, 2 * Time.deltaTime);
                _source.transform.position += _source.transform.forward * _source.speed * Time.deltaTime;
            }
        }
    }
    
    public override void OnSleep()
    {
        base.OnSleep();
       

    }
}
