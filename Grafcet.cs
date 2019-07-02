/*
 * NN Gerard: In this version, I modify the way of static matrices generation to render them logic and easy to modify
 * It is necessary for me to understand carefully the 
 * project by correcting some conception and implementation problems in some classes 
 * Here, time conditions are also managed ...
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GrafcetConvertor;
using System.Windows.Forms;
using System.Timers;
using GrafcetConvertor;


namespace GrafcetConvertor
{
    /// <summary>
    /// Define all the codes used when coding Grafcet elements
    /// </summary>
    public class CodingGrafcetElements
    {
        public static int RISING_EDGE_NUMBER = 0; // Moves from 0 to the number of RE encountered in all the variables
        //Operators cge (CodingGrafcetElement)
        public static string cge_op_OR = "0000";                                            //0b 0000 0000 0000 0000
        public static string cge_op_AND = "0010"; //Really 1000 --> 10 00 --> 00 10 -->0010 //0b 0001 0000 0000 0000
        public static string cge_op_NOT = "0020"; //Really 2000                             //0b 0002 0000 0000 0000
        public static string cge_op_RISING_EDGE = "0030"; //Rising edge: //Really 3000       //0b 0003 0000 0000 0000
        public static string cge_op_TRUE = "0040"; //Really 4000 : It is not an operator, but a constant. But to facilitate its use we consider it to be an operator

        //Variables : Input, Output, Step, Timing
        public static string cge_var_3bits_begin_input = "100"; //0b 1000 0000 NNNN NNNN
        public static string cge_var_3bits_begin_output = "101"; //0b 1010 0000 NNNN NNNN
        public static string cge_var_3bits_begin_step = "110"; //0b 1100 0000 000N NNNN
        public static string cge_var_3bits_begin_timing = "111"; //0b 111S SSSS DDDD DDDD

        //Number of digits for coding
        public static int cge_digits_num_input = 8; //Number of digits on which an input id is coded
        public static int cge_digits_num_output = 8; //Number of digits on which an output id is coded
        public static int cge_digits_num_step = 5; //Number of digits on which step id is coded
        public static int cge_digits_num_RE_NUM = 8; //Number of digits on which a rising edge number is coded

        //Digits on Timing variables 
        public static int cge_digits_duration = 8; //Number of digits on which step id is coded

        //Base begin to code variables for 
        public static int base_input_vars_number = 10;
    }
    /// <summary>
    /// The class defining all the manipulations inside a Grafcet
    /// </summary>
    public class GrafcetClass
    {
        //////////////////////////PARTIE DECLARATION DES DONNEES\\\\\\\\\\\\\\\\\\\\\\\\\\
        /// <summary>
        /// Conains input and output (action) variables
        /// </summary>
        internal List<Variable11> InputOutput_list; // list of all the variables : actions variables and non action variables

        internal List<Step11> Step_list;
        internal List<Transition11> Transition_list;

        //some numbers
        public int number_transitions;
        public int number_steps;
        public int number_actions;
        public int number_inputs; // number of variables in input (simple variables)
        public int number_time_vars;

        ////////////////
        public bool hasTimeCondition;
        //internal List<TimeCondition> timeConditionsList; // To contain time conditions list associated to steps

        //GRAFCET PARAMETERS

        private List<Variable11> Actions_list;
        private List<Variable11> Input_list;
        private List<Variable11> TimeVar_list;
        public List<string> VarTrans_name; // variables used in transitions

        public List<string> actions_names; // contains the names of actions (output variables)
        public List<string> input_names; // contains the names of input variables
        public List<string> vars_to_delete; //contains names of useless and deleted variables
        public string[] time_var_names; // contains the names of input variables

        public List<string> VarTrans_Microcontroller_code;
        public string[] ParseTransCondition;
        public string PICMessage;
        public string DataInEE_array; // An array used in the MPLAB C code
        public string fileNameOnly;
        public string directory_name;
        public string file_name_full;
        private StreamWriter fluxWrite_DynamicLog;
        public string g7_text_struct;
        public bool firstStepOfDynamicEvolution;
        public int clockNumber; // to count the number of clock events in dynamic evolution
        public static int simulation_g7_clock_interval = 2000;
        public static int g7_Board_TimeUnit_TimerStepActivity; //= 1000; //time_unit_of_conditions_comboBox1

        //For convergences and divergences
        private List<ConvDiv11> Simultaneous_Conv_list;
        private List<ConvDiv11> Selection_Div_list;
        private List<ConvDiv11> Simultaneous_Div_list;
        private List<ConvDiv11> Selection_Conv_list;

        //DECLARATION DES MATRICES STATIQUES
        public int[] INIT;
        public int[,] E;
        public int[,] S;
        public int[,] MA;
        public int[,] DELAY_STEPS_MATRIX; // that contains delay durations of steps in transitions
        public List<string>[] RS;
        public List<string>[] RS_Microcontroller;

        //DECLARATION DES VECTEURS DYNAMIQUES
        public int[] INITACTUEL;
        public int[] X_1;
        public int[] X_1old;
        public int[] X_2;
        public int[] FT_old;
        public int[] VT1;
        public int[] VT_new;
        public int[] R1;
        public int[] FT1;
        public int[] VA1; //Vecteur Activation
        public int[] VD1; //Vecteur Desactivation
        public int[] VA;  //Vecteur Activation
        public int[] VD;  //Vecteur Desactivation
        public int[] AD;
        public int[] DX;
        public int[] VDbar;
        public int[] O;
        public int[] Rdyn; // contains the values of variables used in transitions expressions
        private bool g7_has_stable_situation;

        public GrafcetClass()
        {
            this.InitVariables();
        }
        ~GrafcetClass()
        {
            try
            {
                this.fluxWrite_DynamicLog.Dispose();
                this.fluxWrite_DynamicLog.Close();
            }
            catch (ObjectDisposedException ode) { Console.WriteLine(ode.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private void InitVariables()
        {
            // Initializing of the steps and transitions lists
            Step_list = new List<Step11>();
            Transition_list = new List<Transition11>();
            InputOutput_list = new List<Variable11>();
            Actions_list = new List<Variable11>();
            Input_list = new List<Variable11>();
            VarTrans_name = new List<string>();
            VarTrans_Microcontroller_code = new List<string>();
            this.TimeVar_list = new List<Variable11>();

            // for simultaneous and selection convergences and divergences
            this.Simultaneous_Conv_list = new List<ConvDiv11>();
            this.Selection_Div_list = new List<ConvDiv11>();
            this.Simultaneous_Div_list = new List<ConvDiv11>();
            this.Selection_Conv_list = new List<ConvDiv11>();

            Step11.COUNT_STEPS = 0;
            Transition11.COUNT_TRANS = 0;
            Variable11.COUNT_VARS = 0;
        }


        public void ReadUniSimXML2(Stream myStream)
        {
            this.InitVariables();

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.Load(myStream);
            }
            catch (Exception e)
            {
                MessageBox.Show("XML Error: " + e.Message);
            }
            //building of variables list (input variables actions and non action variables)
            XmlNodeList xmlVariableslist = xmldoc.GetElementsByTagName("variable");
            foreach (XmlNode Xmlvariablenode in xmlVariableslist)
            {
                Variable11 aVar = new Variable11(Xmlvariablenode);
                bool var_exist = false;
                foreach (Variable11 exist_var in this.InputOutput_list) {
                    if (exist_var.name.Equals(aVar.name)) {
                        var_exist = true;
                    }
                }
                if (!var_exist)
                {
                    this.InputOutput_list.Add(aVar);
                }
                else {
                    MessageBox.Show("The following variable already exists. It cannot be duplicated ! \n" + aVar.name);
                }
                //Console.WriteLine("A new variables : " + aVar.name);
            }

            // selection of all the elements in the SFC 
            XmlNodeList xmlnodelist = xmldoc.GetElementsByTagName("SFC");

            // NNG: we first build the list of steps and transitions before continuing by the rest : simultaneousConvergences, simultaneousDivergences, selectionConvergences and selectionDivergences
            if (xmlnodelist.Count == 0)
            {
                MessageBox.Show("Your grafcet does not contain steps and transitions");
            }
            else
            {
                foreach (XmlNode Xmlnode in xmlnodelist[0].ChildNodes)
                {
                    switch (Xmlnode.Name)
                    {
                        // RECUPERATION DES DONNEES Pour les steps
                        case "step":
                            Step_list.Add(new Step11(Xmlnode));
                            break;

                        // RECUPERATION DES DONNEES Pour les transitions
                        case "transition":
                            Transition_list.Add(new Transition11(Xmlnode));
                            break;

                        // RECUPERATION DES DONNES POUR LES CONVERGENCES EN "ET"
                        case "simultaneousConvergence":
                            //SFC10.Conv_connectionList(Xmlnode, Transition_list.Cast<SFC10>().ToList());
                            this.Simultaneous_Conv_list.Add(new ConvDiv11(Xmlnode));
                            break;

                        //RECUPERATION DES DONNES POUR LES DIVERGENCES EN "ET"
                        case "simultaneousDivergence":
                            //SFC10.Div_connectionList(Xmlnode, Transition_list.Cast<SFC10>().ToList());
                            this.Simultaneous_Div_list.Add(new ConvDiv11(Xmlnode));
                            break;

                        // RECUPERATION DES DONNES POUR LES CONVERGENCES EN "OU"    
                        case "selectionConvergence":
                            //SFC10.Conv_connectionList(Xmlnode, Step_list.Cast<SFC10>().ToList());
                            this.Selection_Conv_list.Add(new ConvDiv11(Xmlnode));
                            break;

                        // RECUPERATION DES DONNES POUR LES DIVERGENCES EN "OU"
                        case "selectionDivergence":
                            //SFC10.Div_connectionList(Xmlnode, Step_list.Cast<SFC10>().ToList());
                            this.Selection_Div_list.Add(new ConvDiv11(Xmlnode));
                            break;

                        //RECUPERATION DES DONNES POUR LES ACTIONBLOCK
                        case "actionBlock":
                            this.addActionsInABloc(Xmlnode);
                            break;

                        default:
                            break;
                    }
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////
            // To create some link between element to facilitate the matrix generation

            // Case 1: 
            //There could be link as :  Step ==> selectionDivergence ==> simultaneousConvergence   
            //They should become     :  Step ==> simultaneousConvergence
            foreach (ConvDiv11 sim_conv in this.Simultaneous_Conv_list)
            {
                for (int i = 0; i < sim_conv.RefLocalIdList.Count; i++)
                {
                    int refId = sim_conv.RefLocalIdList[i];
                    if (this.getStepIndexByLocalId(refId) < 0)
                    {
                        //MessageBox.Show("There is : Step ==> selectionDivergence ==> simultaneousConvergence");
                        int stepLocalId = this.getStepLocalIdBy_SelectionDivergeceId(refId);
                        sim_conv.RefLocalIdList[sim_conv.RefLocalIdList.IndexOf(refId)] = stepLocalId;
                    }
                }
            }

            // Case 2: 
            //There could be link as: Transition ==> simultaneousDivergence ==> SelectionConvergence
            //They should become    :  Transition ==> SelectionConvergence
            foreach (ConvDiv11 select_conv in this.Selection_Conv_list)
            {
                int refId = select_conv.RefLocalIdList[0];
                if (this.getTransitionIndexByLocalId(refId) < 0)
                {
                    //MessageBox.Show("There is: Transition ==> simultaneousDivergence ==> SelectionConvergence")
                    int transitionLocalId = this.getTransitionLocalIdBy_simultaneousDivergenceId(refId);
                    select_conv.RefLocalIdList[0] = transitionLocalId;
                }
            }

            // End of necessary link creation

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            this.prepare_static_dynamic_Matrix_generation();
        }

        private void prepare_static_dynamic_Matrix_generation()
        {

            // (Here the variable list contains only input variables (a, b, ...) and actions variables (A, B, ...) that are conventionnaly in upper case)
            int numbering_input_vars = 0;
            int numbering_action_vars = 0;

            //Numbering input and output variables
            foreach (Variable11 aVar in this.InputOutput_list)
            {
                if (aVar.IsAnAction)
                {
                    aVar.number = numbering_action_vars;
                    this.Actions_list.Add(aVar);
                    numbering_action_vars++;
                    //MessageBox.Show(aVar.name+" : Is An Action");
                }
                else //Here this variable is an input one
                {
                    aVar.number = numbering_input_vars;
                    this.Input_list.Add(aVar);
                    numbering_input_vars++;
                    //MessageBox.Show(aVar.name + " : Is An Input");
                }
            }

            //VarTrans_name is usefull for the evaluation of transitions
            //VarTrans_name = new List<string>();
            //VarTrans_Microcontroller_code = new List<string>();

            //adding variables in some Lists , even the Rdyn array
            //Order of variables in those vectors is :
            //> 1: input variable
            //> 2: action variable
            //> 3: step variables
            //> 4: timing variables

            //adding variables in some Lists   **  1: input variable
            this.number_inputs = this.Input_list.Count;
            this.number_actions = this.Actions_list.Count;
            //this.input_var_names = new string[this.number_input_vars];
            this.input_names = new List<string>();
            for (int input_i = 0; input_i < this.Input_list.Count; input_i++)
            {
                this.input_names.Add(Input_list[input_i].name);
                this.VarTrans_name.Add(this.input_names[input_i]);
            }

            //adding variables in some Lists   **  2: action/output variable

            this.number_actions = this.Actions_list.Count;

            this.actions_names = new List<string>();
            for (int action_i = 0; action_i < this.number_actions; action_i++)
            {
                this.actions_names.Add(this.Actions_list[action_i].name);
                this.VarTrans_name.Add(this.actions_names[action_i]);
            }

            //Delete all the useless variables

            //Delete useless variables in InputVar_list
            //Tokens used to split transitions in witch to split: ], {, ( ), +, *, ^, !, >
            char[] separator = { ']', '{', '(', ')', '+', '*', '^', '!', '>' };
            vars_to_delete = new List<string>();
            List<Variable11> vars_objects_to_del = new List<Variable11>();

            foreach (Variable11 input_object in this.Input_list)
            {
                string aVar_Name = input_object.name;
                bool that_variable_is_used = false;
                if (this.actions_names.IndexOf(aVar_Name) >= 0)
                {   //aVar_Name is an action
                    that_variable_is_used = true;
                }
                else {
                    //aVar_Name is not an action
                    foreach (Transition11 transition in this.Transition_list)
                    {
                        string condition = transition.Condition;
                        string[] elements = condition.Split(separator);
                        //MessageBox.Show("Index of the var in actions_names. " + aVar_Name + " :  " + this.actions_names.IndexOf(aVar_Name));
                        //MessageBox.Show("Variables to check " + aVar_Name);
                        for (int i = 0; i < elements.Length; i++)
                        {
                            if (elements[i].Equals(aVar_Name))
                            {
                                that_variable_is_used = true;
                                break;
                            }
                        }
                    }
                }
                //Decision
                if (!that_variable_is_used)
                {
                    vars_objects_to_del.Add(input_object);
                }
            }

            if (vars_objects_to_del.Count > 0) {
                string deletedVars = "\nThe following variables were declared in the grafcet model but not used.\nThey have been deleted :\n";
                foreach (Variable11 del_var_object in vars_objects_to_del)
                {
                    string varName_to_delete = del_var_object.name;
                    //MessageBox.Show("Deletion of variable :" + varName_to_delete);
                    deletedVars += "\t" + varName_to_delete + "\n";
                    //Deletion
                    this.InputOutput_list.Remove(del_var_object);
                    this.Input_list.Remove(del_var_object); //Not necessary !

                    this.input_names.Remove(varName_to_delete);
                    this.VarTrans_name.Remove(varName_to_delete);
                }
                MessageBox.Show(deletedVars);
                Console.WriteLine(deletedVars);
            }

            //initialize number of input variables
            this.number_inputs = this.input_names.Count;

            //reorder inputs and output variables process : to be implemented ...
            //Use the following method: Swap(List<string> list, int indexA, int indexB)
            
            this.ending_static_and_dynamic_initializations();
        }

        private void ending_static_and_dynamic_initializations()
        {
            //this function is usefull both in xml generation(prepare_static_dynamic_Matrix_generation) 
            //and g7text generation (Read_g7_TextStructure)
            // Parse time variables and analyse transitions conditions


            //adding variables in some Lists   **  3: step variables
            //NB: step variables are not in the list of variables Variable_list 
            this.number_steps = this.Step_list.Count;
            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                this.VarTrans_name.Add(Step_list[step_i].name);
            }


            //END 1, 2, 3: Call a function to perform add_IO_in_VarTrans_Microcontroller_code to : 
            /*
             Delete useless variables
             initialize number of input & output variables
             reorder inputs and output variables
            */

            this.add_IO_in_VarTrans_Microcontroller_code();

            //adding variables in some Lists   **  4: timing variables

            //parsing of transitions to identify and create time variables 
            foreach (Transition11 transition in this.Transition_list)
            {
                // VarTrans_name and VarTrans_Microcontroller_code are modified in this loop
                this.ParseTimeVariable(transition); // if there are times in conditions, create TimeConditions for it

                //Threat the case where there are [S1.X] : [S1.X] becomes S1
                transition.simplify_active_step_in_conditions();
            }


            this.number_time_vars = this.TimeVar_list.Count();
            this.time_var_names = new string[this.number_time_vars];

            for (int tv_i = 0; tv_i < this.number_time_vars; tv_i++)
            {
                this.time_var_names[tv_i] = this.TimeVar_list[tv_i].name;
            }

            int total_number_vars = this.VarTrans_name.Count;// same as the previous evaluation

            this.Rdyn = new int[total_number_vars];

            for (int i = 0; i < total_number_vars; i++)
            {
                Rdyn[i] = 0;
            }

            //MessageBox.Show("this.Transition_list : " + this.Transition_list.Count);

            //building of stack conditions for transitions
            foreach (Transition11 transition in this.Transition_list)
            {
                transition.analyzeTransitionCondition();
            }

            //Build RS matrix
            this.Build_RS_from_StackTransConditions();

            // generate steps delays matrix in transitions
            this.generate_delay_step_matrix();

            VA = new int[this.number_steps];
            VD = new int[this.number_steps];
            //X_old = new int[this.number_steps];
            O = new int[this.number_actions];
            for (int action_i = 0; action_i < number_actions; action_i++) { O[action_i] = 0; }

            //some initializations
            this.number_transitions = this.Transition_list.Count;

            // Gérard Nzebop : initialise the FT_old vector

            this.FT_old = new int[this.number_transitions];
            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                this.FT_old[trans_i] = 0;
            }
            //NNG : initialise the X_1old vector
            this.X_1old = new int[this.number_steps];
            for (int step_i = 0; step_i < number_steps; step_i++)
            {
                this.X_1old[step_i] = 0;
            }
            this.firstStepOfDynamicEvolution = true;

            // generate steps delays matrix in transitions
            this.generate_delay_step_matrix();

            Console.WriteLine("\nVariables names of transitions : ");
            for (int i = 0; i < this.VarTrans_name.Count; i++)
            {
                Console.WriteLine("variable " + i + " : " + VarTrans_name[i]);
            }
        }

        private void add_IO_in_VarTrans_Microcontroller_code()
        {
            //Add in VarTrans_Microcontroller_code: input variables,  action/output variables, step variables, 
            
            //adding variables in a List  **  1: input variable
            for (int input_i = 0; input_i < this.input_names.Count; input_i++)
            {
                string code_hex2 = VariableToHexString16(CodingGrafcetElements.cge_var_3bits_begin_input, input_i, CodingGrafcetElements.cge_digits_num_input);
                VarTrans_Microcontroller_code.Add(code_hex2);
            }

            //adding variables in a List  **  2: action/output variables
            for (int action_i = 0; action_i < this.number_actions; action_i++)
            {
                string code_hex2 = VariableToHexString16(CodingGrafcetElements.cge_var_3bits_begin_output, action_i, CodingGrafcetElements.cge_digits_num_output);
                VarTrans_Microcontroller_code.Add(code_hex2);
            }

            //adding variables in a List  **  3: step variables
            //NB: step variables are not in the list of variables Variable_list 
            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                string code_hex2 = VariableToHexString16(CodingGrafcetElements.cge_var_3bits_begin_step, step_i, CodingGrafcetElements.cge_digits_num_step);
                this.VarTrans_Microcontroller_code.Add(code_hex2);
            }

            //Display I/O variables
            string ch = "      Input Variables of the system \n******************************************************\n";
            //if(this.VarTrans_name.Count==this.VarTrans_Microcontroller_code.Count)
            for (int i = 0; i < this.input_names.Count; i++)
            {
                ch += "[" + i + "]: " + this.input_names[i] + " (Grafcet) <==> Code " + this.VarTrans_Microcontroller_code[i] + " (Microcontroller)\n";
            }
            ch += "\n      Output Variables of the system \n******************************************************\n";
            //if(this.VarTrans_name.Count==this.VarTrans_Microcontroller_code.Count)
            for (int i = 0; i < this.actions_names.Count; i++)
            {
                ch += "[" + i + "]: " + this.actions_names[i] + " (Grafcet) <==> Code " + (this.VarTrans_Microcontroller_code[i + this.input_names.Count]) + " (Microcontroller)\n";
            }

            MessageBox.Show(ch);
            Console.Write(ch);

        }
        public static void Swap(List<string> list, int indexA, int indexB)
        {
            string tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        /// <summary>
        /// NNG: This function analyseS the string to detect time conditions. If there is in the condition, it renames and create a TimeCondition for those variables
        /// </summary>
        /// <param name="condition">Non analyse ransition</param>
        private void ParseTimeVariable(Transition11 transition)
        {
            // timeConditions is to contain all the time conditions associated to this step. 
            //Exemple: If condition= a+[S0.t>5s]+[S5.t>2m] then we should have timeConditionsList=<[S0.t>5s],[S5.t>2m]>
            // and return a+_t_S0+_t_S5
            //Console.WriteLine("Transition old T"+transition.index+" : "+transition.Condition);
            int cond_index_begin;
            int cond_index_end; ;
            string stringCondition;
            string new_condition = transition.Condition;
            //string new_condition_Microcontroller = transition.Condition;

            int min_cond_lenght = "[S5.t>2m]".Length; // For [S5.t>2m] is the minimal expression on time conditions
            //Console.WriteLine("min_cond_lenght :" + min_cond_lenght);
            int range;
            do
            {
                cond_index_begin = new_condition.IndexOf('[');
                cond_index_end = new_condition.IndexOf(']');
                range = cond_index_end - cond_index_begin + 1;

                //bool cond2 = new_condition.Contains(">") || new_condition.Contains("<")
                if (range >= min_cond_lenght && new_condition.Contains(">"))
                {
                    //MessageBox.Show("old condition = " + transition.Condition);
                    if (!this.hasTimeCondition)
                    {
                        this.hasTimeCondition = true;
                    }
                    stringCondition = new_condition.Substring(cond_index_begin, range);

                    // to modify to take into consideration step S12 , S124, ...
                    int point_index = stringCondition.IndexOf('.');
                    string stepName = stringCondition.Substring(1, point_index - 1);
                    //MessageBox.Show("stringCondition = " + stringCondition + "  Step name = " + stepName);

                    string st_duration = stringCondition.Substring(point_index + 3, stringCondition.Length - (5 + stepName.Length));
                    int duration_ms = TimeCondition.getDurationInMilliseconds(st_duration);
                    // st_duration may be : 25s, 275ms, 2d, ...
                    //MessageBox.Show("TC : " + stringCondition + "\tStep : " + stepName + "\tDuration : " + st_duration + " = " + duration_ms + " ms");
                    int stepIndex = this.getStepIndexByName(stepName);
                    if (!(stepIndex < 0))
                    {
                        this.Transition_list[transition.index].addTransReferenceTimeCondition(stepIndex, duration_ms);
                        int micro_duration = (duration_ms/GrafcetClass.g7_Board_TimeUnit_TimerStepActivity);
                        //MessageBox.Show("micro_duration = " + micro_duration);
                        string Microcontroller_code = StepTimeToHexString16(CodingGrafcetElements.cge_var_3bits_begin_timing, stepIndex, micro_duration);
                        string time_variable_name = this.Step_list[stepIndex].time_cond.addTimeVariableName(transition.index, stepName, duration_ms);
                        this.Step_list[stepIndex].IsUsedInTimeConditions = true;

                        //MessageBox.Show("stringCondition : " + stringCondition + "\n Microcontroller_code :" + Microcontroller_code);

                        //add the new time variable
                        if (this.getVariableIndexbyName_in_VarTrans_name(time_variable_name) < 0)
                        {
                            // This time variable is new. create it and add 
                            Variable11 tv = new Variable11(time_variable_name);
                            this.TimeVar_list.Add(tv);

                            //Adding references to VarTrans_name and VarTrans_Microcontroller_code
                            this.VarTrans_name.Add(time_variable_name);

                            //Add Microcontroller_code to VarTrans_Microcontroller_code list
                            this.VarTrans_Microcontroller_code.Add(Microcontroller_code);
                        }

                        //Update the condition expresssion
                        new_condition = new_condition.Replace(stringCondition, time_variable_name);

                        //new_condition_Microcontroller = new_condition_Microcontroller.Replace(stringCondition, time_variable_name);

                        // indicate that this step is using a time condition
                        if (!this.Step_list[stepIndex].IsUsedInTimeConditions)
                        {
                            this.Step_list[stepIndex].IsUsedInTimeConditions = true;
                            Console.WriteLine("\tNew TV for step S" + stepIndex);
                        }
                        Console.WriteLine("\t" + time_variable_name + " created  !!!\n");
                    }
                    else
                    {
                        string msg = "File Input Error : Step " + stepName + " does not exists";
                        MessageBox.Show(msg);
                        throw new Exception(msg);
                    }
                }

                //MessageBox.Show("new condition : " + new_condition);

                cond_index_begin = new_condition.IndexOf('[');
                cond_index_end = new_condition.IndexOf(']');
                range = cond_index_end - cond_index_begin + 1;

            } while (range >= min_cond_lenght && new_condition.Contains(">"));

            transition.Condition_parsed = new_condition;
            //transition.Condition_new_Microcontroller = new_condition_Microcontroller;
            //Console.WriteLine("Transition new T" + transition.index + " : " + transition.Condition_new);
            Console.WriteLine("Condition new : " + new_condition);
        }

        private int getStepIndexByName(string stepName)
        {
            int stepIndex = -1;
            foreach (Step11 step in this.Step_list)
                if (step.name.Equals(stepName))
                {
                    stepIndex = step.index;
                    break;
                }
            return stepIndex;
        }

        private void addActionsInABloc(XmlNode XmlActionBlocNode)
        {
            int step_ID = Int32.Parse(XmlActionBlocNode.FirstChild.NextSibling.LastChild.Attributes[0].Value);
            int numberActionInNode = XmlActionBlocNode.ChildNodes.Count - 2;

            foreach (XmlNode XmlActionNode in XmlActionBlocNode.ChildNodes)
            {
                if (XmlActionNode.Name.Equals("action"))
                {
                    string ActionName = XmlActionNode.LastChild.Attributes[0].Value;
                    // find the action that has this name
                    foreach (Variable11 aVariable in this.InputOutput_list)
                    {
                        if (aVariable.name.Equals(ActionName))
                        {
                            if (!aVariable.IsAnAction) // because it could already be set to be an action
                                aVariable.IsAnAction = true;
                            aVariable.RefLocalIdSteps.Add(step_ID);
                            aVariable.durations_names.Add(XmlActionNode.Attributes[0].Value);
                        }
                    }
                }
            }
            //if(numberActionInNode == countActions) MessageBox.Show("New Action Bloc Good !");
        }

        // search the step index knowing its localId
        private int getStepIndexByLocalId(int Step_LocalId)
        {
            int stepIndex = -1;
            foreach (Step11 step in this.Step_list)
                if (step.LocalId == Step_LocalId)
                {
                    stepIndex = step.index;
                    break;
                }
            return stepIndex;
        }
        // search the transition index knowing its localId
        private int getTransitionIndexByLocalId(int LocalId_Transition)
        {
            int transitionIndex = -1;
            foreach (Transition11 transition in this.Transition_list)
                if (transition.LocalId == LocalId_Transition)
                {
                    transitionIndex = transition.index;
                    break;
                }
            return transitionIndex;
        }

        public void init_file_location(string dir, string file, string ext)
        {
            this.directory_name = dir;
            this.fileNameOnly = file;
            this.file_name_full = this.directory_name + "\\" + this.fileNameOnly + ext;
        }

        public void create_StreamFile_for_DynamicTimeVarEvents()
        {
            //create a fluxWrite for time condition events
            string strFileName = this.directory_name + "\\" + this.fileNameOnly + "_g7_DynamicTimeVarEvents.txt";
            try
            {
                this.fluxWrite_DynamicLog = new StreamWriter(strFileName);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void finalize_DynamicTimeVarEvents()
        {
            this.fluxWrite_DynamicLog.Flush();
        }

        public void GenerateStaticMatrices()
        {
            ////////////////////////// PARTIE CREATION DES MATRICES \\\\\\\\\\\\\\\\\\\\\\\\\\
            // Création du vecteur ligne INIT
            this.INIT = new int[number_steps];

            for (int step_i = 0; step_i < number_steps; step_i++)
            {
                this.INIT[step_i] = this.Step_list[step_i].isInitialStep ? 1 : 0;
            }

            // REMPLISSAGE DES MATRICES

            //fInitializing the MA matrix of actions
            this.MA = new int[this.number_actions, this.number_steps];
            for (int action_num_i = 0; action_num_i < this.number_actions; action_num_i++)
                for (int step_i = 0; step_i < this.number_steps; step_i++)
                    this.MA[action_num_i, step_i] = 0;

            // complete the MA matrix
            foreach (Variable11 action in this.Actions_list)
            {
                foreach (int stepId in action.RefLocalIdSteps)
                {
                    int step_index = this.getStepIndexByLocalId(stepId);
                    if (step_index >= 0)
                        this.MA[action.number, step_index] = 1;
                }
            }

            //Pour la matrice E des étapes d'entrées des transitions et la matrice S des étapes de sortie des transitions

            // Initialize E ans S to 0
            this.E = new int[number_transitions, number_steps];
            this.S = new int[number_transitions, number_steps];

            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
                for (int step_i = 0; step_i < number_steps; step_i++)
                {
                    this.E[trans_i, step_i] = 0;
                    this.S[trans_i, step_i] = 0;
                }

            // Threatment of simple connections between steps and transitions
            foreach (Transition11 trans in Transition_list)
                foreach (Step11 step in Step_list)
                {
                    // Simple connection from transitions to steps 
                    if (step.RefLocalId == trans.LocalId)
                        S[trans.index, step.index] = 1;
                    // Simple connection from steps to transitions
                    if (trans.RefLocalId == step.LocalId)
                        E[trans.index, step.index] = 1;
                }

            // THREATMENT OF simultaneous_convergences and selection_divergences
            // threatment of simultaneous_convergences
            foreach (ConvDiv11 sim_conv in this.Simultaneous_Conv_list)
            {
                // look for the index transition of THE downstream transition
                int trans_i = 0, index_downstream_transition = -1;
                bool find_transition = false;
                while (!find_transition && trans_i < this.number_transitions)
                {
                    if (this.Transition_list[trans_i].RefLocalId == sim_conv.LocalId)
                    {
                        index_downstream_transition = this.Transition_list[trans_i].index;
                        find_transition = true;
                    }
                    else
                        trans_i++;
                }
                // here the index_downstream_transition is known
                // look for the steps in input of this transition
                foreach (int refLocalId_step in sim_conv.RefLocalIdList)
                {
                    int step_index = this.getStepIndexByLocalId(refLocalId_step);
                    if (step_index >= 0)
                        E[index_downstream_transition, step_index] = 1;
                    else
                    {
                        // The execution is supposed to not pass here because after the reading of XML elements, some usefull link have been added :
                        // Step ==> selectionDivergence ==> simultaneousConvergence   has become  Step ==> simultaneousConvergence 

                        string message = "Step not found !! LocalID=" + refLocalId_step + " in input of the transition with index=" + index_downstream_transition;
                        // It may be it is a selectionDivergence (as in the case of the grafcet grafcet_with_elements.xml)
                        // Step ==> selectionDivergence ==> simultaneousConvergence   should become  Step ==> simultaneousConvergence 
                        int new_step_localId = this.getStepLocalIdBy_SelectionDivergeceId(refLocalId_step);
                        step_index = this.getStepIndexByLocalId(new_step_localId);
                        E[index_downstream_transition, step_index] = 1;

                        Console.Write("\n" + message + " \nBut the problem is solved");
                        MessageBox.Show(message + " \nBut the problem is solved");
                    }
                }
            }

            // threatment of selection_divergences
            foreach (ConvDiv11 select_div in this.Selection_Div_list)
            {
                if (select_div.RefLocalIdList.Count == 1)
                {
                    int index_upStreamStep = this.getStepIndexByLocalId(select_div.ReflocalId);
                    if (index_upStreamStep >= 0)
                    {
                        foreach (Transition11 transition in this.Transition_list)
                            if (transition.RefLocalId == select_div.LocalId)
                            {
                                E[transition.index, index_upStreamStep] = 1;
                                //break; // Just one transition should satisfy this condition
                            }
                    }
                }
                else
                    MessageBox.Show("ERROR: A selection divergence has more than one element in input. Impossible !!!! ");
            }

            // THREATMENT OF simultaneous_divergences and selection_convergences
            // threatment of simultaneous_divergences
            foreach (ConvDiv11 sim_div in this.Simultaneous_Div_list)
            {
                if (sim_div.RefLocalIdList.Count == 1)
                {
                    int Index_upStreamTransition = this.getTransitionIndexByLocalId(sim_div.ReflocalId);
                    if (Index_upStreamTransition >= 0)
                    {
                        foreach (Step11 step in this.Step_list)
                            if (step.RefLocalId == sim_div.LocalId)
                                S[Index_upStreamTransition, step.index] = 1;
                    }
                }
            }

            // threatment of selection_convergences
            foreach (ConvDiv11 select_conv in this.Selection_Conv_list)
            {
                int step_i = 0, Index_downstreamStep = -1;
                bool step_found = false;
                while (!step_found && step_i < this.number_steps)
                {
                    if (this.Step_list[step_i].RefLocalId == select_conv.LocalId)
                    {
                        Index_downstreamStep = this.Step_list[step_i].index;
                        step_found = true;
                    }
                    else
                        step_i++;
                }
                //here the Index_downstreamStep is known
                //look for the transitions in input of this step
                foreach (int LocalId_transition in select_conv.RefLocalIdList)
                {
                    int index_transition = this.getTransitionIndexByLocalId(LocalId_transition);
                    if (index_transition >= 0)
                        S[index_transition, Index_downstreamStep] = 1;
                    else
                    {
                        // The execution is supposed to not pass here because after the reading of XML elements, some usefull link have been added :
                        // Transition ==> simultaneousDivergence ==> SelectionConvergence has become  Transition ==> SelectionConvergence
                        string messageErr = "Transition not found !! LocalID = " + LocalId_transition + " in output of the step with index=" + Index_downstreamStep;
                        // Add code to solve the problem. may be it is a simultaneousDivergence (as the case of the grafcet grafcet_with_elements.xml)
                        //  Transition ==> simultaneousDivergence ==> SelectionConvergence should become Transition ==> SelectionConvergence
                        int new_transition_localId = this.getTransitionLocalIdBy_simultaneousDivergenceId(LocalId_transition);
                        index_transition = this.getTransitionIndexByLocalId(new_transition_localId);
                        S[index_transition, Index_downstreamStep] = 1;
                        string print_mess = "\n" + messageErr + " \nFATAL ERROR";
                        Console.Write(print_mess);
                        MessageBox.Show(print_mess);
                    }
                }
            }

            this.InitializeINITACTUEL();
            this.evaluate_next_validatedTransitions2(); // usefull for time conditions
            this.Build_RS_from_StackTransConditions();

            this.clockNumber = 0; // to count the number of cloc
        }

        private void generate_delay_step_matrix()
        {
            this.DELAY_STEPS_MATRIX = new int[this.number_transitions, this.number_steps];
            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {

                for (int step_i = 0; step_i < this.number_steps; step_i++)
                    this.DELAY_STEPS_MATRIX[trans_i, step_i] = 0;

                if (this.Transition_list[trans_i].hasTimeCondition)
                {
                    for (int step_i = 0; step_i < this.Transition_list[trans_i].timeCondition_StepsIndex.Count; step_i++)
                    {
                        int step_index = this.Transition_list[trans_i].timeCondition_StepsIndex[step_i];
                        int duration = this.Transition_list[trans_i].timeCondition_durations[step_i];
                        this.DELAY_STEPS_MATRIX[trans_i, step_index] = duration;
                    }
                }
            }
        }

        private int getTransitionLocalIdBy_simultaneousDivergenceId(int refLocalId)
        {
            // here we have a simultaneous Divergence in downstream  of a selection divergence
            // and we need the localId of the step in downstream of this simultaneous Divergence
            int downstream_step_localId = -1;
            foreach (ConvDiv11 sim_div in this.Simultaneous_Div_list)
                if (refLocalId == sim_div.LocalId && sim_div.RefLocalIdList.Count == 1)
                {
                    downstream_step_localId = sim_div.RefLocalIdList[0];
                    break;
                }

            return downstream_step_localId;
        }

        private int getStepLocalIdBy_SelectionDivergeceId(int refLocalId)
        {
            // here we have a Selection Divergence in upstream  of a simultaneous divergence
            // and we need the localId of the step in upstream of this one
            int upstream_step_localId = -1;
            foreach (ConvDiv11 select_div in this.Selection_Div_list)
                if (refLocalId == select_div.LocalId && select_div.RefLocalIdList.Count == 1)
                {
                    upstream_step_localId = select_div.RefLocalIdList[0];
                    break;
                }
            /*else
            { // is it impossible to hage many steps upstream of a selection divergence
              // this case is not possible
                throw new NotImplementedException();
            }*/
            return upstream_step_localId;
        }

        // Initialization of ACTUAL INIT to INIT. It is better to use it because we can restart the simulation if necessary by using the same INIT
        private void InitializeINITACTUEL()
        {
            this.INITACTUEL = new int[INIT.Length];
            this.X_1 = new int[INIT.Length];

            for (int step_i = 0; step_i < INIT.Length; step_i++)
            {
                this.INITACTUEL[step_i] = INIT[step_i];
                this.X_1[step_i] = this.INITACTUEL[step_i];
            }
        }

        public void reinitializeG7()
        {
            this.InitializeINITACTUEL();
            foreach (Step11 step in this.Step_list)
            {
                step.time_cond.Disable();
            }
            this.clockNumber = 0;
        }

        public string Transmition_Code(string expression)
        {
            string token = expression;
            if (token == "+")
            {
                token = "0";
            }
            else if (token == "*")
            {
                token = "1";
            }
            else if (token == "!")
            {
                token = "2";
            }
            else if (token == "^")
            {
                token = "3";
            }
            else if (token == "TRUE" || token == "true")
            {
                token = "4";
            }
            else
            {
                token = token.Replace("[", "");
                token = token.Replace(".X]", "");
                for (int j = 0; j < VarTrans_name.Count; j++)
                {
                    if (token.Equals(VarTrans_name[j]))
                    {
                        token = (j + CodingGrafcetElements.base_input_vars_number).ToString();
                    }
                }
            }
            return token;
        }

        public string Transmition_Code_Microcontroller(string expression)
        {
            /*string text="this.VarTrans_name.Count: " + this.VarTrans_name.Count + "\narTrans_Microcontroller_code.Count :" + this.VarTrans_Microcontroller_code.Count;
            MessageBox.Show(text);*/
            string token = expression;
            if (token == "+")
            {
                token = CodingGrafcetElements.cge_op_OR;
            }
            else if (token == "*")
            {
                token = CodingGrafcetElements.cge_op_AND; //Really 1000 --> 10 00 --> 00 10 -->0010
            }
            else if (token == "!")
            {
                token = CodingGrafcetElements.cge_op_NOT; //Really 2000
            }
            else if (token == "^")
            {
                token = CodingGrafcetElements.cge_op_RISING_EDGE; //Really 3000
            }
            else if (token == "TRUE" || token == "true")
            {
                token = CodingGrafcetElements.cge_op_TRUE; //Really 4000
            }
            else
            {
                token = token.Replace("[", "");
                token = token.Replace(".X]", "");
                for (int j = 0; j < VarTrans_name.Count; j++)
                {
                    if (token.Equals(VarTrans_name[j]))
                    {
                        token = VarTrans_Microcontroller_code[j];
                    }
                }
            }
            return token;
        }

        public void Build_RS_from_StackTransConditions()
        {
            // Vecteur RS de l'orde des séquences pour le Microcontroller
            // Replace the Transition Variables by their LocalId + 4
            RS = new List<string>[number_transitions];
            RS_Microcontroller = new List<string>[number_transitions];
            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
            {
                RS[trans_i] = new List<string>();
                RS_Microcontroller[trans_i] = new List<string>();

                //Add Number of elements in transition i coded
                /*RS[trans_i].Add(Transition_list[trans_i].stack_Condition.Count.ToString());
                RS_Microcontroller[trans_i].Add(BinaryStringToHexString16(Convert.ToString(Transition_list[trans_i].stack_Condition.Count, 2)));
                */

                //Add elements of transition i coded
                for (int trans_i_j = 0; trans_i_j < Transition_list[trans_i].stack_Condition.Count; trans_i_j++)
                {
                    string stack_cond_i_j = Transition_list[trans_i].stack_Condition[trans_i_j];
                    RS[trans_i].Add(Transmition_Code(stack_cond_i_j));

                    string rs_Microcontroller = Transmition_Code_Microcontroller(stack_cond_i_j);
                    RS_Microcontroller[trans_i].Add(rs_Microcontroller);
                    
                    //Add the k^th number of Rising edge in the RS lists
                    if (rs_Microcontroller.Equals(CodingGrafcetElements.cge_op_RISING_EDGE))
                    {   //Add the number of that Rising edge in the list
                        RS[trans_i].Add(CodingGrafcetElements.RISING_EDGE_NUMBER.ToString());
                        string rs_micro = BinaryStringToHexString16(Convert.ToString(CodingGrafcetElements.RISING_EDGE_NUMBER++, 2));
                        RS_Microcontroller[trans_i].Add(rs_micro);
                        rs_Microcontroller += "  " + rs_micro;
                    }

                    string a_str = "RS_[" + trans_i + " , " + trans_i_j + "] : " + stack_cond_i_j + " RS_i_j_Microcontroller : " + rs_Microcontroller;
                    Console.WriteLine(a_str);
                    //MessageBox.Show(a_str);
                }

                //Add Number of elements in transition i coded
                int trans_num_elements = RS[trans_i].Count;
                RS[trans_i].Insert(0, trans_num_elements.ToString());
                //RS_Microcontroller[trans_i].Add(BinaryStringToHexString16(Convert.ToString(trans_num_elements, 2)));
                RS_Microcontroller[trans_i].Insert(0, BinaryStringToHexString16(Convert.ToString(trans_num_elements, 2)));
            }
        }

        /// <summary>
        /// This version chechs stable situations
        /// </summary>
        public void GenerateDynamicMatrices()
        {
            /////PARTIE EVOLUTION DYNAMIQUE DE LA SITUATION DU GRAFCET\\\\\\\\\\\\\\\\\\\\\
            if (this.hasTimeCondition)
            {
                this.read_TimeConditions();  // to update Rdyn
            }

            for (int step_i = 0; step_i < number_steps; step_i++)
            {
                //keep the old situation X_1 in the vector X_1old
                if (!this.firstStepOfDynamicEvolution)
                {
                    this.X_1old[step_i] = this.X_1[step_i];
                }
                //update Rdyn
                this.Rdyn[InputOutput_list.Count + step_i] = INITACTUEL[step_i];
            }
            if (this.firstStepOfDynamicEvolution)
            {
                this.firstStepOfDynamicEvolution = false;
            }
            this.clockNumber++;

            //evaluate the first g7 situation and store it in X_1 and INITACTUEL
            this.evaluate_and_fire_transitions();

            //evaluate new validated Transitions. It modifies this.VT_new that will be used after
            this.evaluate_next_validatedTransitions2();

            //evaluate the second g7 situation and store it in X_2 for test
            this.g7_has_stable_situation = !this.is_there_again_any_franchissable_transition();

            //deactivation of actions that was active and that shall be deactivated
            for (int av = 0; av < number_actions; av++)
            {
                if (O[av] == 1)
                {
                    for (int stp = 0; stp < number_steps; stp++)
                    {
                        if (MA[av, stp] == 1 && INITACTUEL[stp] == 0)
                        {
                            O[av] = 0;
                            // update values of actions variables
                            Rdyn[number_inputs + av] = O[av];
                        }
                    }
                }
            }

            //Calcul du vecteur O des sorties à activer
            // activation of new actions that shall be activated

            if (g7_has_stable_situation)
            {
                for (int y = 0; y < number_actions; y++)
                {
                    O[y] = 0;

                    for (int zz = 0; zz < number_steps; zz++)
                    {
                        if (O[y] != 1) // useless to set to 1 O[y] many times
                            if (INITACTUEL[zz] == 1 && MA[y, zz] == 1)
                            {
                                O[y] = 1;
                            }
                    }
                    // update values of actions variables
                    Rdyn[number_inputs + y] = O[y];
                }
            }

            // the Rdyn vector is modified after the generation of dynamic matrices 
            // to set to 0 the steps that have become unactive and set to 1 steps that are newly activated

            /////DISABLING SOME TIME VARIABLES*****AND****UNABLING ANOTHER ONES//////
            if (this.hasTimeCondition)
            {
                this.deactivate_TimeConditions();
                this.activate_TimeConditions();
            }
        }

        private void evaluate_and_fire_transitions()
        {
            // It is to evaluate the new situation of the grafcet
            R1 = new int[number_transitions];
            VT1 = new int[number_transitions];
            VA = new int[number_steps];
            VD = new int[number_steps];
            AD = new int[number_steps];
            VDbar = new int[number_steps];
            DX = new int[number_steps];
            FT1 = new int[number_transitions];

            for (int i = 0; i < number_steps; i++)
            {
                VA[i] = 0; //Etapes à activer
                VD[i] = 0; //Etapes à désactiver
            }

            bool franchissable_transition_found = false;
            this.X_1 = this.INITACTUEL;  //Vecteur X de la situation du grafcet

            for (int i = 0; i < number_transitions; i++)
            {
                //Vecteur VT des transitions validées
                //La transition i est-t-elle validée ?
                VT1[i] = 1;
                for (int y = 0; y < number_steps; y++)
                {
                    if (E[i, y] * X_1[y] != E[i, y])
                    {
                        VT1[i] = 0;
                    }
                }

                //Vecteur R des réceptivités des transitions
                R1[i] = Transition_list[i].EvaluateCondition(this.Rdyn, this.VarTrans_name);

                //Vecteur FT des transitions franchissables
                //La transition i est-t-elle franchissable ?
                FT1[i] = VT1[i] & R1[i];
                if (FT1[i] == 1)
                {
                    franchissable_transition_found = true;
                    //Calcul de la nouvelle situation par franchissement simultané des transitions franchissables
                    //Changement de situation du grafcet ; //Vecteur Activation et Vecteur Desactivation
                    //Il s'agit seulement de la transition i
                    for (int y = 0; y < number_steps; y++)
                    {
                        VA[y] = VA[y] | S[i, y]; //Etapes à activer
                        VD[y] = VD[y] | E[i, y]; //Etapes à désactiver
                    }
                }
            }

            // Evaluation of the new situation INITACTUEL
            if (franchissable_transition_found)
            {
                for (int step_i = 0; step_i < number_steps; step_i++)
                {
                    //Etapes à activer et à désactiver
                    AD[step_i] = VA[step_i] & VD[step_i];
                    VDbar[step_i] = ~VD[step_i];
                    //Vecteur de désactivation DX
                    DX[step_i] = VDbar[step_i] | AD[step_i];
                    //Changement de situation du grafcet
                    INITACTUEL[step_i] = (X_1[step_i] & DX[step_i]) | VA[step_i];

                    //update X_1 vector
                    this.X_1[step_i] = this.INITACTUEL[step_i];

                    //update Rdyn vector
                    this.Rdyn[InputOutput_list.Count + step_i] = INITACTUEL[step_i];
                }
            }
        }

        private bool is_there_again_any_franchissable_transition()
        {
            //After the firing of transitions, we want to know if the actual situation is stable, in oder to activate actions
            //We look ahead to evaluate another situation X_2 of the grafcet. It is usefull to know if X_2 is different to X_1
            //Here we do not need to fire transitions
            //NB: Here, X1, INITACTUEL, VT1 and FT1 shall not be modified. 
            //At the end of the previous method, the vector X_1 is already updated.  X_1 is now the new situation...

            //create and initialize some local variables
            int[] R2 = new int[number_transitions];
            //int[] VT2 = new int[number_transitions];
            int[] FT2 = new int[number_transitions];

            bool franchissable_transition_found = false;

            for (int i = 0; i < number_transitions; i++)
            {
                //the vector VT_new have yet been evaluated

                //Vecteur R des réceptivités des transitions
                R2[i] = Transition_list[i].EvaluateCondition(this.Rdyn, this.VarTrans_name);

                //Vecteur FT des transitions franchissables
                FT2[i] = this.VT_new[i] & R2[i];
                if (FT2[i] == 1)
                {
                    franchissable_transition_found = true;
                    break;
                }
            }
            return franchissable_transition_found;
        }

        private void read_TimeConditions()
        {
            //precondition : the vector TV_new has been in this.evaluate_new_validatedTransitions(); called by this.activate_TimeConditions
            //MessageBox.Show("this.timeConditionsList.Count : " + this.timeConditionsList.Count);
            //Read the value of time conditions to modify this.Rdyn, using this.VarTrans_name

            //the VT_new heve yet been activated
            //this.evaluate_new_validatedTransitions();

            if (this.hasTimeCondition)
            {
                //for (TimeCondition tc in this.timeConditionsList)
                //look for the validated transitions

                for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
                {
                    //MessageBox.Show("tc.IsSatisfied() = " + tc.IsSatisfied());
                    if (this.Transition_list[trans_i].hasTimeCondition && this.VT_new[trans_i] == 1)
                    {
                        for (int tv_num = 0; tv_num < this.Transition_list[trans_i].timeCondition_StepsIndex.Count; tv_num++)
                        {
                            int step_index = this.Transition_list[trans_i].timeCondition_StepsIndex[tv_num];
                            int duration = this.Transition_list[trans_i].timeCondition_durations[tv_num];
                            //if (this.Step_list[step_index].tc.IsActive()) {
                            if (this.Step_list[step_index].time_cond.has_delayed(duration))
                            {

                                string tv_name = TimeCondition.getTimeVariableName(this.Transition_list[trans_i].index, this.Step_list[step_index].name, duration);
                                int time_var_index = this.getVariableIndexbyName_in_VarTrans_name(tv_name);
                                this.Rdyn[time_var_index] = 1;

                                string str_mes = "In ReadTimeCondition \n\tTV : " + tv_name + " (var num : " + time_var_index + ")\n\t TransIndex = " + this.Transition_list[trans_i].index + "\n\t StepIndex=" + step_index + "\n\t Duration=" + duration;
                                Console.WriteLine(str_mes);
                                //MessageBox.Show(str_mes);
                            }
                            //}
                        }
                    }
                }
            }
        }

        private void evaluate_next_validatedTransitions2()
        {
            //look ahead to have validated transitions in oder to activate time variables that are used

            this.VT_new = new int[this.number_transitions];

            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
            {
                //Vecteur VT des transitions validées
                //La transition i est-t-elle validée ?
                this.VT_new[trans_i] = 1;
                for (int step_i = 0; step_i < number_steps; step_i++)
                {
                    if (E[trans_i, step_i] * X_1[step_i] != E[trans_i, step_i])
                    {
                        this.VT_new[trans_i] = 0;
                    }
                }
            }
        }

        private void activate_TimeConditions()
        {
            ///////////////////////////////UNABLING SOME TIME VARIABLES ///////////////////////////////////
            // the Rdyn vector was modified after the generation of dynamic matrices 

            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                if (this.X_1old[step_i] == 0 && this.X_1[step_i] == 1)
                    if (this.Step_list[step_i].IsUsedInTimeConditions)
                    {
                        //Console.WriteLine("Activation of time condition for STEP S"+step_i);
                        bool activated = this.Step_list[step_i].time_cond.Activate();
                        if (activated)
                        {
                            string tc_message = this.getCurrentTime() + " Time Condition of the Step " + this.Step_list[step_i].name + "(Index: " + this.Step_list[step_i].index + ") is now activated";
                            this.fluxWrite_DynamicLog.WriteLine(tc_message);
                            Console.WriteLine(tc_message);
                        }
                    }
            }
        }

        private void deactivate_TimeConditions()
        {
            /////////////////////////////////DISABLING SOME TIME VARIABLES\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                if (this.X_1old[step_i] == 1 && this.X_1[step_i] == 0)
                    if (this.Step_list[step_i].IsUsedInTimeConditions)
                    {
                        //MessageBox.Show("Here to deactivate ...");
                        bool desactivated = this.Step_list[step_i].time_cond.Disable();
                        if (desactivated)
                        {
                            //use the to set to 0 time variables
                            foreach (string tv_name in this.Step_list[step_i].time_cond.time_variables_names)
                            {
                                int variable_index = this.getVariableIndexbyName_in_VarTrans_name(tv_name);
                                this.Rdyn[variable_index] = 0;
                            }
                            string tc_message = this.getCurrentTime() + " Time Condition of Step " + this.Step_list[step_i].name + "(Index: " + this.Step_list[step_i].index + ") is now DeActivated after " + this.Step_list[step_i].time_cond.timer_duration() + " ms";
                            this.fluxWrite_DynamicLog.WriteLine(tc_message);
                            Console.WriteLine(tc_message);
                        }
                    }
            }
        }

        private string getCurrentTime()
        {
            return String.Format("({0:00}:{1:00}:{2:00} <{3:0000} ms>) ", System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);
        }
        /*private string getCurrentTime1()
        {
            return String.Format("({0:0000}-{1:00}-{2:00} $ {3:00}:{4:00}:{5:00} < {6:0000} seconds >)",
                System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day,
                System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);
        }*/
        private int getVariableIndexbyName_in_VarTrans_name(string a_varName)
        {
            // mainly called to obtain time variable index
            int variableIndex = -1;

            for (int var_index = 0; var_index < this.VarTrans_name.Count; var_index++)
            {
                if (a_varName.Equals(this.VarTrans_name[var_index]))
                {
                    variableIndex = var_index;
                    break;
                }
            }
            return variableIndex;
        }

        public static string BinaryStringToHexString16(string binaryStr)
        {
            // TODO: check if all binaryStr is 1's or 0's ... Will throw otherwise
            //string binStr_input = binaryStr; //NNG

            int mod4Len = binaryStr.Length % 16;
            if (mod4Len != 0)
            {
                // pad to length multiple of 16
                int total_binaryStr_width = ((binaryStr.Length / 16) + 1) * 16;
                binaryStr = binaryStr.PadLeft(total_binaryStr_width, '0');
            }

            StringBuilder result = new StringBuilder(binaryStr.Length / 8 + 1);

            // Good if binaryStr.Length = 16 HERE. In fact, 8 = binaryStr.Length - 8. 8 can then be replaced by the second expression in the case where binaryStr.Length > 16 to be sure that all the binary string is converted
            //if (binaryStr.Length == 16)
            {
                for (int i = 8; i >= 0; i -= 8)
                {
                    string eightBitsH = binaryStr.Substring(i, 8);
                    result.AppendFormat("{0:X2}", Convert.ToByte(eightBitsH, 2));
                    //MessageBox.Show("Level "+(i/8)+" : "+eightBitsH+" = "+result.ToString());
                }
            }

            return result.ToString();
        }

        public static string StepTimeToHexString16(string three_first_digits, int stepIndex, int duration_sec)
        {
            //Here, we suppose that var_index <= 32
            //The number of steps should be on 5 bits
            int step_length = CodingGrafcetElements.cge_digits_num_step;
            string step_binary = Convert.ToString(stepIndex, 2);
            int step4Len = step_binary.Length % step_length;
            if (step4Len != 0)
            {
                // pad to length to obtain multiple of 5
                step_binary = step_binary.PadLeft(((step_binary.Length / step_length) + 1) * step_length, '0');
            }

            //the duration should be on 8 bits
            int duration_length = CodingGrafcetElements.cge_digits_duration;
            string duration_binary = Convert.ToString(duration_sec, 2);
            int duration4Len = duration_binary.Length % duration_length;
            if (duration4Len != 0)
            {
                duration_binary = duration_binary.PadLeft(((duration_binary.Length / duration_length) + 1) * duration_length, '0');
            }

            string StepTime_16bit = three_first_digits + step_binary + duration_binary;

            StringBuilder result = new StringBuilder(StepTime_16bit.Length / 8 + 1);

            // TODO: check all bits of variableCode_16bit is 1 or 0 ... If not throw exception otherwise
            // variableCode_16bit.Length = 16, if not change to for (int i = variableCode_16bit.Length - 8; i >= 0; i -= 8)
            for (int i = 8; i >= 0; i -= 8)
            {
                string eightBitsH = StepTime_16bit.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBitsH, 2));
            }
            //MessageBox.Show("In StepTimeToHexString16: Step num : " + stepIndex + " duration : " + duration_sec + " <Result: " + result.ToString());
            return result.ToString();
        }

        public static string VariableToHexString16(string three_first_digits, int var_index, int var_length)
        {
            //The var code should be on 8 bits
            //int var_length = 8;
            string var_code_bin = Convert.ToString(var_index, 2);
            int var4Len = var_code_bin.Length % var_length;
            if (var4Len != 0)
            {
                var_code_bin = var_code_bin.PadLeft(((var_code_bin.Length / var_length) + 1) * var_length, '0');
            }

            //Why 13 - var_lenth? R: The format could be 0b 1000 0000 NNNN NNNN or 0b 1010 0000 NNNN NNNN or 0b 1100 0000 000N NNNN
            string variableCode_16bit = three_first_digits + "".PadRight(13 - var_length, '0') + var_code_bin;

            StringBuilder result = new StringBuilder(variableCode_16bit.Length / 8 + 1);

            // variableCode_16bit.Length = 16, if not change to for (int i = variableCode_16bit.Length - 8; i >= 0; i -= 8)
            // TODO: check all bits of variableCode_16bit is 1 or 0 ... If not throw exception otherwise
            // NNG: transformation of 0xabcd to 0xcdab
            for (int i = 8; i >= 0; i -= 8)
            {
                string eightBitsH = variableCode_16bit.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBitsH, 2));
            }

            //MessageBox.Show("In VariableToHexString16  3digits= :"+three_first_digits+" var_index: " + var_index + " var_length : " + var_length + " <Result: " + result.ToString());
            return result.ToString();
        }

        public static string StepTimeToHexString16_bad(int stepIndex, int duration_sec)
        {
            //Realize the conversion without transforming 0xabcd to 0xcdab
            //Here, we suppose that var_index <= 16
            string step_binary = Convert.ToString(stepIndex, 2);
            int step4Len = step_binary.Length % 4;
            if (step4Len != 0)
            {
                // pad to length to obtain multiple of 4
                step_binary = step_binary.PadLeft(((step_binary.Length / 4) + 1) * 4, '0');
            }

            string duration_binary = Convert.ToString(duration_sec, 2);
            int duration4Len = duration_binary.Length % 10;
            if (duration4Len != 0)
            {
                // pad to length to obtain multiple of 4
                duration_binary = duration_binary.PadLeft(((duration_binary.Length / 10) + 1) * 10, '0');
            }

            string StepTime_16bit = "11" + duration_binary + step_binary;

            StringBuilder result = new StringBuilder(StepTime_16bit.Length / 4 + 1);

            // TODO: check all bits of variableCode_16bit is 1 or 0 ... If not throw exception otherwise
            for (int i = 0; i <= 12; i += 4)
            {
                string eightBitsH = StepTime_16bit.Substring(i, 4);
                result.AppendFormat("{0:X1}", Convert.ToByte(eightBitsH, 2));
            }
            return result.ToString();
        }

        public void BuildPIControllerMessage()
        {
            PICMessage = "";

            //For number of Steps, transitions and actions
            PICMessage += BinaryStringToHexString16(Convert.ToString(this.number_transitions, 2));
            PICMessage += BinaryStringToHexString16(Convert.ToString(this.number_steps, 2));
            PICMessage += BinaryStringToHexString16(Convert.ToString(this.number_inputs, 2));
            PICMessage += BinaryStringToHexString16(Convert.ToString(this.number_actions, 2));

            // For the vector INIT
            string bin_INIT = null;
            for (int y = number_steps - 1; y >= 0; y--)
            {
                bin_INIT = bin_INIT + INIT[y];
            }
            //MessageBox.Show("bin_INIT : " + bin_INIT + "BinStrTo_HEX: " + BinaryStringToHexString16(bin_INIT));
            PICMessage = PICMessage + BinaryStringToHexString16(bin_INIT);

            // For the matrix E
            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
            {
                string bin_E = null;
                for (int step_i = number_steps - 1; step_i >= 0; step_i--)
                {
                    bin_E = bin_E + E[trans_i, step_i].ToString();
                }
                PICMessage = PICMessage + BinaryStringToHexString16(bin_E);
            }

            // For the matrix S
            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
            {
                string bin_S = null;
                for (int step_i = number_steps - 1; step_i >= 0; step_i--)
                {
                    bin_S = bin_S + S[trans_i, step_i].ToString();
                }
                PICMessage = PICMessage + BinaryStringToHexString16(bin_S);
            }

            // For the matrix MA
            for (int action_i = 0; action_i < this.number_actions; action_i++)
            {
                string bin_MA = null;
                for (int y = number_steps - 1; y >= 0; y--)
                {
                    bin_MA = bin_MA + MA[action_i, y].ToString();
                }
                PICMessage = PICMessage + BinaryStringToHexString16(bin_MA);
            }

            // For the matrix RS
            for (int trans_i = 0; trans_i < number_transitions; trans_i++)
            {
                for (int trans_i_j = 0; trans_i_j < RS[trans_i].Count; trans_i_j++)
                {
                    //NNG2 : know if the variable RS[x][y] is a timing variable (code : 1) or not (code : 0)
                    //MessageBox.Show("RS[" + x + "][" + y + "] =" + RS[x][y]);
                    PICMessage = PICMessage + RS_Microcontroller[trans_i][trans_i_j];
                }
            }

            //MessageBox.Show("GeneratePIControllerMessage GOOD !");
            PICMessage = PICMessage + "00FE";

            //this.Build_DatainEE_Array();
        }

        public void Build_DatainEE_Array()
        {
            //This method is use to produce DatainEE that can be added manually 
            //or automatically in the MPLAB C code 
            //before succeeding transmission of data in the EEPROM using Serial Port Terminal
            //It can help to build the project completely in order to write the .HEX code in the board
            string raw_PIC_message = this.PICMessage;
            int PM_length = raw_PIC_message.Length;
            if (PM_length % 4 == 0)
            {
                string a_row_dword, a_DataInEE_dword;
                DataInEE_array = "//N_trans, N_Steps, N_Inputs, N_actions: \n";
                //Concerning number of transitions, steps and actions
                int current_index = 0;
                //int f_block_len = 4 * 2; //Length of the following block
                int f_block_len = 4 * (1 + 1 + 1 + 1); //Length of the following block (num steps + transitions and actions)
                for (int a_dword_index = current_index; a_dword_index < f_block_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "\t0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);
                    DataInEE_array = DataInEE_array + a_DataInEE_dword + ", \n";
                }

                //Concerning vector INIT 
                current_index += f_block_len;
                f_block_len = 4 * 1;

                DataInEE_array = DataInEE_array + "//INIT Vector \n\t";
                for (int a_dword_index = current_index; a_dword_index < current_index + f_block_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);
                    DataInEE_array = DataInEE_array + a_DataInEE_dword + ", ";
                }
                DataInEE_array = DataInEE_array + "\n";

                //Concerning matrix E 
                current_index += f_block_len;
                f_block_len = 4 * number_transitions; // For E ans S

                DataInEE_array = DataInEE_array + "//E matrix \n\t";
                for (int a_dword_index = current_index; a_dword_index < current_index + f_block_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);
                    DataInEE_array = DataInEE_array + a_DataInEE_dword + ", ";
                }
                DataInEE_array = DataInEE_array + "\n";

                //Concerning matrix S 
                current_index += f_block_len;

                DataInEE_array = DataInEE_array + "//S matrix \n\t";
                for (int a_dword_index = current_index; a_dword_index < current_index + f_block_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);
                    DataInEE_array = DataInEE_array + a_DataInEE_dword + ", ";
                }
                DataInEE_array = DataInEE_array + "\n";

                //Concerning MA matrix
                current_index += f_block_len;
                f_block_len = 4 * number_actions;

                DataInEE_array = DataInEE_array + "//MA matrix \n\t";
                for (int a_dword_index = current_index; a_dword_index < current_index + f_block_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);
                    DataInEE_array = DataInEE_array + a_DataInEE_dword + ", ";
                }
                DataInEE_array = DataInEE_array + "\n";

                //Concerning RS matrix
                current_index += f_block_len;
                int FE00_len = 4 * 1; //Because FE00 is not useful here

                //To follow RS
                int trans_i = 0;
                int curr_trans_size = this.RS[trans_i].Count; //Size of the current RS entry
                int dword_in_trans_i_number = 0; // select the first dword in the curr RS entry

                DataInEE_array = DataInEE_array + "//RS matrix \n\t";
                for (int a_dword_index = current_index; a_dword_index < PM_length - FE00_len; a_dword_index += 4)
                {
                    a_row_dword = raw_PIC_message.Substring(a_dword_index, 4);
                    a_DataInEE_dword = "0x" + a_row_dword.Substring(2, 2) + a_row_dword.Substring(0, 2);

                    dword_in_trans_i_number++;

                    if (dword_in_trans_i_number < curr_trans_size)
                    {
                        DataInEE_array = DataInEE_array + a_DataInEE_dword + ", ";
                    }
                    else
                    {
                        //Console.WriteLine("RS[" + trans_i + "] size : " + curr_trans_size);
                        DataInEE_array = DataInEE_array + a_DataInEE_dword + ", \n";
                        DataInEE_array = DataInEE_array + "\t";
                        trans_i++;
                        if (trans_i < number_transitions)
                        {
                            curr_trans_size = this.RS[trans_i].Count;
                            dword_in_trans_i_number = 0;
                        }
                    }
                }
                //Remove the comma at the end
                DataInEE_array = DataInEE_array.Substring(0, DataInEE_array.Length - 4);
            }
            else
            {
                DataInEE_array = "Error in PIC Message";
            }
            Console.Write("\n******************* Data in EEP ROM ******************\n\n" + DataInEE_array + "\n\n");
            Clipboard.SetText(DataInEE_array);
        }
        public void build_g7_Text_structure()
        {
            //some initializations
            this.g7_text_struct = "";

            //add number of transitions
            this.add_g7_text_struct(this.number_transitions, true);
            //add number of steps
            this.add_g7_text_struct(this.number_steps, true);
            //add number of actions
            this.add_g7_text_struct(this.number_actions, true);
            this.add_g7_text_struct("\n");


            //add INIT vector
            for (int step_i = 0; step_i < this.number_steps - 1; step_i++)
                this.add_g7_text_struct(this.INIT[step_i], false);
            this.add_g7_text_struct(this.INIT[this.number_steps - 1], true);
            this.add_g7_text_struct("\n");

            //add E matrix
            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                for (int step_i = 0; step_i < this.number_steps - 1; step_i++)
                {
                    this.add_g7_text_struct(this.E[trans_i, step_i], false);
                }
                this.add_g7_text_struct(this.E[trans_i, this.number_steps - 1], true);
            }
            this.add_g7_text_struct("\n");

            //add S matrix
            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                for (int step_i = 0; step_i < this.number_steps - 1; step_i++)
                {
                    this.add_g7_text_struct(this.S[trans_i, step_i], false);
                }
                this.add_g7_text_struct(this.S[trans_i, this.number_steps - 1], true);
            }
            this.add_g7_text_struct("\n");

            //add MA matrix
            for (int action_i = 0; action_i < this.number_actions; action_i++)
            {
                for (int step_i = 0; step_i < this.number_steps - 1; step_i++)
                {
                    this.add_g7_text_struct(this.MA[action_i, step_i], false);
                }
                this.add_g7_text_struct(this.MA[action_i, this.number_steps - 1], true);
            }
            this.add_g7_text_struct("\n");

            //add number of variables in input
            this.add_g7_text_struct(this.number_inputs, true);

            //add variables used in transitions 1 : input variables
            for (int input_var_i = 0; input_var_i < this.input_names.Count; input_var_i++)
            {
                this.add_g7_text_struct(this.input_names[input_var_i] + "\n");
                Console.WriteLine("Saving input variables : " + this.input_names[input_var_i]);
            }
            this.add_g7_text_struct("\n");

            //add number of variables in output (actions)
            this.add_g7_text_struct(this.number_actions, true);

            //add variables used in transitions 2 : output variables
            for (int action_var_i = 0; action_var_i < this.actions_names.Count; action_var_i++)
            {
                this.add_g7_text_struct(this.actions_names[action_var_i] + "\n");
                Console.WriteLine("Saving Action variables : " + this.actions_names[action_var_i]);
            }
            this.add_g7_text_struct("\n");

            //add transition receptivities expressions
            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                this.add_g7_text_struct(this.Transition_list[trans_i].Condition + "\n");
            }
            this.add_g7_text_struct("\n");


            //add The vector RS of evaluation sequence of receptivities
            //we could also just add transitions expressions
            /*for (int trans_i = 0; trans_i < this.RS.Length; trans_i++) {
                int recept_length = this.RS[trans_i].Count;
                string recept_line = "";
                foreach (string value in this.RS[trans_i]) {
                    recept_line += value + " , ";
                }
                this.add_g7_text_struct(recept_line);
                this.add_g7_text_struct("\n");
            }
            this.add_g7_text_struct("\n");
            */

            /*
            //add DELAY_STEPS_MATRIX matrix : important but ...
            for (int trans_i = 0; trans_i < transitions_number; trans_i++)
            {
                for (int step_i = 0; step_i < steps_number; step_i++)
                {
                    this.add_g7_text_struct(this.DELAY_STEPS_MATRIX[trans_i, step_i], false);
                }
                this.add_g7_text_struct("\n");
            }
            this.add_g7_text_struct("\n");

            */

        }
        private void add_g7_text_struct(int a_number, bool go_to_line)
        {
            if (go_to_line)
                this.g7_text_struct += "" + a_number + "\n";
            else
                this.g7_text_struct += "" + a_number + "  ,  ";
        }
        private void add_g7_text_struct(string a_text)
        {
            this.g7_text_struct += a_text;
        }

        // to read and load a grafcet from a file
        public void Read_Save_g7_TextStructure(StreamReader myStream)
        {
            this.InitVariables();

            this.number_transitions = Int32.Parse(myStream.ReadLine());
            this.number_steps = Int32.Parse(myStream.ReadLine());
            this.number_actions = Int32.Parse(myStream.ReadLine());
            myStream.ReadLine(); // to read an empty line

            Console.WriteLine("number_transitions :  " + this.number_transitions);
            Console.WriteLine("number_steps :  " + this.number_steps);
            Console.WriteLine("number_actions :  " + this.number_actions);

            this.INIT = new int[this.number_steps];
            string a_line = myStream.ReadLine();
            string[] numbers = a_line.Split(',');
            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                this.INIT[step_i] = Int32.Parse(numbers[step_i]);
                Console.WriteLine("INIT[ " + step_i + " ] = " + numbers[step_i]);
            }
            myStream.ReadLine(); // to read an empty line

            // for the matrix E
            this.E = new int[this.number_transitions, this.number_steps];

            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                a_line = myStream.ReadLine();
                numbers = a_line.Split(',');

                for (int step_i = 0; step_i < this.number_steps; step_i++)
                {
                    this.E[trans_i, step_i] = Int32.Parse(numbers[step_i]);
                    Console.WriteLine("E[ " + trans_i + "," + step_i + " ] = " + this.E[trans_i, step_i]);
                }
            }

            myStream.ReadLine(); // to read an empty line

            // for the matrix S
            this.S = new int[this.number_transitions, this.number_steps];

            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                a_line = myStream.ReadLine();
                numbers = a_line.Split(',');

                for (int step_i = 0; step_i < this.number_steps; step_i++)
                {
                    this.S[trans_i, step_i] = Int32.Parse(numbers[step_i]);
                    Console.WriteLine("S[ " + trans_i + "," + step_i + " ] = " + this.S[trans_i, step_i]);
                }
            }

            myStream.ReadLine(); // to read an empty line

            // for the matrix MA
            this.MA = new int[this.number_actions, this.number_steps];

            for (int action_i = 0; action_i < this.number_actions; action_i++)
            {
                a_line = myStream.ReadLine();
                numbers = a_line.Split(',');

                for (int step_i = 0; step_i < this.number_steps; step_i++)
                {
                    this.MA[action_i, step_i] = Int32.Parse(numbers[step_i]);
                    Console.WriteLine("MA[ " + action_i + "," + step_i + " ] = " + this.MA[action_i, step_i]);
                }
            }
            myStream.ReadLine(); // to read an empty line

            //Read variables 

            //Read number of variables in input
            this.number_inputs = Int32.Parse(myStream.ReadLine());

            //adding variables in some Lists   **  1: input variables

            this.input_names = new List<string>();

            for (int input_i = 0; input_i < this.number_inputs; input_i++)
            {
                a_line = myStream.ReadLine();
                this.input_names.Add(a_line);
                this.VarTrans_name.Add(a_line);
                Variable11 anInputVar = new Variable11(a_line);
                this.InputOutput_list.Add(anInputVar);
                this.Input_list.Add(anInputVar);
            }
            myStream.ReadLine(); // to read an empty line

            //Read number of variables in output
            this.number_actions = Int32.Parse(myStream.ReadLine());

            //adding variables in some Lists   **  2: output variables
            //read input variables used in transitions: simple variables (non actions and actions) + steps variables + time variables
            this.actions_names = new List<string>();

            for (int output_i = 0; output_i < this.number_actions; output_i++)
            {
                a_line = myStream.ReadLine();
                this.actions_names.Add(a_line);
                this.VarTrans_name.Add(a_line);
                Variable11 anOutputVar = new Variable11(a_line);
                anOutputVar.IsAnAction = true;
                this.InputOutput_list.Add(anOutputVar);
                this.Actions_list.Add(anOutputVar);
            }
            myStream.ReadLine(); // to read an empty line

            // Read and fill transitions list 

            for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
            {
                a_line = myStream.ReadLine();
                this.Transition_list.Add(new Transition11(a_line));
            }

            //create steps list
            string step_name;
            for (int step_i = 0; step_i < this.number_steps; step_i++)
            {
                step_name = "S" + step_i;
                this.Step_list.Add(new Step11(step_name));
            }

            /*string vars = "";
            for (int var_i = 0; var_i < this.VarTrans_name.Count; var_i++)
            {
                vars = vars+ VarTrans_name[var_i]+"\n";
            }
            MessageBox.Show(vars); */

            this.ending_static_and_dynamic_initializations();

            this.InitializeINITACTUEL();
            this.evaluate_next_validatedTransitions2(); // usefull for time conditions

            // ****************** END OF READING THE FILE ***************** \\
        }

        
        //$$$$$$$$$$$$$$$$$$$$$$$$$$$  ARDUINO CODE GENERAION  $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        /// <summary>
        /// Generate code for the Arduino board : initializations
        /// </summary>

        public void arduino_code_generation_initializations(StreamWriter source_code_stream, string init_timer_file_path, bool analyse_efficiency, bool debug)
        {
            //Prepare transitions conditions for Arduino
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_prepare_clearing_condition(); //To transform variables such as [...] : Step variable & Timing Variablesm);
            }

            //Declare INPUT States 
            if (this.Input_list.Count > 0) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("//**** \t Declare INPUT pins states\t****");

                foreach (Variable11 an_input in this.Input_list)
                {
                    an_input.arduino_code_generation_declare(source_code_stream);
                }
            }
            
            //Declare OUTPUT States 
            if (this.Actions_list.Count > 0) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("//**** \t Declare OUPUT pins states\t****");
                foreach (Variable11 an_output in this.Actions_list)
                {
                    an_output.arduino_code_generation_declare(source_code_stream);
                }
            }
            
            //Declare g7 steps & transitions variables
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare STEPs variables\t****");
            foreach (Step11 a_step in this.Step_list)
            {
                a_step.arduino_code_generation_declare_varStep(source_code_stream);
            }

            //
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare Validated Transitions variables\t****");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_add_declare_ValidatedTransitionsVariable(source_code_stream);
            }

            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare Receptivities of Transitions variables\t****");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_add_declare_ReceptivitiesTransitionsVariable(source_code_stream);
            }

            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare Transitions receptivities variables\t****");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_add_declare_ReceptivityVariable(source_code_stream);
            }

            //Declare g7 steps time variables 
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare STEPs timing variables for duration activity \t****");
            foreach (Step11 a_step in Step_list)
            {
                a_step.arduino_code_generation_declare_timeDuration(source_code_stream);
            }

            if (analyse_efficiency) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("//To measure the efficacy");
                source_code_stream.WriteLine("unsigned int NN = 0;");
                source_code_stream.WriteLine("unsigned int time_val;");
            }

            //To manage the update of steps duration activity
            if (this.hasTimeCondition)
            {
                source_code_stream.WriteLine();
                //To declare the Frequency F of the Timer associated to TU of Timing constraints values
                source_code_stream.WriteLine("//FT = 1000/Board_TIME_UNIT = Frequency of the Step activity callback function Step_timer_update_callback");
                source_code_stream.WriteLine("const unsigned int Board_TIME_UNIT = " + GrafcetClass.g7_Board_TimeUnit_TimerStepActivity + ";");
                source_code_stream.WriteLine();
                //string init_timer_file_path = "..\\Arduino\\init_timer_file";
                G7_Form.appenFileToStreamWriter(init_timer_file_path, source_code_stream);

                //BEGIN Step_timer_update_callback function
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("void Step_timer_update_callback(){");
                foreach (Step11 a_step in Step_list)
                {
                    a_step.arduino_code_generation_time_update(source_code_stream);
                }
                //arduino_code_generation_timer_update
                source_code_stream.WriteLine("}");
                //END Step_timer_update_callback function
            }
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$ Main Functions $$$$$$$$$$$$$$$$$$$
        /// <summary>
        /// generate arduino setup function
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_setup(StreamWriter source_code_stream, bool analyse_efficiency, bool debug)
        {
            //SETUP function
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("void setup(){");
            if (this.hasTimeCondition)
            {
                source_code_stream.WriteLine("\tinitializeTimer1();");
            }
            if (debug || analyse_efficiency)
            {
                source_code_stream.WriteLine("\tSerial.begin(9600);");
            }
            else {
                source_code_stream.WriteLine("\t//Serial.begin(9600);");
            }

            if (this.Input_list.Count > 0) {
                source_code_stream.WriteLine("\t//INIT INPUT PINs");
                foreach (Variable11 an_input in this.Input_list)
                {
                    an_input.arduino_code_generation_setup(source_code_stream);
                }
            }
            
            if (this.Actions_list.Count > 0) {
                source_code_stream.WriteLine("\t//INIT OUTPUT PINs");
                foreach (Variable11 an_output in this.Actions_list)
                {
                    an_output.arduino_code_generation_setup(source_code_stream);
                }
            }

            //Initialize step activity variables (Already done by arduino_code_generation_declare_varStep)

            if (analyse_efficiency)
            {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("\ttime_val = millis();");
                source_code_stream.WriteLine("\tNN = 0;");
            }
            source_code_stream.WriteLine("}");
            //END SETUP function
        }

        /// <summary>
        /// generate arduino loop function
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_loop(StreamWriter source_code_stream, bool analyse_efficiency, bool debug)
        {
            //BEGIN loop function
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("void loop(){");

            //Reading input values
            if (this.Input_list.Count > 0) {
                source_code_stream.WriteLine("\t//Reading input values");
                foreach (Variable11 an_input in this.Input_list)
                {
                    an_input.arduino_code_generation_readInput(source_code_stream);
                }
                source_code_stream.WriteLine();
            }

            //Validated transitions variables VT_i : When many steps are in input, all of them should be active to render VT_i validated (And && condition)
            source_code_stream.WriteLine("\t//Evaluate validated transitions variabless");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                source_code_stream.Write("\tVT_" + (a_transition.index + 1) + " = ");
                string validate_trans_condition = "(";
                for (int step_i = 0; step_i < this.number_steps; step_i++)
                {
                    if (this.E[a_transition.index, step_i] == 1)
                    {
                        validate_trans_condition += this.Step_list[step_i].name + "_X && ";
                    }
                }
                validate_trans_condition = validate_trans_condition.Substring(0, validate_trans_condition.Length - 4);
                validate_trans_condition += ");";
                source_code_stream.WriteLine(validate_trans_condition);
            }

            //Receptivities of transitions
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("\t//Evaluate Receptivities of transitions");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_add_TransitionsReceptivities(source_code_stream);
            }

            //Clearing conditions, or transitions receptivities
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("\t//Evaluate Clearing conditions");
            foreach (Transition11 a_transition in this.Transition_list)
            {
                a_transition.arduino_code_generation_add_FiringTRansitionCondition(source_code_stream);
            }

            //Save Step activity variables state in Old_ corresponding variables
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("\t//Save the states of Step's activity variables in Old_ corresponding variables");
            foreach (Step11 a_step in this.Step_list)
            {
                source_code_stream.Write("\t");
                a_step.arduino_code_generation_update_OldStepVariables(source_code_stream);
            }

            //Evaluate Step variables
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("\t//Evaluate Step variables");
            foreach (Step11 a_step in this.Step_list)
            {
                int step_i = this.Step_list.IndexOf(a_step);
                source_code_stream.Write("\t" + a_step.name + "_X = ");
                string step_eval_expression2 = "";

                //S : With transitions where : this step is in output
                for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
                {
                    if (this.S[trans_i, step_i] == 1)
                    {
                        step_eval_expression2 += "TR_" + (trans_i + 1) + " || ";
                    }
                }
                step_eval_expression2 += "(" + a_step.name + "_X_Old && ";
                //E : With transitions where : this step is in input
                for (int trans_i = 0; trans_i < this.number_transitions; trans_i++)
                {
                    if (this.E[trans_i, step_i] == 1)
                    {
                        step_eval_expression2 += "!TR_" + (trans_i + 1) + " && ";
                    }
                }
                step_eval_expression2 = step_eval_expression2.Substring(0, step_eval_expression2.Length - 4);
                step_eval_expression2 += ")";

                source_code_stream.WriteLine(step_eval_expression2 + ";");
            }


            //Evaluate OUTPUTs variables
            if (this.Actions_list.Count > 0) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("\t//Evaluate OUTPUTs variables");
                foreach (Variable11 an_action in this.Actions_list)
                {
                    int output_i = this.Actions_list.IndexOf(an_action);

                    source_code_stream.Write("\t" + an_action.name + " = ");
                    string action_eval_expression2 = "";

                    foreach (Step11 a_step in this.Step_list)
                    {
                        int step_i = this.Step_list.IndexOf(a_step);
                        if (this.MA[output_i, step_i] == 1)
                        {
                            action_eval_expression2 += a_step.name + "_X" + " || ";
                        }
                    }

                    action_eval_expression2 = action_eval_expression2.Substring(0, action_eval_expression2.Length - 4);
                    source_code_stream.WriteLine(action_eval_expression2 + ";");
                }
            }
            
            //Update OUTPUTs
            if (this.Actions_list.Count > 0) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("\t//Update OUTPUTs");
                foreach (Variable11 an_output in this.Actions_list)
                {
                    an_output.arduino_code_generation_update_Outputs(source_code_stream);
                }
            }
            

            if (this.Actions_list.Count > 0) {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("\t//Update Old OUTPUTs");
                foreach (Variable11 an_output in this.Actions_list)
                {
                    an_output.arduino_code_generation_update_Old_Outputs(source_code_stream);
                }
            }

            if (analyse_efficiency)
            {
                source_code_stream.WriteLine();
                source_code_stream.WriteLine("\tNN++;");
                source_code_stream.WriteLine("\tif(millis() - time_val > 5000){");
                source_code_stream.WriteLine("\t\tSerial.println(NN);");
                source_code_stream.WriteLine("\t\tNN = 0; time_val = millis();");
                source_code_stream.WriteLine("\t}");
            }
            source_code_stream.WriteLine("\tdelay(2);");

            source_code_stream.WriteLine("}");
            //END loop function
        }
    }
}