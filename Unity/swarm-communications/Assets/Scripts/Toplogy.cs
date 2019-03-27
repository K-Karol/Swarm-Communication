using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Toplogy : MonoBehaviour
{
    public float separation_force;
    public float attraction_force;
    public float desired_distance;
    public float mass;

    public List<Vector2> boundry = new List<Vector2>() { new Vector2(-8.5f, 8.5f), new Vector2(0f, 0f) };

    public GameObject topology_obj;
    public GameObject network;
    public Console console;
    public UnityEngine.Object node_prefab;
    public UnityEngine.Object line_prefab;

    public Dictionary<int, Node> nodes = new Dictionary<int, Node>();
    public Line[] lines;
    public Dictionary<Line, LineRenderer> line_renders = new Dictionary<Line, LineRenderer>();

    public Material matTemplate;

    public bool finished = false;

    public void initialise()
    {
        separation_force = -3f;  //will be dynamically generated
        attraction_force = 2f;
        desired_distance = 0.5f;
        mass = 1f;

        topology_obj = this.gameObject;
        network = GameObject.Find("network");
        console = GameObject.Find("Debug").GetComponent<Console>();

        System.Random random = new System.Random();
        Vector2 midpoint_boundry = (boundry[0] + boundry[1]) / 2f;
        foreach (Transform child in network.transform)
        {
            Agent child_agent = child.gameObject.GetComponent<Agent>();
            Node temp_node = new Node();
            temp_node.id = child_agent.id;
            temp_node.active_connections = child_agent.communications.active_connections;
            nodes.Add(child_agent.id, temp_node);
            nodes[child_agent.id].node_obj = Instantiate(node_prefab, this.transform) as GameObject;
            nodes[child_agent.id].node_obj.transform.name = "Agent:" + child_agent.id;
            nodes[child_agent.id].node_obj.GetComponent<SpriteRenderer>().color = child_agent.color;
            nodes[child_agent.id].colour = child_agent.color;
            nodes[child_agent.id].set_disp(midpoint_boundry + new Vector2(random.Next(-2000, 2000), random.Next(-2000, 2000)) / 1000);
        }
        Dictionary<string, Line> temp_line_dict = new Dictionary<string, Line>();
        foreach (var node in nodes)
        {
            foreach (var link in node.Value.active_connections)
            {
                node.Value.linked_nodes.Add(nodes[link.Value.id]);
                Line temp_line = new Line(node.Value.id, link.Value.id);
                if (temp_line.ids[0] >= temp_line.ids[1])
                {
                    if (!(temp_line_dict.ContainsKey(temp_line.ids[0] + "$" + temp_line.ids[1])))
                    {
                        temp_line_dict.Add(temp_line.ids[0] + "$" + temp_line.ids[1], temp_line);
                    }

                }
                else
                {
                    if (!(temp_line_dict.ContainsKey(temp_line.ids[1] + "$" + temp_line.ids[0])))
                    {
                        temp_line_dict.Add(temp_line.ids[1] + "$" + temp_line.ids[0], temp_line);
                    }
                }

            }
        }
        lines = temp_line_dict.Values.ToList().ToArray();

        foreach (Line line in lines)
        {
            GameObject temp_line_obj = Instantiate(line_prefab, this.transform) as GameObject;
            temp_line_obj.name = line.ids[0] + "$" + line.ids[1];
            temp_line_obj.transform.localPosition = new Vector3(0f, 0f, 0f);
            line_renders.Add(line, temp_line_obj.GetComponent<LineRenderer>());
            
        }
        finished = true;



    }
    public void RenderLines()
    {
        foreach(var line in line_renders)
        {
            var points = new Vector3[2];
            points[0] = nodes[line.Key.ids[0]].displacement;
            points[0].z = this.transform.position.z;
            points[1] = nodes[line.Key.ids[1]].displacement;
            points[1].z = this.transform.position.z;
            line.Value.SetPositions(points);
            line.Value.material = new Material(matTemplate);
            line.Value.SetColors(nodes[line.Key.ids[0]].colour, nodes[line.Key.ids[1]].colour);
        }
    }
    void FixedUpdate()
    {
        if(finished == true) {
            apply_forces();
            foreach (Node n in nodes.Values)
            {
                Vector2 temp_pos = n.displacement += n.velocity * Time.fixedDeltaTime;
                if (temp_pos.x < -8.5f)
                {
                    temp_pos.x = -8.5f;
                }
                if (temp_pos.x > 0)
                {
                    temp_pos.x = 0f;
                }
                if (temp_pos.y < 0f)
                {
                    temp_pos.y = 0f;
                }
                if (temp_pos.y > 8.5f)
                {
                    temp_pos.y = 8.5f;
                }
                n.set_disp(temp_pos);
            }
            RenderLines();
        }
        
    }
    void apply_forces()
    {
        List<Node> temp_list = nodes.Values.ToList();
        foreach (Node n in temp_list)
        {
            var notLinked = temp_list.Except(n.linked_nodes);
            foreach (var linkedNode in n.linked_nodes)
            {
                Vector2 displacement = n.displacement - linkedNode.displacement;
                var distance = (displacement).magnitude;
                float applied = 0;
                if (distance > 0)
                {
                    applied = attraction_force * Mathf.Log10(distance / desired_distance);
                }

                linkedNode.velocity += applied * Time.deltaTime * displacement.normalized;
            }
            foreach (var notLinkedNode in notLinked)
            {
                Vector2 displacement = n.displacement - notLinkedNode.displacement;
                var distance = (displacement).magnitude;
                float applied = 0;
                if (distance <= 0)
                {
                    applied = separation_force;
                }
                else if (distance > 0)
                {
                    applied = separation_force / Mathf.Pow(distance, 2);
                }

                notLinkedNode.velocity += applied * Time.deltaTime * displacement.normalized;
            }
        }
    }
    public class Node
    {
        public int id;

        public Vector2 displacement;
        public Vector2 velocity;
        public Vector2 forces;
        public float mass = 1f;

        public GameObject node_obj;

        public List<Node> linked_nodes = new List<Node>();
        public Color colour;

        public Dictionary<int, Link> active_connections;

        //public Node(int id_para, Dictionary<int, Link> active_para)
        //{
        //    int id = id_para;
        //    active_connections = active_para;
        //}
        public void set_disp(Vector2 new_disp)
        {
            displacement = new_disp;
            node_obj.transform.localPosition = displacement;
        }
    }
    public class Line
    {
        public int[] ids;
        public Line(int id1, int id2)
        {
            ids = new int[] { id1, id2 };
        }
    }
}