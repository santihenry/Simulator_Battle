using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public LayerMask node,walls;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Node CurrentNode()
    {
        Node node = new global::Node();
        Collider[] nodeColliders = Physics.OverlapSphere(transform.position, 5, this.node);
        float distance = new float();
        for (int i = 0; i < nodeColliders.Length; i++)
        {
            Vector3 _direction = (nodeColliders[i].transform.position - transform.position).normalized;
            float _distance = Vector3.Distance(transform.position, nodeColliders[i].transform.position);
            if (distance == 0)
            {

                if (!Physics.Raycast(transform.position, _direction, _distance, walls))
                {
                    distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                    node = nodeColliders[i].GetComponent<Node>();
                }
            }
            else
            {
                if (Vector3.Distance(nodeColliders[i].transform.position, transform.position) < distance)
                {

                    if (!Physics.Raycast(transform.position, _direction, _distance, walls))
                    {
                        distance = Vector3.Distance(nodeColliders[i].transform.position, transform.position);
                        node = nodeColliders[i].GetComponent<Node>();
                    }
                }
            }

        }

       
        return node;
    }
   
}
