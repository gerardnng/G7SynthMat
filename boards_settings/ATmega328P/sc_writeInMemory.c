void writeNextIntInEEPROM(unsigned int val) 
{   
	//Serial.print("WRITE EEPROM : "); Serial.println(val, HEX);
	//d?coupage de la variable val qui contient la valeur ? sauvegarder en m?moire
	unsigned char faible = val & 0x00FF; //r?cup?re les 8 bits de droite (poids faible) -> 0010 1100 
	//calcul : 1101 0111 0010 1100 & 0000 0000 1111 1111 = 0010 1100

	unsigned char fort = (val >> 8) & 0x00FF;  //d?cale puis r?cup?re les 8 bits de gauche (poids fort) -> 1101 0111
	//calcul : 1101 0111 0010 1100 >> 8 = 0000 0000 1101 0111 puis le m?me & qu?avant

	//puis on enregistre les deux variables obtenues en m?moire
	EEPROM.write(MatricesDATA_eeprom_adr++, fort) ; //on ?crit les bits de poids fort en premier
	EEPROM.write(MatricesDATA_eeprom_adr++, faible) ; //puis on ?crit les bits de poids faible ? la case suivante
}