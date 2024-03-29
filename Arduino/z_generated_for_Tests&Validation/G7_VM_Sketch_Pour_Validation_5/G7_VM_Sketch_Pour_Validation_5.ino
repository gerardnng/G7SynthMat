/*
Time :	12/08/2016 14:07:44
Generated from the grafcet File: 
	D:\DD2\THESE\aContinuation_Fev2016\ArduinoCompiler_VM_Code\Efficiency_v2\g7s\Group 1\Pour_Validation_5.xml
*/
#include "TimerOne.h"
#include "EEPROM.h"

#define limit_RS  20
#define NMax_Trans  16
#define NMax_Steps 16

#define N_RisingEdges_Max 16

/*Declare Materials: Inputs & Outputs*/
#define NMax_Inputs 7
#define NMax_Outputs 7
/*
//Declare INPUTS pins
#define IN_PIN_0 0
#define IN_PIN_1 1
...

//Declare OUTPUTS pins
#define OUT_PIN_0 7
#define OUT_PIN_1 8
...
*/

//FT = 1000/Board_TIME_UNIT = Frequency of the Step activity callback function Step_timer_update_callback
const unsigned int Board_TIME_UNIT = 100;

//Effective number of Inputs : 0
//Declare INPUT pins
const unsigned int pins_input[] = {};

//Effective number of Outputs : 3
//Declare OUTPUT pins
const unsigned int pins_output[] = {7, 8, 9, 10, 11, 12, 13, 7, 8, 9, 10, 11, 12, 13, 7};


unsigned int MatricesDATA[] = {
//N_trans, N_Steps, N_actions, N_Inputs
	0x0006, 
	0x0006, 
	0x0003, 
	0x0000, 
//INIT Vector 
	0x0009, 
//E matrix 
	0x0002, 0x0008, 0x0010, 0x0004, 0x0020, 0x0001, 
//S matrix 
	0x0004, 0x0010, 0x0008, 0x0020, 0x0001, 0x0002, 
//MA matrix 
	0x0009, 0x0012, 0x0014, 
//RS matrix 
	0x0002, 0xC003, 0x3000, 0x001D, 
	0x0001, 0xE314, 
	0x0001, 0xE414, 
	0x0002, 0xC004, 0x3000, 0x001E, 
	0x0006, 0xC000, 0xC003, 0x1000, 0xC003, 0x3000, 0x001F, 0x0000, 
	0x0002//, 0xA004, 0x3000, 0x0020
};
//From grafcet source File: D:\DD2\THESE\aContinuation_Fev2016\ArduinoCompiler_VM_Code\Efficiency_v2\g7s\Group 1\Pour_Validation_5.xml

/*Declare variables to be stored in RAM*/
unsigned int MatricesDATA_eeprom_adr;
unsigned int N_Trans ;
unsigned int N_Steps ;
unsigned int N_Actions;
unsigned int N_Inputs;

unsigned int E[NMax_Trans];
unsigned int S[NMax_Trans];
unsigned int X_INIT;
unsigned int MA[NMax_Inputs];
unsigned int RS[NMax_Trans][limit_RS];

//Use to manage Rising edge of variables. <=16 rising edges
boolean Old_RisingEdges[N_RisingEdges_Max];
unsigned int old_RE_index ;
boolean old_RE_value;

//Used to evaluate receptiviies
boolean stack_recept_eval[20]; //Old: 4. But it seams that small RAM could necessitates a size greater than 4 
boolean* stack_position = stack_recept_eval; //bool same as boolean

unsigned char* P_Buffer;

unsigned int X; //Actual situation
unsigned int OUT_Port, OUT_Port_old; //Output 'register'
unsigned int IN_Port; //Output 'register'
unsigned int Step_active_timer[NMax_Steps];

//Variables for the dynamic
unsigned int XEi, VA, VD, AD, not_VD, DX; //They are used for all the transitions
boolean FT_i, VT_i, R_i; //They are used for every transition i
unsigned int i, j; //counters or indexes 

//To measure the efficacy
unsigned int NN = 0;
unsigned int time_val;

//FUNCTIONS THAT DEPENDS ON SPECIFIC BOARDS
unsigned int temp_IN_OUT_Reg;

//To manage timer or measuring duration of active steps
//const unsigned int Board_TIME_UNIT = 1; // Board_TIME_UNIT =5 i.e. 5Hz : Number of callback function of Timer1 per second

