/*
Time :	12/08/2016 14:08:12
Generated from the grafcet File: 
	D:\DD2\THESE\aContinuation_Fev2016\ArduinoCompiler_VM_Code\Efficiency_v2\g7s\Group 1\Pour_Validation_5.xml
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

//**** 	 Declare INPUT pins mapped	**** 	 Total Inputs : 0

//**** 	Declare OUTPUT pins mapped 	**** 	 Total Outputs : 3
const unsigned int pin_A = 7;
const unsigned int pin_B = 8;
const unsigned int pin_C = 9;

//**** 	 Declare OUPUT pins states	****
boolean A = false, A_Old = false;
boolean B = false, B_Old = false;
boolean C = false, C_Old = false;

//**** 	 Declare STEPs variables	****
boolean S0_X = true, S0_X_Old = true;
boolean S1_X = false, S1_X_Old = false;
boolean S2_X = false, S2_X_Old = false;
boolean S3_X = true, S3_X_Old = true;
boolean S4_X = false, S4_X_Old = false;
boolean S5_X = false, S5_X_Old = false;

//**** 	 Declare Validated Transitions variables	****
boolean VT_1 = false;
boolean VT_2 = false;
boolean VT_3 = false;
boolean VT_4 = false;
boolean VT_5 = false;
boolean VT_6 = false;

//**** 	 Declare Receptivities of Transitions variables	****
boolean R_1 = false;
boolean R_2 = false;
boolean R_3 = false;
boolean R_4 = false;
boolean R_5 = false;
boolean R_6 = false;

//**** 	 Declare Transitions receptivities variables	****
boolean TR_1 = false;
boolean TR_2 = false;
boolean TR_3 = false;
boolean TR_4 = false;
boolean TR_5 = false;
boolean TR_6 = false;

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

//FT = 1000/Board_TIME_UNIT = Frequency of the Step activity callback function Step_timer_update_callback
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
	//INIT OUTPUT PINs
	pinMode(pin_A, OUTPUT);
	pinMode(pin_B, OUTPUT);
	pinMode(pin_C, OUTPUT);

	time_val = millis();
	NN = 0;
}

void loop(){
	//Evaluate validated transitions variabless
	VT_1 = (S1_X);
	VT_2 = (S3_X);
	VT_3 = (S4_X);
	VT_4 = (S2_X);
	VT_5 = (S5_X);
	VT_6 = (S0_X);

	//Evaluate Receptivities oftransitions
	R_1 = (!S3_X_Old && S3_X);
	R_2 = (S3_duration > 20);
	R_3 = (S4_duration > 20);
	R_4 = (!S4_X_Old && S4_X);
	R_5 = S0_X && S3_X || (!S3_X_Old && S3_X);
	R_6 = (!S4_X_Old && S4_X);

	//Evaluate Clearing conditions
	TR_1 = VT_1 && R_1;
	TR_2 = VT_2 && R_2;
	TR_3 = VT_3 && R_3;
	TR_4 = VT_4 && R_4;
	TR_5 = VT_5 && R_5;
	TR_6 = VT_6 && R_6;

	//Save Step activity variables state in Old_ corresponding variables
	S0_X_Old = S0_X;
	S1_X_Old = S1_X;
	S2_X_Old = S2_X;
	S3_X_Old = S3_X;
	S4_X_Old = S4_X;
	S5_X_Old = S5_X;

	//Evaluate Step variables
	S0_X = TR_5 || (S0_X_Old && !TR_6);
	S1_X = TR_6 || (S1_X_Old && !TR_1);
	S2_X = TR_1 || (S2_X_Old && !TR_4);
	S3_X = TR_3 || (S3_X_Old && !TR_2);
	S4_X = TR_2 || (S4_X_Old && !TR_3);
	S5_X = TR_4 || (S5_X_Old && !TR_5);

	//Evaluate OUTPUTs variables
	A = S0_X || S3_X;
	B = S1_X || S4_X;
	C = S2_X || S4_X;

	//Update OUTPUTs
	if(A_Old != A) {digitalWrite(pin_A, A);}
	if(B_Old != B) {digitalWrite(pin_B, B);}
	if(C_Old != C) {digitalWrite(pin_C, C);}

	//Update Old OUTPUTs
	A_Old = A;
	B_Old = B;
	C_Old = C;

	NN++;
	if(millis() - time_val > 5000){
		Serial.println(NN);
		NN = 0; time_val = millis();
	}
}
