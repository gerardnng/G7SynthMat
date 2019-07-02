using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Timers;
using GrafcetConvertor;
using System.IO;
using System.Windows.Forms;

namespace GrafcetConvertor
{
    class Variable11
    {
        public static int COUNT_VARS = 0; // NNG: use to number variables in the attribute Index
        private int Index;
        public int number;
        private string Name;
        private string initialValue;
        private string var_type;
        public bool IsAnAction;
        public List<int> RefLocalIdSteps;
        public List<string> durations_names; // NNG: only actions having attributes duration
        public List<int> durations_ms;
        ~Variable11() { }
        public Variable11(XmlNode XmlVariableNode)
        {
            this.Index = COUNT_VARS++;
            this.Name = XmlVariableNode.Attributes[0].Value;
            this.initialValue = XmlVariableNode.ChildNodes[1].ChildNodes[0].Attributes[0].Value;
            this.var_type = XmlVariableNode.ChildNodes[0].ChildNodes[0].Name;
            //MessageBox.Show("Type of the node : "+this.var_type);

            switch (this.var_type) {
                case "BOOL":
                    //To update in order to manage types and initial steps
                    if (this.initialValue.Equals("False"))
                    {
                        this.initialValue = "false";
                    }
                    else
                    {
                        this.initialValue = "true";
                    }    
                    break;
                default:
                    MessageBox.Show("Variables of type < "+this.var_type+" > are not yet managed ( "+ this.Name+" )");
                    break;
            }
            
            
            
                
            // concerning actions variables
            this.IsAnAction = false;
            this.durations_names = new List<string>();
            this.durations_ms = new List<int>();
            this.RefLocalIdSteps = new List<int>();
        }
        public Variable11(string name)
        {
            this.Index = COUNT_VARS++;
            this.Name = name;
            // concerning actions variables
            this.IsAnAction = false;
            this.durations_names = new List<string>(); // could not be action variables
            this.RefLocalIdSteps = new List<int>();
        }
        public int index
        {
            get { return this.Index; }
            set { this.Index = value; }
        }
        public string name
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        //ARDUINO CODE GENERATION
        /// <summary>
        /// Generate code for the Arduino board : declaration
        /// </summary>

        public void arduino_code_generation_declare(StreamWriter source_code_stream) {
            source_code_stream.WriteLine("boolean " + this.name + " = " + this.initialValue + ", " + this.name + "_Old = " + this.initialValue + ";");
        }
        /// <summary>
        /// Generate code for the Arduino board : init in setup
        /// </summary>
        public void arduino_code_generation_setup(StreamWriter source_code_stream)
        {
            if(this.IsAnAction)
                source_code_stream.WriteLine("\tpinMode(pin_" + this.name + ", OUTPUT);");
            else
                source_code_stream.WriteLine("\tpinMode(pin_" + this.name + ", INPUT);");
        }

        public void arduino_code_generation_readInput(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("\t" + this.name + " = digitalRead(pin_"+this.name+");");
        }
        /// <summary>
        /// To inert instructions to update the outputs
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_update_Outputs(StreamWriter source_code_stream)
        {
            if (this.IsAnAction)
            {
                //source_code_stream.WriteLine("\t//Update OUTPUT : " + this.name);
                //source_code_stream.WriteLine("\tdigitalWrite(pin_" + this.name + ", " + this.name + ");");
                source_code_stream.WriteLine("\tif("+this.name+"_Old != "+this.name+") {digitalWrite(pin_" + this.name + ", " + this.name + ");}");
            }
        }
        public void arduino_code_generation_update_Old_Outputs(StreamWriter source_code_stream)
        {
            if (this.IsAnAction)
            {
                source_code_stream.WriteLine("\t" + this.name + "_Old = " + this.name + ";");
            }
        }
    }


    class Step11
    {
        public static int COUNT_STEPS = 0;
        public int LocalId;
        public int RefLocalId;
        private int Index;
        private string Name;
        public bool Negated;
        public bool isInitialStep;

