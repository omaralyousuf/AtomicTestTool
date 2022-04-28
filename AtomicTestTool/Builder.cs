/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace AtomicTestTool
{
    internal class Builder
    {
        public static void Main()
        {
            try
            {
                // get Atomic from YAML files
                string[] atomicNum = {"T1016-0", "T1016-1"};
                
                var my_executor = "";
                var my_commands = "";
                ArrayList executorArrayList = new ArrayList();
                ArrayList commandsArrayList = new ArrayList();

                foreach (var i in atomicNum)
                { 
                    string atomicTestNum = i.Substring(0, i.IndexOf('-'));
                    int atomicTechniqueNum = int.Parse(i.Substring(i.LastIndexOf('-') + 1));

                    PowerShell ps = PowerShell.Create();
                    ps.AddCommand("Get-AtomicTechnique")
                      .AddParameter("Path", $@"C:\AtomicRedTeam\atomics\{atomicTestNum}\{atomicTestNum}.yaml");

                    dynamic technique = ps.Invoke()[0];
                    var atomic_tests = technique.atomic_tests;
                    var my_test = atomic_tests[atomicTechniqueNum];
                    
                    my_executor = my_test.executor.name;
                    executorArrayList.Add(my_executor); //Add executer to an ArrayList

                    my_commands = my_test.executor.command;
                    commandsArrayList.Add(my_commands); //Add commands to an ArrayList

                }

                ArrayList exec_command = new ArrayList();

                foreach (string i in executorArrayList)
                {
                    foreach (string j in commandsArrayList)
                    {
                        if (i == "command_prompt")
                        {
                            // replace newlines with ampersand and handle commands with double quotes in them
                            exec_command.Add(j.ToString().Replace("\n", " & ").Replace("\"", "\\\""));
                        }
                        
                        else if (i == "powershell")
                        {
                            exec_command.Add(j);
                            // add semicolon to end of line unless already has one and handle commands with double quotes in them
                            Regex.Replace(j.ToString(), "(?<!;)\n", "; ").Replace("\"", "\\\"");
                        }

                    }
                }
                
                foreach(var exec in exec_command)
                {
                    Console.WriteLine(exec);
                }

                // directory of AtomicTestTool
                var dir = ((Directory.GetParent(Directory.GetCurrentDirectory())).Parent).FullName;
                var file = File.ReadAllText(dir + "\\Program.cs");

                //Convert ArrayLists into string
                String execString = "";
                foreach (String exec in executorArrayList)
                {
                   execString = exec.ToString();
                }
                //Console.WriteLine(execString);

                String commandsString = "";
                foreach (String cmd in exec_command)
                {
                    commandsString = commandsString + cmd.ToString();
                }
                //Console.WriteLine(commandsString);

                var file2 = file.Replace("replace-here", commandsString);
                file2 = file2.Replace("the-executor", execString);
                
                var exportFilePath = (dir + "\\Program2.cs");

                // export the file into exportFilePath
                File.WriteAllText(exportFilePath, file2);

                // cmd / csc.exe/ compile program.cs into art.exe
                string compileCSC = $@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc /reference:C:\Windows\assembly\GAC_MSIL\System.Management.Automation\1.0.0.0__31bf3856ad364e35\system.management.automation.dll /out:{dir}\art.exe {exportFilePath}";

                using (Process process = new Process())
                {
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C " + compileCSC;
                    process.Start();
                    process.StandardInput.Close();
                    process.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
