using System;
using System.Collections;
using System.Collections.Generic;

public class CPHInline
{
    public static int gameCounter, totalDeaths;
    public static Dictionary<string, int> gameList;
    public static Hashtable infoList;
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
        //Initiate newCount for output<int> incase deathset input is numeric
        int newCount;
        //Get Game,used command, death counter of current game and total deaths counter
        var game = args["game"].ToString();
        var action = args["pC_Modify"].ToString();
        string inList = args["inList"].ToString();
        //Get game dictionary and hashtable
        gameList = CPH.GetGlobalVar<Dictionary<string, int>>("pwnedCounter_Games", true);
        infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info", true);
        gameCounter = CPH.GetGlobalVar<int>("death_" + game, true);
        totalDeaths = CPH.GetGlobalVar<int>("total_Deaths", true);
        //Get Commands used. This will be used in case custom command names have been used and set in the arguments.
        string addDeath = args.ContainsKey("addDeath") ? args["addDeath"].ToString() : "add";
        string remDeath = args.ContainsKey("remDeath") ? args["remDeath"].ToString() : "!death-";
        string setDeath = args.ContainsKey("setDeath") ? args["setDeath"].ToString() : "!deathset";
        string resetDeath = args.ContainsKey("resetDeath") ? args["resetDeath"].ToString() : "!deathreset";
        //Getting Death Messages from set arguments. If a custom death message (add,rem,set,reset) are not set, revert to the default
        string defaultDeathMessage = args.ContainsKey("defaultDeathMessage") ? args["defaultDeathMessage"].ToString() : "Message Response not set correctly. Death Counter was still modified.";
        string addDeathMessage = args.ContainsKey("addDeathMessage") ? args["addDeathMessage"].ToString() : defaultDeathMessage;
        string remDeathMessage = args.ContainsKey("remDeathMessage") ? args["remDeathMessage"].ToString() : defaultDeathMessage;
        string setDeathMessage = args.ContainsKey("setDeathMessage") ? args["setDeathMessage"].ToString() : defaultDeathMessage;
        string resetDeathMessage = args.ContainsKey("resetDeathMessage") ? args["resetDeathMessage"].ToString() : defaultDeathMessage;
        //Getting error message when input of setting the counter is not numeric
        string setErrorMessage = args.ContainsKey("setDeathError") ? args["setDeathError"].ToString() : "Input was not numeric.";
		string setErrorMinusMessage = args.ContainsKey("setDeathMinusError") ? args["setDeathMinusError"].ToString() : "Input should be >=0";

        //SafeGuard with inList
        if (inList == "true")
        {
            //First Check if command to add death was used
            if (action.Equals("add"))
            {
                Death_Add(game, addDeathMessage);
            }
            //Check if command to remove death was used
            else if (action.Equals("rem"))
            {
                Death_Rem(game, remDeathMessage);
            }
            //Check if command to set death was used
            else if (action.Equals("set"))
            {
                //Check if the input that was used in deathset is numeric or not.
                bool isNumeric = int.TryParse(args["rawInput"].ToString(), out newCount);
                if (isNumeric)
                {
					if(newCount>=0){
						//Was numeric Death_Set() can be used
						Death_Set(game, newCount, setDeathMessage);
					}else{
						CPH.SendMessage(setErrorMinusMessage);
					}

                }
                else
                {
                    //Was NOT numeric, give response of input not being numeric.
                    CPH.SendMessage(setErrorMessage);
                    //Send Log Information
                    CPH.LogInfo("[pwned Counter] - Input was not numeric.");
                }
            }
            //Check if command to reset death was used
            else if (action.Equals("reset"))
            {
                Death_Reset(game, resetDeathMessage);
            }
        }else{
			CPH.SendMessage("Game not in death counter list. Commands should have been disabled.");
			CPH.LogInfo("[pwned Counter] - SafeGuard to prevent from accidently having commands enabled but current game is not in death counter list. Will run pwnC_Update_Stream to update Commands.");
			CPH.RunAction("pwnC_UpdateStream");
		}

