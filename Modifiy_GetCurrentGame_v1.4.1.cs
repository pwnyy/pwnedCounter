using System;
using System.Collections;
using System.Collections.Generic;

public class CPHInline
{
	public bool Execute()
	{	
		/*
			Hi there, I'm pwnyy! And thank you for using my pwnedCounter aka DeathCounter! 
			I would like to say I am by no means a professional programmer, so there will be
			sections that may be questionable in this program! By any means improve it as you wish!

			All actions of the death counter kind of rely on each other pulling the global variables that
			were set, keep that in mind.

			If you want to contact me, you can do so on https://twitter.com/pwnyy or on the
			Streamer.Bot Discord, just look up pwnyy you should be able to find me.

			To support me https://ko-fi.com/pwnyy , always appreciated!
		
		*/

		//SafeGuard Check
		//Check if this is first time setup or not
		Hashtable checkInfo = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");

		if(checkInfo == null){
			CPH.RunAction("pwnC_UpdateStream",true);
		}

		Hashtable infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info",true);
		string currentGame = (string) infoList["currentGame"];
		CPH.SetArgument("game",currentGame);

		//Second SafeGuard, Checking if game in the counter list.
		Dictionary<string,int> gameList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games",true);
		bool inList = gameList.ContainsKey(currentGame);
		if(inList){
			CPH.SetArgument("inList","true");
		}else{
			CPH.SetArgument("inList","false");
		}
		return true;
	}
}