        //time condition associated to this step
        public TimeCondition time_cond;
        public bool IsUsedInTimeConditions;
        ~Step11() { }
        public Step11(XmlNode XmlStepNode)
        {
            this.Index = COUNT_STEPS++;
            this.Name = XmlStepNode.Attributes[0].Value;
            //MessageBox.Show("new Step = " + this.Name);
            this.Negated = bool.Parse(XmlStepNode.Attributes[5].Value);
            this.isInitialStep = bool.Parse(XmlStepNode.Attributes[4].Value);
            this.LocalId = Int32.Parse(XmlStepNode.Attributes[3].Value);
            this.RefLocalId = XmlStepNode.LastChild.HasChildNodes == true ? Int32.Parse(XmlStepNode.LastChild.ChildNodes[0].Attributes[0].Value) : 0;

            //initializing the time condition
            this.time_cond = new TimeCondition(this.index, this.name);
            this.IsUsedInTimeConditions = false;
        }
        public Step11(string s_name)
        {
            this.Index = COUNT_STEPS++;
            this.Name = s_name;

            //initializing the time condition
            this.time_cond = new TimeCondition(this.index, this.name);
            this.IsUsedInTimeConditions = false;
        }
        public int index
        {
            get { return this.Index; }
            //set { this.Index = value; }
        }
        public string name
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        //Arduino code generation
        /// <summary>
        /// Declare STEPs variables
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_declare_varStep(StreamWriter source_code_stream)
        {
            if(this.isInitialStep)
                source_code_stream.WriteLine("boolean " + this.name + "_X = true, " + this.name + "_X_Old = true;");
            else
                source_code_stream.WriteLine("boolean " + this.name + "_X = false, " + this.name + "_X_Old = false;");
        }
        /// <summary>
        /// Declare STEPs timing variables for duration activity
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_declare_timeDuration(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("unsigned int " + this.name + "_duration = 0;");
        }
        /// <summary>
        /// To init the value of steps activity. But already done by arduino_code_generation_declare_varStep
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_init_varStep(StreamWriter source_code_stream)
        {
            if (this.isInitialStep)
                source_code_stream.WriteLine(this.name + "_X = true;");
            else
                source_code_stream.WriteLine(this.name + "_X = false;");
        }

        /// <summary>
        /// to update the timeDuration, or duration activity of the step 
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_time_update(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("\tif(" + this.name + "_X) {" + this.name + "_duration ++ ;} else {" + this.name + "_duration = 0;}");
        }

        /// <summary>
        /// Update Old Step variables state
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_update_OldStepVariables(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine(this.name + "_X_Old = " + this.name + "_X;");
        }
    }

    /// <summary>
    /// To manage Grafcet transitions elements
    /// </summary>
    class Transition11
    {
        public static int COUNT_TRANS = 0;
        public int LocalId;
        public int RefLocalId;
        private int Index;
        public string Condition;
        public string Condition_parsed;
        public string arduino_R_condition;
        //public string Condition_new_Microcontroller;

        // NNG: concerning time conditions
        public bool hasTimeCondition; // true if there is a time condition associated to this transition
        public List<int> timeCondition_StepsIndex;
        public List<int> timeCondition_durations; // in milliseconds
        // 
        public List<string> stack_Condition; // the conditions rewrited in postfix mode: a+b ==> a  b  +
        //public List<string> stack_Condition_Microcontroller;

        public int index
        {
            get { return this.Index; }
            //set { this.Index = value; }
        }

        ~Transition11() { }

