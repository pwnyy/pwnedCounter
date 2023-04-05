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

		//Get OBS No Counter Message
		string obsNoCounterMessage = args.ContainsKey("obsNoCounterMessage") ? args["obsNoCounterMessage"].ToString() : "No DeathCounter";

		//Get game value 
		string currentGame = args.ContainsKey("game") ? args["game"].ToString() : "Just Chatting";
		//Get value of pwnedCounter_Games to see if there is one yet.
		Hashtable checkInfo = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");
		//init new system bool and inList
		bool newPwnedCounter = false;
		bool inList = false;

		if(checkInfo == null)
		{
			//Init Dictionary
			Dictionary<string, int> newGameList = new Dictionary<string, int>();
			//Set GlobalVar
			CPH.SetGlobalVar("pwnedCounter_Games",newGameList,true);
			CPH.LogInfo("[pwned Counter] - pwnedCounter_Games global variable has been created as it did not exist before.");

			//Create Hashtable
			Hashtable info = new Hashtable();
			//Add currentGame and totalDeaths as KeyValue Pair
			info.Add("currentGame",currentGame);
			info.Add("totalDeaths",0);
			//Set global value with Hashtable.
			CPH.SetGlobalVar("pwnedCounter_Info",info,true);

			CPH.LogInfo("[pwned Counter] - pwnedCounter_Info global variable has been created as it did not exist before.");

			//set newPwnedCounter
			newPwnedCounter = true;
		}

		
		//Set Global hashtable value of current game
		Hashtable infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info",true);
		infoList["currentGame"] = currentGame;
		//Set Hashtable with updated currentGame value
		CPH.SetGlobalVar("pwnedCounter_Info",infoList,true);
		
		/*

		Check Category Section

		*/
		//Get Games Dictionary
		Dictionary<string,int> gameList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games",true);
		if(!newPwnedCounter)
		{
			//Get Games Dictionary
			//Check if game is in dictionary
			inList = gameList.ContainsKey(currentGame);

		}

		if(inList){
			
			//Enable Death Commands from "Death Modify" group
			CPH.RunAction("pwnC_EnableCommands",true);
			CPH.SetArgument("deathCounter",gameList[currentGame].ToString());
		}else{
			//Disable Death Commands from "Death Modify" group
			CPH.RunAction("pwnC_DisableCommands",true);
			/*

				Change "No DeathCounter" to whatever you want the OBS Text source to display if game is not a counter.
			
			*/
			CPH.SetArgument("deathCounter",obsNoCounterMessage);
		}


		//Set argument to use in further sub-actions
		CPH.SetArgument("game",currentGame);
		return true;
	}
}
