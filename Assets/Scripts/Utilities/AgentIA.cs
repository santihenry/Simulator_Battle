using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentIA: MonoBehaviour
{
    
    public Node init;
    public Node finit;
   
    Theta<Node> _thetaStar = new Theta<Node>();
    
    LayerMask wall;
    static AgentIA _instance;
    public static AgentIA instance { get { return _instance; } }
   public AgentIA SetInit(Node initial)
    {
        wall.value = (int)Mathf.Pow(2, 9);
        init = initial;
        return this;
    }
    public AgentIA SetFinal(Node final)
    {
        finit = final;
        return this;
    }
    private void Awake()
    {
        _instance = this;
    }
    
    public List<Node> ThetaPath()
    {
        
        return _thetaStar.Run(init, Satisfies, GetNeighboursCost, Heuristic, InSight, CostTheta);
    }
  
  
    bool InSight(Node grandfather,Node grandson)
    {
        
        var dir = (grandson.transform.position - grandfather.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(grandfather.transform.position,dir.normalized, out hit,dir.magnitude,wall))
        {
            return false;
        }


        return true;
    }
    float CostTheta(Node grandfather, Node grandson)
    {
        return Vector3.Distance(grandfather.transform.position,grandson.transform.position);
    }
    float Heuristic(Node node)
    {
        
            return Vector3.Distance(node.transform.position, finit.transform.position);
        
    }
    bool Satisfies(Node curr)
    {
        return curr.Equals(finit);
    }
    Dictionary<Node, float> GetNeighboursCost(Node curr)
    {
        var dic = new Dictionary<Node, float>();
        if (curr.neighbours.Count > 0)
        {

            foreach (var item in curr.neighbours)
            {
                float cost = 0;
                cost += Vector3.Distance(item.transform.position, curr.transform.position);
                if (item.IsWall)
                    cost *= 100;
                dic.Add(item, cost);
            }
        }
        return dic;
    }
  

 
}
