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
	
	//Serial.print("X = "); Serial.println(X, BIN);
	
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
	//Serial.print("OUT_Port = "); Serial.println(OUT_Port, BIN);
	
	NN++;
	if(millis() - time_val > 5000){
		Serial.println(NN);
		NN = 0; time_val = millis();
	}
	//Serial.println("------------------");