/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using YamlDotNet.Serialization.NamingConventions;

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
                    //Method 1 that runs attacks using cmd.exe
                    
                    // Atomic attack commands (Change the below commands as needed)
                    string[] cmdCommands = { 
                        "ipconfig /all",
                        "netsh interface show interface",
                        "arp -a",
                        "nbtstat -n",
                        "net config"
                    };

                    string[] psCommands = {
                    "New-Item -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Feature'",
                    "New-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Feature' -name 'TamperData' -value 0"
                    };

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
                            strCommand = strCommand + cmdCommands[command] + " & ";
                        }
                        process.StartInfo.Arguments = "/C " + strCommand;
                        process.Start();
                        process.StandardInput.Close();
                        process.WaitForExit();
                        Console.WriteLine(process.StandardOutput.ReadToEnd());
                        Console.ReadKey();
                    }

                    if (psCommands.Length > 0)
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
                            strCommand = strCommand + psCommands[command] + " & ";
                        }
                        process.StartInfo.Arguments = "Powershell -Command " + strCommand;
                        process.Start();
                        process.StandardInput.Close();
                        process.WaitForExit();
                        Console.WriteLine(process.StandardOutput.ReadToEnd());
                        Console.ReadKey();
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
