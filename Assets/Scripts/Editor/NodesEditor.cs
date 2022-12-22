using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
[CustomEditor(typeof(NodeManager))]
public class NodesEditor : Editor
{
    NodeManager _target;
    bool gizmos;
    
    private void OnEnable()
    {
        _target = (NodeManager)target;

    }
    public override void OnInspectorGUI()
    {
        _target.node = (Node)EditorGUILayout.ObjectField("Node", _target.node,typeof(Node),true);
        _target.x = EditorGUILayout.IntField("columns", _target.x);
        _target.y = EditorGUILayout.IntField("rows", _target.y);
        _target.gap = EditorGUILayout.Slider("gap multiplier", _target.gap,0,1);
        _target.nodeSize = EditorGUILayout.Slider("nodes size", _target.nodeSize, 0.1f, 2);

        if (gizmos)
        {

            if (GUILayout.Button("Debug On"))
            {
                var nodes = FindObjectsOfType<Node>();
                foreach (var node in nodes)
                {
                    node.debug = true;

                }
                gizmos = false;
            }
            
        }
        else
        {
            if (GUILayout.Button("Debug Off"))
            {
                var nodes = FindObjectsOfType<Node>();
                foreach (var node in nodes)
                {
                    node.debug = false;
                }
                gizmos = true;
            }
        }
        if (GUILayout.Button("Create"))
        {
            Destroy();
            _target.GenerateNodes();

            var nodes =FindObjectsOfType<Node>();
            foreach (var node in nodes)
            {
                node.size = _target.nodeSize;
                node.Create();
                node.debug = true;
            }
            gizmos = false;
        }
        if (GUILayout.Button("GetNeighbours"))
        {
            var nodes = FindObjectsOfType<Node>();
            
            foreach (var node in nodes)
            {
                node.Clear();
                node.GetNeighbours();
                if (!node.IsWall)
                    _target.nodes.Add(node);
            }
            
        }
        if (GUILayout.Button("Clear"))
        {
            var nodes = FindObjectsOfType<Node>();
            foreach (var node in nodes)
            {
                node.Clear();
            }
        }
        if (GUILayout.Button("Destroy"))
        {
            Destroy();
           
        }
    }
    public void Destroy()
    {
        if (_target.father)
            DestroyImmediate(_target.father.gameObject);
        else
        {
            DestroyImmediate(GameObject.Find("Nodes"));
        }
        _target.nodes.Clear();
        gizmos = true;
    }
}
