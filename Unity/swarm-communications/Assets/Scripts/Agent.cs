using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class Agent : MonoBehaviour {
    public int id;
    //private string comm_method;
    private Console console;
    public Color color;

    //public string communication_method { get { return comm_method; } }
    public Communications communications;
    public GameObject network_obj;

    // Use this for initialization
    public void initialise(int agent_id_para, string comm_method_para, GameObject network_obj_para) {
        id = agent_id_para;
        color = UnityEngine.Random.ColorHSV(0f, 1f, 0.3f, 0.7f, 0.6f, 1f, 1f, 1f);
        network_obj = network_obj_para;
        this.name = "Agent:" + id.ToString();
        console = GameObject.Find("Debug").GetComponent<Console>();
        communications = new Communications(id, comm_method_para, console);
        console.log(this.name + " was initialised");
    }
    public void set_connection_mode(string mode_para)
    {
        if (mode_para == "all")
        {
            List<Agent> available_connections = communications.Scan_network(network_obj);
            foreach (Agent agent in available_connections)
            {
                communications.connect(agent);
            }
        }
        else if (mode_para == "random")
        {
            List<Agent> available_connections = communications.Scan_network(network_obj);
            System.Random random = new System.Random();
            int random_number_of_connections = random.Next(0, available_connections.Count - 1);
            List<int> used_up = new List<int>();
            for (int i = 0; i < random_number_of_connections; i++)
            {
                bool pass = false;
                while (pass != true) {
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
    public void parse_chain()
    {
        console.log("Agent"+id+"'s blockchain info");
        for (int i = 1; i < communications.block_chain.Count<Block>();i++)
        {
            string xml_data = communications.block_chain[i].data;
            XDocument data = XDocument.Parse(xml_data);
            console.sub(data.Root.Element("Type").Value + "=>"+ data.Root.Element("Content").Value);

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

    private Dictionary<int, string> crypto_public = new Dictionary<int, string>();
    private string crypto_private;

    private Console console;
    private Block[] blockchain;
    public Block[] block_chain { get { return blockchain; } }
    private List<int[]> blockchain_node_tracker;
    private Blockchain block_class;

    public Communications(int agent_id_para, string comm_method_para, Console console_reference)
    {
        id = agent_id_para;
        comm_method = comm_method_para;
        console = console_reference;
        self_link = new Link(id, comm_method, this);
        block_class = new Blockchain();
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
    //public void attatch_key(int id_para,string key)
    //{
    //    crypto_public.Add(id_para, key);
    //}
    public void attach_blockchain(Block[] block_c)
    {
        blockchain = block_c; // with genesis block
        blockchain_node_tracker = new List<int[]>() { new int[] { 0 } };
    }

    //public void share_bc(Agent target_agent)
    //{

    //}

    //public Dictionary<string, Block[]> accept_bc(Block[] incoming_bc)
    //{
    //    /*
    //    Dictionary<string, Block[]> return_handle = new Dictionary<string, Block[]>();
    //    string handle = "null";
    //    int code = block_class.check_chain(incoming_bc);
    //    if(code == 0)
    //    {
    //        int code2 = block_class.compare_full(incoming_bc, blockchain);
    //        if(code2 == 0) { //possible update

    //        }
    //    }
    //    return return_handle; */

    //    //Recognise the change, whether it as a simple block update, and do any merge requests, by re-arranging the blocks' index according to timestamp. if identical, this instance will put it on last.
    //    //This will return the store and return the modofied chain with a code for the initial sender to recognoise and update their chain in accordance to the one provided.


    //}
    public void distribute_command(string type, string command)
    {
        XElement block_xml = new XElement("Data",
        new XElement("Type",type),
        new XElement("Content",command)
        );

        Block dist_block = new Block(DateTime.Now, block_xml.ToString());
        blockchain = block_class.add_Block(blockchain, dist_block.data, dist_block.time_stamp,dist_block.Hash);
        List<int> temp;
        foreach (KeyValuePair<int,Link> a in active_connections)
        {
            temp = new List<int>() { a.Key };
            blockchain_node_tracker.Add(temp.ToArray<int>());
            share_block(dist_block, a.Key);
            
        }
        
        
        

    }
    public void share_variable(String[] variable)
    {
        
        distribute_command("variable", variable[0] + "=" + variable[1] + ";" + id);
    }
    public void share_command(String[] command)
    {
        distribute_command("command", command[0] + ";" + command[1] + ";" + id);
    }
    public void share_block(Block share,int target_id)
    {
        Link target_link = active_connections[target_id];
        target_link.self.accept_block(share, id);
    }
    public void accept_block(Block shared,int node_share_id)
    {
        //if(blockchain_node_tracker.Count > shared.index)
        //{
        //    //if (!blockchain_node_tracker[shared.index].Contains<int>(node_share_id))
        //    //{
        //    //    List<int> temp = blockchain_node_tracker[shared.index].ToList<int>();
        //    //    temp.Add(node_share_id);
        //    //    blockchain_node_tracker[shared.index] = temp.ToArray();
        //    //}

        //    //List<int> block_senders = blockchain_node_tracker[shared.index].ToList<int>();
        //    //List<int> connections = active_connections.Keys.ToList<int>();
        //    //bool identical = block_senders.All(connections.Contains) && block_senders.Count == block_senders.Count;
        //    //if(identical == true)
        //    //{
        //    //    //add to blockchain

        //    //}
        //    Block savedBlock = blockchain[shared.index];
        //    if(savedBlock)


        //} else
        //{
        //    blockchain_node_tracker[shared.index] = new int[node_share_id];
        //    blockchain = block_class.attatch_block(blockchain, shared);
        //    distribute(shared,blockchain_node_tracker[shared.index]);
        //}

        string block_hash = shared.Hash;
        Dictionary<int, string> compare = new Dictionary<int, string>();
        Array.ForEach<Block>(blockchain, x => compare.Add(x.index, x.Hash));
        if (!compare.Values.Contains<string>(block_hash))
        {
            blockchain = block_class.add_Block(blockchain, shared.data, shared.time_stamp, shared.Hash);
            blockchain_node_tracker.Add(new int[] { node_share_id });
            apply_info(shared.data);
            distribute(shared, blockchain_node_tracker[blockchain.Count<Block>() - 1], blockchain.Count<Block>() - 1);
        }
        else
        {
            int id_c = get_id(compare, block_hash);
            if (id_c != 0)
            {
                List<int> temp = blockchain_node_tracker[id_c].ToList<int>();
                temp.Add(node_share_id);
                blockchain_node_tracker[id_c] = temp.ToArray<int>();
                distribute(shared, blockchain_node_tracker[id_c],id_c);

            }
        }
    }
    private void apply_info(string data)
    {
        Debug.Log("CURRENT AGENT" + id);
        XDocument command = XDocument.Parse(data);
        Debug.Log(command);
        Debug.Log("%%%%%%%%%%%%%%");
    }
    public void distribute(Block block,int[] track,int b_n_tid)
    {
        List<int> targets = active_connections.Keys.ToList<int>();
        List<int> track_list = track.ToList<int>();
        foreach(int id_c in track)
        {
            targets.Remove(id_c);
        }
        List<int> temp;
        foreach (int id_c in targets)
        {
            temp = blockchain_node_tracker[b_n_tid].ToList<int>();
            temp.Add(id_c);
            blockchain_node_tracker[b_n_tid] = temp.ToArray<int>();
            share_block(block, id_c);
            
            
            
        }
        
    }
    private int get_id(Dictionary<int,string> compare, string block_hash)
    {
        foreach (KeyValuePair<int, string> x in compare)
        {
            if (x.Value == block_hash)
            {
                return x.Key;
            }
        }
        return 0;
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
