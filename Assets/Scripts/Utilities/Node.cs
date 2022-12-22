using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    public List<Node> neighbours = new List<Node>();
    public bool IsWall;
    Renderer _renderer;
    public LayerMask node, wall;
    public float distance;
    bool _gizmos;
    public float size;
    
    public bool debug {set{_gizmos=value;}}
    
    private void Start()
    {

        
    }
    public void Create()
    {
        
        if (Physics.CheckBox(transform.position, Vector3.one * size, Quaternion.identity, wall))
            IsWall = true;
        else IsWall = false;
        if (IsWall) GetComponent<Collider>().enabled = false;
        else GetComponent<Collider>().enabled = true;
    }
    public void GetNeighbours()
    {
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.back);
    }

    public void Clear()
    {
        neighbours.Clear();
    }
    private void Update()
    {
      

    }
    void GetNeightbourd(Vector3 dir )
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, dir, out hit, distance, wall)) 
        { 
            if (Physics.Raycast(transform.position, dir, out hit, distance, node))
            {
                neighbours.Add(hit.collider.GetComponent<Node>());
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (_gizmos)
        {
            if (!IsWall)
            {

                if (neighbours.Count < 1)
                    Gizmos.color = Color.red;
                else if (neighbours.Count >= 1 && neighbours.Count < 4) Gizmos.color = Color.yellow;
                else
                    Gizmos.color = Color.green;
            }
            else Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position, Vector3.one * size);
        }
        //Gizmos.DrawRay(transform.position, Vector3.right * distance);

        //Gizmos.DrawRay(transform.position, Vector3.left * distance);

        //Gizmos.DrawRay(transform.position, Vector3.forward * distance);

        //Gizmos.DrawRay(transform.position, Vector3.back * distance);
        //Gizmos.DrawCube(transform.position, Vector3.one/5);
        //Gizmos.DrawWireCube(transform.position, Vector3.one * 0.4f);

    }
    private void OnDrawGizmosSelected()
    {
        if (_gizmos)
        {


            Gizmos.DrawRay(transform.position, Vector3.right * distance);

            Gizmos.DrawRay(transform.position, Vector3.left * distance);

            Gizmos.DrawRay(transform.position, Vector3.forward * distance);

            Gizmos.DrawRay(transform.position, Vector3.back * distance);
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.4f);
        }

    }
}