/* **
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run mutiple Atomic
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
                    String[] cmdCommands = { "ipconfig /all & netsh interface show interface & arp -a & nbtstat -n & net config & schtasks.exe /create /tn mytask /st 00:00 /sc once /f /tr \"powershell.exe Get-Date | Out-File -Append $env:Temp/dates.txt\" & ","netsh advfirewall firewall show rule name=all & ","" };
                    String[] cmdExecutors = { "command_prompt","command_prompt","" };
                    
                    for (int i = 0; i < cmdExecutors.Length; i++)
                    {
                        for (int j = 0; j < cmdCommands.Length; j++)
                        {
                            //Method 1 that runs Atomics using cmd.exe
                            if (cmdExecutors[i] == "command_prompt")
                            {
                                //Start command prompt process
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.UseShellExecute = false;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.RedirectStandardError = true;
                                process.StartInfo.RedirectStandardInput = true;
                                process.StartInfo.FileName = "cmd.exe";
                                process.StartInfo.Arguments = "/C " + cmdCommands[j];
                                process.Start();
                                process.StandardInput.Close();
                                var lines = process.StandardOutput.ReadToEnd();
                                process.WaitForExit();

                                //Show command window in debug mode
                                //#if DEBUG
                                Console.WriteLine(lines);
                                Console.ReadKey();
                                //#endif
                            }

                            //Method 2 that runs Atomics Powershell 
                            //else if (cmdExecutor == "sh")
                            //{
                            //    PowerShell ps = PowerShell.Create();
                            //    ps.AddScript(cmdCommand);

                            //    var retobj = ps.Invoke();

                            //    StringBuilder stringBuilder = new StringBuilder();
                            //    foreach (PSObject obj in retobj)
                            //    {
                            //        stringBuilder.AppendLine(obj.ToString());
                            //    }

                            //    var rez = stringBuilder.ToString();

                            //    //Show command window in debug mode
                            //    //#if DEBUG
                            //    Console.WriteLine(rez);
                            //    Console.ReadKey();
                            //    //#endif
                            //}
                        }
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