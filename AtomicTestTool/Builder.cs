/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
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
                // get the commands from YAML file
                string atomicTestNum = "T1016";
                int atomicTestSubNum = 2;

                PowerShell ps = PowerShell.Create();
                ps.AddCommand("Get-AtomicTechnique").AddParameter("Path", $@"C:\Users\asmith\Documents\code\atomic-red-team\atomics\{atomicTestNum}\{atomicTestNum}.yaml");

                dynamic technique = ps.Invoke()[0];
                var atomic_tests = technique.atomic_tests;
                var my_test = atomic_tests[atomicTestSubNum];
                var my_executor = my_test.executor.name;
                var my_commands = my_test.executor.command;
                String exec_command = "";

                if(my_executor == "command_prompt")
                {
                    exec_command = my_commands.Replace("\n", " & "); // replace newlines with ampersand
                }
                else if (my_executor == "powershell")
                {
                    exec_command = Regex.Replace(my_commands, "(?<!;)\n", "; "); // add semicolon to end of line unless already has one
                }

                // directory of AtomicTestTool
                var dir = ((Directory.GetParent(Directory.GetCurrentDirectory())).Parent).FullName;

                var file = File.ReadAllText(dir + "\\Program.cs");

                // handle commands with double quotes in them, then replace into program.cs
                var escaped_command = exec_command.Replace("\"", "\\\"");
                var file2 = file.Replace("replace-here", escaped_command);
                file2 = file2.Replace("the-executor", my_executor);

                var exportFilePath = (dir + "\\Program2.cs");

                // export the file into exportFilePath
                File.WriteAllText(exportFilePath, file2);

                // cmd / csc.exe/ compile program.cs into art.exe
                string compileCSC = $@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc /reference:C:\Windows\assembly\GAC_MSIL\System.Management.Automation\1.0.0.0__31bf3856ad364e35\system.management.automation.dll /out:{dir}\art.exe {exportFilePath}";

                using (Process process3 = new Process())
                {
                    process3.StartInfo.CreateNoWindow = true;
                    process3.StartInfo.UseShellExecute = false;
                    process3.StartInfo.RedirectStandardOutput = true;
                    process3.StartInfo.RedirectStandardError = true;
                    process3.StartInfo.RedirectStandardInput = true;
                    process3.StartInfo.FileName = "cmd.exe";
                    process3.StartInfo.Arguments = "/C " + compileCSC;
                    process3.Start();
                    process3.StandardInput.Close();
                    process3.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
