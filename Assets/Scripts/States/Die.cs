using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die<T> : State<T>
{
    Enemy _source;
    public Die(Enemy outerEnemy)
    {
        _source = outerEnemy;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _source.barLife.value = 0;
        _source.animator.SetBool("die",true);
        _source.GetComponent<Rigidbody>().isKinematic = true;
        _source.GetComponent<Collider>().enabled = false;
        _source.GetComponent<Enemy>().enabled = false;
        _source.barLife.gameObject.SetActive(false);
        if (_source.team == EnemyTeam.blue)
            CheckWinnerTeam.instance.blueMembers -= 1;
        else
            CheckWinnerTeam.instance.redMembers -= 1;
    }

    
}
