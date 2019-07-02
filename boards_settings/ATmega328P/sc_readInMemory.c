unsigned int readNextIntFromEEPROM()
{
	unsigned int val = 0 ; //variable de type int, vide, qui va contenir le r?sultat de la lecture

	unsigned char fort = EEPROM.read(MatricesDATA_eeprom_adr++);   //r?cup?re les 8 bits de gauche (poids fort) -> 1101 0111
	unsigned char faible = EEPROM.read(MatricesDATA_eeprom_adr++); //r?cup?re les 8 bits de droite (poids faible) -> 0010 1100

	//assemblage des deux variable pr?c?dentes
	val = fort ;         // val vaut alors 0000 0000 1101 0111
	val = val << 8 ;     // val vaut maintenant 1101 0111 0000 0000 (d?calage)
	val = val | faible ; // utilisation du masque
	//calcul : 1101 0111 0000 0000 | 0010 1100 = 1101 0111 0010 1100
	//Serial.print("READ EEPROM : "); Serial.println(val, HEX);
	
	return val ; //on n?oublie pas de retourner la valeur lue !
}