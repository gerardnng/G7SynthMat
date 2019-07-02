
/*Declare Assembly Functions*/
#include "uart.h"
unsigned int asmNOT(unsigned int);
unsigned int asmAND(unsigned int, unsigned int);
unsigned int asmOR(unsigned int, unsigned int);
unsigned int asmRLNC(unsigned int, unsigned int);
unsigned int asmRRNC(unsigned int, unsigned int);
_Bool Test_bit(unsigned int,unsigned int);

/*Declare Procedure*/
void Dynamic_Loop(void);
void Step_and_Output_update(void);
void Output_Update(void);
void RS_Parse(unsigned int a);
void Input_Parse(unsigned int a);
void Output_Parse(unsigned int a);
void Step_Parse(unsigned int a);
void Step_time_Parse(unsigned int a);


int main(void);
void __attribute__((interrupt,no_auto_psv)) _U1RXInterrupt();
//void __attribute__((interrupt, no_auto_psv)) _INT0Interrupt();
void __attribute__((__interrupt__, __auto_psv__)) _T3Interrupt(void);
//void __attribute__((interrupt, no_auto_psv)) __T1Interrupt(void);
void _ISR _DefaultInterrupt(void);

//void init_INT0();
void init_ports();
void init_Timer3();
void init_UART();
void EEPROM_Write ();
void EEPROM_Read ();

/*Declare variables to be stored in RAM*/
unsigned int N_tran =  5;
unsigned int N_MA =  4 ;
unsigned int DatainRAM[limit_Matrix_Static];
unsigned int E[limit_transition];
unsigned int S[limit_transition];
unsigned int X_INIT;
unsigned int MA[limit_MA];
unsigned int RS[limit_transition][limit_RS];

_Bool stack[4];
_Bool* TOS = stack; // Can be uses as number of step, input, output, ...

unsigned char* P_Buffer = (unsigned char*) DatainRAM;
unsigned int Nu_BufferIndex;

unsigned int X; //Actual situation
unsigned int Q_Port; //Output 'register'
unsigned int Step_active_timer[limit_step];


//////////////////////////////
/*	 	Main Program 	 	*/
/////////////////////////////
int main(void)
{
	init_ports();
	init_Timer3();
	init_UART();                	

    /*Copy Static Matrix from DataEEPROM to "DatainRAM" in RAM*/
 	EEPROM_Read ();
    
	/*Generate Dynamic Matrix*/
	X = X_INIT;
	Step_and_Output_update();
	Dynamic_Loop();
	return 0;
}

void Dynamic_Loop(void)
{
	unsigned int XEi,VT,R,FT,VA,VD,AD,not_VD,DX;
	unsigned int i,j;
	while(1)
	{
		//Reorganize the code in order to Add here the reading of inputs into a variable IN_VARS
		VA = 0;
		VD = 0;
		for (i = 0; i < N_tran; i++)
    	{
			XEi = asmAND(X,E[i]);
	    	if (XEi == E[i])  {VT=1;}
			else {VT=0;}

			//Evaluation of the value of transition i
			for (j = 1; j <= RS[i][0]; j++)
			{
				RS_Parse(RS[i][j]); 
			}
			R = (unsigned int) *--TOS; 
		
			FT = asmAND(VT,R);
			if (FT !=0)
			{
				VA = asmOR(VA,S[i]); //S[i] is a 16 bits (2 bytes) data (unsigned int)
				VD = asmOR(VD,E[i]);
			}		           
    	}
		
		//Le franchissement est simultan? apr?s calcul des transitions franchissables
		
		AD = asmAND(VA,VD);
		not_VD = asmNOT(VD);
		DX = asmOR(not_VD,AD);
		X = asmAND(X,DX);
		X = asmOR(X,VA);
		Step_and_Output_update();		
	}
}

