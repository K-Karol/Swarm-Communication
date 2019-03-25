using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
public class ParameterExecution : MonoBehaviour {

    private int number_of_agents;
    private string comm_method;

    private Console console;  //reference to the debug console

    public UnityEngine.Object agent_prefab; //all agents will be spawned as a seperate gameobject, or thread tbh.
    public GameObject network_obj;  //All agents will be children of this obj.

    private Text agent_text;
    private Text comm_text;

    public bool init_done = false;

    public Dictionary<int, GameObject> agents_dict = new Dictionary<int, GameObject>(); //Keep track of all dynamically generated agents in a dict

    public void change_scene(int number_of_agents_para,string comm_method_para) //Upon calling this, it will load itself into the next scene and execute the simulation
    {
        number_of_agents = number_of_agents_para;
        comm_method = comm_method_para;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Loading execution screen");
        SceneManager.LoadScene("execution");
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "execution" && init_done == false)
        {
            init_done = true;
            initialise();    //once the scene is changed, it will call the init method
        }
    }


    void initialise()
    {
        //getting objects / classess. Mostly validation and destorying this gameobject to prevent any bad crashes
        this.name = "ExecutionManager";
        network_obj = GameObject.Find("network");
        if(network_obj == null)
        {
            Debug.LogError("Network obj not found");
            Destroy(this.gameObject);
        }

        console = GameObject.Find("Debug").GetComponent<Console>();
        if (console == null)
        {
            Debug.LogError("debug obj/class console not found");
            Destroy(this.gameObject);
        }

        agent_text = GameObject.Find("agentNO").GetComponent<Text>();
        if (agent_text == null)
        {
            Debug.LogError("agent_text obj/text class not found");
            Destroy(this.gameObject);
        }
        agent_text.text = number_of_agents.ToString();

        comm_text = GameObject.Find("commMO").GetComponent<Text>();
        if (comm_text == null)
        {
            Debug.LogError("comm_text obj/text class not found");
            Destroy(this.gameObject);
        }
        comm_text.text = comm_method.ToString();

        console.log("All variables initialised");

        for (int i = 1; i <= number_of_agents; i++) //will generate all agent gameobjects
        {
            agents_dict.Add(i, Instantiate(agent_prefab, network_obj.transform) as GameObject);
            Agent temp = agents_dict[i].GetComponent<Agent>();
            temp.initialise(i,comm_method,network_obj);
        }

    }
    public void network_Connect(string method_para)
    {
        if(method_para == "random")
        {
            foreach (var agent in agents_dict)
            {
                Agent agent_class = agent.Value.GetComponent<Agent>();
                agent_class.set_connection_mode("random");
            }
        }
        else if(method_para == "all")
        {
            foreach(var agent in agents_dict)
            {
                Agent agent_class = agent.Value.GetComponent<Agent>();
                agent_class.set_connection_mode("all");
            }
        }
        else
        {
            console.log("Command does not exist");
        }
    }
	
}