        /// <param name="condition">condition associated to a transition</param>
        public Transition11(string condition)
        {
            this.Index = COUNT_TRANS;
            COUNT_TRANS++;
            this.Condition = condition;

            this.hasTimeCondition = false;
            timeCondition_StepsIndex = new List<int>();
            timeCondition_durations = new List<int>();
        }
        public Transition11(int local_id, int refLocal_id, string condition)
        {
            this.Index = COUNT_TRANS++;
            this.LocalId = local_id;
            this.RefLocalId = refLocal_id;
            this.Condition = condition;

            this.hasTimeCondition = false;
            timeCondition_StepsIndex = new List<int>();
            timeCondition_durations = new List<int>();
        }

        public void analyzeTransitionCondition()
        {
            if (this.Condition_parsed.Length > 0)
            {
                this.stack_Condition = Eval.analyze(this.Condition_parsed);
                //this.stack_Condition = Eval.analyze(this.Condition);
                //this.stack_Condition_Microcontroller = Eval.analyze(this.Condition_new_Microcontroller);
            }
            // display stack condition
            /*string ToShow = "Stack condition of ( " + this.Condition + " ) is : ";
            foreach (string st in stack_Condition)
                ToShow += st + "  ";
            MessageBox.Show(ToShow + " \nStack size = " + this.stack_Condition.Count);
            foreach (string st in stack_Condition_new)
                ToShow += st + "  ";
            MessageBox.Show(ToShow + " \nStack size = " + this.stack_Condition_new.Count);
            */
        }

        public Transition11(XmlNode XmlTransitionNode)
        {
            this.Index = COUNT_TRANS++;
            this.LocalId = Int32.Parse(XmlTransitionNode.Attributes[2].Value);
            this.RefLocalId = Int32.Parse(XmlTransitionNode.ChildNodes[1].ChildNodes[0].Attributes[0].Value);
            this.Condition = XmlTransitionNode.LastChild.ChildNodes[0].Attributes[0].Value;

            //has time condition ?
            this.hasTimeCondition = false;
            timeCondition_StepsIndex = new List<int>();
            timeCondition_durations = new List<int>();
        }

        public void addTransReferenceTimeCondition(int stepIndex, int duration)
        {
            this.timeCondition_StepsIndex.Add(stepIndex);
            this.timeCondition_durations.Add(duration);
            this.hasTimeCondition = true;
            Console.WriteLine("\nNew reference TC from TransIndex = " + this.index + " StepIndex=" + stepIndex + " Duration=" + duration);
        }

        public void simplify_active_step_in_conditions()
        {
            //Threat the case where there are [S1.X], [S1.X] becomes S1
            if (this.Condition_parsed.Contains("["))
            {
                this.Condition_parsed = this.Condition_parsed.Replace("[", "");
                this.Condition_parsed = this.Condition_parsed.Replace(".X]", "");
                this.Condition_parsed = this.Condition_parsed.Replace(".x]", "");
            }
        }

        public int EvaluateCondition(int[] Rdyn, List<string> VarTrans_name)
        {
            List<string> stack_Code = new List<string>();
            foreach (string item in this.stack_Condition)
            {
                string token = item;
                if (token == "TRUE" || token == "true")
                {
                    token = "1";
                }
                else
                {
                    if (token.Contains("["))
                    {
                        // [S1.X] becomes S1
                        token = token.Replace("[", "");
                        token = token.Replace(".X]", "");
                        token = token.Replace(".x]", "");
                    }

                    for (int j = 0; j < VarTrans_name.Count; j++)
                    {
                        if (token.Equals(VarTrans_name[j]))
                        {
                            token = Rdyn[j].ToString();
                        }
                    }
                }
                stack_Code.Add(token);
            }
            //Eval eval = new Eval();
            return Convert.ToInt32(Eval.Execute(stack_Code));
        }


