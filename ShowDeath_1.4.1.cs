using System;
using System.Collections;
using System.Collections.Generic;

public class CPHInline
{	
	public static string currentGame;
	public static int counter;
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
		//Get messages
		string inListMessage = args.ContainsKey("inListMessage") ? args["inListMessage"].ToString() : "Message Response not set correctly. Check previous message arguments.";
		string notInListMessage = args.ContainsKey("notInListMessage") ? args["notInListMessage"].ToString() : "Message Response not set correctly. Check previous message arguments.";
		
		//Get OBS No Counter Message
		string obsNoCounterMessage = args.ContainsKey("obsNoCounterMessage") ? args["obsNoCounterMessage"].ToString() : "No DeathCounter";

		//Check if this is first time setup or not
		Hashtable checkInfo = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info");
		if(checkInfo == null){
			CPH.RunAction("pwnC_UpdateStream",true);
		}
		//Get current Game in Hashtable
		Hashtable infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info",true);
		currentGame = (string) infoList["currentGame"];
		
		//Get list of games that have counters
		Dictionary<string,int> gameList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games",true);
		bool inList = false;
		//Check if the currentgame is in the list
		inList = gameList.ContainsKey(currentGame);

		if(inList){
			counter = gameList[currentGame];
			CPH.SendMessage(ReplaceArgs(inListMessage,inList));
			//Set argument of deathCounter to further use in subactions if needed.
			CPH.SetArgument("deathCounter",counter);
			
		}else{
			CPH.SendMessage(ReplaceArgs(notInListMessage,inList));
			
			/*

				Change "No DeathCounter" to whatever you want the OBS Text source to display if game is not a counter.
			
			*/
			CPH.SetArgument("deathCounter",obsNoCounterMessage);
		}

		//Set argument of game to further use in subactions if needed.
		CPH.SetArgument("game",currentGame);

		return true;
	}

	private string ReplaceArgs(string oldMessage,bool inList)
	{
		//Replace %game% with the current game
		string newMessage = oldMessage.Replace("%game%",currentGame.ToString());
		//Replace %deathCounter% with the counter
		if(inList){
			newMessage = newMessage.Replace("%deathCounter%",counter.ToString());
		}
		//return the new string where %deathCounter% is replaced.
		return newMessage;
	}
}
