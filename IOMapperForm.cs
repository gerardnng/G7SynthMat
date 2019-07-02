using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrafcetConvertor.BoardDefinition;

namespace GrafcetConvertor
{
    public partial class IOMapperForm : Form
    {
        private System.Windows.Forms.Label[] input_number_labels;
        private System.Windows.Forms.Label[] input_system_labels;
        private System.Windows.Forms.ComboBox[] input_board_device_comboBoxes;

        private System.Windows.Forms.Label[] output_number_labels;
        private System.Windows.Forms.Label[] output_system_labels;
        private System.Windows.Forms.ComboBox[] output_board_device_comboBoxes;
        
        public GrafcetClass g7_current;

        private int board_index_displayed = - 1;
        private int y_begin_pos;

        public IOMapper io_mapper;

        public IOMapperForm(IOMapper io_map, GrafcetClass g7)
        {
            this.InitializeComponent();
            this.io_mapper = io_map;
            this.g7_current = g7;
            this.initialize_display_settings();
        }

        public void initialize_display_settings() {
            this.load_IOMapper_settings();
            this.init_display_IOMappingInfos();
        }

        public void load_IOMapper_settings() {
            this.io_mapper = new IOMapper();
            this.Text = "Mapping of Inputs/Outputs. Board (" +
            "µC : " + this.io_mapper.get_device() + ",  " +
            "Family : " + this.io_mapper.get_family() + ",  " +
            "Processor : " + this.io_mapper.get_processor() + ")";
            this.y_begin_pos = 40;
        }

        public void init_display_IOMappingInfos() {
            this.board_index_displayed = IOMapper.selected_µC_index; //Why ?
            this.InitializeComponent_Inputs();
            this.InitializeComponent_Outputs();
            /*
            if (this.g7_current != g7 || this.board_index_displayed != this.io_mapper.selected_µC_index) {
                this.g7_current = g7;
                this.board_index_displayed = this.io_mapper.selected_µC_index;
                this.InitializeComponent_Inputs();
                this.InitializeComponent_Outputs();
            }
             */
        }



        private void OK_board_button_Click(object sender, EventArgs e)
        {
            //Search for a repetition inside PINs used to map variables
            if (this.there_are_repetitions_in_pin_mapping())
            {
                MessageBox.Show("Error: There are some Pins used many times to map.\n Check and correct your mapping !");
            }
            else {
                //Update the modes of the pins that are mapped, and also indicate the mapped variable
                /*NB:  The objective is to do such that inside< this.iomapper.micros_list[this.iomapper.current_board_index].pins_list_digital>
                       All the IOPin have the mode configured to "INPUT", "OUTPUT" or null. After it, They can be directly used when generating codes
                */
                //Reinitialization of all PINs mode
                this.io_mapper.reinit_All_PINs();

                //Updating the mode of PINs & the mapped variables
                for (int i_input = 0; i_input < this.g7_current.number_inputs; i_input++)
                {
                    string selected_address = this.input_board_device_comboBoxes[i_input].SelectedItem.ToString();
                    string input_variable = this.g7_current.input_names[i_input]; ;
                    this.io_mapper.update_PIN(selected_address, input_variable, IOPin.pin_mode_in);
                }

                for (int i_output = 0; i_output < this.g7_current.number_actions; i_output++)
                {
                    string selected_address = this.output_board_device_comboBoxes[i_output].SelectedItem.ToString();
                    string output_variable = this.g7_current.actions_names[i_output];
                    this.io_mapper.update_PIN(selected_address, output_variable, IOPin.pin_mode_out);
                }
  
                this.Visible = false;
                this.io_mapper.is_mapped = true;
                //MessageBox.Show("Is mapped value = " + this.io_mapper.is_mapped);
            }
        }
       
        private bool there_are_repetitions_in_pin_mapping(){
            int IN_displayed_size = this.g7_current.number_inputs;
            int OUT_displayed_size = this.g7_current.number_actions;

            int Total_number_PINs = this.io_mapper.getCurrentBoard().pins_list_digital.Count;

            //Build the list of selected PINs
            List<string> selected_pins = new List<string>();
            for (int in_pin_num = 0; in_pin_num < IN_displayed_size; in_pin_num++)
            {
                selected_pins.Add(this.input_board_device_comboBoxes[in_pin_num].SelectedItem.ToString());
            }
            for (int out_pin_num = 0; out_pin_num < OUT_displayed_size - 1; out_pin_num++)
            {
                selected_pins.Add(this.output_board_device_comboBoxes[out_pin_num].SelectedItem.ToString());
            }

            //Analyse the list of selected PINs
            for (int pin_num = 0; pin_num < Total_number_PINs; pin_num++) { 
                IOPin io_pin = this.io_mapper.getCurrentBoard().pins_list_digital[pin_num];
                int count_nb_mapped = selected_pins.Count(adr => adr == io_pin.pin_address);
                if (count_nb_mapped > 1)
                    return true;
            }
            return false;
        }