        //Arduino code generation
        /// <summary>
        /// Arduino preparation of code generation : Modify transition conditions to create this.arduino_condition
        /// </summary>
        public void arduino_code_generation_prepare_clearing_condition() {
            this.arduino_R_condition = this.Condition;

            //Transform Timing variables
            int cond_index_begin;
            int cond_index_end; ;
            string stringCondition;
            
            int min_cond_lenght = "[S5.t>2m]".Length;
            int range;
            do{//loops two times in order to takle cases where [S0.x] occurs before [S0.t>4s]; for example: [S0.x] * [S0.t>4s]
                //Transform Timing condition
                do
                {
                    cond_index_begin = this.arduino_R_condition.IndexOf('[');
                    cond_index_end = this.arduino_R_condition.IndexOf(']');
                    range = cond_index_end - cond_index_begin + 1;

                    if (range >= min_cond_lenght && this.arduino_R_condition.Contains(">"))
                    {
                        stringCondition = this.arduino_R_condition.Substring(cond_index_begin, range);
                        int point_index = stringCondition.IndexOf('.');
                        string stepName = stringCondition.Substring(1, point_index - 1);
                        string st_duration = stringCondition.Substring(point_index + 3, stringCondition.Length - (5 + stepName.Length));
                        int duration_ms = TimeCondition.getDurationInMilliseconds(st_duration);
                        //Update arduino_condition. "TIMES_UNIT" will be replaced by "*frequency_timer" after the replacement of * by &&
                        //string new_stringCondition = "(" + stepName + "_duration > " + (duration_ms / GrafcetClass.g7_TIME_UNIT_TimingConstraints) + "TIMES_UNIT)";
                        string new_stringCondition = "(" + stepName + "_duration > " + (duration_ms / GrafcetClass.g7_Board_TimeUnit_TimerStepActivity) + ")";
                        this.arduino_R_condition = this.arduino_R_condition.Replace(stringCondition, new_stringCondition);//duration_ms / 1000

                        //MessageBox.Show("Transition: this.arduino_condition = " + this.arduino_condition);
                    }

                    cond_index_begin = this.arduino_R_condition.IndexOf('[');
                    cond_index_end = this.arduino_R_condition.IndexOf(']');
                    range = cond_index_end - cond_index_begin + 1;
                } while (range >= min_cond_lenght && this.arduino_R_condition.Contains(">"));

                //MessageBox.Show("Arduino Condition After Condition Time parse : " + this.arduino_condition);

                //Now Time conditions have finished. If there are [ symbols, it is because of step variable 
                //Transform Step variable in transition receptivity
                do
                {
                    cond_index_begin = this.arduino_R_condition.IndexOf('[');
                    cond_index_end = this.arduino_R_condition.IndexOf(']');
                    range = cond_index_end - cond_index_begin + 1;
                    if (this.arduino_R_condition.Contains(".X]"))
                    {
                        stringCondition = this.arduino_R_condition.Substring(cond_index_begin, range);
                        int point_index = stringCondition.IndexOf('.');
                        string stepName = stringCondition.Substring(1, point_index - 1);
                        this.arduino_R_condition = this.arduino_R_condition.Replace(stringCondition, stepName + "_X");
                        //MessageBox.Show("Arduino step variable : " + stepName + "_X \t 1-Arduino Condition : " + this.arduino_condition);
                    }
                } while (this.arduino_R_condition.Contains("[") && this.arduino_R_condition.Contains(".X"));

                //It is necessary to parse this condition to build a tree and to generate the appropriate condition for Arduino
                //Lets consider only Simple Rising Edge
                while (this.arduino_R_condition.Contains('^'))
                {
                    int index_begin = this.arduino_R_condition.IndexOf("^") + 1;
                    int index_end = index_begin;
                    while ((index_end < this.arduino_R_condition.Length) && (this.arduino_R_condition.ElementAt(index_end) != '+') && (this.arduino_R_condition.ElementAt(index_end) != '*') && (this.arduino_R_condition.ElementAt(index_end) != ')'))
                    {
                        index_end++;
                    }
                    index_end--;
                    int var_range = index_end - index_begin + 1;
                    string varName = this.arduino_R_condition.Substring(index_begin, var_range);

                    string str_condition = "(!" + varName + "_Old && " + varName + ")";
                    this.arduino_R_condition = this.arduino_R_condition.Replace("^" + varName, str_condition);

                    //MessageBox.Show(this.Condition + "\nvarName: " + varName + "\n str_condition: " + str_condition);
                }
                //MessageBox.Show("Arduino Condition After Rising Edge parse : \n" + this.arduino_condition);
            } while (this.arduino_R_condition.Contains("["));

            this.arduino_R_condition = this.arduino_R_condition.Replace("+"," || ");
            this.arduino_R_condition = this.arduino_R_condition.Replace("*", " && ");
            //this.arduino_condition = this.arduino_condition.Replace("TIMES_UNIT", "*frequency_timer");
            //MessageBox.Show("Arduino Condition After Step Variable parse : " + this.arduino_condition);
        }

