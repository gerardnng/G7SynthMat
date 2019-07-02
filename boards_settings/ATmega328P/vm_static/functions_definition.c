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

			/*
			Serial.print("RE_index = "); Serial.println(old_RE_index);
			Serial.print("Old value = "); Serial.println(old_RE_value);
			Serial.print("New value = "); Serial.println(stack_v1);
			Serial.print("RESULT : "); Serial.println(*stack_position);
			Serial.println("");
			*/
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

void EEPROM_Write()
{
	MatricesDATA_eeprom_adr = 0;
	unsigned int M_size = sizeof(MatricesDATA)/2; //To modify dynamically inside MPLAB

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
	
	Serial.print("N_Trans = "); Serial.println(N_Trans);
	Serial.print("N_Steps = "); Serial.println(N_Steps);
	Serial.print("N_Actions = "); Serial.println(N_Actions);
	Serial.print("N_Inputs = "); Serial.println(N_Inputs);
	Serial.print("X_INIT = "); Serial.println(X_INIT);

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
	
	Serial.println("End EEPROM_Read");
}

