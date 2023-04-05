using System;
using System.Collections;
using System.Collections.Generic;

public class CPHInline
{
	public bool Execute()
	{	
		
		//Check if this is first time setup or not
		Hashtable checkInfo = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");
		
		if(checkInfo == null){
			CPH.RunAction("pwnC_UpdateStream",true);
		}
		
		//Get current Game
		Hashtable infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info",true);
		string currentGame = (string) infoList["currentGame"];
		CPH.SetArgument("game",currentGame);
		return true;
	}
}