        private void InitializeComponent_Inputs()
        {
            //MessageBox.Show("InitializeComponent_Inputs called");

            this.input_number_labels = new System.Windows.Forms.Label[this.g7_current.number_inputs];
            this.input_system_labels = new System.Windows.Forms.Label[this.g7_current.number_inputs];
            this.input_board_device_comboBoxes = new System.Windows.Forms.ComboBox[this.g7_current.number_inputs];

            this.input_groupBox.Controls.Clear();

            // 
            // input_number_labels
            // 

            for (int i_input = 0; i_input < this.g7_current.number_inputs; i_input++)
            {
                this.input_number_labels[i_input] = new System.Windows.Forms.Label();
                this.input_number_labels[i_input].AutoSize = true;
                //this.input_number_labels[i_input].BackColor = System.Drawing.Color.LightSteelBlue;
                this.input_number_labels[i_input].Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.input_number_labels[i_input].ForeColor = System.Drawing.Color.Indigo;
                this.input_number_labels[i_input].Location = new System.Drawing.Point(6, this.y_begin_pos + 30 * i_input);
                this.input_number_labels[i_input].Name = "input_number_label4";
                this.input_number_labels[i_input].Size = new System.Drawing.Size(21, 20);
                this.input_number_labels[i_input].TabIndex = 52 + 10 * i_input;
                this.input_number_labels[i_input].Text = (i_input + 1) + "-";
                this.input_number_labels[i_input].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.input_number_labels[i_input].Enabled = true;
                this.input_groupBox.Controls.Add(this.input_number_labels[i_input]);
            }


            // 
            // input_system_labels
            // 

            for (int i_input = 0; i_input < this.g7_current.number_inputs; i_input++)
            {
                this.input_system_labels[i_input] = new System.Windows.Forms.Label();
                this.input_system_labels[i_input].AutoSize = true;
                //this.input_system_labels[i_input].BackColor = System.Drawing.Color.MediumSpringGreen;
                this.input_system_labels[i_input].Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.input_system_labels[i_input].ForeColor = System.Drawing.Color.Black;
                this.input_system_labels[i_input].Location = new System.Drawing.Point(38, this.y_begin_pos + 30 * i_input);
                this.input_system_labels[i_input].Name = "input_system_label1";
                this.input_system_labels[i_input].Size = new System.Drawing.Size(29, 20);
                this.input_system_labels[i_input].TabIndex = 24 + 10 * i_input;
                this.input_system_labels[i_input].Text = this.g7_current.input_names[i_input];
                this.input_system_labels[i_input].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                this.input_groupBox.Controls.Add(this.input_system_labels[i_input]);
            }


            // 
            // input_board_device_comboBoxes
            // 
            for (int i_input = 0; i_input < this.g7_current.number_inputs; i_input++)
            {
                this.input_board_device_comboBoxes[i_input] = new System.Windows.Forms.ComboBox();
                this.input_board_device_comboBoxes[i_input].Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.input_board_device_comboBoxes[i_input].ForeColor = System.Drawing.Color.Blue;
                this.input_board_device_comboBoxes[i_input].FormattingEnabled = true;
                this.input_board_device_comboBoxes[i_input].Location = new System.Drawing.Point(136, this.y_begin_pos + 30 * i_input);
                this.input_board_device_comboBoxes[i_input].Name = "input_board_device_comboBox";
                this.input_board_device_comboBoxes[i_input].Size = new System.Drawing.Size(150, 23);
                this.input_board_device_comboBoxes[i_input].TabIndex = 51 + 16 * i_input;
                
                //Add board devices

                int N_Pins = this.io_mapper.getCurrentBoard().pins_list_digital.Count;

                for (int pin_num = 0; pin_num < N_Pins; pin_num++)
                {
                    IOPin in_Pin = this.io_mapper.getCurrentBoard().pins_list_digital[pin_num];
                    string address = in_Pin.pin_address;
                    //MessageBox.Show("BEFORE : in_Pin.type = " + in_Pin.type);
                    if (in_Pin.type.Equals(IOPin.pin_type_io) | in_Pin.type.Equals(IOPin.pin_type_in))
                    {
                        this.input_board_device_comboBoxes[i_input].Items.Add(address);
                    }
                    else
                    {
                        MessageBox.Show("Error : The PIN address " + address + " can not be configured in INPUT");
                    }
                }
                
                if (i_input < N_Pins)
                {
                    this.input_board_device_comboBoxes[i_input].SelectedIndex = i_input;
                }
                else
                {   //Should not run that part. If it is run, there is a lack of control 
                    MessageBox.Show("ERROR: The input variable " + this.g7_current.input_names[i_input] + " cannot be mapped. \n\tOut of range of input devices of the selected board !");
                }

                this.input_groupBox.Controls.Add(this.input_board_device_comboBoxes[i_input]);
            }

        }

