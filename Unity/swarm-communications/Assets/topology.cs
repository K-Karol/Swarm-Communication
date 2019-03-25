using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class topology : MonoBehaviour
{
    public GameObject cam;
    public renderLines camRender;
    public float linkForce;
    public float separationForce;
    public float size;
    public float minDistance;   
    public Vector2 initPos = new Vector2(-4.25f,4.25f);

    public Dictionary<int, Node> nodes = new Dictionary<int, Node>();
    public bool sim = true;
    public bool ready = false;
    public UnityEngine.Object nodeP;
    //public Dictionary<int, GameObject> nodesDict = new Dictionary<int, GameObject>();
    public console debug;
    //-3 3.5
    // Use this for initialization
    public void init()
    {
        camRender = cam.GetComponent<renderLines>();
        linkForce = 1f;
        separationForce = 2f;
        minDistance = 2f;
        debug = GameObject.Find("Debug").GetComponent<console>();
        GameObject net = GameObject.Find("network");
        List<Transform> temp = new List<Transform>();
        foreach (Transform child in net.transform)
        {
            temp.Add(child);
        }
        Transform[] agents = temp.ToArray();
        System.Random ran = new System.Random();
        for (int i = 0; i < agents.Length; i++)
        {
            
            float ranTempX = ran.Next(-3000, 3000);
            float ranTempY = ran.Next(-3000, 3000);
            Transform tempTrans = agents[i];
            agent agentClass = tempTrans.GetComponent<agent>();
            communications agentComms = agentClass.comm;

            List<link> links = agentComms.connections;

            int tempID;
            string[] tempName = tempTrans.name.Split(':');
            if (Int32.TryParse(tempName[1], out tempID))
            {
                
                Vector2 randomP = new Vector2(ranTempX/1000f, ranTempY/1000f);
                nodes.Add(tempID, new Node() { id = tempID, connections = links,pos=initPos+randomP,obj = Instantiate(nodeP,this.transform) as GameObject});
                nodes[tempID].obj.transform.localPosition = initPos+randomP;
                nodes[tempID].obj.transform.name = tempID + "_node";
            }
            else
            {
                debug.log("ERROR: Could not parse 'tempID' in the Update function, in the subroutine where the child.name is split into the ID and parsed");
            }

        }

        foreach (var n in nodes)
        {
            foreach (link l in n.Value.connections)
            {
                Node tN = nodes[l.agentID];
                n.Value.linked.Add(tN);
            }
        }

        foreach(var n in nodes)
        {
            foreach(Node l in n.Value.linked)
            {
                line tempLine = new line() { members = new List<Node>() { n.Value, l } };
                bool exists = false;
                foreach(line lin in l.strings)
                {
                    List<Node> tempMembers = lin.members;
                    if(tempMembers.Contains(n.Value) && tempMembers.Contains(l))
                    {
                        exists = true;
                        break;
                    }
                }
                if(exists != true)
                {
                    n.Value.strings.Add(tempLine);
                    l.strings.Add(tempLine);
                }
            }
        }
        

        ready = true;


        

        //    for(int i = 0; i < nodes.Length; i++)
        //    {
        //        nodesDict.Add(i, Instantiate(nodeP, this.transform) as GameObject);
        //        nodesDict[i].transform.localPosition = new Vector3(-3f + 1f*(i), 3.5f, this.transform.position.z);
        //    }
        //}
        //public void refresh(string target)
        //{
        //    int tempID;
        //    string[] tempName = target.Split(':');
        //    if (Int32.TryParse(tempName[1], out tempID))
        //    {
        //        GameObject tempAgent = GameObject.Find(target);
        //        agent tempClass = tempAgent.GetComponent<agent>();
        //        nodesDict[tempID - 1].GetComponent<SpriteRenderer>().color = tempClass.colour;
        //    }
        //    else
        //    {
        //        debug.log("ERROR: Could not parse 'tempID' in the Update function, in the subroutine where the child.name is split into the ID and parsed");
        //    }
        //}

    }
    void Update()
    {
        if(sim == true) //cannot be < -8.5 on the x and > 8.5 on y ; < 0 on the y and >0 on the x
        {
            ApplyForces();
            
            foreach(Node n in nodes.Values)
            {
                Vector2 tPos = n.pos += n.vel * Time.deltaTime;
                if(tPos.x < -8.5f)
                {
                    tPos.x = -8.5f;
                }
                if (tPos.x > 0)
                {
                    tPos.x = 0f;
                }
                if (tPos.y < 0f)
                {
                    tPos.y = 0f;
                }
                if (tPos.y > 8.5f)
                {
                    tPos.y = 8.5f;
                }
                n.pos = tPos;
                n.obj.transform.localPosition = tPos;
                
            }

        }
    }
    public List<List<Vector2>> PrepLines()
    {
        List<line> allLines = new List<line>();
        List<List<Vector2>> coords = new List<List<Vector2>>(); //2d array
        foreach (Node n in nodes.Values)
        {
            foreach (line l in n.strings)
            {
                allLines.Add(l);
            }
            foreach (line l in allLines)
            {
                coords.Add(
                    new List<Vector2>
                    {
                    l.members[0].obj.transform.position,
                    l.members[1].obj.transform.position
                    });
            }
        }
        foreach (List<Vector2> xy in coords)
        {
            if (xy[0].magnitude > xy[1].magnitude)
            {
                var tmp = xy[1];
                xy[1] = xy[0];
                xy[0] = tmp;
            }
        }
        return coords = new HashSet<List<Vector2>>(coords).ToList();


    }
    void Start()
    {
        print("Init");
    }
    public class Node
    {
        public int id;
        public Vector2 pos;
        public Vector2 vel;
        public Vector2 acc;
        public List<Node> linked = new List<Node>(); // nodes
        public Color col;
        public List<link> connections;  //communications class variable
        public List<line> strings = new List<line>();
        public GameObject obj;
    }
    public class line
    {
        public List<Node> members = new List<Node>();
    }
    public void ApplyForces()
    {
        List<Node> tempList = new List<Node>();
        foreach(var n in nodes)
        {
            tempList.Add(n.Value);
        }
        foreach(var n in nodes)
        {
            var notLinked = tempList.Except(n.Value.linked);
            foreach(var linkedNode in n.Value.linked)
            {
                Vector2 displacement = n.Value.pos - linkedNode.pos;
                var distance = (displacement).magnitude;
                float applied = 0;
                if(distance > 0)
                {
                    applied = linkForce * Mathf.Log10(distance / minDistance);
                }

                linkedNode.vel += applied * Time.deltaTime * displacement.normalized;
            }
            foreach (var notLinkedNode in notLinked)
            {
                Vector2 displacement = n.Value.pos -  notLinkedNode.pos;
                var distance = (displacement).magnitude;
                float applied = 0;
                if (distance <= 0)
                {
                    applied = separationForce;
                } else if (distance > 0)
                {
                    applied = separationForce / Mathf.Pow(distance, 2);
                }

                notLinkedNode.vel += applied * Time.deltaTime * displacement.normalized;
            }



        }
    }
}
