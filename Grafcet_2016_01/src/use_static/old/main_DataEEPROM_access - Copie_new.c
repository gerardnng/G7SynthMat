/**********************************************************************
* ? 2005 Microchip Technology Inc.
*
* FileName:        DataEEPROM.h
* Dependencies:    Header (.h) files if applicable, see below
* Processor:       dsPIC30Fxxxx
* Compiler:        MPLAB? C30 v3.01 or higher
* IDE:             MPLAB? IDE v7.52 or later
* Dev. Board Used: dsPICDEM 1.1 Development Board
* Hardware Dependencies: None
*
* SOFTWARE LICENSE AGREEMENT:
* Microchip Technology Incorporated ("Microchip") retains all ownership and 
* intellectual property rights in the code accompanying this message and in all 
* derivatives hereto.  You may use this code, and any derivatives created by 
* any person or entity by or on your behalf, exclusively with Microchip's 
* proprietary products.  Your acceptance and/or use of this code constitutes 
* agreement to the terms and conditions of this notice.
*
* CODE ACCOMPANYING THIS MESSAGE IS SUPPLIED BY MICROCHIP "AS IS".  NO 
* WARRANTIES, WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED 
* TO, IMPLIED WARRANTIES OF NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR A 
* PARTICULAR PURPOSE APPLY TO THIS CODE, ITS INTERACTION WITH MICROCHIP'S 
* PRODUCTS, COMBINATION WITH ANY OTHER PRODUCTS, OR USE IN ANY APPLICATION. 
*
* YOU ACKNOWLEDGE AND AGREE THAT, IN NO EVENT, SHALL MICROCHIP BE LIABLE, WHETHER 
* IN CONTRACT, WARRANTY, TORT (INCLUDING NEGLIGENCE OR BREACH OF STATUTORY DUTY), 
* STRICT LIABILITY, INDEMNITY, CONTRIBUTION, OR OTHERWISE, FOR ANY INDIRECT, SPECIAL, 
* PUNITIVE, EXEMPLARY, INCIDENTAL OR CONSEQUENTIAL LOSS, DAMAGE, FOR COST OR EXPENSE OF 
* ANY KIND WHATSOEVER RELATED TO THE CODE, HOWSOEVER CAUSED, EVEN IF MICROCHIP HAS BEEN 
* ADVISED OF THE POSSIBILITY OR THE DAMAGES ARE FORESEEABLE.  TO THE FULLEST EXTENT 
* ALLOWABLE BY LAW, MICROCHIP'S TOTAL LIABILITY ON ALL CLAIMS IN ANY WAY RELATED TO 
* THIS CODE, SHALL NOT EXCEED THE PRICE YOU PAID DIRECTLY TO MICROCHIP SPECIFICALLY TO 
* HAVE THIS CODE DEVELOPED.
*
* You agree that you are solely responsible for testing the code and 
* determining its suitability.  Microchip has no obligation to modify, test, 
* certify, or support the code.
*
* REVISION HISTORY:
*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
* Author                 Date      Comments on this revision
*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
* EB/HV                  11/02/05  First release of source file
* GM                     10/19/07  Revised to use new library functions
*
*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
*
* ADDITIONAL NOTES:
*
**********************************************************************/    
 

#include <p30f4013.h>
#include <libpic30.h>

/*Declare Constraints*/
# define limit_RS  16
# define limit_Matrix_Static  64
# define limit_transition  16
# define limit_MA  8
# define limit_step 16

/*Declare PIC Configurations*/
_FOSC(CSW_FSCM_OFF & XT_PLL8); /* Set up for XTxPLL8 mode since */ //FOSC=10MHz,Fcy=20MHz
                                /* we will be tuning the FRC in this example */
_FWDT(WDT_OFF);                 /* Turn off the Watch-Dog Timer.  */
_FBORPOR(MCLR_EN & PWRT_OFF);   /* Enable MCLR reset pin and turn off the power-up timers. */
_FGS(CODE_PROT_OFF);            /* Disable Code Protection */


/*Declare Materials: Inputs and Ouputs*/

/*Declare Materials: Inputs*/
//Effective number of Inputs for the current system : 5
# define pin_I0 PORTBbits.RB6
# define pin_I1 PORTBbits.RB12
# define pin_I2 PORTBbits.RB2
# define pin_I3 PORTBbits.RB1
# define pin_I4 PORTBbits.RB0
# define pin_I5 PORTBbits.RB3
# define pin_I6 PORTBbits.RB4
# define pin_I7 PORTBbits.RB5
# define pin_I8 PORTBbits.RB7
# define pin_I9 PORTBbits.RB8
# define pin_I10 PORTBbits.RB9
# define pin_I11 PORTBbits.RB10
# define pin_I12 PORTBbits.RB11

/*Declare Materials: Outputs*/
//Effective number of Outputs for the current system : 4
# define pin_Q0 PORTDbits.RD8
# define pin_Q1 PORTFbits.RF0
# define pin_Q2 PORTFbits.RF1
# define pin_Q3 PORTFbits.RF6
# define pin_Q4 PORTFbits.RF2
# define pin_Q5 PORTFbits.RF3
# define pin_Q6 PORTFbits.RF4
# define pin_Q7 PORTFbits.RF5


unsigned int _EEDATA(2) DatainEE[] = {
	0x0005, 
	0x0004, 
	0x0009, 0x0002, 0x0004, 0x0010, 0x0020, 
	0x0012, 0x0004, 0x0001, 0x0020, 0x0008, 
	0x0009, 
	0x0002, 0x0010, 0x0004, 0x0020, 
	0x0001, 0x0004, 
	0x0001, 0x0005, 
	0x0002, 0x0007, 0x0002, 
	0x0001, 0x0006, 
	0x0002, 0x0008, 0x0002
};
//From grafcet File: D:\DD2\THESE\aDone_2013_2015\g7\g7_remplissage_reservoirs.xml

/*Declare Assembly Functions*/
//#include "uart.h"
unsigned int asmNOT(unsigned int);
unsigned int asmAND(unsigned int, unsigned int);
unsigned int asmOR(unsigned int, unsigned int);
unsigned int asmRLNC(unsigned int, unsigned int);
unsigned int asmRRNC(unsigned int, unsigned int);
_Bool Test_bit(unsigned int,unsigned int );

/*Declare Procedures*/
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
		Select_bit = asmRLNC(Select_bit,1);
	}
	Q_Port = O;
	Output_Update();	
}

void Output_Update(void)
{
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
		//or simply for (int k=0; k<4;k++) c = asmRRNC(c);
		//c = (c>>4);
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
	ADPCFG = 0xFFFF;         					// initialize AN pins as digital
    TRISB  = 0xFFFF;		 					// Port B as Input
	
	PORTF  = 0x0000;
	LATF   = 0x0000;
	TRISF  = 0x0000;		 					// Port F as Output
	PORTDbits.RD8  = 0;							
	LATDbits.LATD8 = 0;
	TRISDbits.TRISD8  = 0;		 				// RD8 as Output
	
	PORTDbits.RD3  = 0;							// Indicator to UART transmission complete
	LATDbits.LATD3 = 0;
	TRISDbits.TRISD3  = 0;		 				// RD3 as Output
	PORTCbits.RC13  = 1;		 				// RC13 is always = 1 as UART TX Pin	
	TRISCbits.TRISC13  = 0;		 				// RC13 as Output 
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

//Assembly functions body
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