void initializeTimer1(){
  //Timer1.initialize(1000000);     // initialize timer1, and set a 1 second period
  unsigned int FT_Steps = 1000/Board_TIME_UNIT; //FT_Steps = frequency_timer
  Timer1.initialize(1000000/FT_Steps);       // initialize timer1, and set a 1 second period
  //Timer1.pwm(9, 512);             // setup pwm on pin 9, 50% duty cycle, if there is pinMode(9, OUTPUT); It present the frequency of the timer
  Timer1.attachInterrupt(Step_timer_update_callback);   // attaches callback() as a timer overflow interrupt
}

void Reading_Inputs()
{
	for(i=0; i<N_Inputs;i++){
		temp_IN_OUT_Reg = digitalRead(pins_input[i]) << i;
		IN_Port = asmOR(IN_Port, temp_IN_OUT_Reg);
	}
}

void Update_Outputs(void)
{
  bool old_state,new_state;
  for(i=0; i<N_Actions;i++){
    old_state = getBitWithIndex(OUT_Port_old,i);
    new_state = getBitWithIndex(OUT_Port,i);
    if(old_state != new_state){digitalWrite(pins_output[i], new_state);}
  }
  OUT_Port_old = OUT_Port;
}

void setup(){
  Serial.begin(9600);
  //Save Matrices into EEPROM
  //EEPROM_Write(); 
  /*Copy Static Matrix from MatricesDATA to "DatainRAM"(G7 data) in RAM*/
  //EEPROM_Read();
  //Always call this function after EEPROM_Read() function to initialize the variables: //N_trans, N_Steps, N_actions, N_Inputs

  Read_EEPROM_Data();
  
  init_IO_ports();
  
  X = X_INIT;
  initializeTimer1(); 
  for(i=0; i<N_RisingEdges_Max; i++){ Old_RisingEdges[i] = false;}
  
  time_val = millis();
  NN = 0;
}

void loop(){
  Reading_Inputs();
  //Evaluate new Grafcet situation
  VA = 0; VD = 0;
  for (i = 0; i < N_Trans; i++)
  {
	//Transition i: Evaluate if i is FIREBLE
    XEi = asmAND(X,E[i]);
    if (XEi == E[i]) {VT_i=1;} else {VT_i=0;}
    for (j = 1; j <= RS[i][0]; j++)
    {
      RS_Parse(RS[i][j]); 
    }
	//Transition i: Evaluate its receptivity
    R_i = (boolean) *(--stack_position); 
    FT_i = VT_i && R_i;
	//Transition i: Steps to Activate and steps to Desactivate
    if (FT_i !=0)
    {
      VA = asmOR(VA,S[i]); //S[i] is a 16 bits (2 bytes) data (unsigned int)
      VD = asmOR(VD,E[i]);
    }
  }
  //Transition i: Evaluate new situation X (FIRE all the fireble transitions and desactivate upstream steps)
  AD = asmAND(VA,VD);
  not_VD = asmNOT(VD);
  DX = asmOR(not_VD,AD);
  X = asmAND(X,DX);
  X = asmOR(X,VA);
  
  //Evaluate OUTPUTS
  unsigned int XMA;
  OUT_Port = 0x0000;
  for (i = 0; i < N_Actions; i++)
  {
    XMA = asmAND(X,MA[i]);
    if (XMA != 0)
    {
      temp_IN_OUT_Reg = 1<<i;
      OUT_Port = asmOR(OUT_Port,temp_IN_OUT_Reg);
    }
  }
  Update_Outputs();
  
	NN++;
	if(millis() - time_val > 5000){
		Serial.println(NN);
		NN = 0; time_val = millis();
	}
	delay(1);
}

void Step_timer_update_callback()
{
  for (i = 0; i < N_Steps; i++)
  {
    if (getBitWithIndex(X,i) == 0){ //step i is not active
      Step_active_timer[i] = 0; 
    }
    else 
    {
      Step_active_timer[i] += 1;  
    }
  }
}

//Tools to parse an RS element
unsigned int number;
void RS_Parse(unsigned int a)
{
  number = asmAND(a,0x8000);
  if (number == 0)
  {
    Operator_Parse(a);
  }
  else //if (number == 1)
  {
    unsigned int var_type = asmAND(a,0x6000);
    var_type = var_type >> (3*4+1); //Format: 0b 1--X XXXX XXXX XXXX
    
    switch(var_type)
    {
      case 0 :
      {
        Input_Parse(a); 
        break;
      }
      case 1 :
      {
        Output_Parse(a); 
        break;
      }
      case 2 :
      {
        Step_Parse(a); 
        break;
      }
      case 3 :
      {
        Step_time_Parse(a); 
        break;
      }
    }
  }
}