        return true;
    }

    //Method for incrementing the death counter. Sending out the message out later on.
    private bool Death_Add(string game, string message)
    {
        //Increment Game counter
        gameList[game]++;
        int newCounter = gameList[game];
        //Set global variable of game to the new counter
        CPH.SetGlobalVar("pwnedCounter_Games", gameList, true);
        //Increment totalDeaths
        int newTotal = Convert.ToInt32(infoList["totalDeaths"]);
        newTotal++;
        infoList["totalDeaths"] = newTotal;
        //Set global variable of total deaths to the new value
        CPH.SetGlobalVar("pwnedCounter_Info", infoList, true);
        CPH.LogInfo("[pwned Counter] - Counter of Game " + game + " was set to " + newCounter + ".");
        //Set deathCounter argument for potential subactions
        CPH.SetArgument("deathCounter", newCounter);
        //Replace %deathCounter% in message with the actual value
        string output = ReplaceCounterVar(message, newCounter);
        //Send message to Twitch
        CPH.SendMessage(output);
        return true;
    }

    private bool Death_Rem(string game, string message)
    {
        int oldCounter = gameList[game];
        int newCounter = 0;
        if (oldCounter > 0)
        {
            //Decrement newCounter
            gameList[game]--;
            newCounter = gameList[game];
            //Set global variable of game to the new counter
            CPH.SetGlobalVar("pwnedCounter_Games", gameList, true);
            //decrement totalDeaths
            int newTotal = Convert.ToInt32(infoList["totalDeaths"]);
            newTotal--;
            infoList["totalDeaths"] = newTotal;
            //Set global variable of total deaths to the new value
            CPH.SetGlobalVar("pwnedCounter_Info", infoList, true);
        }

        CPH.LogInfo("[pwned Counter] - Counter of Game " + game + " was set to " + newCounter + ".");
        //Set deathCounter argument for potential subactions
        CPH.SetArgument("deathCounter", newCounter);
        //Replace %deathCounter% in message with the actual value
        string output = ReplaceCounterVar(message, newCounter);
        //Send message to Twitch
        CPH.SendMessage(output);
        return true;
    }

    private bool Death_Set(string game, int newCounter, string message)
    {
        //Set global variable of game to the new counter
        gameList[game] = newCounter;
        CPH.SetGlobalVar("pwnedCounter_Games", gameList, true);
        //Sum gamelist value for new totalDeaths
        int gameSum = 0;
        foreach (KeyValuePair<string, int> entry in gameList)
        {
            gameSum = gameSum + entry.Value;
        }

        //Set Hashtable value of totalDeaths and save to globals
        infoList["totalDeaths"] = gameSum;
        CPH.SetGlobalVar("pwnedCounter_Info", infoList, true);
        CPH.LogInfo("[pwned Counter] - Counter of Game " + game + " was set to " + newCounter + ".");
        //Set deathCounter argument for potential subactions
        CPH.SetArgument("deathCounter", newCounter);
        //Replace %deathCounter% in message with the actual value
        string output = ReplaceCounterVar(message, newCounter);
        //Send message to Twitch
        CPH.SendMessage(output);

        return true;
    }

    private bool Death_Reset(string game, string message)
    {
        //Set total deaths by subtracting old counter of game from total deaths
        gameList[game] = 0;
        //Set global variable of total deaths to the new value
        CPH.SetGlobalVar("pwnedCounter_Games", gameList, true);
        //Sum gamelist value for new totalDeaths
        int gameSum = 0;
        foreach (KeyValuePair<string, int> entry in gameList)
        {
            gameSum = gameSum + entry.Value;
        }

        //Set Hashtable value of totalDeaths and save to globals
        infoList["totalDeaths"] = gameSum;
        CPH.SetGlobalVar("pwnedCounter_Info", infoList, true);
        CPH.LogInfo("[pwned Counter] - Counter of Game " + game + " was set to 0.");
        //Set deathCounter argument for potential subactions
        CPH.SetArgument("deathCounter", 0);
        //Replace %deathCounter% in message with the actual value
        string output = ReplaceCounterVar(message, 0);
        //Send message to Twitch
        CPH.SendMessage(output);
        return true;
    }

    private string ReplaceCounterVar(string oldMessage, int counter)
    {
        //Replace %deathCounter% with the counter
        string newMessage = oldMessage.Replace("%deathCounter%", counter.ToString());
        //return the new string where %deathCounter% is replaced.
        return newMessage;
    }
}