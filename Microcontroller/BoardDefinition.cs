using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrafcetConvertor.BoardDefinition
{
    public class IOPin
    {
        public string pin_address;
        public string type;             //may be configure in Input only, Output only, in INPUT/OUTPUT

        public string mode;             //is configured in INPUT/OUTPUT
        public string variable_mapped;  //What is displayed to 

        public IOPin(string pin_adr, string type)
        {
            this.pin_address = pin_adr;
            this.type = type;
            this.mode = pin_mode_default; //To be modified later
        }

        public static string pin_type_io = "IO";
        public static string pin_type_in = "In";
        public static string pin_type_out = "Out";

        public static string pin_mode_in = "Input";
        public static string pin_mode_out = "Output";
        public static string pin_mode_default = "Undefined";
    }

    public class Memory {
        public string type;
        public int size;
        public string unit;
        public Memory(string t, int s, string u) {
            this.type = t;
            this.size = s;
            this.unit = u;
        }
    }
    public class RamMemory: Memory {
        public RamMemory(string type, int size, string unit) 
            : base(type, size, unit)
        {
        }
    }
    public class DataMemory : Memory
    {
        public DataMemory(string type, int size, string unit)
            : base(type, size, unit)
        {
        }
    }
    public class ProgramMemory : Memory
    {
        public ProgramMemory(string type, int size, string unit)
            : base(type, size, unit)
        {
        }
    }
    public class Architecture
    {
        public int lenght;
        public string unit;
        public Architecture(int lenght, string unit)
        {
            this.lenght = lenght;
            this.unit = unit;
        }
    }
    public class Processor
    {
        public string name;
        public int speed;
        public string unit;
        public Processor(int speed, string unit)
        {
            this.speed = speed;
            this.unit = unit;
            this.name = this.speed + " " + this.unit;
        }
    }
    public class Manufacturer
    {
        public List<Family> family_list;
        public string name;
        public Manufacturer(string name)
        {
            this.name = name;
            this.family_list = new List<Family>();
        }
    }
    public class Family
    {
        public string name;
        public Family(string name)
        {
            this.name = name;
        }
    }

    public class Microcontroller
    {
        public string name;
        public Family family;
        public Manufacturer manufacturer;

        public Architecture architecture; //architecture = Size of the word memory
        public Processor processor;
        
        //Memories
        public RamMemory ramMemory;
        public ProgramMemory programMemory;
        public DataMemory dataMemory;

        public List<IOPin> pins_list_digital;
        public int max_digital_pin; //Maximum number of Digital Pins
        public List<IOPin> pins_list_analog;
        public int max_analog_pin;  //Maximum number of Analog Pins

        //SC = source_code
        public string sc_header_path;

        public string sc_read_pins_path;
        public string sc_write_pins_path;

        public string sc_pinMode_config_path;
        public string sc_timer_definition_path;

        public string sc_readInMemory_path;
        public string sc_writeInMemory_path;

        //Other paths to files that are static
        public static string sc_PinsDeclare_path = "..\\boards_settings\\ATmega328P\\vm_static\\pins_declaration.c";
        public static string sc_program_control_path = "..\\boards_settings\\ATmega328P\\vm_static\\control_program.c";
        public static string sc_functions_definition = "..\\boards_settings\\ATmega328P\\vm_static\\functions_definition.c";

        public static string sc_vm_queue = "..\\boards_settings\\ATmega328P\\vm_static\\Queue_Simple.c";
        public static string sc_vm_queue_AnalyzeEfficiency = "..\\boards_settings\\ATmega328P\\vm_static\\Queue_Efficiency.c";

        public Microcontroller(string name)
        {
            this.name = name;
            this.pins_list_digital = new List<IOPin>();
            this.pins_list_analog = new List<IOPin>();
        }

        public void setFamily(string fam_name) {
            this.family = new Family(fam_name);
        }
        public void setManufacturer(string manufact_name) {
            this.manufacturer = new Manufacturer(manufact_name);
        }

        public void setArchitecture(int lenght, string unit)
        {
            this.architecture = new Architecture(lenght, unit);
        }
        public void setProcessor(int speed, string unit)
        {
            this.processor = new Processor(speed, unit);
        }
        //Add Memories
        public void setRamMemory(string type, int size, string unit)
        {
            this.ramMemory = new RamMemory(type, size, unit);
        }
        public void setDataMemory(string type, int size, string unit)
        {
            this.dataMemory = new DataMemory(type, size, unit);
        }
        public void setProgramMemory(string type, int size, string unit)
        {
            this.programMemory = new ProgramMemory(type, size, unit);
        }

        //Add pin 
        public void addPinDigital(string pin_number, string type)
        {
            this.pins_list_digital.Add(new IOPin(pin_number, type));
        }
        public void addPinAnalog(string pin_number, string type)
        {
            this.pins_list_analog.Add(new IOPin(pin_number, type));
        }

        //Set path file
        
        public void set_SC_header_path(string path) {
            this.sc_header_path = path;
        }
        public void set_SC_read_pins_path(string path)
        {
            this.sc_read_pins_path = path;
        }
        public void set_SC_write_pins_path(string path)
        {
            this.sc_write_pins_path = path;
        }
        public void set_SC_pinMode_config_path(string path)
        {
            this.sc_pinMode_config_path = path;
        }
        public void set_SC_timer_definition_path(string path)
        {
            this.sc_timer_definition_path = path;
        }
        public void set_SC_readInMemory_path(string path)
        {
            this.sc_readInMemory_path = path;
        }
        public void set_SC_writeInMemory_path(string path)
        {
            this.sc_writeInMemory_path = path;
        }
    }

}
