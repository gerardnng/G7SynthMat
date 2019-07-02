
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using SerialPortTerminal;
using SerialPortTerminal.Properties;
using Unisim;
using System.Reflection;
using System.Diagnostics;
using System.Text;

using GrafcetConvertor.BoardDefinition;

namespace GrafcetConvertor
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class G7_Form : Form
    {
        public Button open_g7_button;
        public TextBox static_g7_textBox;
        public OpenFileDialog openXmlG7;
        public TextBox simul_textBox;
        public System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Button start_button;
        public Label label5;
        public Button clearAll_button;
        private Button button6;
        private Timer timer1;
        public Button step_step_button;
        private BindingSource form1BindingSource;
        public TextBox PIC_Message_textBox;
        //private Timer timer2;
        private IContainer components;

        private List<int> clock_values; //To manage clock intervals values
        private int[] Board_TU_values = { 1000, 500, 100, 50, 1 }; //To manage Time Unit of the steps activity, in order to help the evaluation of
                                                                    // timing conditions like [S0.t>1100ms]
        public string Directory_name_current;
        private string File_name_current;
        private string File_name_only_current;
        private bool display_all_dynamic_vectors;
        private List<GrafcetClass> GrafcetsList; // List of Grafcets

        //NEW Ap 2016
        private IOMapperForm io_mapper_form; //To manage informations on boards
        private IOMapper io_mapper_settings; //Contains all the informations about the selected board and the IO mapping

        private int selected_g7_i;
        public Button stop_button;
        public Label label2;
        public Button clear_button;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private CheckBox[] tabCheckbox;
        public Button checkAll_button;
        public Button send_pic_message_button;
        public Button open_serial_port_button;
        public Button open_simulations_button;
        private GroupBox groupBox2;
        public Button reinit_button;
        private GroupBox groupBox3;
        public Button save_g7_text_button;
        public Button open_g7_text_button;
        private GroupBox groupBox4;
        public Label label4;
        public Label label6;
        private ComboBox clock_interval_comboBox;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private CheckBox display_all_dynamic_vector_checkbox;
        private TabControl board_descript;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        public TextBox condensed_g7_data_textBox;
        public Label label3;
        public Label label8;
        public Label label7;
        public Button generate_hex_board_button1;
        public Button save_hex_data_button2;
        public Button open_hex_board_data_button1;
        private ComboBox g7_names_comboBox1;
        private TextBox size_textBox1;
        private CheckedListBox input_checkedListBox;
        public Button button1;
        private TabPage tabPage5;
        public Button export_button;
        public Button build_project_button;
        private GroupBox groupBox7;
        public Label label9;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        public Button upload_button;
        public Label label1;
        public TextBox textBox1;
        public Button open_g7_editor_UniSim_button;
        private GroupBox groupBox10;
        public Button save_g7_hex_button;
        public Button open_g7_hex_button;
        public Button io_map_button;
        private ComboBox board_device_comboBox;
        public Label label10;
        private TextBox board_processor_textBox3;
        private TextBox board_family_textBox3;
        public Button reload_boards_settings_button;
        public Button code_generation_button;
        private CheckBox code_vm_checkBox;
        private CheckBox code_analyse_efficiency_checkBox;
        private CheckBox code_debug_checkBox;
        private TabPage tabPage4;
        private GroupBox groupBox12;
        public Label label14;
        public Button Delete_Board_Button;
        public Button Edit_Board_Button;
        public Button NewBoard_Button;
        private TextBox board_manufacturer;
        public Label label15;
        private TextBox board_family;
        public Label label13;
        private TextBox board_name;
        private GroupBox groupBox13;
        private ComboBox processorUnit;
        private ComboBox wordMemory_comboBox;
        public Label label16;
        private TextBox processorSpeed;
        public Label label17;
        public Label label18;
        private GroupBox groupBox14;
        private ComboBox ram_unit;
        private TextBox eeprom_size;
        public Label label22;
        private TextBox flash_size;
        public Label label20;
        private TextBox ram_size;
        public Label label21;
        private ComboBox eeprom_unit;
        private ComboBox flash_unit;
        public Label label24;
        public Label label23;
        private CheckBox eeprom_check;
        private GroupBox groupBox15;
        public Label label25;
        public Label label26;
        private TextBox file_timer_config;
        public Button Timer_Btn_Select;
        private GroupBox groupBox16;
        public Button Save_Board_Button;
        public Button Cancel_Board_Button;
        private TextBox pin_number_add;
        public Label label19;
        public Button PINs_Add;
        public Button Pins_ViewList;
        public Label label11;
        private ComboBox Board_Time_Period_comboBox;
        private GroupBox groupBox11;
        public Button select_read_input_Button;
        public Button select_write_output_Button;
        private TextBox input_reading_file;
        private TextBox output_writing_file;
        public Label label12;
        private GroupBox groupBox1;


        public G7_Form()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.disable_simulations_buttons();
            this.send_pic_message_button.Enabled = false;
            this.save_hex_data_button2.Enabled = false;
            this.generate_hex_board_button1.Enabled = false;
            this.save_g7_text_button.Enabled = false;
            this.save_g7_hex_button.Enabled = false;
            this.save_g7_hex_button.Enabled = false;
            this.open_simulations_button.Enabled = false;
            this.display_all_dynamic_vectors = false;

            this.board_device_comboBox.Enabled = false;
            this.build_project_button.Enabled = false;
            this.code_generation_button.Enabled = false;
            this.export_button.Enabled = false;
            this.upload_button.Enabled = false;
            this.io_map_button.Enabled = false;

            //Microconrolers data
            this.eeprom_size.Enabled = false;
            this.eeprom_unit.Enabled = false;
            this.enableBoardDefinitionSettings(false);

            this.initClockInterval();
            this.init_Time_Unit_of_conditions_values();
            this.initBoardsinfos();
            //this.initIOManager();
            this.GrafcetsList = new List<GrafcetClass>();
        }

        private void initClockInterval()
        {
            // click interval definition
            this.clock_values = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                this.clock_interval_comboBox.Items.Add(i);
                this.clock_values.Add(i);
            }
            for (int i = 10; i < 100; i += 10)
            {
                this.clock_interval_comboBox.Items.Add(i);
                this.clock_values.Add(i);
            }
            for (int i = 100; i < 1000; i += 100)
            {
                this.clock_interval_comboBox.Items.Add(i);
                this.clock_values.Add(i);
            }
            for (int i = 1000; i < 10000; i += 1000)
            {
                this.clock_interval_comboBox.Items.Add(i);
                this.clock_values.Add(i);
            }
            this.clock_interval_comboBox.SelectedIndex = this.clock_values.IndexOf(GrafcetClass.simulation_g7_clock_interval);
        }

        private void init_Time_Unit_of_conditions_values()
        {
            for (int i = 0; i < this.Board_TU_values.Length; i++)
            {
                this.Board_Time_Period_comboBox.Items.Add(this.Board_TU_values[i]);
            }
            //The more interestion value could be 100. If Duration = 900ms, ifthen[TD= 100 ms ==> Coef = 9], whereas ifthen[TD= 1000 ms ==> Coef = 0]
            GrafcetClass.g7_Board_TimeUnit_TimerStepActivity = 100;
            this.Board_Time_Period_comboBox.SelectedIndex = 2;
        }

        private void initBoardsinfos() {
            this.ram_unit.Items.Add("Ko");
            this.ram_unit.Items.Add("Mo");
            this.ram_unit.SelectedIndex = 0;

            this.flash_unit.Items.Add("Ko");
            this.flash_unit.Items.Add("Mo");
            this.flash_unit.SelectedIndex = 0;

            this.eeprom_unit.Items.Add("Ko");
            this.eeprom_unit.Items.Add("Mo");
            this.eeprom_unit.SelectedIndex = 0;

            this.processorUnit.Items.Add("MHz");
            this.processorUnit.Items.Add("MIPs");
            this.processorUnit.SelectedIndex = 0;

            this.wordMemory_comboBox.Items.Add("8 bits");
            this.wordMemory_comboBox.Items.Add("16 bits");
            this.wordMemory_comboBox.Items.Add("32 bits");
            this.wordMemory_comboBox.Items.Add("64 bits");
            this.wordMemory_comboBox.SelectedIndex = 0;
        }

        private void initIOManager()
        {
            this.board_device_comboBox.Items.Clear();

            this.io_mapper_settings = new IOMapper();
            foreach (Microcontroller aBoad in this.io_mapper_settings.micros_list)
            {
                this.board_device_comboBox.Items.Add(aBoad.name);
            }
            this.board_device_comboBox.Items.Add("None");

            IOMapper.selected_µC_index = -1;
            this.board_device_comboBox.SelectedIndex = this.io_mapper_settings.micros_list.Count;
            //Make sure that there is not an instance of IO_MapperèForm
            this.io_mapper_form = null;
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(G7_Form));
            this.open_g7_button = new System.Windows.Forms.Button();
            this.static_g7_textBox = new System.Windows.Forms.TextBox();
            this.openXmlG7 = new System.Windows.Forms.OpenFileDialog();
            this.simul_textBox = new System.Windows.Forms.TextBox();
            this.start_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.clearAll_button = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.step_step_button = new System.Windows.Forms.Button();
            this.PIC_Message_textBox = new System.Windows.Forms.TextBox();
            this.stop_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.clear_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.checkAll_button = new System.Windows.Forms.Button();
            this.send_pic_message_button = new System.Windows.Forms.Button();
            this.open_serial_port_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.open_simulations_button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reinit_button = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.input_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.save_g7_text_button = new System.Windows.Forms.Button();
            this.open_g7_text_button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.clock_interval_comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.display_all_dynamic_vector_checkbox = new System.Windows.Forms.CheckBox();
            this.board_descript = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.save_g7_hex_button = new System.Windows.Forms.Button();
            this.open_g7_hex_button = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.export_button = new System.Windows.Forms.Button();
            this.upload_button = new System.Windows.Forms.Button();
            this.build_project_button = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.code_generation_button = new System.Windows.Forms.Button();
            this.code_debug_checkBox = new System.Windows.Forms.CheckBox();
            this.code_analyse_efficiency_checkBox = new System.Windows.Forms.CheckBox();
            this.code_vm_checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.board_processor_textBox3 = new System.Windows.Forms.TextBox();
            this.board_device_comboBox = new System.Windows.Forms.ComboBox();
            this.board_family_textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.reload_boards_settings_button = new System.Windows.Forms.Button();
            this.io_map_button = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.save_hex_data_button2 = new System.Windows.Forms.Button();
            this.open_hex_board_data_button1 = new System.Windows.Forms.Button();
            this.generate_hex_board_button1 = new System.Windows.Forms.Button();
            this.condensed_g7_data_textBox = new System.Windows.Forms.TextBox();
            this.size_textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.Save_Board_Button = new System.Windows.Forms.Button();
            this.Cancel_Board_Button = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.eeprom_check = new System.Windows.Forms.CheckBox();
            this.eeprom_unit = new System.Windows.Forms.ComboBox();
            this.flash_unit = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.ram_unit = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.eeprom_size = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.flash_size = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.ram_size = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.Delete_Board_Button = new System.Windows.Forms.Button();
            this.Edit_Board_Button = new System.Windows.Forms.Button();
            this.NewBoard_Button = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.file_timer_config = new System.Windows.Forms.TextBox();
            this.Board_Time_Period_comboBox = new System.Windows.Forms.ComboBox();
            this.Timer_Btn_Select = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.processorUnit = new System.Windows.Forms.ComboBox();
            this.wordMemory_comboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.processorSpeed = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.select_read_input_Button = new System.Windows.Forms.Button();
            this.select_write_output_Button = new System.Windows.Forms.Button();
            this.output_writing_file = new System.Windows.Forms.TextBox();
            this.input_reading_file = new System.Windows.Forms.TextBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.pin_number_add = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.PINs_Add = new System.Windows.Forms.Button();
            this.Pins_ViewList = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.board_manufacturer = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.board_family = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.board_name = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.g7_names_comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.open_g7_editor_UniSim_button = new System.Windows.Forms.Button();
            this.form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.board_descript.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // open_g7_button
            // 
            this.open_g7_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_g7_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_g7_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_g7_button.Location = new System.Drawing.Point(21, 34);
            this.open_g7_button.Name = "open_g7_button";
            this.open_g7_button.Size = new System.Drawing.Size(182, 30);
            this.open_g7_button.TabIndex = 0;
            this.open_g7_button.Text = "OPEN A GRAFCET";
            this.open_g7_button.UseVisualStyleBackColor = false;
            this.open_g7_button.Click += new System.EventHandler(this.open_g7_button_Click);
            // 
            // static_g7_textBox
            // 
            this.static_g7_textBox.Font = new System.Drawing.Font("Calibri", 11F);
            this.static_g7_textBox.ForeColor = System.Drawing.Color.Yellow;
            this.static_g7_textBox.Location = new System.Drawing.Point(153, 6);
            this.static_g7_textBox.Multiline = true;
            this.static_g7_textBox.Name = "static_g7_textBox";
            this.static_g7_textBox.ReadOnly = true;
            this.static_g7_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.static_g7_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.static_g7_textBox.Size = new System.Drawing.Size(692, 538);
            this.static_g7_textBox.TabIndex = 2;
            // 
            // openXmlG7
            // 
            this.openXmlG7.FileName = "openFileDialog1";
            this.openXmlG7.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // simul_textBox
            // 
            this.simul_textBox.AcceptsTab = true;
            this.simul_textBox.BackColor = System.Drawing.SystemColors.Control;
            this.simul_textBox.Font = new System.Drawing.Font("Calibri", 11F);
            this.simul_textBox.ForeColor = System.Drawing.SystemColors.Desktop;
            this.simul_textBox.Location = new System.Drawing.Point(279, 75);
            this.simul_textBox.Multiline = true;
            this.simul_textBox.Name = "simul_textBox";
            this.simul_textBox.ReadOnly = true;
            this.simul_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.simul_textBox.Size = new System.Drawing.Size(532, 469);
            this.simul_textBox.TabIndex = 7;
            this.simul_textBox.WordWrap = false;
            // 
            // start_button
            // 
            this.start_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.start_button.Font = new System.Drawing.Font("Trebuchet MS", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_button.ForeColor = System.Drawing.Color.Maroon;
            this.start_button.Location = new System.Drawing.Point(6, 61);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(111, 32);
            this.start_button.TabIndex = 14;
            this.start_button.Text = "START>";
            this.start_button.UseVisualStyleBackColor = false;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(669, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 23);
            this.label5.TabIndex = 21;
            this.label5.Text = "Grafcets List";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clearAll_button
            // 
            this.clearAll_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.clearAll_button.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearAll_button.ForeColor = System.Drawing.Color.Maroon;
            this.clearAll_button.Location = new System.Drawing.Point(12, 28);
            this.clearAll_button.Name = "clearAll_button";
            this.clearAll_button.Size = new System.Drawing.Size(81, 34);
            this.clearAll_button.TabIndex = 22;
            this.clearAll_button.Text = "Clear All";
            this.clearAll_button.UseVisualStyleBackColor = false;
            this.clearAll_button.Click += new System.EventHandler(this.clearAll_button_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1493, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(131, 61);
            this.button6.TabIndex = 27;
            this.button6.Text = "CLOSE ALL";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // step_step_button
            // 
            this.step_step_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.step_step_button.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.step_step_button.ForeColor = System.Drawing.Color.Maroon;
            this.step_step_button.Location = new System.Drawing.Point(123, 62);
            this.step_step_button.Name = "step_step_button";
            this.step_step_button.Size = new System.Drawing.Size(111, 32);
            this.step_step_button.TabIndex = 29;
            this.step_step_button.Text = "Step by Step";
            this.step_step_button.UseVisualStyleBackColor = false;
            this.step_step_button.Click += new System.EventHandler(this.step_step_button_Click);
            // 
            // PIC_Message_textBox
            // 
            this.PIC_Message_textBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PIC_Message_textBox.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PIC_Message_textBox.ForeColor = System.Drawing.Color.Green;
            this.PIC_Message_textBox.Location = new System.Drawing.Point(307, 103);
            this.PIC_Message_textBox.Multiline = true;
            this.PIC_Message_textBox.Name = "PIC_Message_textBox";
            this.PIC_Message_textBox.ReadOnly = true;
            this.PIC_Message_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PIC_Message_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.PIC_Message_textBox.Size = new System.Drawing.Size(504, 273);
            this.PIC_Message_textBox.TabIndex = 32;
            // 
            // stop_button
            // 
            this.stop_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.stop_button.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop_button.ForeColor = System.Drawing.Color.Maroon;
            this.stop_button.Location = new System.Drawing.Point(6, 99);
            this.stop_button.Name = "stop_button";
            this.stop_button.Size = new System.Drawing.Size(111, 32);
            this.stop_button.TabIndex = 34;
            this.stop_button.Text = "<STOP";
            this.stop_button.UseVisualStyleBackColor = false;
            this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(311, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 23);
            this.label2.TabIndex = 35;
            this.label2.Text = "Hexadecimal board data";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clear_button
            // 
            this.clear_button.BackColor = System.Drawing.SystemColors.Control;
            this.clear_button.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear_button.ForeColor = System.Drawing.Color.Maroon;
            this.clear_button.Location = new System.Drawing.Point(6, 136);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(127, 32);
            this.clear_button.TabIndex = 36;
            this.clear_button.Text = "Clear Window";
            this.clear_button.UseVisualStyleBackColor = false;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Orchid;
            this.progressBar1.Location = new System.Drawing.Point(308, 8);
            this.progressBar1.Maximum = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(346, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 37;
            this.progressBar1.Visible = false;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(279, 55);
            this.progressBar2.Maximum = 10;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(532, 14);
            this.progressBar2.Step = 2;
            this.progressBar2.TabIndex = 38;
            this.progressBar2.Visible = false;
            // 
            // checkAll_button
            // 
            this.checkAll_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.checkAll_button.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkAll_button.ForeColor = System.Drawing.Color.Maroon;
            this.checkAll_button.Location = new System.Drawing.Point(12, 67);
            this.checkAll_button.Name = "checkAll_button";
            this.checkAll_button.Size = new System.Drawing.Size(84, 36);
            this.checkAll_button.TabIndex = 41;
            this.checkAll_button.Text = "Check All";
            this.checkAll_button.UseVisualStyleBackColor = false;
            this.checkAll_button.Click += new System.EventHandler(this.checkAll_button_Click);
            // 
            // send_pic_message_button
            // 
            this.send_pic_message_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.send_pic_message_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.send_pic_message_button.ForeColor = System.Drawing.Color.Maroon;
            this.send_pic_message_button.Location = new System.Drawing.Point(8, 32);
            this.send_pic_message_button.Name = "send_pic_message_button";
            this.send_pic_message_button.Size = new System.Drawing.Size(175, 36);
            this.send_pic_message_button.TabIndex = 42;
            this.send_pic_message_button.Text = "Send PIC Message";
            this.send_pic_message_button.UseVisualStyleBackColor = false;
            this.send_pic_message_button.Click += new System.EventHandler(this.send_pic_message_button_Click);
            // 
            // open_serial_port_button
            // 
            this.open_serial_port_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_serial_port_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_serial_port_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_serial_port_button.Location = new System.Drawing.Point(191, 32);
            this.open_serial_port_button.Name = "open_serial_port_button";
            this.open_serial_port_button.Size = new System.Drawing.Size(254, 36);
            this.open_serial_port_button.TabIndex = 43;
            this.open_serial_port_button.Text = "Open Serial Port Terminal ";
            this.open_serial_port_button.UseVisualStyleBackColor = false;
            this.open_serial_port_button.Click += new System.EventHandler(this.open_serial_port_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.send_pic_message_button);
            this.groupBox1.Controls.Add(this.open_serial_port_button);
            this.groupBox1.Font = new System.Drawing.Font("Californian FB", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(307, 452);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 75);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Port Terminal Access";
            // 
            // open_simulations_button
            // 
            this.open_simulations_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.open_simulations_button.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_simulations_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_simulations_button.Location = new System.Drawing.Point(6, 21);
            this.open_simulations_button.Name = "open_simulations_button";
            this.open_simulations_button.Size = new System.Drawing.Size(74, 35);
            this.open_simulations_button.TabIndex = 45;
            this.open_simulations_button.Text = "Open";
            this.open_simulations_button.UseVisualStyleBackColor = false;
            this.open_simulations_button.Click += new System.EventHandler(this.open_simulations_button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.reinit_button);
            this.groupBox2.Controls.Add(this.open_simulations_button);
            this.groupBox2.Controls.Add(this.clear_button);
            this.groupBox2.Controls.Add(this.start_button);
            this.groupBox2.Controls.Add(this.stop_button);
            this.groupBox2.Controls.Add(this.step_step_button);
            this.groupBox2.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(6, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 174);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulations";
            // 
            // reinit_button
            // 
            this.reinit_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.reinit_button.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reinit_button.ForeColor = System.Drawing.Color.DarkRed;
            this.reinit_button.Location = new System.Drawing.Point(123, 99);
            this.reinit_button.Name = "reinit_button";
            this.reinit_button.Size = new System.Drawing.Size(85, 32);
            this.reinit_button.TabIndex = 46;
            this.reinit_button.Text = "ReInit";
            this.reinit_button.UseVisualStyleBackColor = false;
            this.reinit_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.input_checkedListBox);
            this.groupBox3.Controls.Add(this.clearAll_button);
            this.groupBox3.Controls.Add(this.checkAll_button);
            this.groupBox3.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.Navy;
            this.groupBox3.Location = new System.Drawing.Point(6, 193);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 351);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input Variables";
            // 
            // input_checkedListBox
            // 
            this.input_checkedListBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.input_checkedListBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.input_checkedListBox.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input_checkedListBox.FormattingEnabled = true;
            this.input_checkedListBox.HorizontalScrollbar = true;
            this.input_checkedListBox.Location = new System.Drawing.Point(114, 28);
            this.input_checkedListBox.Name = "input_checkedListBox";
            this.input_checkedListBox.Size = new System.Drawing.Size(120, 304);
            this.input_checkedListBox.TabIndex = 53;
            // 
            // save_g7_text_button
            // 
            this.save_g7_text_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.save_g7_text_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_g7_text_button.ForeColor = System.Drawing.Color.Maroon;
            this.save_g7_text_button.Location = new System.Drawing.Point(25, 24);
            this.save_g7_text_button.Name = "save_g7_text_button";
            this.save_g7_text_button.Size = new System.Drawing.Size(67, 32);
            this.save_g7_text_button.TabIndex = 47;
            this.save_g7_text_button.Text = "Save";
            this.save_g7_text_button.UseVisualStyleBackColor = false;
            this.save_g7_text_button.Click += new System.EventHandler(this.save_g7_text_button_Click);
            // 
            // open_g7_text_button
            // 
            this.open_g7_text_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_g7_text_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_g7_text_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_g7_text_button.Location = new System.Drawing.Point(26, 67);
            this.open_g7_text_button.Name = "open_g7_text_button";
            this.open_g7_text_button.Size = new System.Drawing.Size(66, 32);
            this.open_g7_text_button.TabIndex = 48;
            this.open_g7_text_button.Text = "Open";
            this.open_g7_text_button.UseVisualStyleBackColor = false;
            this.open_g7_text_button.Click += new System.EventHandler(this.open_g7_text_button_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.clock_interval_comboBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.ForeColor = System.Drawing.Color.Navy;
            this.groupBox4.Location = new System.Drawing.Point(520, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 42);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Clock";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(205, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 22);
            this.label6.TabIndex = 51;
            this.label6.Text = "ms";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clock_interval_comboBox
            // 
            this.clock_interval_comboBox.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clock_interval_comboBox.ForeColor = System.Drawing.Color.Blue;
            this.clock_interval_comboBox.FormattingEnabled = true;
            this.clock_interval_comboBox.Location = new System.Drawing.Point(129, 14);
            this.clock_interval_comboBox.Name = "clock_interval_comboBox";
            this.clock_interval_comboBox.Size = new System.Drawing.Size(70, 24);
            this.clock_interval_comboBox.TabIndex = 50;
            this.clock_interval_comboBox.SelectedIndexChanged += new System.EventHandler(this.clock_comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label4.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(9, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 49;
            this.label4.Text = "Clock Interval";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.save_g7_text_button);
            this.groupBox5.Controls.Add(this.open_g7_text_button);
            this.groupBox5.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.Color.Navy;
            this.groupBox5.Location = new System.Drawing.Point(17, 84);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(124, 112);
            this.groupBox5.TabIndex = 52;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Grafcet Text";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.display_all_dynamic_vector_checkbox);
            this.groupBox6.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.Color.Navy;
            this.groupBox6.Location = new System.Drawing.Point(279, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(235, 42);
            this.groupBox6.TabIndex = 52;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Display Mode";
            // 
            // display_all_dynamic_vector_checkbox
            // 
            this.display_all_dynamic_vector_checkbox.AutoSize = true;
            this.display_all_dynamic_vector_checkbox.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_all_dynamic_vector_checkbox.ForeColor = System.Drawing.Color.Black;
            this.display_all_dynamic_vector_checkbox.Location = new System.Drawing.Point(6, 15);
            this.display_all_dynamic_vector_checkbox.Name = "display_all_dynamic_vector_checkbox";
            this.display_all_dynamic_vector_checkbox.Size = new System.Drawing.Size(223, 23);
            this.display_all_dynamic_vector_checkbox.TabIndex = 53;
            this.display_all_dynamic_vector_checkbox.Text = "Display all dynamic vectors";
            this.display_all_dynamic_vector_checkbox.UseVisualStyleBackColor = true;
            this.display_all_dynamic_vector_checkbox.CheckedChanged += new System.EventHandler(this.display_all_dynamic_vector_checkbox_CheckedChanged);
            // 
            // board_descript
            // 
            this.board_descript.Controls.Add(this.tabPage1);
            this.board_descript.Controls.Add(this.tabPage2);
            this.board_descript.Controls.Add(this.tabPage5);
            this.board_descript.Controls.Add(this.tabPage3);
            this.board_descript.Controls.Add(this.tabPage4);
            this.board_descript.Font = new System.Drawing.Font("Calisto MT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.board_descript.Location = new System.Drawing.Point(12, 70);
            this.board_descript.Name = "board_descript";
            this.board_descript.SelectedIndex = 0;
            this.board_descript.Size = new System.Drawing.Size(854, 585);
            this.board_descript.TabIndex = 53;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage1.Controls.Add(this.groupBox10);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.static_g7_textBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(846, 550);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MATRICES";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.save_g7_hex_button);
            this.groupBox10.Controls.Add(this.open_g7_hex_button);
            this.groupBox10.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox10.ForeColor = System.Drawing.Color.Navy;
            this.groupBox10.Location = new System.Drawing.Point(17, 242);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(124, 112);
            this.groupBox10.TabIndex = 52;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Grafcet Hex";
            // 
            // save_g7_hex_button
            // 
            this.save_g7_hex_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.save_g7_hex_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_g7_hex_button.ForeColor = System.Drawing.Color.Maroon;
            this.save_g7_hex_button.Location = new System.Drawing.Point(25, 25);
            this.save_g7_hex_button.Name = "save_g7_hex_button";
            this.save_g7_hex_button.Size = new System.Drawing.Size(67, 32);
            this.save_g7_hex_button.TabIndex = 47;
            this.save_g7_hex_button.Text = "Save";
            this.save_g7_hex_button.UseVisualStyleBackColor = false;
            this.save_g7_hex_button.Click += new System.EventHandler(this.save_g7_text_button_Click);
            // 
            // open_g7_hex_button
            // 
            this.open_g7_hex_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_g7_hex_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_g7_hex_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_g7_hex_button.Location = new System.Drawing.Point(26, 67);
            this.open_g7_hex_button.Name = "open_g7_hex_button";
            this.open_g7_hex_button.Size = new System.Drawing.Size(66, 32);
            this.open_g7_hex_button.TabIndex = 48;
            this.open_g7_hex_button.Text = "Open";
            this.open_g7_hex_button.UseVisualStyleBackColor = false;
            this.open_g7_hex_button.Click += new System.EventHandler(this.open_g7_hex_button_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage2.Controls.Add(this.simul_textBox);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.progressBar2);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(846, 550);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SIMULATION";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage5.Controls.Add(this.groupBox9);
            this.tabPage5.Controls.Add(this.groupBox8);
            this.tabPage5.Controls.Add(this.groupBox7);
            this.tabPage5.Location = new System.Drawing.Point(4, 31);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(846, 550);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "CODE GENERATION";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox1);
            this.groupBox9.Controls.Add(this.export_button);
            this.groupBox9.Controls.Add(this.upload_button);
            this.groupBox9.Controls.Add(this.build_project_button);
            this.groupBox9.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.Navy;
            this.groupBox9.Location = new System.Drawing.Point(630, 29);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(193, 303);
            this.groupBox9.TabIndex = 48;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Build project";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(6, 27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox1.Size = new System.Drawing.Size(160, 93);
            this.textBox1.TabIndex = 49;
            this.textBox1.Text = "To build project, exportr and /or load the Hex program in the board memory";
            // 
            // export_button
            // 
            this.export_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.export_button.Enabled = false;
            this.export_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.export_button.ForeColor = System.Drawing.Color.Maroon;
            this.export_button.Location = new System.Drawing.Point(20, 178);
            this.export_button.Name = "export_button";
            this.export_button.Size = new System.Drawing.Size(146, 44);
            this.export_button.TabIndex = 43;
            this.export_button.Text = "EXPORT";
            this.export_button.UseVisualStyleBackColor = false;
            this.export_button.Click += new System.EventHandler(this.export_button_Click);
            // 
            // upload_button
            // 
            this.upload_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.upload_button.Enabled = false;
            this.upload_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upload_button.ForeColor = System.Drawing.Color.Maroon;
            this.upload_button.Location = new System.Drawing.Point(20, 240);
            this.upload_button.Name = "upload_button";
            this.upload_button.Size = new System.Drawing.Size(146, 44);
            this.upload_button.TabIndex = 43;
            this.upload_button.Text = "PROGRAM";
            this.upload_button.UseVisualStyleBackColor = false;
            // 
            // build_project_button
            // 
            this.build_project_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.build_project_button.Enabled = false;
            this.build_project_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.build_project_button.ForeColor = System.Drawing.Color.Maroon;
            this.build_project_button.Location = new System.Drawing.Point(20, 126);
            this.build_project_button.Name = "build_project_button";
            this.build_project_button.Size = new System.Drawing.Size(146, 44);
            this.build_project_button.TabIndex = 43;
            this.build_project_button.Text = "BUILD Project";
            this.build_project_button.UseVisualStyleBackColor = false;
            this.build_project_button.Click += new System.EventHandler(this.build_project_button_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.code_generation_button);
            this.groupBox8.Controls.Add(this.code_debug_checkBox);
            this.groupBox8.Controls.Add(this.code_analyse_efficiency_checkBox);
            this.groupBox8.Controls.Add(this.code_vm_checkBox);
            this.groupBox8.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.Color.Navy;
            this.groupBox8.Location = new System.Drawing.Point(371, 29);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(236, 303);
            this.groupBox8.TabIndex = 48;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Code generation";
            // 
            // code_generation_button
            // 
            this.code_generation_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.code_generation_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_generation_button.ForeColor = System.Drawing.Color.Maroon;
            this.code_generation_button.Location = new System.Drawing.Point(58, 230);
            this.code_generation_button.Name = "code_generation_button";
            this.code_generation_button.Size = new System.Drawing.Size(110, 44);
            this.code_generation_button.TabIndex = 49;
            this.code_generation_button.Text = "Generate";
            this.code_generation_button.UseVisualStyleBackColor = false;
            this.code_generation_button.Click += new System.EventHandler(this.code_generation_button_Click);
            // 
            // code_debug_checkBox
            // 
            this.code_debug_checkBox.AutoSize = true;
            this.code_debug_checkBox.Enabled = false;
            this.code_debug_checkBox.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_debug_checkBox.ForeColor = System.Drawing.Color.Black;
            this.code_debug_checkBox.Location = new System.Drawing.Point(11, 101);
            this.code_debug_checkBox.Name = "code_debug_checkBox";
            this.code_debug_checkBox.Size = new System.Drawing.Size(185, 23);
            this.code_debug_checkBox.TabIndex = 54;
            this.code_debug_checkBox.Text = "Code for Debug Mode";
            this.code_debug_checkBox.UseVisualStyleBackColor = true;
            // 
            // code_analyse_efficiency_checkBox
            // 
            this.code_analyse_efficiency_checkBox.AutoSize = true;
            this.code_analyse_efficiency_checkBox.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_analyse_efficiency_checkBox.ForeColor = System.Drawing.Color.Black;
            this.code_analyse_efficiency_checkBox.Location = new System.Drawing.Point(11, 68);
            this.code_analyse_efficiency_checkBox.Name = "code_analyse_efficiency_checkBox";
            this.code_analyse_efficiency_checkBox.Size = new System.Drawing.Size(157, 23);
            this.code_analyse_efficiency_checkBox.TabIndex = 54;
            this.code_analyse_efficiency_checkBox.Text = "Analyse Efficiency";
            this.code_analyse_efficiency_checkBox.UseVisualStyleBackColor = true;
            // 
            // code_vm_checkBox
            // 
            this.code_vm_checkBox.AutoSize = true;
            this.code_vm_checkBox.Checked = true;
            this.code_vm_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.code_vm_checkBox.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_vm_checkBox.ForeColor = System.Drawing.Color.Black;
            this.code_vm_checkBox.Location = new System.Drawing.Point(11, 39);
            this.code_vm_checkBox.Name = "code_vm_checkBox";
            this.code_vm_checkBox.Size = new System.Drawing.Size(185, 23);
            this.code_vm_checkBox.TabIndex = 54;
            this.code_vm_checkBox.Text = "Virtual Machine Code";
            this.code_vm_checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.board_processor_textBox3);
            this.groupBox7.Controls.Add(this.board_device_comboBox);
            this.groupBox7.Controls.Add(this.board_family_textBox3);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.reload_boards_settings_button);
            this.groupBox7.Controls.Add(this.io_map_button);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Font = new System.Drawing.Font("Californian FB", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.Color.Navy;
            this.groupBox7.Location = new System.Drawing.Point(18, 29);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(325, 303);
            this.groupBox7.TabIndex = 48;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Board Settings";
            // 
            // board_processor_textBox3
            // 
            this.board_processor_textBox3.BackColor = System.Drawing.Color.White;
            this.board_processor_textBox3.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.board_processor_textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.board_processor_textBox3.Location = new System.Drawing.Point(97, 134);
            this.board_processor_textBox3.Margin = new System.Windows.Forms.Padding(0);
            this.board_processor_textBox3.Name = "board_processor_textBox3";
            this.board_processor_textBox3.ReadOnly = true;
            this.board_processor_textBox3.Size = new System.Drawing.Size(206, 23);
            this.board_processor_textBox3.TabIndex = 49;
            // 
            // board_device_comboBox
            // 
            this.board_device_comboBox.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.board_device_comboBox.ForeColor = System.Drawing.Color.Blue;
            this.board_device_comboBox.FormattingEnabled = true;
            this.board_device_comboBox.Location = new System.Drawing.Point(97, 49);
            this.board_device_comboBox.Name = "board_device_comboBox";
            this.board_device_comboBox.Size = new System.Drawing.Size(206, 24);
            this.board_device_comboBox.TabIndex = 50;
            this.board_device_comboBox.SelectedIndexChanged += new System.EventHandler(this.board_device_comboBox_SelectedIndexChanged);
            // 
            // board_family_textBox3
            // 
            this.board_family_textBox3.BackColor = System.Drawing.Color.White;
            this.board_family_textBox3.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.board_family_textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.board_family_textBox3.Location = new System.Drawing.Point(97, 92);
            this.board_family_textBox3.Margin = new System.Windows.Forms.Padding(0);
            this.board_family_textBox3.Name = "board_family_textBox3";
            this.board_family_textBox3.ReadOnly = true;
            this.board_family_textBox3.Size = new System.Drawing.Size(206, 23);
            this.board_family_textBox3.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 22);
            this.label1.TabIndex = 49;
            this.label1.Text = "Family";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // reload_boards_settings_button
            // 
            this.reload_boards_settings_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.reload_boards_settings_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reload_boards_settings_button.ForeColor = System.Drawing.Color.DarkGreen;
            this.reload_boards_settings_button.Location = new System.Drawing.Point(13, 255);
            this.reload_boards_settings_button.Name = "reload_boards_settings_button";
            this.reload_boards_settings_button.Size = new System.Drawing.Size(204, 34);
            this.reload_boards_settings_button.TabIndex = 43;
            this.reload_boards_settings_button.Text = "Relod Board Settings";
            this.reload_boards_settings_button.UseVisualStyleBackColor = false;
            this.reload_boards_settings_button.Click += new System.EventHandler(this.reload_boards_settings_button_Click);
            // 
            // io_map_button
            // 
            this.io_map_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.io_map_button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.io_map_button.ForeColor = System.Drawing.Color.Maroon;
            this.io_map_button.Location = new System.Drawing.Point(13, 174);
            this.io_map_button.Name = "io_map_button";
            this.io_map_button.Size = new System.Drawing.Size(136, 34);
            this.io_map_button.TabIndex = 43;
            this.io_map_button.Text = "IO Mapping";
            this.io_map_button.UseVisualStyleBackColor = false;
            this.io_map_button.Click += new System.EventHandler(this.io_map_button_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label10.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(9, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 22);
            this.label10.TabIndex = 49;
            this.label10.Text = "Device";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label9.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(9, 134);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 22);
            this.label9.TabIndex = 49;
            this.label9.Text = "Processor";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage3.Controls.Add(this.save_hex_data_button2);
            this.tabPage3.Controls.Add(this.open_hex_board_data_button1);
            this.tabPage3.Controls.Add(this.generate_hex_board_button1);
            this.tabPage3.Controls.Add(this.PIC_Message_textBox);
            this.tabPage3.Controls.Add(this.condensed_g7_data_textBox);
            this.tabPage3.Controls.Add(this.size_textBox1);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 31);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(846, 550);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "HEX DATA";
            // 
            // save_hex_data_button2
            // 
            this.save_hex_data_button2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.save_hex_data_button2.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_hex_data_button2.ForeColor = System.Drawing.Color.Maroon;
            this.save_hex_data_button2.Location = new System.Drawing.Point(570, 382);
            this.save_hex_data_button2.Name = "save_hex_data_button2";
            this.save_hex_data_button2.Size = new System.Drawing.Size(67, 32);
            this.save_hex_data_button2.TabIndex = 47;
            this.save_hex_data_button2.Text = "Save";
            this.save_hex_data_button2.UseVisualStyleBackColor = false;
            this.save_hex_data_button2.Click += new System.EventHandler(this.save_hex_board_data_Click);
            // 
            // open_hex_board_data_button1
            // 
            this.open_hex_board_data_button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_hex_board_data_button1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_hex_board_data_button1.ForeColor = System.Drawing.Color.Maroon;
            this.open_hex_board_data_button1.Location = new System.Drawing.Point(657, 382);
            this.open_hex_board_data_button1.Name = "open_hex_board_data_button1";
            this.open_hex_board_data_button1.Size = new System.Drawing.Size(66, 32);
            this.open_hex_board_data_button1.TabIndex = 48;
            this.open_hex_board_data_button1.Text = "Open";
            this.open_hex_board_data_button1.UseVisualStyleBackColor = false;
            this.open_hex_board_data_button1.Click += new System.EventHandler(this.open_hex_board_data_button1_Click);
            // 
            // generate_hex_board_button1
            // 
            this.generate_hex_board_button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.generate_hex_board_button1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generate_hex_board_button1.ForeColor = System.Drawing.Color.Maroon;
            this.generate_hex_board_button1.Location = new System.Drawing.Point(495, 38);
            this.generate_hex_board_button1.Name = "generate_hex_board_button1";
            this.generate_hex_board_button1.Size = new System.Drawing.Size(257, 32);
            this.generate_hex_board_button1.TabIndex = 42;
            this.generate_hex_board_button1.Text = "Generate Hex board Data";
            this.generate_hex_board_button1.UseVisualStyleBackColor = false;
            this.generate_hex_board_button1.Click += new System.EventHandler(this.generate_hex_board_button_Click);
            // 
            // condensed_g7_data_textBox
            // 
            this.condensed_g7_data_textBox.Font = new System.Drawing.Font("Candara", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.condensed_g7_data_textBox.ForeColor = System.Drawing.Color.Yellow;
            this.condensed_g7_data_textBox.Location = new System.Drawing.Point(15, 103);
            this.condensed_g7_data_textBox.Multiline = true;
            this.condensed_g7_data_textBox.Name = "condensed_g7_data_textBox";
            this.condensed_g7_data_textBox.ReadOnly = true;
            this.condensed_g7_data_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.condensed_g7_data_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.condensed_g7_data_textBox.Size = new System.Drawing.Size(279, 441);
            this.condensed_g7_data_textBox.TabIndex = 2;
            // 
            // size_textBox1
            // 
            this.size_textBox1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.size_textBox1.Font = new System.Drawing.Font("Calisto MT", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.size_textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.size_textBox1.Location = new System.Drawing.Point(402, 383);
            this.size_textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.size_textBox1.Name = "size_textBox1";
            this.size_textBox1.ReadOnly = true;
            this.size_textBox1.Size = new System.Drawing.Size(114, 30);
            this.size_textBox1.TabIndex = 20;
            this.size_textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label3.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(30, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 23);
            this.label3.TabIndex = 35;
            this.label3.Text = "Condensed Grafcet matrices";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(394, 308);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 23);
            this.label8.TabIndex = 35;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label7.Font = new System.Drawing.Font("Trebuchet MS", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(339, 386);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 23);
            this.label7.TabIndex = 35;
            this.label7.Text = "Size : ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tabPage4.Controls.Add(this.Save_Board_Button);
            this.tabPage4.Controls.Add(this.Cancel_Board_Button);
            this.tabPage4.Controls.Add(this.groupBox14);
            this.tabPage4.Controls.Add(this.Delete_Board_Button);
            this.tabPage4.Controls.Add(this.Edit_Board_Button);
            this.tabPage4.Controls.Add(this.NewBoard_Button);
            this.tabPage4.Controls.Add(this.groupBox15);
            this.tabPage4.Controls.Add(this.groupBox13);
            this.tabPage4.Controls.Add(this.groupBox11);
            this.tabPage4.Controls.Add(this.groupBox16);
            this.tabPage4.Controls.Add(this.groupBox12);
            this.tabPage4.Location = new System.Drawing.Point(4, 31);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(846, 550);
            this.tabPage4.TabIndex = 5;
            this.tabPage4.Text = "BOARDS DEFINITION";
            // 
            // Save_Board_Button
            // 
            this.Save_Board_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Save_Board_Button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save_Board_Button.ForeColor = System.Drawing.Color.Maroon;
            this.Save_Board_Button.Location = new System.Drawing.Point(613, 16);
            this.Save_Board_Button.Name = "Save_Board_Button";
            this.Save_Board_Button.Size = new System.Drawing.Size(96, 33);
            this.Save_Board_Button.TabIndex = 51;
            this.Save_Board_Button.Text = "Save";
            this.Save_Board_Button.UseVisualStyleBackColor = false;
            this.Save_Board_Button.Click += new System.EventHandler(this.Save_Board_Button_Click);
            // 
            // Cancel_Board_Button
            // 
            this.Cancel_Board_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Cancel_Board_Button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Board_Button.ForeColor = System.Drawing.Color.Maroon;
            this.Cancel_Board_Button.Location = new System.Drawing.Point(495, 16);
            this.Cancel_Board_Button.Name = "Cancel_Board_Button";
            this.Cancel_Board_Button.Size = new System.Drawing.Size(102, 33);
            this.Cancel_Board_Button.TabIndex = 51;
            this.Cancel_Board_Button.Text = "Cancel";
            this.Cancel_Board_Button.UseVisualStyleBackColor = false;
            this.Cancel_Board_Button.Click += new System.EventHandler(this.Cancel_Board_Button_Click);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.eeprom_check);
            this.groupBox14.Controls.Add(this.eeprom_unit);
            this.groupBox14.Controls.Add(this.flash_unit);
            this.groupBox14.Controls.Add(this.label24);
            this.groupBox14.Controls.Add(this.ram_unit);
            this.groupBox14.Controls.Add(this.label23);
            this.groupBox14.Controls.Add(this.eeprom_size);
            this.groupBox14.Controls.Add(this.label22);
            this.groupBox14.Controls.Add(this.flash_size);
            this.groupBox14.Controls.Add(this.label20);
            this.groupBox14.Controls.Add(this.ram_size);
            this.groupBox14.Controls.Add(this.label21);
            this.groupBox14.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox14.ForeColor = System.Drawing.Color.Navy;
            this.groupBox14.Location = new System.Drawing.Point(19, 230);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(378, 141);
            this.groupBox14.TabIndex = 50;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Memory Sizes";
            // 
            // eeprom_check
            // 
            this.eeprom_check.AutoSize = true;
            this.eeprom_check.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eeprom_check.ForeColor = System.Drawing.Color.Black;
            this.eeprom_check.Location = new System.Drawing.Point(6, 97);
            this.eeprom_check.Name = "eeprom_check";
            this.eeprom_check.Size = new System.Drawing.Size(145, 23);
            this.eeprom_check.TabIndex = 54;
            this.eeprom_check.Text = "Data (EEPROM)";
            this.eeprom_check.UseVisualStyleBackColor = true;
            this.eeprom_check.CheckedChanged += new System.EventHandler(this.eeprom_check_CheckedChanged);
            // 
            // eeprom_unit
            // 
            this.eeprom_unit.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eeprom_unit.ForeColor = System.Drawing.Color.Blue;
            this.eeprom_unit.FormattingEnabled = true;
            this.eeprom_unit.Location = new System.Drawing.Point(270, 93);
            this.eeprom_unit.Name = "eeprom_unit";
            this.eeprom_unit.Size = new System.Drawing.Size(80, 24);
            this.eeprom_unit.TabIndex = 52;
            // 
            // flash_unit
            // 
            this.flash_unit.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flash_unit.ForeColor = System.Drawing.Color.Blue;
            this.flash_unit.FormattingEnabled = true;
            this.flash_unit.Location = new System.Drawing.Point(270, 59);
            this.flash_unit.Name = "flash_unit";
            this.flash_unit.Size = new System.Drawing.Size(80, 24);
            this.flash_unit.TabIndex = 52;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label24.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(225, 96);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(39, 20);
            this.label24.TabIndex = 51;
            this.label24.Text = "Unit";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ram_unit
            // 
            this.ram_unit.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ram_unit.ForeColor = System.Drawing.Color.Blue;
            this.ram_unit.FormattingEnabled = true;
            this.ram_unit.Location = new System.Drawing.Point(270, 23);
            this.ram_unit.Name = "ram_unit";
            this.ram_unit.Size = new System.Drawing.Size(80, 24);
            this.ram_unit.TabIndex = 52;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label23.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(225, 62);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(39, 20);
            this.label23.TabIndex = 51;
            this.label23.Text = "Unit";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eeprom_size
            // 
            this.eeprom_size.Location = new System.Drawing.Point(156, 93);
            this.eeprom_size.Name = "eeprom_size";
            this.eeprom_size.Size = new System.Drawing.Size(63, 27);
            this.eeprom_size.TabIndex = 52;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label22.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(225, 26);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(39, 20);
            this.label22.TabIndex = 51;
            this.label22.Text = "Unit";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flash_size
            // 
            this.flash_size.Location = new System.Drawing.Point(156, 60);
            this.flash_size.Name = "flash_size";
            this.flash_size.Size = new System.Drawing.Size(63, 27);
            this.flash_size.TabIndex = 50;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label20.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Location = new System.Drawing.Point(12, 63);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(118, 20);
            this.label20.TabIndex = 51;
            this.label20.Text = "Flash (Program)";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ram_size
            // 
            this.ram_size.Location = new System.Drawing.Point(156, 25);
            this.ram_size.Name = "ram_size";
            this.ram_size.Size = new System.Drawing.Size(63, 27);
            this.ram_size.TabIndex = 49;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label21.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(12, 27);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 20);
            this.label21.TabIndex = 49;
            this.label21.Text = "RAM";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Delete_Board_Button
            // 
            this.Delete_Board_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Delete_Board_Button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete_Board_Button.ForeColor = System.Drawing.Color.Maroon;
            this.Delete_Board_Button.Location = new System.Drawing.Point(369, 16);
            this.Delete_Board_Button.Name = "Delete_Board_Button";
            this.Delete_Board_Button.Size = new System.Drawing.Size(105, 33);
            this.Delete_Board_Button.TabIndex = 49;
            this.Delete_Board_Button.Text = "Delete";
            this.Delete_Board_Button.UseVisualStyleBackColor = false;
            // 
            // Edit_Board_Button
            // 
            this.Edit_Board_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Edit_Board_Button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Edit_Board_Button.ForeColor = System.Drawing.Color.Maroon;
            this.Edit_Board_Button.Location = new System.Drawing.Point(264, 16);
            this.Edit_Board_Button.Name = "Edit_Board_Button";
            this.Edit_Board_Button.Size = new System.Drawing.Size(87, 33);
            this.Edit_Board_Button.TabIndex = 49;
            this.Edit_Board_Button.Text = "Edit";
            this.Edit_Board_Button.UseVisualStyleBackColor = false;
            // 
            // NewBoard_Button
            // 
            this.NewBoard_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.NewBoard_Button.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewBoard_Button.ForeColor = System.Drawing.Color.Maroon;
            this.NewBoard_Button.Location = new System.Drawing.Point(119, 16);
            this.NewBoard_Button.Name = "NewBoard_Button";
            this.NewBoard_Button.Size = new System.Drawing.Size(130, 33);
            this.NewBoard_Button.TabIndex = 49;
            this.NewBoard_Button.Text = "New Board";
            this.NewBoard_Button.UseVisualStyleBackColor = false;
            this.NewBoard_Button.Click += new System.EventHandler(this.NewBoard_Button_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label11);
            this.groupBox15.Controls.Add(this.file_timer_config);
            this.groupBox15.Controls.Add(this.Board_Time_Period_comboBox);
            this.groupBox15.Controls.Add(this.Timer_Btn_Select);
            this.groupBox15.Controls.Add(this.label25);
            this.groupBox15.Controls.Add(this.label26);
            this.groupBox15.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox15.ForeColor = System.Drawing.Color.Navy;
            this.groupBox15.Location = new System.Drawing.Point(445, 69);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(378, 105);
            this.groupBox15.TabIndex = 48;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Timer Configuration";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label11.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(217, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 20);
            this.label11.TabIndex = 51;
            this.label11.Text = "ms";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // file_timer_config
            // 
            this.file_timer_config.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_timer_config.Location = new System.Drawing.Point(115, 26);
            this.file_timer_config.Name = "file_timer_config";
            this.file_timer_config.ReadOnly = true;
            this.file_timer_config.Size = new System.Drawing.Size(200, 22);
            this.file_timer_config.TabIndex = 54;
            // 
            // Board_Time_Period_comboBox
            // 
            this.Board_Time_Period_comboBox.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board_Time_Period_comboBox.ForeColor = System.Drawing.Color.Blue;
            this.Board_Time_Period_comboBox.FormattingEnabled = true;
            this.Board_Time_Period_comboBox.Location = new System.Drawing.Point(115, 61);
            this.Board_Time_Period_comboBox.Name = "Board_Time_Period_comboBox";
            this.Board_Time_Period_comboBox.Size = new System.Drawing.Size(78, 26);
            this.Board_Time_Period_comboBox.TabIndex = 50;
            // 
            // Timer_Btn_Select
            // 
            this.Timer_Btn_Select.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Timer_Btn_Select.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Timer_Btn_Select.ForeColor = System.Drawing.Color.Black;
            this.Timer_Btn_Select.Location = new System.Drawing.Point(321, 22);
            this.Timer_Btn_Select.Name = "Timer_Btn_Select";
            this.Timer_Btn_Select.Size = new System.Drawing.Size(51, 30);
            this.Timer_Btn_Select.TabIndex = 53;
            this.Timer_Btn_Select.Text = "...";
            this.Timer_Btn_Select.UseVisualStyleBackColor = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label25.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(12, 61);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(52, 20);
            this.label25.TabIndex = 51;
            this.label25.Text = "Period";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label26.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(12, 28);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(85, 20);
            this.label26.TabIndex = 49;
            this.label26.Text = "File Config";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.processorUnit);
            this.groupBox13.Controls.Add(this.wordMemory_comboBox);
            this.groupBox13.Controls.Add(this.label12);
            this.groupBox13.Controls.Add(this.label16);
            this.groupBox13.Controls.Add(this.processorSpeed);
            this.groupBox13.Controls.Add(this.label17);
            this.groupBox13.Controls.Add(this.label18);
            this.groupBox13.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox13.ForeColor = System.Drawing.Color.Navy;
            this.groupBox13.Location = new System.Drawing.Point(19, 403);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(378, 105);
            this.groupBox13.TabIndex = 48;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Architecture and Processor";
            // 
            // processorUnit
            // 
            this.processorUnit.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processorUnit.ForeColor = System.Drawing.Color.Blue;
            this.processorUnit.FormattingEnabled = true;
            this.processorUnit.Location = new System.Drawing.Point(273, 63);
            this.processorUnit.Name = "processorUnit";
            this.processorUnit.Size = new System.Drawing.Size(89, 24);
            this.processorUnit.TabIndex = 52;
            // 
            // wordMemory_comboBox
            // 
            this.wordMemory_comboBox.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wordMemory_comboBox.ForeColor = System.Drawing.Color.Blue;
            this.wordMemory_comboBox.FormattingEnabled = true;
            this.wordMemory_comboBox.Location = new System.Drawing.Point(130, 28);
            this.wordMemory_comboBox.Name = "wordMemory_comboBox";
            this.wordMemory_comboBox.Size = new System.Drawing.Size(89, 24);
            this.wordMemory_comboBox.TabIndex = 52;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label12.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(228, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 20);
            this.label12.TabIndex = 51;
            this.label12.Text = "bits";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label16.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(228, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 20);
            this.label16.TabIndex = 51;
            this.label16.Text = "Unit";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processorSpeed
            // 
            this.processorSpeed.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processorSpeed.Location = new System.Drawing.Point(131, 60);
            this.processorSpeed.Name = "processorSpeed";
            this.processorSpeed.Size = new System.Drawing.Size(88, 25);
            this.processorSpeed.TabIndex = 50;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label17.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(12, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 20);
            this.label17.TabIndex = 51;
            this.label17.Text = "Processor Speed";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label18.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(12, 28);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 20);
            this.label18.TabIndex = 49;
            this.label18.Text = "Word Memory";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.select_read_input_Button);
            this.groupBox11.Controls.Add(this.select_write_output_Button);
            this.groupBox11.Controls.Add(this.output_writing_file);
            this.groupBox11.Controls.Add(this.input_reading_file);
            this.groupBox11.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.ForeColor = System.Drawing.Color.Navy;
            this.groupBox11.Location = new System.Drawing.Point(445, 230);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(372, 113);
            this.groupBox11.TabIndex = 48;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Select file definition of Reading/Writing PINs";
            // 
            // select_read_input_Button
            // 
            this.select_read_input_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.select_read_input_Button.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.select_read_input_Button.ForeColor = System.Drawing.Color.Black;
            this.select_read_input_Button.Location = new System.Drawing.Point(16, 30);
            this.select_read_input_Button.Name = "select_read_input_Button";
            this.select_read_input_Button.Size = new System.Drawing.Size(81, 30);
            this.select_read_input_Button.TabIndex = 53;
            this.select_read_input_Button.Text = "Inputs";
            this.select_read_input_Button.UseVisualStyleBackColor = false;
            // 
            // select_write_output_Button
            // 
            this.select_write_output_Button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.select_write_output_Button.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.select_write_output_Button.ForeColor = System.Drawing.Color.Black;
            this.select_write_output_Button.Location = new System.Drawing.Point(19, 66);
            this.select_write_output_Button.Name = "select_write_output_Button";
            this.select_write_output_Button.Size = new System.Drawing.Size(78, 30);
            this.select_write_output_Button.TabIndex = 52;
            this.select_write_output_Button.Text = "Outputs";
            this.select_write_output_Button.UseVisualStyleBackColor = false;
            // 
            // output_writing_file
            // 
            this.output_writing_file.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_writing_file.Location = new System.Drawing.Point(115, 69);
            this.output_writing_file.Name = "output_writing_file";
            this.output_writing_file.ReadOnly = true;
            this.output_writing_file.Size = new System.Drawing.Size(241, 22);
            this.output_writing_file.TabIndex = 50;
            // 
            // input_reading_file
            // 
            this.input_reading_file.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input_reading_file.Location = new System.Drawing.Point(115, 34);
            this.input_reading_file.Name = "input_reading_file";
            this.input_reading_file.ReadOnly = true;
            this.input_reading_file.Size = new System.Drawing.Size(241, 22);
            this.input_reading_file.TabIndex = 50;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.pin_number_add);
            this.groupBox16.Controls.Add(this.label19);
            this.groupBox16.Controls.Add(this.PINs_Add);
            this.groupBox16.Controls.Add(this.Pins_ViewList);
            this.groupBox16.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox16.ForeColor = System.Drawing.Color.Navy;
            this.groupBox16.Location = new System.Drawing.Point(445, 403);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(372, 105);
            this.groupBox16.TabIndex = 48;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Pins Config (Inputs/Outputs)";
            // 
            // pin_number_add
            // 
            this.pin_number_add.Font = new System.Drawing.Font("Californian FB", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pin_number_add.Location = new System.Drawing.Point(203, 31);
            this.pin_number_add.Name = "pin_number_add";
            this.pin_number_add.Size = new System.Drawing.Size(80, 25);
            this.pin_number_add.TabIndex = 55;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label19.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(135, 34);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(60, 20);
            this.label19.TabIndex = 54;
            this.label19.Text = "number";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PINs_Add
            // 
            this.PINs_Add.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.PINs_Add.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PINs_Add.ForeColor = System.Drawing.Color.Black;
            this.PINs_Add.Location = new System.Drawing.Point(16, 28);
            this.PINs_Add.Name = "PINs_Add";
            this.PINs_Add.Size = new System.Drawing.Size(105, 30);
            this.PINs_Add.TabIndex = 53;
            this.PINs_Add.Text = "Add Pins";
            this.PINs_Add.UseVisualStyleBackColor = false;
            // 
            // Pins_ViewList
            // 
            this.Pins_ViewList.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Pins_ViewList.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pins_ViewList.ForeColor = System.Drawing.Color.Black;
            this.Pins_ViewList.Location = new System.Drawing.Point(16, 69);
            this.Pins_ViewList.Name = "Pins_ViewList";
            this.Pins_ViewList.Size = new System.Drawing.Size(168, 30);
            this.Pins_ViewList.TabIndex = 52;
            this.Pins_ViewList.Text = "View Pins List";
            this.Pins_ViewList.UseVisualStyleBackColor = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.board_manufacturer);
            this.groupBox12.Controls.Add(this.label15);
            this.groupBox12.Controls.Add(this.board_family);
            this.groupBox12.Controls.Add(this.label13);
            this.groupBox12.Controls.Add(this.board_name);
            this.groupBox12.Controls.Add(this.label14);
            this.groupBox12.Font = new System.Drawing.Font("Californian FB", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.ForeColor = System.Drawing.Color.Navy;
            this.groupBox12.Location = new System.Drawing.Point(19, 69);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(311, 126);
            this.groupBox12.TabIndex = 48;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "General";
            // 
            // board_manufacturer
            // 
            this.board_manufacturer.Location = new System.Drawing.Point(130, 93);
            this.board_manufacturer.Name = "board_manufacturer";
            this.board_manufacturer.Size = new System.Drawing.Size(175, 27);
            this.board_manufacturer.TabIndex = 52;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label15.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(12, 95);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 20);
            this.label15.TabIndex = 53;
            this.label15.Text = "Manufacturer";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // board_family
            // 
            this.board_family.Location = new System.Drawing.Point(130, 57);
            this.board_family.Name = "board_family";
            this.board_family.Size = new System.Drawing.Size(175, 27);
            this.board_family.TabIndex = 50;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label13.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(12, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 20);
            this.label13.TabIndex = 51;
            this.label13.Text = "Family";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // board_name
            // 
            this.board_name.Location = new System.Drawing.Point(130, 21);
            this.board_name.Name = "board_name";
            this.board_name.Size = new System.Drawing.Size(175, 27);
            this.board_name.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label14.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(12, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 20);
            this.label14.TabIndex = 49;
            this.label14.Text = "Board Name";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // g7_names_comboBox1
            // 
            this.g7_names_comboBox1.Font = new System.Drawing.Font("Californian FB", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g7_names_comboBox1.ForeColor = System.Drawing.Color.Black;
            this.g7_names_comboBox1.FormattingEnabled = true;
            this.g7_names_comboBox1.Location = new System.Drawing.Point(209, 37);
            this.g7_names_comboBox1.Name = "g7_names_comboBox1";
            this.g7_names_comboBox1.Size = new System.Drawing.Size(569, 24);
            this.g7_names_comboBox1.TabIndex = 50;
            this.g7_names_comboBox1.SelectedIndexChanged += new System.EventHandler(this.g7_names_comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Maroon;
            this.button1.Location = new System.Drawing.Point(788, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.open_g7_button_Click);
            // 
            // open_g7_editor_UniSim_button
            // 
            this.open_g7_editor_UniSim_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.open_g7_editor_UniSim_button.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_g7_editor_UniSim_button.ForeColor = System.Drawing.Color.Maroon;
            this.open_g7_editor_UniSim_button.Location = new System.Drawing.Point(21, 1);
            this.open_g7_editor_UniSim_button.Name = "open_g7_editor_UniSim_button";
            this.open_g7_editor_UniSim_button.Size = new System.Drawing.Size(260, 30);
            this.open_g7_editor_UniSim_button.TabIndex = 0;
            this.open_g7_editor_UniSim_button.Text = "OPEN GRAFCET EDITOR (UniSim2)";
            this.open_g7_editor_UniSim_button.UseVisualStyleBackColor = false;
            this.open_g7_editor_UniSim_button.Click += new System.EventHandler(this.open_g7_editor_UniSim_button_Click);
            // 
            // G7_Form
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 19);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(889, 665);
            this.Controls.Add(this.board_descript);
            this.Controls.Add(this.g7_names_comboBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.open_g7_editor_UniSim_button);
            this.Controls.Add(this.open_g7_button);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(120, 5);
            this.MaximizeBox = false;
            this.Name = "G7_Form";
            this.Text = "GrafcetConverter Programmer Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.board_descript.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new G7_Form());

            //aTest();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private static void aTest()
        {
            int n, p;
            n = -5; while (n <= 5) n++; Console.WriteLine("A : n = " + n);
            n = p = -8; while (n <= 8) n += p++; Console.WriteLine("B : n = " + n);
            n = p = -8; while (n <= 8) n += ++p; Console.WriteLine("C : n = " + n);
            n = p = -8; while (p <= 5) n += p++; Console.WriteLine("D : n = " + n);
            n = p = 5; while (p <= 5) n += ++p; Console.WriteLine("E : n = " + n);
        }

        // saving the PICMessage obtained in a file
        private void saveGrafcetStaticStructureToFile(string grafcetStaticStructure)
        {
            string strfilename = Directory_name_current + "\\" + this.File_name_only_current + "_g7_static.txt";
            try
            {
                StreamWriter fluxWrite = new StreamWriter(strfilename);
                fluxWrite.Write(grafcetStaticStructure);
                fluxWrite.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: Could not open the file " + strfilename + ".\n *Original error: " + e.Message);
            }

        }

        private GrafcetClass getCurrentGrafcet() {
            return this.GrafcetsList[this.selected_g7_i];
        }

        // saving the PICMessage obtained in a file
        private void savePICMessageToFile(string PICMessage, string DATAinEE)
        {
            //string strfilename = Directory_name_current + "\\" + this.File_name_only_current + "_g7_PICMessagesFile.txt";
            string strfilename = this.getCurrentGrafcet().directory_name + "\\" + this.getCurrentGrafcet().fileNameOnly + "_g7_PICMessagesFile.txt";
            try
            {
                StreamWriter fluxWrite = new StreamWriter(strfilename);
                fluxWrite.Write(PICMessage);
                fluxWrite.Write("\n\n\t DATAinEE :\n\n" + DATAinEE);
                fluxWrite.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: Could not open the file " + strfilename + ".\n *Original error: " + e.Message);
            }
        }

        // Gestion des variables et donc du vecteur R
        private void MyboxHandler(object sender, EventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            this.GrafcetsList[selected_g7_i].Rdyn[Convert.ToInt32(btn.Tag)] = 0;

            if (btn.Checked == true)
            {
                this.GrafcetsList[selected_g7_i].Rdyn[Convert.ToInt32(btn.Tag)] = 1;
            }
        }


        private object Math(string p)
        {
            throw new NotImplementedException();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// To allow the grafcet situation to evaluate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.GrafcetsList[selected_g7_i].GenerateDynamicMatrices();
            this.displayDynamicMatrices();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void displayDynamicMatrices()
        {
            // Initialiser la barre de progrès du processus
            progressBar2.Visible = true;
            //this.progressBar2.
            this.progressBar2.Increment(-10); // Reinitialiser le progress bar
            this.progressBar2.PerformStep();  // avancer à 1/10 la barre2  de progrès
            this.progressBar2.PerformStep();  // avancer à 2/10 la barre2  de progrès
            this.progressBar2.PerformStep();  // avancer à 3/10 la barre2  de progrès
            this.progressBar2.PerformStep();  // avancer à 4/10 la barre2  de progrès

            ////////////////////AFFICHAGE DES DONNEES\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\            
            if (this.display_all_dynamic_vectors)
            {
                simul_textBox.AppendText("X : The vector X of the grafcet state is :" + Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
                {
                    simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().Step_list[x].name + "\t"));
                }

                simul_textBox.AppendText(Environment.NewLine);
                if (this.getCurrentGrafcet().clockNumber > 1)
                {
                    for (int x = 0; x < this.getCurrentGrafcet().X_1.Length; x++)
                    {
                        simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().X_1old[x] + "\t"));
                    }
                }
                else
                {
                    for (int x = 0; x < this.getCurrentGrafcet().X_1.Length; x++)
                    {
                        simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().INIT[x] + "\t"));
                    }
                }
                simul_textBox.AppendText(Environment.NewLine);
                simul_textBox.AppendText(Environment.NewLine);

                simul_textBox.AppendText("R : The vector R of receptivities is :" + Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
                {
                    simul_textBox.AppendText(Convert.ToString("R" + x + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().R1.Length; x++)
                {
                    simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().R1[x] + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
                simul_textBox.AppendText(Environment.NewLine);

                simul_textBox.AppendText("VT : The vector VT of validate transitions is:" + Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
                {
                    simul_textBox.AppendText(Convert.ToString("VT" + x + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().VT1.Length; x++)
                {
                    simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().VT1[x] + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
                simul_textBox.AppendText(Environment.NewLine);

                simul_textBox.AppendText("FT : The vector FT of franchissable transitions is:" + Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
                {
                    simul_textBox.AppendText(Convert.ToString("FT" + x + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
                for (int x = 0; x < this.getCurrentGrafcet().FT1.Length; x++)
                {
                    simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().FT1[x] + "\t"));
                }
                simul_textBox.AppendText(Environment.NewLine);
            }

            this.progressBar2.PerformStep();  // avancer à 5/10 la barre2  de progrès

            simul_textBox.AppendText(Environment.NewLine);

            simul_textBox.AppendText("X : The vector X of the new grafcet situation is :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().Step_list[x].name + "\t"));
            }

            this.progressBar2.PerformStep();  // avancer à 6/10 la barre2  de progrès

            simul_textBox.AppendText(Environment.NewLine);

            for (int x = 0; x < this.getCurrentGrafcet().X_1.Length; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().X_1[x] + "\t"));
            }

            this.progressBar2.PerformStep();  // avancer à 7/10 la barre2  de progrès

            simul_textBox.AppendText(Environment.NewLine);
            simul_textBox.AppendText(Environment.NewLine);

            simul_textBox.AppendText("O : The output vector O of actions is :" + Environment.NewLine);

            this.progressBar2.PerformStep();  // avancer à 8/10 la barre2  de progrès

            for (int x = 0; x < this.getCurrentGrafcet().number_actions; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().VarTrans_name[this.getCurrentGrafcet().number_inputs + x] + "\t"));
            }

            this.progressBar2.PerformStep();  // avancer à 9/10 la barre2  de progrès

            simul_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_actions; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().O[x] + "\t"));
            }

            this.progressBar2.PerformStep();  // avancer à 10/10 la barre2  de progrès

            simul_textBox.AppendText(Environment.NewLine);
            simul_textBox.AppendText("___________________________________________________________________________________\n");
            simul_textBox.AppendText(Environment.NewLine);
        }

        private void open_g7_button_Click(object sender, EventArgs e)
        {
            this.start_button.Text = "START>";
            progressBar1.Visible = true;
            this.progressBar1.Increment(-10);// Reinitialiser le progress bar
            //timer1.Stop(); // stop timer1 if running 

            this.clearing_All_forms_ReINIT();

            this.progressBar1.PerformStep(); //Avancer à 1 la barre

            // début du traitement
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //dialog box permitting to open the XML file
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                this.Directory_name_current = Path.GetDirectoryName(path);
                this.File_name_current = Path.GetFileName(path);
                this.File_name_only_current = this.File_name_current.Substring(0, this.File_name_current.IndexOf('.'));
                string strfilename = this.Directory_name_current + "\\" + this.File_name_only_current + "result_PICMessageFile.txt";

                this.progressBar1.PerformStep(); //Avancer à 2 la barre

                //try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream) //myStream = utilisation du nom du fichier ouvert
                        {
                            // Construction du grafcet à l'aide du flux lu du fichier sélectionné
                            GrafcetClass aG7 = new GrafcetClass();

                            aG7.init_file_location(this.Directory_name_current, this.File_name_only_current, ".xml");
                            aG7.ReadUniSimXML2(myStream);

                            this.progressBar1.PerformStep(); //Avancer à 3 la barre

                            aG7.GenerateStaticMatrices();

                            this.add_new_G7(aG7);

                            /////////////////////////PARTIE AFFICHAGE DES DONNES\\\\\\\\\\\\\\\\\\\\\\\\\\
                            /*this.displayStaticData();
                            this.displayStaticData_condensed();
                            this.finalize_displayStaticData();*/

                            //MessageBox.Show("Building grafcet matrices : Process Completed !");
                        }
                    }
                }
                // Gérard: to uncomment 
                /*catch (Exception ex)
                {
                    MessageBox.Show("Error, Could not read file from disk. Original error: " + ex.Message);
                }*/
            }
            // fin du traitement
            progressBar1.Visible = false;
        }

        private void displayStaticData()
        {
            static_g7_textBox.Clear();

            this.progressBar1.PerformStep(); //Avancer à 4 la barre

            //Affichage du nombre de transitions, d'étapes et d'actions
            static_g7_textBox.AppendText("Number of transitions \t:    " + this.getCurrentGrafcet().number_transitions + Environment.NewLine);
            static_g7_textBox.AppendText("Number of steps \t\t:    " + this.getCurrentGrafcet().number_steps + Environment.NewLine);
            static_g7_textBox.AppendText("Number of inputs \t\t:    " + this.getCurrentGrafcet().number_inputs + Environment.NewLine);
            static_g7_textBox.AppendText("Number of actions \t\t:    " + this.getCurrentGrafcet().number_actions + Environment.NewLine);

            // Affichage du vecteur INIT
            static_g7_textBox.AppendText(Environment.NewLine);
            static_g7_textBox.AppendText("The vector INIT of the initial situation is :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().Step_list[x].name));
            }
            static_g7_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().INIT[x]));
            }
            static_g7_textBox.AppendText(Environment.NewLine);


            // Affichage de la matrice E
            static_g7_textBox.AppendText(Environment.NewLine + "The matrix E of steps in Upstream of transitions :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().Step_list[x].name));
            }
            static_g7_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("T" + (x + 1)));
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().E[x, y]));
                }
                static_g7_textBox.AppendText(Environment.NewLine);
            }

            this.progressBar1.PerformStep(); //Avancer à 5 la barre

            // Affichage de la matrice S
            static_g7_textBox.AppendText(Environment.NewLine + "The matrix S of steps in Downstream of transitions :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().Step_list[x].name));
            }
            static_g7_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("T" + (x + 1)));
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().S[x, y]));
                }
                static_g7_textBox.AppendText(Environment.NewLine);
            }


            this.progressBar1.PerformStep(); //Avancer à 6 la barre

            //Affichage de la matrice MA
            static_g7_textBox.AppendText(Environment.NewLine + "The matrix MA of actions is :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().Step_list[x].name));
            }
            static_g7_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_actions; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().VarTrans_name[this.getCurrentGrafcet().number_inputs + x]));
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    static_g7_textBox.AppendText(Convert.ToString("\t" + this.getCurrentGrafcet().MA[x, y]));
                }
                static_g7_textBox.AppendText(Environment.NewLine);
            }

            this.progressBar1.PerformStep(); //Avancer à 7 la barre


            //Affichage du vecteur RS
            static_g7_textBox.AppendText(Environment.NewLine);
            static_g7_textBox.AppendText("The vector RS of evaluation sequence of receptivities is :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                static_g7_textBox.AppendText(Convert.ToString("RS[" + x + "]" + ":  "));
                for (int y = 0; y < this.getCurrentGrafcet().RS[x].Count; y++)
                {
                    static_g7_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().RS[x][y] + "  "));
                }
                static_g7_textBox.AppendText(Environment.NewLine);
            }

            // Construction du checkbox des variables
            this.tabCheckbox = new CheckBox[this.getCurrentGrafcet().number_inputs];
            int top = 0;
            int left = 2;
            for (int i = 0; i < this.getCurrentGrafcet().number_inputs; i++)
            {
                this.tabCheckbox[i] = new CheckBox();

                this.tabCheckbox[i].BackColor = Color.LightSteelBlue;
                this.tabCheckbox[i].ForeColor = Color.Black;

                this.tabCheckbox[i].AutoSize = false;
                this.tabCheckbox[i].AutoEllipsis = true;
                this.tabCheckbox[i].Left = left;
                this.tabCheckbox[i].Top = top;
                this.tabCheckbox[i].Checked = false;
                this.tabCheckbox[i].Tag = i;
                this.tabCheckbox[i].Text = this.getCurrentGrafcet().VarTrans_name[i];
                this.tabCheckbox[i].Click += new EventHandler(this.MyboxHandler);
                this.input_checkedListBox.Controls.Add(this.tabCheckbox[i]);
                top += this.tabCheckbox[i].Height + 2;
                this.tabCheckbox[i].Enabled = false;
                //this.tabCheckbox[i].Enabled = true;
            }

        }

        private void displayStaticData_condensed()
        {
            this.progressBar1.PerformStep(); //Avancer à 8 la barre
            //Affichage du nombre de transitions, d'étapes et d'actions
            this.condensed_g7_data_textBox.Clear();
            condensed_g7_data_textBox.AppendText(" #  Transitions\t:  " + this.getCurrentGrafcet().number_transitions + Environment.NewLine);
            condensed_g7_data_textBox.AppendText(" #  Seps\t\t:  " + this.getCurrentGrafcet().number_steps + Environment.NewLine);
            condensed_g7_data_textBox.AppendText(" #  Inputs\t\t:  " + this.getCurrentGrafcet().number_inputs + Environment.NewLine);
            condensed_g7_data_textBox.AppendText(" #  Actions\t:  " + this.getCurrentGrafcet().number_actions + Environment.NewLine);
            condensed_g7_data_textBox.AppendText("------------------------------------------" + Environment.NewLine);

            // E, S, MA et INIT en EN MODE BRUT
            condensed_g7_data_textBox.AppendText(Environment.NewLine + "INIT:" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                condensed_g7_data_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().INIT[x]));
            }
            condensed_g7_data_textBox.AppendText(Environment.NewLine);

            condensed_g7_data_textBox.AppendText(Environment.NewLine + "E:" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    condensed_g7_data_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().E[x, y]));
                }
                condensed_g7_data_textBox.AppendText(Environment.NewLine);
            }


            condensed_g7_data_textBox.AppendText(Environment.NewLine + "S:" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    condensed_g7_data_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().S[x, y]));
                }
                condensed_g7_data_textBox.AppendText(Environment.NewLine);
            }

            this.progressBar1.PerformStep(); //Avancer à 9 la barre

            condensed_g7_data_textBox.AppendText(Environment.NewLine + "MA:" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_actions; x++)
            {
                for (int y = 0; y < this.getCurrentGrafcet().number_steps; y++)
                {
                    condensed_g7_data_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().MA[x, y]));
                }
                condensed_g7_data_textBox.AppendText(Environment.NewLine);
            }


            //Affichage du vecteur RS
            condensed_g7_data_textBox.AppendText(Environment.NewLine);
            condensed_g7_data_textBox.AppendText("RS Array" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_transitions; x++)
            {
                condensed_g7_data_textBox.AppendText(Convert.ToString("RS[" + x + "]" + " :  "));
                for (int y = 0; y < this.getCurrentGrafcet().RS[x].Count; y++)
                {
                    condensed_g7_data_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().RS[x][y] + "  "));
                }
                condensed_g7_data_textBox.AppendText(Environment.NewLine);
            }
            this.progressBar1.PerformStep(); //Avancer à 10 la barre
        }

        private void finalize_displayStaticData()
        {
            static_g7_textBox.Multiline = true;              // Set the Multiline property to true.
            static_g7_textBox.AcceptsReturn = false;         // Allow the TAB key to be entered in the TextBox control.
            static_g7_textBox.AcceptsTab = true;             // Allow the TAB key to be entered in the TextBox control.
            static_g7_textBox.WordWrap = false;              // Set WordWrap to true to allow text to wrap to the next line.
            static_g7_textBox.ScrollBars = ScrollBars.Both;  // Add vertical and horizontal scroll bars to the TextBox control.

            simul_textBox.Clear();
            simul_textBox.AcceptsReturn = false;

            this.open_simulations_button.Enabled = true;

            this.progressBar1.PerformStep(); //Avancer à 10 la barre

            this.generate_hex_board_button1.Enabled = true;
            this.save_g7_text_button.Enabled = true;
            //this.save_g7_hex_button.Enabled = true;
        }


        private void generate_hex_board_button_Click(object sender, EventArgs e)
        {
            this.generate_hex_board_data();
        }

        private void generate_hex_board_data()
        {
            this.PIC_Message_textBox.Text = "";
            this.size_textBox1.Text = "";

            this.GrafcetsList[selected_g7_i].BuildPIControllerMessage();
            this.GrafcetsList[selected_g7_i].Build_DatainEE_Array();

            if (this.GrafcetsList[selected_g7_i].PICMessage != null)
            {
                string pic_message = this.getCurrentGrafcet().PICMessage;

                //Modify and add Mapping info to the PIC Message
                //if (this.io_mapper_form.io_mapper.is_mapped)
                {
                    //Add to PIC Message mapping informations

                    //Set the PIC Message
                    this.PIC_Message_textBox.Text = pic_message;
                    this.size_textBox1.Text = Convert.ToString((this.getCurrentGrafcet().PICMessage.Length / 2) + " bytes");

                    this.save_hex_data_button2.Enabled = true;
                    this.send_pic_message_button.Enabled = true;
                    this.board_device_comboBox.Enabled = true;
                    this.export_button.Enabled = false;
                    this.upload_button.Enabled = false;

                    MessageBox.Show("Generating hex board data : Process Completed !");
                }
                /*else
                {
                    MessageBox.Show("You should map your board pins !");
                }*/
            }
        }

        private void checkAll_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.getCurrentGrafcet().number_inputs; i++)
            {
                this.tabCheckbox[i].Checked = true;
                //MessageBox.Show("Checking " + this.GrafcetsList[selected_g7_i].VarTrans_name[i] + " : " + this.tabCheckbox[i]);
            }
        }

        private void clearAll_button_Click(object sender, EventArgs e)
        {
            /*button4.Enabled = false;
            button2.Enabled = false;
            this.checkedListBox1.Controls.Clear();*/
            for (int i = 0; i < this.getCurrentGrafcet().number_inputs; i++)
            {
                this.tabCheckbox[i].Checked = false;
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            this.progressBar2.Visible = true;

            this.stop_button.Enabled = true;
            this.open_g7_button.Enabled = false;
            this.open_g7_text_button.Enabled = false;
            this.start_button.Enabled = false;
            this.clear_button.Enabled = false;

            timer1.Start();
        }

        private void displayInitialSituation()
        {
            simul_textBox.AppendText("X : The vector X of the grafcet state is :" + Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().number_steps; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().Step_list[x].name + "\t"));
            }
            simul_textBox.AppendText(Environment.NewLine);
            for (int x = 0; x < this.getCurrentGrafcet().X_1.Length; x++)
            {
                simul_textBox.AppendText(Convert.ToString(this.getCurrentGrafcet().X_1[x] + "\t"));
            }
            simul_textBox.AppendText(Environment.NewLine);
            simul_textBox.AppendText(Environment.NewLine);
            simul_textBox.AppendText("_________________________________________________________________________________\n");
            simul_textBox.AppendText(Environment.NewLine);
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            this.open_g7_button.Enabled = true;
            this.open_g7_text_button.Enabled = true;

            this.stop_button.Enabled = false;
            this.clear_button.Enabled = true;
            this.start_button.Text = "Continue>";
            this.start_button.Enabled = true;
            this.reinit_button.Enabled = true;
            this.progressBar2.Visible = false;


            this.GrafcetsList[selected_g7_i].finalize_DynamicTimeVarEvents();

            timer1.Stop();
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            progressBar2.Visible = false;
            simul_textBox.Clear();
        }

        private void step_step_button_Click(object sender, EventArgs e)
        {
            // Appel à l'évolution de la situation du grafcet
            this.GrafcetsList[selected_g7_i].GenerateDynamicMatrices();
            this.displayDynamicMatrices();
        }

        private void open_simulations_button_Click(object sender, EventArgs e)
        {
            this.enable_simulations_buttons();

            this.GrafcetsList[selected_g7_i].create_StreamFile_for_DynamicTimeVarEvents();
            // other configurations for the simulation
            if (this.GrafcetsList[selected_g7_i].hasTimeCondition)
            {
                this.step_step_button.Enabled = false;
                //this.timer1.Interval = Grafcet.getLeastTimeCondition()/2;
            }
        }
        private void enable_simulations_buttons()
        {
            this.checkAll_button.Enabled = true;
            this.clearAll_button.Enabled = true;
            this.start_button.Enabled = true;
            this.step_step_button.Enabled = true;

            this.open_simulations_button.Enabled = false;

            // enabling input variables
            //this.input_checkkedListBox.Enabled = true;
            for (int i = 0; i < this.getCurrentGrafcet().number_inputs; i++)
            {
                this.tabCheckbox[i].Enabled = true;
                //MessageBox.Show("Enabling " + this.GrafcetsList[selected_g7_i].VarTrans_name[i]);
            }
        }
        private void disable_simulations_buttons()
        {
            this.checkAll_button.Enabled = false;
            this.clearAll_button.Enabled = false;
            this.clear_button.Enabled = false;
            this.start_button.Enabled = false;
            this.step_step_button.Enabled = false;
            this.stop_button.Enabled = false;
            this.reinit_button.Enabled = false;
        }

        private void open_serial_port_button_Click(object sender, EventArgs e)
        {
            frmTerminal myTerminal = new frmTerminal("");
            myTerminal.Show();
        }
        private void send_pic_message_button_Click(object sender, EventArgs e)
        {
            int nb_g7 = this.GrafcetsList.Count;
            if (nb_g7 > 0 && this.selected_g7_i <= nb_g7)
            {
                if (this.GrafcetsList[this.selected_g7_i].PICMessage != "")
                {
                    frmTerminal myTerminal = new frmTerminal(this.getCurrentGrafcet().PICMessage);
                    myTerminal.Show();
                }
                else
                {
                    MessageBox.Show("Generate the hexadecimal board data ! ");
                }
            }
            else
                if (this.PIC_Message_textBox.Text != null)
                {
                    frmTerminal myTerminal = new frmTerminal(this.PIC_Message_textBox.Text);
                    myTerminal.Show();
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.reinit_button.Enabled = false;
            this.simul_textBox.Clear();
            progressBar2.Visible = false;
            this.start_button.Text = "START>";
            this.GrafcetsList[selected_g7_i].reinitializeG7();
        }

        private void save_g7_text_button_Click(object sender, EventArgs e)
        {
            //try
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "g7 files (*.g7)|*.g7";
            saveFileDialog1.FilterIndex = 3;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = this.File_name_only_current;// +".g7";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.GrafcetsList[selected_g7_i].build_g7_Text_structure();

                //Console.WriteLine(Grafcet.g7_text_struct);
                //MessageBox.Show("File name: " + saveFileDialog1.FileName);

                string strfilename = saveFileDialog1.FileName;
                StreamWriter fluxWrite = new StreamWriter(strfilename);
                fluxWrite.Write(this.GrafcetsList[selected_g7_i].g7_text_struct);
                fluxWrite.Close();
                MessageBox.Show("Grafcet text structure saved !");
            }
            /*catch (Exception e) {
                MessageBox.Show("Error: Could not create the file.");
            }*/
        }

        private void save_hex_board_data_Click(object sender, EventArgs e)
        {
            //try Modify
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "hex files (*.hex)|*.hex";
            saveFileDialog1.FilterIndex = 4;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = this.File_name_only_current;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strfilename = saveFileDialog1.FileName;
                StreamWriter fluxWrite = new StreamWriter(strfilename);
                fluxWrite.Write(this.GrafcetsList[selected_g7_i].PICMessage);
                fluxWrite.Close();
                MessageBox.Show("Hexadecimal board message saved !");
            }
            /*catch (Exception e) {
                MessageBox.Show("Error: Could not create the file.");
            }*/
        }

        private void open_hex_board_data_button1_Click(object sender, EventArgs e)
        {
            this.PIC_Message_textBox.Clear();
            this.size_textBox1.Clear();
            // début du traitement
            Stream myStream1 = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //dialog box permitting to open the XML file
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "hex files (*.hex)|*.hex";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream1 = openFileDialog1.OpenFile()) != null)
                {
                    using (myStream1)
                    {
                        StreamReader myStreamReader = new StreamReader(myStream1);
                        this.PIC_Message_textBox.Text = myStreamReader.ReadLine();
                        this.size_textBox1.Text = Convert.ToString((this.PIC_Message_textBox.Text.Length / 2) + " bytes");
                        this.send_pic_message_button.Enabled = true;
                    }
                }
            }
        }

        private void open_g7_hex_button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not yet implemented ... ");
        }

        private void open_g7_text_button_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            this.start_button.Text = "START>";
            this.progressBar1.Increment(-10);// Reinitialiser le progress bar
            //timer1.Stop(); // stop timer1 if running 

            this.static_g7_textBox.Clear();
            this.simul_textBox.Clear();
            this.condensed_g7_data_textBox.Clear();
            this.PIC_Message_textBox.Clear();
            this.size_textBox1.Clear();
            this.input_checkedListBox.Controls.Clear();

            // disable simulations buttons
            this.open_simulations_button.Enabled = false;
            this.disable_simulations_buttons();

            this.send_pic_message_button.Enabled = false;
            this.save_hex_data_button2.Enabled = false;
            this.generate_hex_board_button1.Enabled = false;
            this.save_g7_text_button.Enabled = false;
            this.save_g7_hex_button.Enabled = false;

            this.board_device_comboBox.Enabled = false;
            this.build_project_button.Enabled = false;
            this.code_generation_button.Enabled = false;
            this.io_map_button.Enabled = false;
            this.export_button.Enabled = false;
            this.upload_button.Enabled = false;

            this.initIOManager();

            this.progressBar1.PerformStep(); //Avancer à 1 la barre

            // début du traitement
            Stream myStream1 = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //dialog box permitting to open the XML file
            openFileDialog1.Filter = "g7 files (*.g7)|*.g7";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                this.Directory_name_current = Path.GetDirectoryName(path);
                this.File_name_current = Path.GetFileName(path);
                this.File_name_only_current = this.File_name_current.Substring(0, this.File_name_current.IndexOf('.'));
                string strfilename = this.Directory_name_current + "\\" + this.File_name_only_current + "result_PICMessageFile.txt";

                this.progressBar1.PerformStep(); //Avancer à 2 la barre

                //try
                {
                    if ((myStream1 = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream1) //myStream = utilisation du nom du fichier ouvert
                        {
                            StreamReader myStreamReader = new StreamReader(myStream1);
                            // Construction du grafcet à l'aide du flux lu du fichier sélectionné
                            GrafcetClass aG7 = new GrafcetClass();

                            aG7.init_file_location(this.Directory_name_current, this.File_name_only_current, ".txt");

                            aG7.Read_Save_g7_TextStructure(myStreamReader);


                            // Static matrices rare yet generated

                            //MessageBox.Show("Good Generation !");

                            this.progressBar1.PerformStep();

                            this.add_new_G7(aG7);
                            /*
                            this.GrafcetsList.Add(aG7);
                            
                            this.selected_g7_i = GrafcetsList.IndexOf(aG7);
                            this.g7_names_comboBox1.Items.Add(aG7.name);
                            this.g7_names_comboBox1.SelectedIndex = this.selected_g7_i;
                            */
                            /////////////////////////PARTIE AFFICHAGE DES DONNES\\\\\\\\\\\\\\\\\\\\\\\\\\
                            /*this.displayStaticData();
                            this.displayStaticData_condensed();

                            this.finalize_displayStaticData();*/

                            MessageBox.Show("Building grafcet matrices : Process Completed !");
                        }
                    }

                }

            }

            progressBar1.Visible = false;
        }
        private void add_new_G7(GrafcetClass aG7)
        {
            int new_g7_combo_index = this.g7_names_comboBox1.Items.IndexOf(aG7.file_name_full);
            //MessageBox.Show("g7_combo_index : " + new_g7_combo_index);

            if (new_g7_combo_index >= 0)
            {
                //This grafcet exists in the list: it is an old one but may habe been updated
                MessageBox.Show("The file :\n" + aG7.file_name_full + " \talready exists. \n\nIt will be reopened !");
                this.GrafcetsList.RemoveAt(new_g7_combo_index);
                this.g7_names_comboBox1.Items.RemoveAt(new_g7_combo_index);
            }

            this.GrafcetsList.Add(aG7);
            this.g7_names_comboBox1.Items.Add(aG7.file_name_full);
            this.selected_g7_i = GrafcetsList.IndexOf(aG7);

            //The following instruction creates an event: the method g7_names_comboBox1_SelectedIndexChanged is called
            this.g7_names_comboBox1.SelectedIndex = this.selected_g7_i;

            /*this.displayStaticData();
            this.displayStaticData_condensed();
            this.finalize_displayStaticData();*/
        }

        private void clock_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int item_index = this.clock_interval_comboBox.SelectedIndex;
            int new_clock_interval = this.clock_values[item_index];
            GrafcetClass.simulation_g7_clock_interval = new_clock_interval;
            Console.WriteLine("\nNew Grafcet Clock Interval : " + new_clock_interval + " ms");
        }

        private void display_all_dynamic_vector_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.display_all_dynamic_vector_checkbox.Checked)
                this.display_all_dynamic_vectors = true;
            else
                this.display_all_dynamic_vectors = false;
        }

        private void g7_names_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the index of the current grafcet
            this.selected_g7_i = this.g7_names_comboBox1.SelectedIndex;

            progressBar1.Visible = true;
            this.progressBar1.Increment(-10);// Reinitialiser le progress bar

            this.progressBar1.PerformStep(); //Avancer à 1 la barre

            static_g7_textBox.Clear();
            simul_textBox.Clear();
            PIC_Message_textBox.Clear();
            size_textBox1.Clear();
            this.input_checkedListBox.Controls.Clear();

            this.progressBar1.PerformStep(); //Avancer à 3 la barre

            // disable simulations buttons
            this.open_simulations_button.Enabled = false;
            this.disable_simulations_buttons();
            this.send_pic_message_button.Enabled = false;
            this.save_hex_data_button2.Enabled = false;
            this.generate_hex_board_button1.Enabled = false;
            this.save_g7_text_button.Enabled = false;
            this.save_g7_hex_button.Enabled = false;

            this.board_device_comboBox.Enabled = false;
            this.build_project_button.Enabled = false;
            this.code_generation_button.Enabled = false;
            this.io_map_button.Enabled = false;
            this.export_button.Enabled = false;
            this.upload_button.Enabled = false;

            this.initIOManager();

            this.progressBar1.PerformStep(); //Avancer à 3 la barre

            /////////////////////////PARTIE AFFICHAGE DES DONNES\\\\\\\\\\\\\\\\\\\\\\\\\\
            this.displayStaticData();
            this.displayStaticData_condensed();
            this.finalize_displayStaticData();
            this.generate_hex_board_data();

            progressBar1.Visible = false;
        }

        private void open_UniSim_button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("");
            /*
            String strAppDir = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetName().CodeBase);
            String strFullPathToMyFile = Path.Combine(strAppDir, "fileName.txt");

            MessageBox.Show(String.Format("Path to the application is: '{0}'." +
                "\nFull path to the file in the application folder is: '{1}'",
                strAppDir, strFullPathToMyFile));
            */

            MainForm mf = new MainForm();
            mf.Show();    //(new MainForm()).Show(); 
        }

        private void dsPIC_build_compiler_source_code()
        {
            MessageBox.Show("Not yet implemented !");
        }

        private void dsPIC_build_virtualMachine_source_code()
        {
            //generate the DatainEE Array
            //use later : string directPath = this.Directory_name + "\\" + fileName;
            string final_source_code_file_path = "..\\Grafcet_2016_01\\src\\main_DataEEPROM_access.c";
            Microcontroller actual_board = this.io_mapper_settings.getCurrentBoard();
            string header_source_path = actual_board.sc_header_path;   //"..\\Grafcet_2015_04\\src\\use_static\\head_DataEEPROM.c";
            string source_code_header_declarePINs_path = actual_board.sc_header_path;
            string queue_source_path = Microcontroller.sc_functions_definition;     //"..\\Grafcet_2015_04\\src\\use_static\\queue_DataEEPROM.c";

            // Writing in the source code
            StreamWriter sWriter = new StreamWriter(final_source_code_file_path);

            //Adding the General header to the source code
            StreamReader sReader = new StreamReader(header_source_path);
            string current_line;
            while ((current_line = sReader.ReadLine()) != null)
            {
                sWriter.WriteLine(current_line);
            }
            sReader.Close();
            //Adding to the header the PINs declaration
            sReader = new StreamReader(source_code_header_declarePINs_path);
            while ((current_line = sReader.ReadLine()) != null)
            {
                sWriter.WriteLine(current_line);
            }
            sReader.Close();

            //Adding declaration of Materials from I/O conf to the source code: Inputs and Ouputs
            int N_input_pins_effective = this.io_mapper_settings.getNumberOfInputPins();
            int N_ouput_pins_effective = this.io_mapper_settings.getNumberOfOutputPins();
            int board_index = IOMapper.selected_µC_index;

            sWriter.WriteLine();
            sWriter.WriteLine("//Map Inputs with INPUT PINs");
            sWriter.WriteLine("//Effective number of Inputs for the current system : " + N_input_pins_effective);

            for (int i_input = 0; i_input < N_input_pins_effective + N_ouput_pins_effective; i_input++)
            {
                IOPin pin_elt = this.io_mapper_settings.getCurrentBoard().pins_list_digital[i_input];
                if (pin_elt.mode.Equals(IOPin.pin_mode_in))
                {
                    string pin_address = pin_elt.pin_address;
                    sWriter.WriteLine("# define pin_I" + i_input + " " + pin_address);
                } 
            }

            sWriter.WriteLine("\n//Map Outputs with OUTPUT PINs");
            sWriter.WriteLine("//Effective number of Outputs for the current system : " + N_ouput_pins_effective);

            for (int i_input = 0; i_input < N_input_pins_effective + N_ouput_pins_effective; i_input++)
            {
                IOPin pin_elt = this.io_mapper_settings.getCurrentBoard().pins_list_digital[i_input];
                if (pin_elt.mode.Equals(IOPin.pin_mode_out))
                {
                    string pin_address = pin_elt.pin_address;
                    sWriter.WriteLine("# define pin_I" + i_input + " " + pin_address);
                }
            }
            
            string output_board_real;
            /*for (int i_out_eff = 0; i_out_eff < N_ouput_effective; i_out_eff++)
            {
                output_board_real = this.io_form.iomapper.getRealOutputWithView(board_index, this.io_form.selected_outputs[i_out_eff]);
                sWriter.WriteLine("# define " + ("pin_Q" + i_out_eff + " ") + output_board_real);
            }

            //Complete the "useless" materials OUTPUT
            int OUT_total_size = actual_board.outputs.Count;
            string out_device_view;
            int i_output_next = N_ouput_effective;
            for (int i_output_device = 0; i_output_device < OUT_total_size; i_output_device++)
            {
                out_device_view = actual_board.outputs[i_output_device].io_view;
                if (!this.io_form.selected_outputs.Contains(out_device_view))
                {
                    output_board_real = this.io_form.iomapper.getRealOutputWithView(board_index, out_device_view);
                    sWriter.WriteLine("# define " + ("pin_Q" + i_output_next + " ") + output_board_real);
                    i_output_next++;
                }
            }

            */
            //Adding data of data_inEE_array (synthesis matrices)
            sWriter.WriteLine("\n");
            sWriter.WriteLine("unsigned int _EEDATA(2) DatainEE[] = {");
            sWriter.WriteLine(this.getCurrentGrafcet().DataInEE_array);
            sWriter.WriteLine("\n};");
            sWriter.WriteLine("//From grafcet source File: " + this.getCurrentGrafcet().file_name_full);

            //Adding the queue to the source code
            sReader = new StreamReader(queue_source_path);
            while ((current_line = sReader.ReadLine()) != null)
            {
                sWriter.WriteLine(current_line);
            }
            sReader.Close();

            sWriter.Close();
        }

        public static void appenFileToStreamWriter(string file_path, StreamWriter s_writer)
        {
            StreamReader sReader = new StreamReader(file_path);
            string current_line;
            while ((current_line = sReader.ReadLine()) != null)
            {
                s_writer.WriteLine(current_line);
            }
            sReader.Close();
        }

        private string code_gen_initializations(string prefix_name, string extension, string board_name)
        {
            string fileName = prefix_name + "_" + board_name + "_" + this.getCurrentGrafcet().fileNameOnly;
            string fileName_ext = fileName + "." + extension;
            string directPath = this.getCurrentGrafcet().directory_name + "\\" + fileName;

            //Create folder directPath
            try
            {
                DirectoryInfo dir = Directory.CreateDirectory(directPath);
                Console.WriteLine("The directory < " + directPath + " > was created successfully at {0}.", Directory.GetCreationTime(directPath));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            //Returns the path of the file in which code will be generated
            return directPath + "\\" + fileName_ext; ;
        }

        private void Arduino_build_simple_source_code()
        {
            Microcontroller selected_µC = this.io_mapper_settings.getCurrentBoard();
            GrafcetClass g7_current = this.getCurrentGrafcet();

            string source_code_file_path = this.code_gen_initializations("G7_Sketch", "ino", selected_µC.name);

            StreamWriter source_code_stream = new StreamWriter(source_code_file_path);

            source_code_stream.WriteLine("/*");
            source_code_stream.WriteLine("Time :\t" + DateTime.Now.ToString());
            source_code_stream.WriteLine("Generated from the grafcet File: \n\t" + this.getCurrentGrafcet().file_name_full);
            source_code_stream.WriteLine("*/");

            if (g7_current.hasTimeCondition)
            {
                source_code_stream.WriteLine("#include \"TimerOne.h\" ");
            }

            //Adding to the header the PINs declaration :Useless
            //G7_Form.appenFileToStreamWriter(selected_µC.sc_header_path, source_code_stream);
            
            //Declare INPUT PINs
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \t Declare INPUT pins mapped\t**** \t Total Inputs : " + g7_current.number_inputs);
            if (g7_current.number_inputs > 0) {
                foreach ( IOPin pin_elt in selected_µC.pins_list_digital)
                {
                    if (pin_elt.mode.Equals(IOPin.pin_mode_in))
                    {
                        source_code_stream.WriteLine("const unsigned int pin_" + pin_elt.variable_mapped + " = " + pin_elt.pin_address + ";");
                    }
                }
            }
            
            //Declare OUTPUT PINs
            source_code_stream.WriteLine();
            source_code_stream.WriteLine("//**** \tDeclare OUTPUT pins mapped \t**** \t Total Outputs : " + g7_current.number_actions);
            if (g7_current.number_actions > 0)
            {
                foreach (IOPin pin_elt in selected_µC.pins_list_digital)
                {
                    if (pin_elt.mode.Equals(IOPin.pin_mode_out))
                    {
                        source_code_stream.WriteLine("const unsigned int pin_" + pin_elt.variable_mapped + " = " + pin_elt.pin_address + ";");
                    }
                }
            }
            
            //Additional options
            bool analyse_efficiency_code = false, debug_code = false;
            if (this.code_analyse_efficiency_checkBox.Checked)
                analyse_efficiency_code = true;
            if (this.code_debug_checkBox.Checked)
                debug_code = true;

            g7_current.arduino_code_generation_initializations(source_code_stream, selected_µC.sc_timer_definition_path, analyse_efficiency_code, debug_code);
            g7_current.arduino_code_generation_setup(source_code_stream, analyse_efficiency_code, debug_code);
            g7_current.arduino_code_generation_loop(source_code_stream, analyse_efficiency_code, debug_code);

            //Close the stream to save
            source_code_stream.Close();
        }

        private void Arduino_build_VirtualMachine_source_code()
        {

            GrafcetClass g7_current = this.getCurrentGrafcet();

            Microcontroller selected_µC = this.io_mapper_settings.getCurrentBoard();

            string source_code_file_path = code_gen_initializations("G7_VM_Sketch", "ino", selected_µC.name);
            StreamWriter sWriter = new StreamWriter(source_code_file_path);

            //To be modified
            //string header_source_path = actual_µC.sc_header_path;   //"..\\Grafcet_2015_04\\src\\use_static\\head_DataEEPROM.c";
            //string source_code_header_declarePINs_path = actual_µC.sc_header_path;
            
            //this.build_Source_Code_File_with_data_inEE_array(data_inEE_array, final_source_code_file_path, header_source_path, queue_source_path);

            //Add meta data
            sWriter.WriteLine("/*");
            sWriter.WriteLine("Time :\t" + DateTime.Now.ToString()); 
            sWriter.WriteLine("Generated from the grafcet File: \n\t" + this.getCurrentGrafcet().file_name_full);
            sWriter.WriteLine("*/");

            //Adding the header to the source code
            G7_Form.appenFileToStreamWriter(selected_µC.sc_header_path, sWriter);

            //Adding to the header the PINs declaration
            G7_Form.appenFileToStreamWriter(Microcontroller.sc_PinsDeclare_path, sWriter);

            //Adding declaration of Materials from I/O conf to the source code: Inputs and Ouputs
            int N_inputs_model = this.io_mapper_settings.getNumberOfInputPins();
            //int IN_total_size = actual_board.inputs.Count;

            int N_outputs_model = this.io_mapper_settings.getNumberOfOutputPins();
            //int OUT_total_size = actual_board.outputs.Count;

            //if (g7_current.hasTimeCondition) //Commented because it is a Virtual Machine
            {
                sWriter.WriteLine();
                sWriter.WriteLine("//FT = 1000/Board_TIME_UNIT = Frequency of the Step activity callback function Step_timer_update_callback");
                sWriter.WriteLine("const unsigned int Board_TIME_UNIT = " + GrafcetClass.g7_Board_TimeUnit_TimerStepActivity + ";");
            }

            //Declare INPUT pins
            sWriter.WriteLine();
            sWriter.WriteLine("//Effective number of Inputs : \nconst unsigned int N_inputs = " + N_inputs_model + ";");
            sWriter.WriteLine("//Declare INPUT pins");
            sWriter.Write("const unsigned int pins_input[] = {");

            //Add only the PINs that are selected for Inputs
            string board_IN_pin_addresses = ""; 
            bool there_is_input = false;
            foreach (IOPin pin_elt in selected_µC.pins_list_digital)
            {
                if (pin_elt.mode.Equals(IOPin.pin_mode_in))
                {
                    board_IN_pin_addresses += pin_elt.pin_address + ", ";
                    there_is_input = true;
                }
            }
            if (there_is_input) {
                board_IN_pin_addresses = board_IN_pin_addresses.Substring(0, board_IN_pin_addresses.Length - 2);
            }
            sWriter.Write(board_IN_pin_addresses);
            sWriter.WriteLine("};\n");

            //Declare OUTPUT pins
            sWriter.WriteLine("//Effective number of Outputs : \nconst unsigned int N_outputs =  " + N_outputs_model + ";");
            sWriter.WriteLine("//Declare OUTPUT pins");
            sWriter.Write("const unsigned int pins_output[] = {");
            string board_OUT_pin_addresses = "";
            bool there_is_output = false;
            foreach (IOPin pin_elt in selected_µC.pins_list_digital)
            {
                if (pin_elt.mode != null)
                    if (pin_elt.mode.Equals(IOPin.pin_mode_out))
                    {
                        board_OUT_pin_addresses += pin_elt.pin_address + ", ";
                        there_is_output = true;
                    }
            }
            if (there_is_output)
            {
                board_OUT_pin_addresses = board_OUT_pin_addresses.Substring(0, board_OUT_pin_addresses.Length - 2);
            }
            sWriter.Write(board_OUT_pin_addresses);
            sWriter.WriteLine("};\n");

            //Adding data_inEE_array data (synthesis matrices)
            sWriter.WriteLine();
            sWriter.WriteLine("unsigned int MatricesDATA[] = {");
            sWriter.WriteLine(g7_current.DataInEE_array);
            sWriter.WriteLine("};");
            sWriter.WriteLine("//From grafcet source File: " + g7_current.file_name_full);

            //Adding the queue to the source code
            if (this.code_analyse_efficiency_checkBox.Checked){
                G7_Form.appenFileToStreamWriter(Microcontroller.sc_vm_queue_AnalyzeEfficiency, sWriter);
            }
            else{
                G7_Form.appenFileToStreamWriter(Microcontroller.sc_vm_queue, sWriter);
            }
            // Writing in the source code
             
            sWriter.Close();
        }

        private static string RunExternalCommands(string filename, string arguments = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }

            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.Append(args.Data);

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                //Generate an exception if the code error is different from 0
                if (arguments != null)
                {
                    throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
                }
                else
                {
                    throw new Exception(Format(filename) + " finished with exit code = " + process.ExitCode + ": " + message);
                }
            }
        }

        private static string Format(string filename, string arguments = null)
        {
            string the_message = "'" + filename +
            ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
            "'";
            Console.WriteLine(the_message);
            return the_message;

        }


        static void call_run_cmd_v0(object sender, EventArgs e)
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.

            //strCommandParameters are parameters to pass to program
            //p.StartInfo.Arguments = strCommandParameters;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.FileName = "E:\\tp_poo\\test_c_sharp\\compile.bat";
            p.StartInfo.FileName = "build_commands\\compile.bat";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            p.WaitForExit();
        }

        private void export_button_Click(object sender, EventArgs e)
        {
            this.upload_button.Enabled = true;
            this.save_g7_text_button_Click(sender, e);
        }

        private void open_g7_editor_UniSim_button_Click(object sender, EventArgs e)
        {
            /*
            String strAppDir = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetName().CodeBase);
            String strFullPathToMyFile = Path.Combine(strAppDir, "fileName.txt");

            MessageBox.Show(String.Format("Path to the application is: '{0}'." +
                "\nFull path to the file in the application folder is: '{1}'",
                strAppDir, strFullPathToMyFile));
            */

            MainForm mf = new MainForm();
            mf.Show();    //(new MainForm()).Show(); 
        }

        private void board_device_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string µC_name = this.board_device_comboBox.SelectedItem.ToString();
            if (!µC_name.Equals("None"))
            {
                int new_board_i = this.board_device_comboBox.SelectedIndex;
                IOMapper.selected_µC_index = new_board_i;

                //MessageBox.Show("Board selected Index = " + new_board_i);
                
                if (this.io_mapper_form != null) {
                    IOMapper.selected_µC_index = new_board_i;
                }
                
                if (this.GrafcetsList.Count > 0)
                {
                    int nb_pins = this.io_mapper_settings.getCurrentBoard().pins_list_digital.Count;

                    int in_num_syst = this.getCurrentGrafcet().number_inputs;
                    int out_num_syst = this.getCurrentGrafcet().number_actions;

                    if (in_num_syst + out_num_syst > nb_pins)
                    {
                        string error_message = "The selected board cannot map your I/O system: \n";
                        error_message +=
                            "\tNumber of PINs  : " + nb_pins + "\n" +
                            "\tSystem number of Inputs & Outputs Variables : " + (in_num_syst + out_num_syst)
                        ;
                        error_message += "\n";
                        
                        MessageBox.Show(error_message);
                        this.board_device_comboBox.SelectedItem = this.board_device_comboBox.Items[this.board_device_comboBox.Items.Count - 1];
                    }
                    else
                    {
                        IOMapper.selected_µC_index = new_board_i;
                        this.io_mapper_form = null;
                        //this.io_mapper_form.clear_group_Boxes();

                        this.board_family_textBox3.Text = this.io_mapper_settings.get_family();
                        this.board_processor_textBox3.Text = this.io_mapper_settings.get_processor();
                        this.board_device_comboBox.SelectedItem = this.board_device_comboBox.Items[IOMapper.selected_µC_index];
                    }
                }

                //Enable buttons
                this.code_generation_button.Enabled = false;
                this.io_map_button.Enabled = true;
                this.build_project_button.Enabled = false;
            }
            else
            {
                this.board_family_textBox3.Text = "";
                this.board_processor_textBox3.Text = "";

                this.build_project_button.Enabled = false;
                //this.reload_boards_settings_button.Enabled = false;
                this.export_button.Enabled = false;
                this.upload_button.Enabled = false;
            }
        }

        private void io_map_button_Click(object sender, EventArgs e)
        {
            if (this.io_mapper_form != null) {
                this.io_mapper_form.io_mapper.is_mapped = false;
                //MessageBox.Show("this.io_mapper_settings = " + this.io_mapper_settings + "\n this.io_mapper_form.io_mapper = " +this.io_mapper_form.io_mapper);
            }

            if (!this.board_device_comboBox.SelectedItem.ToString().Equals("None"))
            {
                int board_index = this.board_device_comboBox.SelectedIndex;
                IOMapper.selected_µC_index = board_index;
                int nb_pins = this.io_mapper_settings.getCurrentBoard().pins_list_digital.Count;
                if (this.io_mapper_form != null) {
                    nb_pins = this.io_mapper_form.io_mapper.getCurrentBoard().pins_list_digital.Count;
                }
                //MessageBox.Show("Board Index Begin = " + IOMapper.selected_µC_index + "\nNum of pins = " + nb_pins);

                if (this.GrafcetsList.Count > 0)
                {
                    int input_num_real = this.getCurrentGrafcet().number_inputs;
                    int output_num_real = this.getCurrentGrafcet().number_actions;

                    if (input_num_real + output_num_real > nb_pins)
                    {
                        string error_message = "The selected board cannot map your I/O system: \n";
                        error_message +=
                            "\tNumber of PINs  : " + nb_pins + "\n" +
                            "\tSystem number of Inputs & Outputs Variables : " + (input_num_real + output_num_real)
                        ;
                        error_message += "\nSelect another board";

                        MessageBox.Show(error_message);
                        this.board_device_comboBox.SelectedItem = this.board_device_comboBox.Items[this.board_device_comboBox.Items.Count - 1];
                    }
                    else
                    {
                        if (this.io_mapper_form == null)
                        {
                            //MessageBox.Show("io_mapper_form is null. Building new IO Mapper ");
                            this.build_io_mapping_infos();
                        }
                        else {
                            if (!this.io_mapper_form.Visible)
                                this.io_mapper_form.Visible = true;
                        }

                        //Activate the building project functionality
                        this.code_generation_button.Enabled = true;
                    }
                }
                this.code_generation_button.Enabled = true;

                //MessageBox.Show("Board Index End = " + IOMapper.selected_µC_index + "\nNum of pins = " + this.io_mapper_form.io_mapper.getCurrentBoard().pins_list_digital.Count);
            }
            else
            {
                MessageBox.Show("Select a valid Device");
            }
        }

        private void build_io_mapping_infos() {
            this.io_mapper_form = new IOMapperForm(this.io_mapper_settings, this.getCurrentGrafcet());
            this.io_mapper_form.Visible = true;
        }

        private void code_generation_button_Click(object sender, EventArgs e)
        {
            //Very important : Update the IOMapper <io_mapper_settings>. The IO Mapper that resuls from the mapping is this.io_mapper_form.io_mapper 
            this.io_mapper_settings = this.io_mapper_form.io_mapper;

            if (this.io_mapper_settings.is_mapped)
            {
                string name = this.io_mapper_settings.getCurrentBoard().name;

                if (name.Equals("EasyDspic4A"))
                {
                    if (this.code_vm_checkBox.Checked)
                        this.dsPIC_build_virtualMachine_source_code();
                    else
                        this.dsPIC_build_compiler_source_code();
                }
                else
                    if (name.Contains("ATmega") || name.Contains("Arduino"))
                    {
                        if (this.code_vm_checkBox.Checked)
                            this.Arduino_build_VirtualMachine_source_code();
                        else
                            this.Arduino_build_simple_source_code();
                    }

                this.build_project_button.Enabled = true;

                MessageBox.Show("Code Generation process ended !");
            }
            else
            {
                MessageBox.Show("The Inputs/Outputs are not mapped !");
                this.export_button.Enabled = false;
            }
        }

        private void build_project_button_Click(object sender, EventArgs e)
        {
            /*
            Microcontroller actual_board = this.io_form.iomapper.micros_list[this.io_form.iomapper.current_board_index];
            string device_name = actual_board.name;

            string make_file_path = actual_board.make_file_path;
            string arguments = null;
            try
            {
                string run_result = RunExternalCommands(make_file_path);
                MessageBox.Show("Build Project Finished !\n" + run_result);
            }
            catch (Exception eee)
            {
                MessageBox.Show("OS error while running " + Format(make_file_path, arguments) + ": " + eee.Message);
            }

            this.export_button.Enabled = true;
            */

            /*if (device_name.Equals("EasyDspic4A"))
            {
                
            }
            else
            {
                MessageBox.Show("The device " + device_name + " is not yet programmed");
            }*/
        }

        private void Time_Unit_of_conditions_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int item_index = this.Board_Time_Period_comboBox.SelectedIndex;
            int new_Board_Time_Unit = this.Board_TU_values[item_index]; //this.clock_values[item_index];
            GrafcetClass.g7_Board_TimeUnit_TimerStepActivity = new_Board_Time_Unit;
            Console.WriteLine("\nNew Board Time Unit of Steps activity TU set : " + new_Board_Time_Unit + " ms");
            if (new_Board_Time_Unit < 55)
                MessageBox.Show("The New Board Time Unit TU = " + new_Board_Time_Unit+" ms you have selected may not guarantee the well functioning of the board program");
            this.clearing_All_forms_ReINIT();
        }

        private void clearing_All_forms_ReINIT() {
            //Clearing all the displayed informations
            this.static_g7_textBox.Clear();
            this.simul_textBox.Clear();
            this.condensed_g7_data_textBox.Clear();
            this.PIC_Message_textBox.Clear();
            this.size_textBox1.Clear();
            this.input_checkedListBox.Controls.Clear();

            // disable simulations buttons
            this.open_simulations_button.Enabled = false;
            this.disable_simulations_buttons();
            this.send_pic_message_button.Enabled = false;
            this.save_hex_data_button2.Enabled = false;
            this.generate_hex_board_button1.Enabled = false;
            this.save_g7_text_button.Enabled = false;
            this.save_g7_hex_button.Enabled = false;

            this.build_project_button.Enabled = false;
            this.code_generation_button.Enabled = false;
            this.export_button.Enabled = false;
            this.upload_button.Enabled = false;
            this.board_device_comboBox.Enabled = false;

            CodingGrafcetElements.RISING_EDGE_NUMBER = 0;

            this.initIOManager();
        }

        private void eeprom_check_CheckedChanged(object sender, EventArgs e)
        {
            if (this.eeprom_check.Checked)
            {
                this.eeprom_size.Enabled = true;
                this.eeprom_unit.Enabled = true;
            }
            else {
                this.eeprom_size.Enabled = false;
                this.eeprom_unit.Enabled = false;
            }
        }

        private void enableBoardDefinitionSettings(bool value) {
            this.board_name.Enabled = value;
            this.board_family.Enabled = value;
            this.board_manufacturer.Enabled = value;
            this.ram_size.Enabled = value;
            this.flash_size.Enabled = value;
            this.eeprom_check.Enabled = value;
            //this.eeprom_size.Enabled = value;
            this.wordMemory_comboBox.Enabled = value;
            this.processorSpeed.Enabled = value;
            this.file_timer_config.Enabled = value;
            this.Board_Time_Period_comboBox.Enabled = value;
            this.select_read_input_Button.Enabled = value;
            this.select_write_output_Button.Enabled = value;
            this.PINs_Add.Enabled = value;
            this.pin_number_add.Enabled = value;
            this.Pins_ViewList.Enabled = value;
        }
        private void cleanBoardDefinitionSettings() {
            this.board_name.Text = "";
            this.board_family.Text = "";
            this.board_manufacturer.Text = "";
            this.ram_size.Text = "";
            this.flash_size.Text = "";
            this.eeprom_size.Text = "";
            this.processorSpeed.Text = "";
            //this.wordMemory_comboBox.SelectedIndex = 0;
            this.file_timer_config.Text = "";
            this.input_reading_file.Text = "";
            this.output_writing_file.Text = "";
            this.pin_number_add.Text = "";
        }
        private void reload_boards_settings_button_Click(object sender, EventArgs e)
        {
            this.initIOManager();
            string message_show = "Current microcontroller list :\n";
            foreach (Microcontroller µC in this.io_mapper_settings.micros_list) {
                message_show += "- " + µC.name + "\tfamily: " + µC.family.name + "\tSpeed : " + µC.processor.speed + µC.processor.unit + "\n";
            }
            MessageBox.Show(message_show);
        }
        private bool validateBoardDefinitionSettings()
        {
            bool answer = true;

            return answer;
        }
        
        private void NewBoard_Button_Click(object sender, EventArgs e)
        {
            this.enableBoardDefinitionSettings(true);
        }

        private void Cancel_Board_Button_Click(object sender, EventArgs e)
        {
            this.cleanBoardDefinitionSettings();
        }

        private void Save_Board_Button_Click(object sender, EventArgs e)
        {
            if (this.validateBoardDefinitionSettings()) { 
                
            }
        }

        

    }
}