void Step_and_Output_update(void)
{
	unsigned int i, XMA, O;

	//Reinitialize to 0 all the active duration of steps
	for (i = 0; i < limit_step; i++)
    {
		if (Test_bit(X,i) == 0)
		{
			Step_active_timer[i] = 0;	
		}
	}

	O = 0;
	unsigned int Select_bit = 0x0001;
	for (i = 0; i < N_MA; i++)
    {
		XMA =0;
		XMA = asmAND(X,MA[i]);
		if (XMA != 0)
		{
			O = asmOR(O,Select_bit); 
		}
		Select_bit = asmRLNC(Select_bit, 1);
	}
	Q_Port = O;
	Output_Update();	
}

void Output_Update(void)
{
//Run on the board to test that outputs are updated. 
// If not, it is because pin_Q0 is directly connected to OUT_PIN_0, not to PORTFbits.RF0. 
// Solution: Try to do this direct connection by 
//	1- Commenting all what is inside the PIC_G7_head_DeclarePINs.c file
//	2- Defining in the board_settings.xml file: <option value="%Out_0" pin="OUT_PIN_0"/>
//		by: <option value="%Out_0" pin="PORTFbits.RF0"/>
	pin_Q0 = Test_bit(Q_Port,0);
	pin_Q1 = Test_bit(Q_Port,1);
	pin_Q2 = Test_bit(Q_Port,2);
	pin_Q3 = Test_bit(Q_Port,3);
	pin_Q4 = Test_bit(Q_Port,4);
	pin_Q5 = Test_bit(Q_Port,5);
	pin_Q6 = Test_bit(Q_Port,6);
	pin_Q7 = Test_bit(Q_Port,7);
}

void RS_Parse(unsigned int a)
{
	unsigned int b = asmAND(a,0x8000);
	if (b == 0)
	{
		b = asmAND(a,0x4000);
		if (b == 0)
		{
			Input_Parse(a);	//Read the value in input of the concerned variable
							//It may just be a *TOS = IN_VARS[a-4] when inputs are read in IN_VARS
		}
		else
		{
			Output_Parse(a);
		}	
	}
	else
	{
		b = asmAND(a,0x4000);
		if (b == 0)
		{
			Step_Parse(a);	
		}
		else
		{
			Step_time_Parse(a);
		}	
	}
}

void Input_Parse(unsigned int a)
{
	_Bool stack_v1;
	_Bool stack_v2;
	unsigned int b = asmAND(a,0x000F);
	switch (b)
	{
		case 0 :
		{
			stack_v1 = *--TOS;
			stack_v2 = *--TOS;
			*TOS++ = stack_v1 | stack_v2; 
			break;
		}
		case 1 :
		{
			stack_v1 = *--TOS;
			stack_v2 = *--TOS;
			*TOS++ = stack_v1 & stack_v2;
			break;
		}
		case 2 :
		{
			stack_v1 = *--TOS;
			*TOS++ = !stack_v1;
			break;
		}
		case 3:
		{
			*TOS++ = 1;
			break;
		}
		case 4:
		{
			*TOS++ = pin_I0;
			break;
		}
		case 5:
		{	
			*TOS++ = pin_I1;
			break;
		}
		case 6:
		{
			*TOS++ = pin_I2;
			break;
		}
		case 7:
		{
			*TOS++ = pin_I3;
			break;
		}
		case 8:
		{
			*TOS++ = pin_I4;
			break;
		}
		case 9:
		{
			*TOS++ = pin_I5;
			break;
		}
		case 10:
		{
			*TOS++ = pin_I6;
			break;
		}
		case 11:
		{
			*TOS++ = pin_I7;
			break;
		}
		case 12:
		{
			*TOS++ = pin_I8;
			break;
		}
		case 13:
		{
			*TOS++ = pin_I9;
			break;
		}
		case 14:
		{
			*TOS++ = pin_I10;
			break;
		}
		case 15:
		{
			*TOS++ = pin_I11;
			break;		
		}
		default :
		{
			*TOS++ = 0;	
		}
	}
}

void Output_Parse(unsigned int a)
{
	*TOS++ = Test_bit(Q_Port,a);	
}