void Operator_Parse(unsigned int a)
{
  boolean stack_v1;
  boolean stack_v2;
  number = asmAND(a,0x7000); //Format: 0b 0--- 0000 0000 0000
  number = number>>(3*4);
  
  switch (number)
  {
    case 0 :  //OR(+) 0x0000
    {
      stack_v1 = *(--stack_position);
      stack_v2 = *(--stack_position);
      *stack_position = stack_v1 | stack_v2; 
      break;
    }
    case 1 :  //AND (*)  = 0x1000
    {
      stack_v1 = *(--stack_position);
      stack_v2 = *(--stack_position);
      *stack_position = stack_v1 & stack_v2;
      break;
    }
    case 2 : // NOT(!)  0x2000
    {
      stack_v1 = *(--stack_position);
      *stack_position = !stack_v1;
      break;
    }
    case 3: //RISING EDGE(^) 0x3000
    {
		//Get old value
		j++;
		old_RE_index = RS[i][j]; //Look ahead in RS to get the index of the current Rising Edge
		old_RE_value = Old_RisingEdges[old_RE_index];
		//Get current value
		stack_v1 = *(--stack_position);
		//eval new value
		*stack_position = !old_RE_value & stack_v1;
		//Store the new value in Old_RisingEdges
		Old_RisingEdges[old_RE_index] = *stack_position;
		break;
    }
    case 4: //The True/true value
    {
      *stack_position = 1;
      break;
    }
  }
  stack_position++;
}


void Input_Parse(unsigned int a)
{
  number = asmAND(a,0x00FF); //Format: 0b 1000 0000 NNNN NNNN
  
  if(number < N_Inputs){
    *stack_position = getBitWithIndex(IN_Port, number);
  }
  else{
    *stack_position = 0;
  }
  stack_position++;
}

void Output_Parse(unsigned int a)
{
  number = asmAND(a,0x00FF); //Format: 0b 1010 0000 NNNN NNNN
  if(number < N_Actions){
    *stack_position = getBitWithIndex(OUT_Port,number);
  }
  else{
    *stack_position = 0;
  }
  stack_position++;
}

void Step_Parse(unsigned int a)
{
  number = asmAND(a,0x001F); //Format : 0b 1100 0000 000N NNNN
  if(number < N_Steps){
    *stack_position = getBitWithIndex(X,number);
  }
  else{
    *stack_position = 0;
  }
  stack_position++;
}

void Step_time_Parse(unsigned int a)
{
  number = asmAND(a,0x1F00); //Format : 0b 111S SSSS DDDD DDDD
  number = (number >> 2*4);
  //number is supposed to be <16. But it will be extend to 31
  if (getBitWithIndex(X,number) == 0) //Step n� number is not active
  {
    *stack_position = 0;
  }
  else //Step n� number is active
  {
    unsigned int SDuration = asmAND(a,0x00FF);
    unsigned int current_duration = Step_active_timer[number];
    
    if (current_duration >= SDuration)
    {
      *stack_position = 1;
    }
    else
    {
      *stack_position = 0;
    }
  }
  stack_position++;
}

void init_IO_ports()
{
  //init inputs & outputs
  for(i=0; i<N_Inputs;i++){ //We don't know the exact number of inputs
    pinMode(pins_input[i], INPUT);  
  }
  
  for(i=0; i<N_Actions;i++){
    pinMode(pins_output[i], OUTPUT); 
  }
  OUT_Port = 0x0000;
  IN_Port = 0x0000;
}

/*Define usefull Assebly Functions*/
unsigned int asmNOT(unsigned int reg_data){
  //Exemple asmNOT(0x0000[1010])ie 10 --> 0x0000[0101] ie 3
  //return 0xffff^reg_data;
  return ~reg_data;
}
unsigned int asmAND(unsigned int a, unsigned int b){
  //Perform the AND bit per bits of a and b. Ex: asmAND(0x000[0101], 0x000[1100]) = 0x0100
  return a&b;
}
unsigned int asmOR(unsigned int a, unsigned int b){
  return a|b;
}

//To return the bit number i of the register reg_name
boolean getBitWithIndex(unsigned int reg_name, unsigned int bit_index){
  return ((reg_name>>(asmAND(bit_index,0x000F))) & 0x0001); //asmAND(bit_number,0x000F); makes sure that the value is between 0 and 16
}