        /// <summary>
        /// Arduino code generation: Declare Transitions's Validated Variables
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_add_declare_ValidatedTransitionsVariable(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("boolean VT_" + (this.Index + 1) + " = false;");
        }

        /// <summary>
        /// Arduino code generation: Declare Transitions's Receptivities Variables
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_add_declare_ReceptivitiesTransitionsVariable(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("boolean R_" + (this.Index + 1) + " = false;");
        }

        /// <summary>
        /// Declare Transitions receptivities variables
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_add_declare_ReceptivityVariable(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("boolean TR_" + (this.Index + 1) + " = false;");
        }
        /// <summary>
        /// Arduino code generation: Evaluate Transitions's Receptivities R_i
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_add_TransitionsReceptivities(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("\tR_" + (this.Index + 1) + " = " + this.arduino_R_condition + ";");
        }
        /// <summary>
        /// Arduino code generation: Evaluate Transitions's Firing condition TR_i
        /// </summary>
        /// <param name="source_code_stream"></param>
        public void arduino_code_generation_add_FiringTRansitionCondition(StreamWriter source_code_stream)
        {
            source_code_stream.WriteLine("\tTR_" + (this.Index + 1) + " = VT_" + (this.Index + 1) + " && R_" + (this.Index + 1) + ";");
        }
    }


    class TimeCondition
    {
        public int stepIndex;
        public string stepName;
        private System.Timers.Timer timer;
        //private bool isSatisfied; // is true if the elapsed time duration is satisfied 
        private bool isActive; // because many conditions could use this step. if it is already active, 
        //and another step wants to activate it again, we would not do it
        public List<string> time_variables_names;
        private int stepClock_number; // the timer is going to evoluate steps by steps until the associated step becomes unactive
        private static int stepClock_time_duration = 500;
        private bool stop_tc;
        ~TimeCondition()
        {
            //this.timer.Dispose(); 
        }

        public TimeCondition(int sIndex, string stepName)
        {
            this.stepIndex = sIndex;
            this.stepName = stepName;
            this.time_variables_names = new List<string>();
            this.isActive = false;
        }

        public string addTimeVariableName(int trans_index, string step_name, int duration_ms)
        {
            string tv_name = getTimeVariableName(trans_index, step_name, duration_ms);
            if (!this.time_variables_names.Contains(tv_name))
            {
                this.time_variables_names.Add(tv_name);
            }
            return tv_name;
        }

        public static string getTimeVariableName(int trans_index, string step_name, int duration_ms)
        {
            return "_tv_T" + trans_index + "_" + step_name + "_" + duration_ms;
        }

        public bool IsActive()
        {
            return this.isActive; ;
        }

