/* **
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;

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
                    string cmdCommands = "replace-here";
                    String cmdExecutor = "the-executor";

                    //Method 1 that runs Atomics using cmd.exe
                    if (cmdExecutor == "command_prompt")
                    {
                        //Start command prompt process
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/C " + cmdCommands;
                        process.Start();
                        process.StandardInput.Close();
                        var lines = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        //Show command window in debug mode
                        #if DEBUG
                            Console.WriteLine(lines);
                            Console.ReadKey();
                        #endif
                    }

                    //Method 2 that runs Atomics Powershell 
                    else if (cmdExecutor == "powershell")
                    {
                        PowerShell ps = PowerShell.Create();
                        ps.AddScript(cmdCommands);

                        var retobj = ps.Invoke();

                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (PSObject obj in retobj)
                        {
                            stringBuilder.AppendLine(obj.ToString());
                        }

                        var rez = stringBuilder.ToString();

                        //Show command window in debug mode
                        #if DEBUG
                            Console.WriteLine(rez);
                            Console.ReadKey();
                        #endif
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