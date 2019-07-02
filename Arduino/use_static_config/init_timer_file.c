//To manage timer or measuring duration of active steps
//const unsigned int Board_TIME_UNIT = 1; // Board_TIME_UNIT =5 i.e. 5Hz : Number of callback function of Timer1 per second

void initializeTimer1(){
  //Timer1.initialize(1000000);     // initialize timer1, and set a 1 second period
  unsigned int FT_Steps = 1000/Board_TIME_UNIT; //FT_Steps = frequency_timer
  Timer1.initialize(1000000/FT_Steps);       // initialize timer1, and set a 1 second period
  //Timer1.pwm(9, 512);             		// setup pwm on pin 9, 50% duty cycle, if there is pinMode(9, OUTPUT); It present the frequency of the timer
  Timer1.attachInterrupt(Step_timer_update_callback);   // attaches callback() as a timer overflow interrupt
}