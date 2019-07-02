
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