void Step_Parse(unsigned int a)
{
	*TOS++ = Test_bit(X,a);	
}

void Step_time_Parse(unsigned int a)
{
	if (Test_bit(X,a) == 0) // Is step number asmAND(a,0x000F) be 0 in X ? If yes this step is not active
	{
		*TOS++ = 0;
	}
	else
	{
		unsigned int c = asmAND(a,0x3FF0);
		//or simply for (int k=0; k<4;k++) c = asmRRNC(c,1);
		c = asmRRNC(c,4);
		
		unsigned int b = asmAND(a,0x000F);
		if (Step_active_timer[b] >= c)
		{
			*TOS++ = 1;
		}
		else
		{
			*TOS++ = 0;
		}	
	}	
}

//////////////////////////////
/*	 Interrupt Blocks 	 	*/
/////////////////////////////

void __attribute__((interrupt, no_auto_psv)) _DefaultInterrupt(void)
{
    while(1) ClrWdt()
}

void __attribute__((interrupt, no_auto_psv)) _T3Interrupt()
{
  IFS0bits.T3IF = 0;    	  // Clear the Timer3 interrupt status flag
  //** user code starts here
 unsigned int i; 
 for (i = 0; i < limit_step; i++)
  {
	if (Test_bit(X,i) != 0) //Test_bit(X,i)=0 means that step i is not active
	{
		Step_active_timer[i] = Step_active_timer[i] + 1;	
	}
  }
}

void __attribute__((interrupt, no_auto_psv)) _U1RXInterrupt()
{
	IFS0bits.U1RXIF = 0;						// manually cleared U1RX Interrupt flag
	unsigned char UART_RX = U1RXREG;
	unsigned int i;
	if( UART_RX != 0xFE )						// If the full transmittion message is not completely received
	{	
		*P_Buffer++ = UART_RX;
		Nu_BufferIndex++;
	}
	else
	{
		PORTDbits.RD3 = 1;
		for (i = Nu_BufferIndex; i <limit_Matrix_Static*2; i++)
		{
			*P_Buffer++ = 0;
		}
		Nu_BufferIndex = 0;
		P_Buffer = (unsigned char*) DatainRAM;
		EEPROM_Write ();
		//PORTDbits.RD3 = 0; 				
	}		
} 

//////////////////////////////
/*	 	Initial Blocks 	 	*/
/////////////////////////////

void init_ports()
{
	// initialize AN pins as digital
	ADPCFG = 0xFFFF; 
	// Port B as Input
	TRISB  = 0xFFFF;
	
	// Port F as Output
	PORTF  = 0x0000;
	LATF   = 0x0000;
	TRISF  = 0x0000;	
	
	
	// PORTDbits.RD8 as Output
	PORTDbits.RD8  = 0;							
	LATDbits.LATD8 = 0;
	TRISDbits.TRISD8  = 0;	 				
	
	// Indicator to UART transmission complete
	PORTDbits.RD3  = 0;	
	LATDbits.LATD3 = 0;
	// RD3 as Output
	TRISDbits.TRISD3  = 0;		 				
	// RC13 is always = 1 as UART TX Pin
	PORTCbits.RC13  = 1;		 				
	// RC13 as Output	
	TRISCbits.TRISC13  = 0;		 				 
}

void init_Timer3() 
{
  // configuration of 32-bit timer, Timer 3 and Timer 2
  T2CON = 0; 				// Stops any 16/32-bit Timer3 operation
  T3CON = 0; 				// Stops any 16-bit Timer3 operation. For 32-bit timer case, T3CON register is ignored
  TMR3 = 0;					// CLR TMR3 ; Clear contents of the Timer3 timer register
  TMR2 = 0; 				// Clear contents of the Timer3 timer register
  PR3 = 0x0001; 			// Load the Period Registers with value 0x0001
  PR2 = 0x312D;				// Load the Period Registers with value 0x312D
  IPC1bits.T3IP = 1;   		// Interrupt priority level = 1
  IFS0bits.T3IF = 0;    	// Clear the Timer3 interrupt status flag
  IEC0bits.T3IE = 1; 		// Enable Timer3 interrupts
  T2CON = 0x8038;           // Timer3 ON, internal clock FCY, prescaler 1:256, 32-bit
}

