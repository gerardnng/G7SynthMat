/*
Time :	18/08/2016 13:13:55
Generated from the grafcet File: 
	D:\DD2\THESE\aContinuation_Fev2016\ArduinoCompiler_VM_Code\Efficiency_v2\g7s\Efficiency_Eval_9.xml
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

//**** 	 Declare INPUT pins mapped	**** 	 Total Inputs : 12
const unsigned int pin_mR1 = 2;
const unsigned int pin_hR1 = 3;
const unsigned int pin_bR2 = 4;
const unsigned int pin_bWD = 5;
const unsigned int pin_pV1 = 6;
const unsigned int pin_pE1 = 2;
const unsigned int pin_pE2 = 3;
const unsigned int pin_pE4 = 4;
const unsigned int pin_pE5 = 5;
const unsigned int pin_a = 6;
const unsigned int pin_b = 2;
const unsigned int pin_c = 3;

//**** 	Declare OUTPUT pins mapped 	**** 	 Total Outputs : 10
const unsigned int pin_MIP = 7;
const unsigned int pin_MSP = 8;
const unsigned int pin_V1 = 9;
const unsigned int pin_MX11 = 10;
const unsigned int pin_MX12 = 11;
const unsigned int pin_MX21 = 12;
const unsigned int pin_MX22 = 13;
const unsigned int pin_New1 = 7;
const unsigned int pin_New2 = 8;
const unsigned int pin_New3 = 9;

//**** 	 Declare INPUT pins states	****
boolean mR1 = false, mR1_Old = false;
boolean hR1 = false, hR1_Old = false;
boolean bR2 = false, bR2_Old = false;
boolean bWD = false, bWD_Old = false;
boolean pV1 = false, pV1_Old = false;
boolean pE1 = false, pE1_Old = false;
boolean pE2 = false, pE2_Old = false;
boolean pE4 = false, pE4_Old = false;
boolean pE5 = false, pE5_Old = false;
boolean a = false, a_Old = false;
boolean b = false, b_Old = false;
boolean c = false, c_Old = false;

//**** 	 Declare OUPUT pins states	****
boolean MIP = false, MIP_Old = false;
boolean MSP = false, MSP_Old = false;
boolean V1 = false, V1_Old = false;
boolean MX11 = false, MX11_Old = false;
boolean MX12 = false, MX12_Old = false;
boolean MX21 = false, MX21_Old = false;
boolean MX22 = false, MX22_Old = false;
boolean New1 = false, New1_Old = false;
boolean New2 = false, New2_Old = false;
boolean New3 = false, New3_Old = false;

//**** 	 Declare STEPs variables	****
boolean S0_X = true, S0_X_Old = true;
boolean S1_X = false, S1_X_Old = false;
boolean S2_X = false, S2_X_Old = false;
boolean S3_X = false, S3_X_Old = false;
boolean S4_X = false, S4_X_Old = false;
boolean S5_X = false, S5_X_Old = false;
boolean S6_X = false, S6_X_Old = false;
boolean S7_X = false, S7_X_Old = false;
boolean S8_X = false, S8_X_Old = false;
boolean S9_X = false, S9_X_Old = false;
boolean S10_X = false, S10_X_Old = false;
boolean S11_X = false, S11_X_Old = false;
boolean S12_X = false, S12_X_Old = false;
boolean S13_X = false, S13_X_Old = false;

//**** 	 Declare Validated Transitions variables	****
boolean VT_1 = false;
boolean VT_2 = false;
boolean VT_3 = false;
boolean VT_4 = false;
boolean VT_5 = false;
boolean VT_6 = false;
boolean VT_7 = false;
boolean VT_8 = false;
boolean VT_9 = false;
boolean VT_10 = false;
boolean VT_11 = false;
boolean VT_12 = false;
boolean VT_13 = false;
boolean VT_14 = false;
boolean VT_15 = false;
boolean VT_16 = false;

//**** 	 Declare Receptivities of Transitions variables	****
boolean R_1 = false;
boolean R_2 = false;
boolean R_3 = false;
boolean R_4 = false;
boolean R_5 = false;
boolean R_6 = false;
boolean R_7 = false;
boolean R_8 = false;
boolean R_9 = false;
boolean R_10 = false;
boolean R_11 = false;
boolean R_12 = false;
boolean R_13 = false;
boolean R_14 = false;
boolean R_15 = false;
boolean R_16 = false;

//**** 	 Declare Transitions receptivities variables	****
boolean TR_1 = false;
boolean TR_2 = false;
boolean TR_3 = false;
boolean TR_4 = false;
boolean TR_5 = false;
boolean TR_6 = false;
boolean TR_7 = false;
boolean TR_8 = false;
boolean TR_9 = false;
boolean TR_10 = false;
boolean TR_11 = false;
boolean TR_12 = false;
boolean TR_13 = false;
boolean TR_14 = false;
boolean TR_15 = false;
boolean TR_16 = false;

//**** 	 Declare STEPs timing variables for duration activity 	****
unsigned int S0_duration = 0;
unsigned int S1_duration = 0;
unsigned int S2_duration = 0;
unsigned int S3_duration = 0;
unsigned int S4_duration = 0;
unsigned int S5_duration = 0;
unsigned int S6_duration = 0;
unsigned int S7_duration = 0;
unsigned int S8_duration = 0;
unsigned int S9_duration = 0;
unsigned int S10_duration = 0;
unsigned int S11_duration = 0;
unsigned int S12_duration = 0;
unsigned int S13_duration = 0;

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
	if(S6_X) {S6_duration ++ ;} else {S6_duration = 0;}
	if(S7_X) {S7_duration ++ ;} else {S7_duration = 0;}
	if(S8_X) {S8_duration ++ ;} else {S8_duration = 0;}
	if(S9_X) {S9_duration ++ ;} else {S9_duration = 0;}
	if(S10_X) {S10_duration ++ ;} else {S10_duration = 0;}
	if(S11_X) {S11_duration ++ ;} else {S11_duration = 0;}
	if(S12_X) {S12_duration ++ ;} else {S12_duration = 0;}
	if(S13_X) {S13_duration ++ ;} else {S13_duration = 0;}
}

void setup(){
	initializeTimer1();
	Serial.begin(9600);
	//INIT INPUT PINs
	pinMode(pin_mR1, INPUT);
	pinMode(pin_hR1, INPUT);
	pinMode(pin_bR2, INPUT);
	pinMode(pin_bWD, INPUT);
	pinMode(pin_pV1, INPUT);
	pinMode(pin_pE1, INPUT);
	pinMode(pin_pE2, INPUT);
	pinMode(pin_pE4, INPUT);
	pinMode(pin_pE5, INPUT);
	pinMode(pin_a, INPUT);
	pinMode(pin_b, INPUT);
	pinMode(pin_c, INPUT);
	//INIT OUTPUT PINs
	pinMode(pin_MIP, OUTPUT);
	pinMode(pin_MSP, OUTPUT);
	pinMode(pin_V1, OUTPUT);
	pinMode(pin_MX11, OUTPUT);
	pinMode(pin_MX12, OUTPUT);
	pinMode(pin_MX21, OUTPUT);
	pinMode(pin_MX22, OUTPUT);
	pinMode(pin_New1, OUTPUT);
	pinMode(pin_New2, OUTPUT);
	pinMode(pin_New3, OUTPUT);

	time_val = millis();
	NN = 0;
}

void loop(){
	//Reading input values
	mR1 = digitalRead(pin_mR1);
	hR1 = digitalRead(pin_hR1);
	bR2 = digitalRead(pin_bR2);
	bWD = digitalRead(pin_bWD);
	pV1 = digitalRead(pin_pV1);
	pE1 = digitalRead(pin_pE1);
	pE2 = digitalRead(pin_pE2);
	pE4 = digitalRead(pin_pE4);
	pE5 = digitalRead(pin_pE5);
	a = digitalRead(pin_a);
	b = digitalRead(pin_b);
	c = digitalRead(pin_c);

	//Evaluate validated transitions variabless
	VT_1 = (S0_X);
	VT_2 = (S1_X);
	VT_3 = (S1_X);
	VT_4 = (S1_X);
	VT_5 = (S2_X);
	VT_6 = (S2_X);
	VT_7 = (S3_X);
	VT_8 = (S3_X);
	VT_9 = (S6_X);
	VT_10 = (S7_X);
	VT_11 = (S5_X && S8_X);
	VT_12 = (S10_X);
	VT_13 = (S9_X && S11_X);
	VT_14 = (S4_X);
	VT_15 = (S12_X);
	VT_16 = (S13_X);

	//Evaluate Receptivities oftransitions
	R_1 = ((pE1 || pE2 || pE5) && bR2 || (pE4 || pE5) && bWD || pV1) && !mR1;
	R_2 = a && b || bR2;
	R_3 = !bR2 && bWD || a && c;
	R_4 = !bR2 && !bWD;
	R_5 = pE1 || pE2 || pE5;
	R_6 = !(pE1 || pE2 || pE5);
	R_7 = pE4 || pE5;
	R_8 = !(pE4 || pE5);
	R_9 = !pE1 || hR1 || !bR2;
	R_10 = !pE2 || hR1 || !bR2;
	R_11 = (hR1 || !pE5 || !bR2) && (S5_duration > 200);
	R_12 = !pE4 || hR1;
	R_13 = (hR1 || !bWD || !pE5) && (S9_duration > 200);
	R_14 = pV1;
	R_15 = hR1 || !pV1;
	R_16 = true;

	//Evaluate Clearing conditions
	TR_1 = VT_1 && R_1;
	TR_2 = VT_2 && R_2;
	TR_3 = VT_3 && R_3;
	TR_4 = VT_4 && R_4;
	TR_5 = VT_5 && R_5;
	TR_6 = VT_6 && R_6;
	TR_7 = VT_7 && R_7;
	TR_8 = VT_8 && R_8;
	TR_9 = VT_9 && R_9;
	TR_10 = VT_10 && R_10;
	TR_11 = VT_11 && R_11;
	TR_12 = VT_12 && R_12;
	TR_13 = VT_13 && R_13;
	TR_14 = VT_14 && R_14;
	TR_15 = VT_15 && R_15;
	TR_16 = VT_16 && R_16;

	//Save Step activity variables state in Old_ corresponding variables
	S0_X_Old = S0_X;
	S1_X_Old = S1_X;
	S2_X_Old = S2_X;
	S3_X_Old = S3_X;
	S4_X_Old = S4_X;
	S5_X_Old = S5_X;
	S6_X_Old = S6_X;
	S7_X_Old = S7_X;
	S8_X_Old = S8_X;
	S9_X_Old = S9_X;
	S10_X_Old = S10_X;
	S11_X_Old = S11_X;
	S12_X_Old = S12_X;
	S13_X_Old = S13_X;

	//Evaluate Step variables
	S0_X = TR_16 || (S0_X_Old && !TR_1);
	S1_X = TR_1 || (S1_X_Old && !TR_2 && !TR_3 && !TR_4);
	S2_X = TR_2 || (S2_X_Old && !TR_5 && !TR_6);
	S3_X = TR_3 || TR_6 || (S3_X_Old && !TR_7 && !TR_8);
	S4_X = TR_4 || TR_8 || (S4_X_Old && !TR_14);
	S5_X = TR_5 || (S5_X_Old && !TR_11);
	S6_X = TR_5 || (S6_X_Old && !TR_9);
	S7_X = TR_9 || (S7_X_Old && !TR_10);
	S8_X = TR_10 || (S8_X_Old && !TR_11);
	S9_X = TR_7 || (S9_X_Old && !TR_13);
	S10_X = TR_7 || (S10_X_Old && !TR_12);
	S11_X = TR_12 || (S11_X_Old && !TR_13);
	S12_X = TR_14 || (S12_X_Old && !TR_15);
	S13_X = TR_11 || TR_13 || TR_15 || (S13_X_Old && !TR_16);

	//Evaluate OUTPUTs variables
	MIP = S1_X || S9_X;
	MSP = S1_X || S5_X;
	V1 = S12_X;
	MX11 = S6_X || S8_X;
	MX12 = S7_X || S8_X;
	MX21 = S10_X;
	MX22 = S11_X;
	New1 = S13_X;
	New2 = S13_X;
	New3 = S13_X;

	//Update OUTPUTs
	if(MIP_Old != MIP) {digitalWrite(pin_MIP, MIP);}
	if(MSP_Old != MSP) {digitalWrite(pin_MSP, MSP);}
	if(V1_Old != V1) {digitalWrite(pin_V1, V1);}
	if(MX11_Old != MX11) {digitalWrite(pin_MX11, MX11);}
	if(MX12_Old != MX12) {digitalWrite(pin_MX12, MX12);}
	if(MX21_Old != MX21) {digitalWrite(pin_MX21, MX21);}
	if(MX22_Old != MX22) {digitalWrite(pin_MX22, MX22);}
	if(New1_Old != New1) {digitalWrite(pin_New1, New1);}
	if(New2_Old != New2) {digitalWrite(pin_New2, New2);}
	if(New3_Old != New3) {digitalWrite(pin_New3, New3);}

	//Update Old OUTPUTs
	MIP_Old = MIP;
	MSP_Old = MSP;
	V1_Old = V1;
	MX11_Old = MX11;
	MX12_Old = MX12;
	MX21_Old = MX21;
	MX22_Old = MX22;
	New1_Old = New1;
	New2_Old = New2;
	New3_Old = New3;

	NN++;
	if(millis() - time_val > 5000){
		Serial.println(NN);
		NN = 0; time_val = millis();
	}
	delay(2);
}
