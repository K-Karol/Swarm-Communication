using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParameterManager : MonoBehaviour {
    private int number_of_agents;
    private bool agents_pass = false;   //bool used later in if loop to make sure the input is validated/exists

    private string[] communication_methods = new string[] { "Mesh" };   //Different communication methods. Might have to delete this
    private string comm_method;  //stores the current method
    private bool method_pass = false;   //used in validation

    public InputField agent_input;  //UI input from inspector
    public Dropdown communication_dropdown; //UI input from inspector

    private ParameterExecution para_class; //Gameobject with the ParamaterExecution script will be passed onto the next scene. reference to the class 
    private GameObject para_obj;    //Will hold reference to the generated GameObject

    public UnityEngine.Object para_prefab; //Gameoject prefab with the ParameterExecution class attatched


    // Use this for initialization
    void Start () {
        if (para_prefab == null)
        {
            Debug.LogError("Parameter Execution prefab not passed through the inspector");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Execute()   //called from execution button from GUI
    {
        if (Int32.TryParse(agent_input.text, out number_of_agents)) //input validation
        {
            agents_pass = true;
            Debug.Log("Agent number was parsed succesfully");

        }
        else
            Debug.LogError("Agent number could not be parsed");

        comm_method = communication_methods[communication_dropdown.value];
        method_pass = true;

        if (agents_pass == true && method_pass == true)
        {
            Debug.Log("Data validated");

            //init object which will be passed onto the next scene
            para_obj = Instantiate(para_prefab, this.transform) as GameObject;
            para_obj.transform.parent = null;
            para_class = para_obj.GetComponent<ParameterExecution>();
            para_class.change_scene(number_of_agents,comm_method);
        }
    }
}
