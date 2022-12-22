using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(MinionCreator))]
public class MinionCreatorEditor : Editor
{
    MinionCreator _target;
    private void OnEnable()
    {
        _target = (MinionCreator)target;
    }
    public override void OnInspectorGUI()
    {



        GUI.BeginGroup(new Rect(0, 10, Screen.width, 600));
        _target.prefab = (EnemyMinion)EditorGUILayout.ObjectField("Minion", _target.prefab, typeof(EnemyMinion), true);
        GUI.BeginGroup(new Rect(18, 25, Screen.width, 600));
        GUI.Label(new Rect(0, 0, Screen.width, 20), "Cantidad de minions");
        if (_target.enemyList.Count == 0) GUI.enabled = false;
        if (GUI.Button(new Rect(Screen.width*.405f, 0, 20, 20), "◄"))
        {
            _target.enemyList.RemoveAt(_target.enemyList.Count - 1);
            DestroyImmediate(_target.lastCreated.gameObject,true);
            if (_target.enemyList.Count > 0)
                _target.lastCreated= _target.enemyList[_target.enemyList.Count - 1];
            if(_target.index>0)_target.index--;
        }
        if (_target.enemyList.Count < 10)
            GUI.enabled = true;
        else
            GUI.enabled = false;
        GUI.Label(new Rect(Screen.width * .405f+40, 0, 30, 20), _target.enemyList.Count.ToString());
        if (GUI.Button(new Rect(Screen.width * .405f + 70, 0, 20, 20), "►"))
        {
            var position = _target.color == 0 ? _target.bluePositions : _target.redPositions;
            _target.lastCreated= Instantiate(_target.prefab, position[_target.index],_target.transform.rotation);
            _target.lastCreated.GetComponent<Flocking>().leaderToFollow = _target.gameObject.transform;
            _target.lastCreated.team = (EnemyTeam)_target.color;
            _target.enemyList.Add(_target.lastCreated);
            if (_target.index < 10) _target.index++;

        }
        
        GUI.EndGroup();
        GUI.EndGroup(); 
        GUILayout.Space(40);

        _target.color = EditorGUILayout.Popup(_target.color, _target.team);
        GUILayout.Space(50);
        

    }

}
