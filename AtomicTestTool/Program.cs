/* **
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Diagnostics;

namespace AtomicTestTool
{
    internal class Program
    {
        public static void Main()
        {
            try
            {
                
                using (Process process = new Process())
                {
                    
                    // Atomic attack commands fed in from builder
                    string[] cmdCommands = {
                    "replace-here"
                    };

                    string[] psCommands = {
                    };

                    //Method 1 that runs attacks using cmd.exe
                    if (cmdCommands.Length > 0)
                    {
                        //Start command prompt process
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.FileName = "cmd.exe";

                        //Loop thru the array of commands
                        string strCommand = "";
                        for (int command = 0; command < cmdCommands.Length; command++)
                        {
                            if (cmdCommands[command] != "")
                            {
                                strCommand = strCommand + cmdCommands[command] + " & ";
                            }
                        }
                        process.StartInfo.Arguments = "/C " + strCommand;
                        process.Start();
                        process.StandardInput.Close();
                        process.WaitForExit();

                        //Uncomment the two line below to show the cmd window
                        //Console.WriteLine(process.StandardOutput.ReadToEnd());
                        //Console.ReadKey();
                    }

                    else
                    {
                        //Method 2 runs attacks using Powershell
                        //Start command prompt process
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.FileName = "cmd.exe";

                        //Loop thru the array of commands
                        string strCommand = "";
                        for (int command = 0; command < psCommands.Length; command++)
                        {
                            if (psCommands[command] != "")
                            {
                                strCommand = strCommand + psCommands[command] + " & ";
                            }
                        }
                        process.StartInfo.Arguments = "/C Powershell -Command " + strCommand;
                        process.Start();
                        process.StandardInput.Close();
                        process.WaitForExit();

                        //Uncomment the two line below to show the cmd window
                        ////Console.WriteLine(process.StandardOutput.ReadToEnd());
                        ////Console.ReadKey();
                    }
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
