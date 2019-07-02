/*
Time :	10/08/2016 16:59:41
Generated from the grafcet File: 
	D:\DD2\THESE\aContinuation_Fev2016\ArduinoCompiler_VM_Code\Efficiency_v2\g7s\Group 1\feu_exemple2_1.xml
*/
#include "TimerOne.h" 
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

//**** 	 Declare INPUT pins mapped	**** 	 Total Inputs : 1
const unsigned int pin_a = 2;

//**** 	Declare OUTPUT pins mapped 	**** 	 Total Outputs : 3
const unsigned int pin_o0 = 7;
const unsigned int pin_o1 = 8;
const unsigned int pin_o2 = 9;

//**** 	 Declare INPUT pins states	****
boolean a = false, a_Old = false;

//**** 	 Declare OUPUT pins states	****
boolean o0 = false, o0_Old = false;
boolean o1 = false, o1_Old = false;
boolean o2 = false, o2_Old = false;

//**** 	 Declare STEPs variables	****
boolean S0_X = true, S0_X_Old = true;
boolean S1_X = false, S1_X_Old = false;
boolean S2_X = false, S2_X_Old = false;
boolean S3_X = false, S3_X_Old = false;
boolean S4_X = false, S4_X_Old = false;
boolean S5_X = false, S5_X_Old = false;

//**** 	 Declare Transitions receptivities variables	****
boolean TR_1 = false;
boolean TR_2 = false;
boolean TR_3 = false;
boolean TR_4 = false;
boolean TR_5 = false;
boolean TR_6 = false;
boolean TR_7 = false;

//**** 	 Declare STEPs timing variables for duration activity 	****
unsigned int S0_duration = 0;
unsigned int S1_duration = 0;
unsigned int S2_duration = 0;
unsigned int S3_duration = 0;
unsigned int S4_duration = 0;
unsigned int S5_duration = 0;

//To measure the efficacy
unsigned int NN = 0;
unsigned int time_val;

////FT = 1000/Board_TIME_UNIT = Frequency of the Step activity callback function Step_timer_update_callback
const unsigned int Board_TIME_UNIT = 100;

//To manage timer or measuring duration of active steps
//const unsigned int Board_TIME_UNIT = 1; // Board_TIME_UNIT =5 i.e. 5Hz : Number of callback function of Timer1 per second

void initializeTimer1(){
  //Timer1.initialize(1000000);     // initialize timer1, and set a 1 second period
  unsigned int FT_Steps = 1000/Board_TIME_UNIT; //FT_Steps = frequency_timer
  Timer1.initialize(1000000/FT_Steps);       // initialize timer1, and set a 1 second period
  //Timer1.pwm(9, 512);             		// setup pwm on pin 9, 50% duty cycle, if there is pinMode(9, OUTPUT); It present the frequency of the timer
  Timer1.attachInterrupt(Step_timer_update_callback);   // attaches callback() as a timer overflow interrupt
}

void Step_timer_update_callback(){
	if(S0_X) {S0_duration ++ ;} else {S0_duration = 0;}
	if(S1_X) {S1_duration ++ ;} else {S1_duration = 0;}
	if(S2_X) {S2_duration ++ ;} else {S2_duration = 0;}
	if(S3_X) {S3_duration ++ ;} else {S3_duration = 0;}
	if(S4_X) {S4_duration ++ ;} else {S4_duration = 0;}
	if(S5_X) {S5_duration ++ ;} else {S5_duration = 0;}
}

void setup(){
	initializeTimer1();
	Serial.begin(9600);
	//INIT INPUT PINs
	pinMode(pin_a, INPUT);
	//INIT OUTPUT PINs
	pinMode(pin_o0, OUTPUT);
	pinMode(pin_o1, OUTPUT);
	pinMode(pin_o2, OUTPUT);

	time_val = millis();
	NN = 0;
}

void loop(){
	//Reading input values
	a = digitalRead(pin_a);

	//Clearing conditions
	TR_1 = (S0_duration > 100) && !a;
	TR_2 = (S1_duration > 40);
	TR_3 = (S2_duration > 10);
	TR_4 = (S3_duration > 100);
	TR_5 = (S4_duration > 40);
	TR_6 = (S5_duration > 10);
	TR_7 = a;

	//Evaluate Step variables
	S0_X = TR_6 || (S0_X && !TR_1) || (S0_X && !TR_7);
	S1_X = TR_1 || (S1_X && !TR_2);
	S2_X = TR_2 || (S2_X && !TR_3);
	S3_X = TR_3 || TR_7 || (S3_X && !TR_4);
	S4_X = TR_4 || (S4_X && !TR_5);
	S5_X = TR_5 || (S5_X && !TR_6);

	//Evaluate OUTPUTs variables
	o0 = S0_X || S2_X || S3_X || S4_X || S5_X;
	o1 = S0_X || S1_X || S3_X || S4_X || S5_X;
	o2 = S0_X || S3_X || S5_X;

	//Update OUTPUTs
	 digitalWrite(pin_o0, o0);
	 digitalWrite(pin_o1, o1);
	 digitalWrite(pin_o2, o2);

	//Update Old OUTPUTs
	o0_Old = o0;
	o1_Old = o1;
	o2_Old = o2;

	//Update Old Step variables state
	S0_X_Old = S0_X;
	S1_X_Old = S1_X;
	S2_X_Old = S2_X;
	S3_X_Old = S3_X;
	S4_X_Old = S4_X;
	S5_X_Old = S5_X;

	NN++;
	if(millis() - time_val > 5000){
		Serial.println(NN);
		NN = 0; time_val = millis();
	}
}