        private void InitializeComponent_Outputs()
        {
            this.output_number_labels = new System.Windows.Forms.Label[this.g7_current.number_actions];
            this.output_system_labels = new System.Windows.Forms.Label[this.g7_current.number_actions];
            this.output_board_device_comboBoxes = new System.Windows.Forms.ComboBox[this.g7_current.number_actions];

            this.output_groupBox.Controls.Clear();

            // 
            // output_number_labels
            // 
            for (int i_output = 0; i_output < this.g7_current.number_actions; i_output++)
            {
                this.output_number_labels[i_output] = new System.Windows.Forms.Label();
                this.output_number_labels[i_output].AutoSize = true;
                //this.output_number_labels[i_output].BackColor = System.Drawing.Color.LightSteelBlue; 
                this.output_number_labels[i_output].Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.output_number_labels[i_output].ForeColor = System.Drawing.Color.Indigo;
                this.output_number_labels[i_output].Location = new System.Drawing.Point(6, this.y_begin_pos + 30 * i_output);
                this.output_number_labels[i_output].Name = "input_system_label1";
                this.output_number_labels[i_output].Size = new System.Drawing.Size(29, 20);
                this.output_number_labels[i_output].TabIndex = 24 + 10 * i_output;
                this.output_number_labels[i_output].Text = (i_output + 1) + "-";
                this.output_number_labels[i_output].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                this.output_groupBox.Controls.Add(this.output_number_labels[i_output]);
            }


            // 
            // output_system_labels
            // 
            for (int i_output = 0; i_output < this.g7_current.number_actions; i_output++)
            {
                this.output_system_labels[i_output] = new System.Windows.Forms.Label();
                this.output_system_labels[i_output].AutoSize = true;
                //this.output_system_labels[i_output].BackColor = System.Drawing.Color.Thistle;
                this.output_system_labels[i_output].Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.output_system_labels[i_output].ForeColor = System.Drawing.Color.Black;
                this.output_system_labels[i_output].Location = new System.Drawing.Point(38, this.y_begin_pos + 30 * i_output);
                this.output_system_labels[i_output].Name = "input_number_label4";
                this.output_system_labels[i_output].Size = new System.Drawing.Size(21, 20);
                this.output_system_labels[i_output].TabIndex = 52 + 10 * i_output;
                this.output_system_labels[i_output].Text = this.g7_current.actions_names[i_output];
                this.output_system_labels[i_output].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.output_system_labels[i_output].Enabled = true;
                this.output_groupBox.Controls.Add(this.output_system_labels[i_output]);
            }

            // 
            // output_board_device_comboBoxes
            // 
            
            for (int i_output = 0; i_output < this.g7_current.number_actions; i_output++)
            {
                this.output_board_device_comboBoxes[i_output] = new System.Windows.Forms.ComboBox();
                this.output_board_device_comboBoxes[i_output].Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.output_board_device_comboBoxes[i_output].ForeColor = System.Drawing.Color.Red;
                this.output_board_device_comboBoxes[i_output].FormattingEnabled = true;
                this.output_board_device_comboBoxes[i_output].Location = new System.Drawing.Point(136, this.y_begin_pos + 30 * i_output);
                this.output_board_device_comboBoxes[i_output].Name = "input_board_device_comboBox";
                this.output_board_device_comboBoxes[i_output].Size = new System.Drawing.Size(150, 23);
                this.output_board_device_comboBoxes[i_output].TabIndex = 51 + 17 * i_output;
                this.output_board_device_comboBoxes[i_output].SelectedIndexChanged += new EventHandler(output_board_device_comboBoxes_SelectedIndexChanged);

                //Add output board device 
                int N_Pins = this.io_mapper.getCurrentBoard().pins_list_digital.Count;
                for (int pin = 0; pin < N_Pins; pin++)
                {
                    IOPin in_Pin = this.io_mapper.getCurrentBoard().pins_list_digital[pin];
                    string pin_adr = in_Pin.pin_address;

                    if (in_Pin.type.Equals(IOPin.pin_type_io) | in_Pin.type.Equals(IOPin.pin_type_out))
                    {
                        this.output_board_device_comboBoxes[i_output].Items.Add(pin_adr);
                    }
                    else
                    {
                        MessageBox.Show("Error : The PIN address " + pin_adr + " can not be configured in OUTPUT");
                    }
                }
                if (i_output < N_Pins)
                {
                    this.output_board_device_comboBoxes[i_output].SelectedIndex = this.g7_current.number_inputs + i_output;
                }
                else
                {//Should not run that part. If it is run, there is a lack of control 
                    MessageBox.Show("ERROR: The output variable " + this.g7_current.actions_names[i_output] + " cannot be mapped. \n\tOut of range of output devices of the selected board !");
                }
                
                this.output_groupBox.Controls.Add(this.output_board_device_comboBoxes[i_output]);
            }
        }
        
        public void clear_group_Boxes() {
            this.input_groupBox.Controls.Clear();
            this.output_groupBox.Controls.Clear();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        //To save the selected index of a comboBox that contains Pins addresses
        private void output_board_device_comboBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox combo = (System.Windows.Forms.ComboBox)sender;
            //MessageBox.Show("Selected = " + combo.SelectedItem.ToString());
        }

        //To hide The close button and the others
        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }
    }
}