        private void timer_Tick(object sender, EventArgs e) //ElapsedEventArgs e
        {
            this.stepClock_number++;
            Console.WriteLine("\nTimer tick : Step n° " + stepIndex + " Duration:" + this.timer_duration() + " ms");

            if (this.stop_tc)
            {
                this.isActive = false;
                this.timer.Stop();
                this.timer.Dispose();
                Console.WriteLine("STOP TC on Step n°" + this.stepIndex + " stopped after " + this.timer_duration() + " ms");
                this.stepClock_number = 0;
            }
        }
        public bool Activate()
        {

            if (!this.isActive)
            {
                this.stepClock_number = 0;
                this.stop_tc = false;
                //concerning the timer
                this.timer = new System.Timers.Timer();
                this.timer.Elapsed += new ElapsedEventHandler(this.timer_Tick);
                this.timer.Interval = stepClock_time_duration;
                this.timer.AutoReset = true; // The Timer should raise the Elapsed event each time the interval elapses
                this.timer.Start();

                this.isActive = true;
                Console.WriteLine("Time variable for Step S" + this.stepIndex + " Activated \n");
            }
            return this.isActive;
        }

        public bool Disable()
        {
            if (this.isActive)
            {
                this.stop_tc = true;
            }
            return this.stop_tc;
            // or return this.isActive && (this.stop_tc = false);
        }

        public int timer_duration()
        {
            return this.stepClock_number * stepClock_time_duration;
        }
        public bool has_delayed(int duration_millis)
        {
            //permits to know if this specific duration has been elapsed
            //The value returned permits to validate a time condition
            return this.isActive && (this.timer_duration() >= duration_millis);
        }
        public static void set_stepClock_time_duration(int duration_millis)
        {
            stepClock_time_duration = duration_millis;
        }
        internal static int getDurationInMilliseconds(string duration_st)
        {
            int duration_int = 0, index_unit = 0;
            while ("0123456789".Contains(duration_st[index_unit]))
                index_unit++;
            string duration = duration_st.Substring(0, index_unit);
            string t_unit = duration_st.Substring(index_unit, duration_st.Length - index_unit);

            duration_int = Int32.Parse(duration);
            switch (t_unit)
            {
                case "ms":
                    duration_int *= 1;
                    break;
                case "s":
                    duration_int *= 1000;
                    break;
                case "m":
                    duration_int *= 60 * 1000;
                    break;
                case "h":
                    duration_int *= 60 * 60 * 1000;
                    break;
                case "d":
                    duration_int *= 24 * 60 * 60 * 1000;
                    break;
                default:
                    duration_int = 0;
                    break;
            }
            //MessageBox.Show("Dutation =" + duration + "  Unit = " + unit + " In milliseconds : " + duration_int);
            return duration_int;
        }
    }


    // SimultaneousConv and SelectionDiv : Elements that are downstream steps and upstream transitions
    // SimultaneousDiv and SelectionConv : Elements that are downstream transitions and upstream steps
    // We model them using this same class

    class ConvDiv11
    {
        public int LocalId;
        public int ReflocalId; // usefull when it is a simultaneousDivergence or a selectionDivergence
        public List<int> RefLocalIdList; // usefull when it is a simultaneous convergence or a selection convergence
        public bool isADivergenceConnection;
        public ConvDiv11(XmlNode XmlConvDivNode)
        {
            this.LocalId = Int32.Parse(XmlConvDivNode.Attributes[0].Value);
            this.RefLocalIdList = new List<int>();
            if (XmlConvDivNode.LastChild.ChildNodes.Count == 1)
            {
                //It is a simultaneousDivergence or a selectionDivergence
                this.ReflocalId = Int32.Parse(XmlConvDivNode.LastChild.LastChild.Attributes[0].Value);
                this.RefLocalIdList.Add(this.ReflocalId);
                this.isADivergenceConnection = true;
                //MessageBox.Show(XmlConvDivNode.Name + " Found !! LocalId=" + this.LocalId + "  Reference : refLocalId=" + this.ReflocalId);
            }
            else
            {
                // it is a simultaneous convergence or a selection convergence
                foreach (XmlNode connection in XmlConvDivNode.LastChild.ChildNodes)
                    this.RefLocalIdList.Add(Int32.Parse(connection.Attributes[0].Value));
                this.isADivergenceConnection = false;
            }
        }
    }
}
