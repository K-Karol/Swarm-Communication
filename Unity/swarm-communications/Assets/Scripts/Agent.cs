using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Agent : MonoBehaviour {
    public int id;
    //private string comm_method;
    private Console console;
    public Color color;

    //public string communication_method { get { return comm_method; } }
    public Communications communications;
    public GameObject network_obj;

    // Use this for initialization
    public void initialise(int agent_id_para,string comm_method_para,GameObject network_obj_para) {
        id = agent_id_para;
        color = UnityEngine.Random.ColorHSV(0f, 1f,0.3f,0.7f,0.6f,1f,1f,1f);
        network_obj = network_obj_para;
        this.name = "Agent:" + id.ToString();
        console = GameObject.Find("Debug").GetComponent<Console>();
        communications = new Communications(id, comm_method_para, console);
        console.log(this.name + " was initialised");
    }
    public void set_connection_mode(string mode_para)
    {
        if(mode_para == "all")
        {
            List<Agent> available_connections = communications.Scan_network(network_obj);
            foreach(Agent agent in available_connections)
            {
                communications.connect(agent);
            }
        }
        else if(mode_para == "random")
        {
            List<Agent> available_connections = communications.Scan_network(network_obj);
            System.Random random = new System.Random();
            int random_number_of_connections = random.Next(1, available_connections.Count-1);
            List<int> used_up = new List<int>();
            for(int i = 0;i< random_number_of_connections; i++)
            {
                bool pass = false;
                while(pass != true) {
                    int random_index = random.Next(0, available_connections.Count);
                    if (!used_up.Contains(random_index))
                    {
                        used_up.Add(random_index);
                        communications.connect(available_connections[random_index]);
                        pass = true;
                    }
                }
                
            }

        }
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
public class Communications
{
    public int id;
    private string comm_method;
    public string Communication_method { get { return comm_method; } }
    public Dictionary<int,Link> active_connections = new Dictionary<int, Link>();
    public Link self_link;

    private Console console;

    public Communications(int agent_id_para, string comm_method_para, Console console_reference)
    {
        id = agent_id_para;
        comm_method = comm_method_para;
        console = console_reference;
        self_link = new Link(id, comm_method, this);
    }
    public List<Agent> Scan_network(GameObject network_obj_para)
    {
        List<Agent> available_connections = new List<Agent>();
        foreach (Transform child in network_obj_para.transform)
        {
            int temp_agent_id;
            string[] temp_split_name = child.name.Split(':');
            if (Int32.TryParse(temp_split_name[1], out temp_agent_id))
            {
                if (!active_connections.ContainsKey(temp_agent_id) && temp_agent_id != id)
                {
                    available_connections.Add(child.GetComponent<Agent>());
                }
            }
            else
            {
                console.log("Could not parse agent ID (Agent/Communications/Scan_Network_Mesh");
            }
        }
        return available_connections;
    }
    public void connect(Agent target_agent)
    {
        int target_id = target_agent.id;
        console.log("Attempting connection: Agent:" + id + ";" + "Agent:" + target_agent.id);
        Communications target_comms = target_agent.communications;
        Link target_link = target_comms.incoming_Connections(self_link);
        if (!(active_connections.ContainsKey(target_id)) && target_link != null)
        {
            active_connections.Add(target_id,target_link);
            console.log("Connection successfull: Agent:" + id + ";" + "Agent:" + target_agent.id);
        }
    }
    public Link incoming_Connections(Link incoming_link)
    {
        if (!(active_connections.ContainsKey(incoming_link.id)))
        {
            active_connections.Add(incoming_link.id,incoming_link);
            return self_link;
        }
        return null;
    }
}
public class Link
{
    public int id;
    private string comm_method;
    public string Communication_method { get { return comm_method; } }
    public Communications self;
    public Link(int agent_id_para, string comms_method_para, Communications self_para)
    {

        id = agent_id_para;
        comm_method = comms_method_para;
        self = self_para;
    }
}
