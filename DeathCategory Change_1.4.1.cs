using System;
using System.Collections;
using System.Collections.Generic;

public class CPHInline
{

	//Init currentGame string
	string currentGame;

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

		//pGet current game
        currentGame = args["game"].ToString();
		
		//Check if deathcategory was used via command
		bool isCommand = args.ContainsKey("command");
		//Get first input of command
		string input = args.ContainsKey("rawInput") ? args["rawInput"].ToString() : "";
		
		//Get command inputs
		string inputAdd = args.ContainsKey("inputAdd") ? args["inputAdd"].ToString() : "add";
		string inputRem = args.ContainsKey("inputRem") ? args["inputRem"].ToString() : "rem";
		
		//Just a default return string if for some reason the arguments where disabled
		string noMessage = "Key of message was not found. Was the Set-Argument sub action disabled?";
		//Get add messages
		string categoryAddedMessage = args.ContainsKey("categoryAddedMessage") ? args["categoryAddedMessage"].ToString() : noMessage;
		string categoryAlreadyAdded = args.ContainsKey("categoryAlreadyAdded") ? args["categoryAlreadyAdded"].ToString() : noMessage;
		//Get remove messages
		string categoryRemovedMessage = args.ContainsKey("categoryRemovedMessage") ? args["categoryRemovedMessage"].ToString() : noMessage;
		string categoryAlreadyRemoved = args.ContainsKey("categoryAlreadyRemoved") ? args["categoryAlreadyRemoved"].ToString() : noMessage;

		//Get OBS No Counter Message
		string obsNoCounterMessage = args.ContainsKey("obsNoCounterMessage") ? args["obsNoCounterMessage"].ToString() : "No DeathCounter";
		
		//Get message for wrong or empty input
		string	wrongInput = args["wrongInput"].ToString();
		/*

		Check if it was used properly via chat command and if it has inputs.
		Check inputs and make actions accordingly.
	
		*/
		if(!isCommand)
		{
			CPH.LogInfo("[pwned Counter] - Category change should be used via chat command as to not accidentaly reset a category.");
		}else{
			if(input == inputAdd)
			{
				bool addSuccess = AddCategory(currentGame);
				if(addSuccess){
					CPH.SendMessage(categoryAddedMessage);
					CPH.SetArgument("deathCounter","0");
				}else{
					CPH.SendMessage(categoryAlreadyAdded);
					
					CPH.SetArgument("deathCounter",CPH.GetGlobalVar<int>("death_" + currentGame, true));
				}
				
			}else if (input == inputRem){
				
				bool remSuccess = RemCategory(currentGame);
				if(remSuccess){
					CPH.SendMessage(categoryRemovedMessage);
				}else{
					CPH.SendMessage(categoryAlreadyRemoved);
				}
				CPH.SetArgument("deathCounter",obsNoCounterMessage);
			}else{
				CPH.SendMessage(wrongInput);
			}
		}
        return true;
    }
	
	public bool AddCategory(string currentGame){
		CheckDeathGlobal();
		bool success = true;

		//Get gameList table
		Dictionary<string,int> deathList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games",true);
		
		if (!deathList.ContainsKey(currentGame))
        {
            //Add current game to the games
            deathList.Add(currentGame,0);

			//Save dictionary babck to global var
			CPH.SetGlobalVar("pwnedCounter_Games", deathList, true);

			//Enable Death Commands from "Death Modify" group
			CPH.RunAction("pwnC_EnableCommands",true);
			success = true;
            //Message that is send to the log
            CPH.LogInfo("[pwned Counter] - Category " + currentGame + " was added to pwnedCounter_Games");
        }
        else
        {
			success = false;
            //Message that is send to the log
            CPH.LogInfo("[pwned Counter] - Category " + currentGame + " was already in pwnedCounter_Games");
        }
		
		return success;
	}

	public bool RemCategory(string currentGame){
		CheckDeathGlobal();
		bool success = true;
		
		//Get gameList table
		Dictionary<string,int> deathList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games",true);

        //yAdd current game to the death categories
        
        if (!deathList.ContainsKey(currentGame))
        {
			
			success = false;
            //Message that is send to the log
            CPH.LogInfo("[pwned Counter] - Category " + currentGame + " was not removed. It may not have been in the list.");
        }
        else
        {
			
			//Disable Command Group "Death Modify"
			CPH.RunAction("pwnC_DisableCommands",true);

            //Get amount of deaths in variable and total deaths
            int gameDeaths = deathList[currentGame];
			
			//Get Hashtable
			Hashtable pC_Info = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");

			//Remove KeyValue pair of currentGame
			deathList.Remove(currentGame);

			//Sum up all games in dictionary to prevent miscalculation
			int gameSum = 0;
			foreach(KeyValuePair<string,int> entry in deathList)
			{
				gameSum = gameSum + entry.Value;

			}
			//Set totalDeaths value of hashtable
			pC_Info["totalDeaths"] = gameSum;
			
			//Save hashtable to globals
			CPH.SetGlobalVar("pwnedCounter_Info",pC_Info,true);
			
			//Save dictionary to globals
			CPH.SetGlobalVar("pwnedCounter_Games",deathList,true);
			success = true;
            //Message that is send to the log if successfully added a game to the list
            CPH.LogInfo("[pwned Counter] - Category " + currentGame + " removed successfully.");
        }
		return success;
	}

	public void CheckDeathGlobal(){
		
		//string checkGamelist = CPH.GetGlobalVar<string>("pwnedCounter_Games");
		Hashtable checkInfo = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");
		if(checkInfo == null)
		{
			Dictionary<string, int> gameList = new Dictionary<string, int>();
			CPH.SetGlobalVar("pwnedCounter_Games",gameList,true);
			CPH.LogInfo("[pwned Counter] - pwnedCounter_Games global variable has been created as it did not exist before.");
			//Create Hashtable
			Hashtable info = new Hashtable();
			//Add currentGame and totalDeaths as KeyValue Pair
			info.Add("currentGame",currentGame);
			info.Add("totalDeaths",0);
			//Set global value with Hashtable.
			CPH.SetGlobalVar("pwnedCounter_Info",info,true);

			CPH.LogInfo("[pwned Counter] - pwnedCounter_Info global variable has been created as it did not exist before.");
		}
	}
	
}