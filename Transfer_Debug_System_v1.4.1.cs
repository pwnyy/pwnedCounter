using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        //Get command input
        string input = args["rawInput"].ToString();
        //string input = args.ContainsKey("rawInput") ? args["rawInput"].ToString() : "none";
        input = input.ToLower();
        if (String.Equals(input, "transfer"))
        {
            //Set filepath of deathCategories.txt
            string deathFile = @"data\deathCategories.txt";
            //check if deathCategories.txt exists, if not create file
            if (!File.Exists(deathFile))
            {
                //If file does not exist there is no need to transfer counter to the new system.
                CPH.SendMessage("File was not found. Transfer failed.");
            //Log if file has not existed before, then create file
            }
            else
            {
                //Get old total Deaths
                int oldTotal = CPH.GetGlobalVar<int>("total_Deaths", true);
                //Get old current Game
                string oldCurrentGame = CPH.GetGlobalVar<string>("pc_currentGame", true);
                //Set Hashtable and Dictionary
                Hashtable infoList = new Hashtable();
                Dictionary<string, int> gameList = new Dictionary<string, int>();
                //yRead file to array and convert to list
                string[] lines = File.ReadAllLines(deathFile);
                if (lines.Length > 0)
                {
                    var oldList = new List<string>(lines);
                    //Fill Dictionary with old list values
                    int gameSum = 0;
                    string transferInfo = "";
                    foreach (string oldGame in oldList)
                    {
                        int counter = CPH.GetGlobalVar<int>("death_" + oldGame, true);
                        gameSum = gameSum + counter;
                        gameList.Add(oldGame, counter);
                        transferInfo = transferInfo + " , " + oldGame + " : " + counter;
                        //Unset all death_ variables
                        CPH.UnsetGlobalVar("death_" + oldGame, true);
                    }

                    CPH.LogInfo("[pwned Counter] - Following games and values have been tranfered to the new system: " + transferInfo);
                    //Set Hashtable value of currentGame and total Deaths
                    infoList.Add("currentGame", oldCurrentGame);
                    infoList.Add("totalDeaths", gameSum);
                    //Set Global Variables with Hastable and Dictionary
                    CPH.SetGlobalVar("pwnedCounter_Info", infoList, true);
                    CPH.SetGlobalVar("pwnedCounter_Games", gameList, true);
                    //Unset old globals and delete old file
                    CPH.UnsetGlobalVar("pc_currentGame", true);
                    CPH.UnsetGlobalVar("total_Deaths");
                    File.Delete(deathFile);
                    CPH.LogInfo("[pwned Counter] - Old global variables have been unset and deathCategories.txt has been deleted. pwned Counter System has been updated to use Hashtable and Dictionary.");
                    CPH.SendMessage("pwned Counter transfered old deathcounters to new system.");
                }
                else
                {
                    File.Delete(deathFile);
                    CPH.LogInfo("[pwned Counter] - deathCategories.txt was empty. To prevent overwriting an existing newer system no changes have been made. deathCategories.txt was deleted. ");
                    CPH.SendMessage("As file was empty, no transfer has been made and file has been deleted.");
                }
            }
        }
        else if (String.Equals(input, "debug"))
        {
			
            Hashtable infoList = CPH.GetGlobalVar<Hashtable>("pwnedCounter_Info", true);
			string infoListJson = JsonConvert.SerializeObject(infoList);
            Dictionary<string,int> gameList = CPH.GetGlobalVar<Dictionary<string,int>>("pwnedCounter_Games", true);
			string gameListJson = JsonConvert.SerializeObject(gameList);
            CPH.LogDebug("[pwned Counter] InfoList: " + infoListJson);
            CPH.LogDebug("[pwned Counter] GameList: " + gameListJson);
			CPH.SendMessage("Check your Log File.(Debug Mode should be enabled)");
        }
        else
        {
            CPH.SendMessage("Please use either transfer or debug as input.");
        }

        return true;
    }
}