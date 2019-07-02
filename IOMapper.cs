using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

using GrafcetConvertor.BoardDefinition;

namespace GrafcetConvertor
{
    
    
    public class IOMapper
    {
        public static int selected_µC_index; //Will be reinitialized in the G7_Form method : initIOManager()
        public bool is_mapped;

        public static string µC_file_settings_name = "..\\boards_settings\\micros_settings.xml";

        public List<Microcontroller> micros_list;

        public IOMapper()
        {
            this.readBoardInfos();
        }
        private void readBoardInfos() {
            this.micros_list = new List<Microcontroller>();
            
            XmlDocument xml_doc = new XmlDocument();
            try
            {
                xml_doc.Load(µC_file_settings_name);
            }
            catch (Exception e) {
                MessageBox.Show("XML Error: " + e.Message);
            }

            XmlNodeList xml_boards = xml_doc.GetElementsByTagName("microcontroller");
            int boards_size = xml_boards.Count;

            Microcontroller micro;
            foreach (XmlNode a_xml_board in xml_boards)
            {
                micro = new Microcontroller(a_xml_board.Attributes[0].Value);
                micro.setFamily(a_xml_board.Attributes[1].Value);
                micro.setManufacturer(a_xml_board.Attributes[2].Value);

                //Architecture
                string size_archi_str = a_xml_board.ChildNodes[0].Attributes[0].Value; 
                string unit_archi_str = a_xml_board.ChildNodes[0].Attributes[1].Value;
                micro.setArchitecture(Int32.Parse(size_archi_str), unit_archi_str);

                //Processor
                string speed_proc_str = a_xml_board.ChildNodes[1].Attributes[0].Value;
                string unit_proc_str = a_xml_board.ChildNodes[1].Attributes[1].Value;
                micro.setProcessor(Int32.Parse(speed_proc_str), unit_proc_str);

                //Ram Memory
                string ram_type = a_xml_board.ChildNodes[2].Attributes[0].Value;
                string ram_size = a_xml_board.ChildNodes[2].Attributes[1].Value;
                string ram_unit = a_xml_board.ChildNodes[2].Attributes[2].Value;
                micro.setRamMemory(ram_type, Int32.Parse(ram_size), ram_unit);

                //Program Memory
                string prog_type = a_xml_board.ChildNodes[3].Attributes[0].Value;
                string prog_size = a_xml_board.ChildNodes[3].Attributes[1].Value;
                string prog_unit = a_xml_board.ChildNodes[3].Attributes[2].Value;
                micro.setProgramMemory(prog_type, Int32.Parse(prog_size), prog_unit);

                //Data Memory 
                string data_type = a_xml_board.ChildNodes[4].Attributes[0].Value;
                string data_size = a_xml_board.ChildNodes[4].Attributes[1].Value;
                string data_unit = a_xml_board.ChildNodes[4].Attributes[2].Value;
                micro.setDataMemory(data_type, Int32.Parse(data_size), data_unit);

                //Read Digital pins and save
                micro.max_digital_pin = Int32.Parse(a_xml_board.ChildNodes[5].Attributes[0].Value);
                int N_Digit_Pins = a_xml_board.ChildNodes[5].ChildNodes.Count;
                for (int i = 0; i < N_Digit_Pins; i++)
                {
                    string pin_address = a_xml_board.ChildNodes[5].ChildNodes[i].Attributes[0].Value;
                    string pin_type = a_xml_board.ChildNodes[5].ChildNodes[i].Attributes[1].Value;
                    micro.pins_list_digital.Add(new IOPin(pin_address, pin_type));
                }

                //Read Analog pins and save
                micro.max_analog_pin = Int32.Parse(a_xml_board.ChildNodes[6].Attributes[0].Value);
                int N_Analog_Pins = a_xml_board.ChildNodes[6].ChildNodes.Count;
                for (int i = 0; i < N_Analog_Pins; i++)
                {
                    string pin_address = a_xml_board.ChildNodes[6].ChildNodes[i].Attributes[0].Value;
                    string pin_type = a_xml_board.ChildNodes[6].ChildNodes[i].Attributes[1].Value;
                    micro.pins_list_analog.Add(new IOPin(pin_address, pin_type));
                }

                micro.set_SC_header_path(a_xml_board.ChildNodes[7].ChildNodes[0].Attributes[0].Value);
                micro.set_SC_read_pins_path(a_xml_board.ChildNodes[7].ChildNodes[1].Attributes[0].Value);
                micro.set_SC_write_pins_path(a_xml_board.ChildNodes[7].ChildNodes[2].Attributes[0].Value);
                micro.set_SC_pinMode_config_path(a_xml_board.ChildNodes[7].ChildNodes[3].Attributes[0].Value);
                micro.set_SC_timer_definition_path(a_xml_board.ChildNodes[7].ChildNodes[4].Attributes[0].Value);
                micro.set_SC_readInMemory_path(a_xml_board.ChildNodes[7].ChildNodes[5].Attributes[0].Value);
                micro.set_SC_writeInMemory_path(a_xml_board.ChildNodes[7].ChildNodes[6].Attributes[0].Value);
                
                this.micros_list.Add(micro);
            }
        }

        public string get_device()
        {
            return this.micros_list[IOMapper.selected_µC_index].name;
        }

        public string get_family() {
            return this.micros_list[IOMapper.selected_µC_index].family.name;
        }

        public string get_processor()
        {
            return this.micros_list[IOMapper.selected_µC_index].processor.name;
        }
        /*
        public string getRealInputWithView(int selected_board_i, string var_name)
        {
            string input_board_real = "";
            foreach (IOPin pin_elt in this.micros_list[selected_board_i].pins_list_digital)
            {
                if (pin_elt.pin_address.Equals(var_name))
                {
                    input_board_real = pin_elt.pin_address;
                    break;
                }
            }

            return input_board_real;
        }
        */
        public void reinit_All_PINs()
        {
            foreach (IOPin pin_elt in this.getCurrentBoard().pins_list_digital)
            {
                pin_elt.mode = IOPin.pin_mode_default;
            }
        }
        public void update_PIN(string address, string variable, string mode) {
            foreach (IOPin pin_elt in this.getCurrentBoard().pins_list_digital) {
                if (pin_elt.pin_address.Equals(address))
                {
                    pin_elt.variable_mapped = variable;
                    pin_elt.mode = mode;
                    //MessageBox.Show("Updating address :" + address + " variable : " + variable + " mode:" + mode);
                    return;
                }
            }
        }

        public int getNumberOfInputPins() {
            int nb = 0;
            foreach (IOPin pin_elt in this.getCurrentBoard().pins_list_digital) {
                if (pin_elt.mode.Equals(IOPin.pin_mode_in))
                    nb++;
            }
            return nb;
        }
        public int getNumberOfOutputPins()
        {
            int nb = 0;
            foreach (IOPin pin_elt in this.getCurrentBoard().pins_list_digital)
            {
                if (pin_elt.mode.Equals(IOPin.pin_mode_out))
                    nb++;
            }
            return nb;
        }
        public Microcontroller getCurrentBoard() {
            return this.micros_list[IOMapper.selected_µC_index];
        }
    }
}
