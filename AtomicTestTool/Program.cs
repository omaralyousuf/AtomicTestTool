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

                    // Command Line
                    if (strCommand.Length < 40)
                    {
                        process.StartInfo.Arguments = "/C " + strCommand;
                    }

                    // Powershell
                    else
                    {
                    process.StartInfo.Arguments = "/C Powershell -Command " + strCommand;
                    }

                    process.Start();
                    process.StandardInput.Close();
                    var lines = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    //Uncomment the two line below to show the cmd window
                    Console.WriteLine(lines);
                    Console.ReadKey();
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