//////////////////////////////
/*    EEPROM Blocks     */
/////////////////////////////
/*
void EEPROM_Write()
{
  MatricesDATA_eeprom_adr = 0;
  unsigned int M_size = sizeof(MatricesDATA) /2; //To modify dynamically inside MPLAB
  
  for(i=0; i<M_size ; i++){
    writeNextIntInEEPROM(MatricesDATA[i]);
  }
}

void EEPROM_Read()
{
  MatricesDATA_eeprom_adr = 0;
  
  N_Trans = readNextIntFromEEPROM();
  N_Steps = readNextIntFromEEPROM();
  N_Actions  = readNextIntFromEEPROM();
  N_Inputs = readNextIntFromEEPROM();
  X_INIT = readNextIntFromEEPROM();

  for (i = 0; i < N_Trans; i++)
  { 
    E[i] = readNextIntFromEEPROM();
  }
  for (i = 0; i < N_Trans; i++)
  { 
    S[i] = readNextIntFromEEPROM();
  }
  for (i = 0; i < N_Actions; i++)
  { 
    MA[i] = readNextIntFromEEPROM();
  }
  for (i = 0; i < N_Trans; i++)
  { 
    unsigned int RS_length = readNextIntFromEEPROM();
    RS[i][0] = RS_length;
    for (j = 1; j <= RS_length; j++)
    {
      RS[i][j] = readNextIntFromEEPROM();
    }
    for (j = RS_length+1; j < limit_RS; j++)
    {
      RS[i][j] = 0;
    }
  }
}
*/

void Read_EEPROM_Data(){
  int M_size = sizeof(MatricesDATA)/2; //To modify dynamically inside MPLAB
  Serial.print("MSize = "); Serial.println(M_size);
  for(i=0; i<M_size ; i++){
    Serial.print("MatricesDATA_"); Serial.print(i); Serial.print(" = "); Serial.println(MatricesDATA[i], HEX);
  }
  

  int M_index = 0;
  N_Trans = MatricesDATA[M_index++];
  N_Steps = MatricesDATA[M_index++];
  N_Actions  = MatricesDATA[M_index++];
  N_Inputs = MatricesDATA[M_index++];
  X_INIT = MatricesDATA[M_index++];

  for (i = 0; i < N_Trans; i++)
  { 
    E[i] = MatricesDATA[M_index++];
  }
  for (i = 0; i < N_Trans; i++)
  { 
    S[i] = MatricesDATA[M_index++];
  }
  for (i = 0; i < N_Actions; i++)
  { 
    MA[i] = MatricesDATA[M_index++];
  }
  for (i = 0; i < N_Trans; i++)
  { 
    unsigned int RS_length = MatricesDATA[M_index++];
    RS[i][0] = RS_length;
    for (j = 1; j <= RS_length; j++)
    {
      RS[i][j] = MatricesDATA[M_index++];
    }
    for (j = RS_length+1; j < limit_RS; j++)
    {
      RS[i][j] = 0;
    }
  }
}


void writeNextIntInEEPROM(unsigned int val) 
{   
    //d�coupage de la variable val qui contient la valeur � sauvegarder en m�moire
    unsigned char faible = val & 0x00FF; //r�cup�re les 8 bits de droite (poids faible) -> 0010 1100 
    //calcul : 1101 0111 0010 1100 & 0000 0000 1111 1111 = 0010 1100

    unsigned char fort = (val >> 8) & 0x00FF;  //d�cale puis r�cup�re les 8 bits de gauche (poids fort) -> 1101 0111
    //calcul : 1101 0111 0010 1100 >> 8 = 0000 0000 1101 0111 puis le m�me & qu�avant

    //puis on enregistre les deux variables obtenues en m�moire
    EEPROM.write(MatricesDATA_eeprom_adr++, fort) ; //on �crit les bits de poids fort en premier
    EEPROM.write(MatricesDATA_eeprom_adr++, faible) ; //puis on �crit les bits de poids faible � la case suivante
}

unsigned int readNextIntFromEEPROM()
{
    unsigned int val = 0 ; //variable de type int, vide, qui va contenir le r�sultat de la lecture

    unsigned char fort = EEPROM.read(MatricesDATA_eeprom_adr++);   //r�cup�re les 8 bits de gauche (poids fort) -> 1101 0111
    unsigned char faible = EEPROM.read(MatricesDATA_eeprom_adr++); //r�cup�re les 8 bits de droite (poids faible) -> 0010 1100

    //assemblage des deux variable pr�c�dentes
    val = fort ;         // val vaut alors 0000 0000 1101 0111
    val = val << 8 ;     // val vaut maintenant 1101 0111 0000 0000 (d�calage)
    val = val | faible ; // utilisation du masque
    // calcul : 1101 0111 0000 0000 | 0010 1100 = 1101 0111 0010 1100

    return val ; //on n�oublie pas de retourner la valeur lue !
}
