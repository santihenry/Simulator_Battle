using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int x;
    public int y;
    public Node node;
    public float gap;
    GameObject _father;
    public bool _debug;
    public static NodeManager _instance;
    public List<Node> nodes=new List<Node>();
    public float nodeSize;


    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject father { get { return _father; } }

    public void GenerateNodes()
    {
        if (!_father)
            _father = new GameObject("Nodes");
        
        
        for (int i = 0; i < x; i++)
        {
            for (int k = 0; k < y; k++)
            {
                
                var position = transform.position+new Vector3(i*gap,0,k*gap);
                Instantiate(node.gameObject, position, Quaternion.identity,_father.transform);
            }
        }
        
    }
}