/* Set Up UART */
 void init_UART()
 {   
	unsigned int baud;
    unsigned int U1MODEvalue;
    unsigned int U1STAvalue;
    CloseUART1();
    _U1RXIP = 1;            						// Set UART1 interrupt priority
	IFS0bits.U1RXIF = 0;							// clear interrupt flag of rx
 	IEC0bits.U1RXIE = 1;							// enable rx recieved data interrupt   
    U1MODEvalue =0x8400;		 					// Enable, 8data, no parity, 1 stop
    U1STAvalue = 0x0000;							// Disable TX
    baud=129;    									// 9600Baud for 20MIP, Fosc = 10MHz (See FRM Tables) 
    OpenUART1(U1MODEvalue, U1STAvalue, baud);
	Nu_BufferIndex =0;
	P_Buffer = (unsigned char*) DatainRAM;
 }


//////////////////////////////
/*	 	EEPROM Blocks 	 	*/
/////////////////////////////

//7FFC00
void EEPROM_Read ()
{
 	unsigned int i,j;
	_prog_addressT p = 0x7FFC00;          					//makes p a pointer
 	_init_prog_address(p, DatainEE);     					//points it to the EE space
 	_memcpy_p2d16(DatainRAM,p,limit_Matrix_Static*2);   	//copies EE array into RAM array

	unsigned int* P_RAM = DatainRAM;
	N_tran = *P_RAM++;
	N_MA = *P_RAM++;
	for (i = 0; i < N_tran; i++)
	{	
		E[i] = *P_RAM++;
	}
	for (i = 0; i < N_tran; i++)
	{	
		S[i] = *P_RAM++;
	}
	X_INIT = *P_RAM++;
	for (i = 0; i < N_MA; i++)
	{	
		MA[i] = *P_RAM++;
	}
	for (i = 0; i < N_tran; i++)
	{	
		unsigned int RS_length = *P_RAM++;
		RS[i][0] = RS_length;
		for (j = 1; j <= RS_length; j++)
		{
			RS[i][j] = *P_RAM++;
		}
		for (j = RS_length+1; j < limit_RS; j++)
		{
			RS[i][j] = 0;
		}
	}
}

void EEPROM_Write ()
{
	unsigned int i,j;
	int Array1inRAM[16];
	unsigned int k = 0;
	//_erase_eedata_all();    
	//unsigned int* P_RAM =  DatainRAM;
	_prog_addressT p = 0x7FFC00;
	for (i = 0; i < 4; i++)
	{	
 		_erase_eedata(p, _EE_ROW);
		_wait_eedata();
		for (j = 0; j < 16; j++)
		{
			Array1inRAM [j] =  DatainRAM[k++];
		}
		_write_eedata_row(p, Array1inRAM);   				//write the contents of the RAM array to EE
 		_wait_eedata();        								//wait
		p = p +  _EE_ROW; 
 	} 	
}

//Define Assembly functions (body)
unsigned int asmNOT(unsigned int reg_data){
  //Exemple asmNOT(0x000[1010])ie 10 --> 0x000[0101] ie 3
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

//In the following function, asmAND(n,0x000F); to just consider that n is between 0 and 16
//To return the bit number i of the register reg_name
_Bool Test_bit(unsigned int reg_name, unsigned int bit_index){
  return (reg_name>>(asmAND(bit_index,0x000F))) & 0x0001; 
}
unsigned int asmRLNC(unsigned int a, unsigned int n){
  //Move from right to left or simply multiply by 2
  return a<<(asmAND(n,0x000F));  
}
unsigned int asmRRNC(unsigned int a, unsigned int n){
  //Move from left to right or simply divide by 2
  return a>>(asmAND(n,0x000F));   
}