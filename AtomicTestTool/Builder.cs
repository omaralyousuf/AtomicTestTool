/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AtomicTestTool
{
    internal class Builder
    {
        public static void cmd()
        {

            // Get the commands from YAML file
            string atomicTestNum = "T1016";
            string atomicTestSubNum = "0";

            string tests =
            "(Get-AtomicTechnique -Path C:\\atomicredteam\\atomics\\" 
            + atomicTestNum + "\\" + atomicTestNum + ".yaml).atomic_tests[" + atomicTestSubNum + "].executor.command";

            Process process = new Process();

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C Powershell -Command " + tests;
            process.Start();
            process.StandardInput.Close();
            process.WaitForExit();

            string[] line = new string[10];
            int indexer = 0;

            while (!process.StandardOutput.EndOfStream)
            {
                line[indexer] = process.StandardOutput.ReadLine();
                indexer++; // increment
            }

            string cmd = string.Join("\", \"", line);

            //Console.WriteLine(cmd);
            // var file = FileReader("path-here")
            var file = File.ReadAllText("C:\\users\\oyousuf\\AtomicTestTool\\AtomicTestTool\\Program.cs");

            // file.replace("//replace-here",line-array)
            var file2 = file.Replace("replace-here", cmd);

            // exportFilePath = AnyFilePath\SomeFolderName\anyFilename.cs
            var exportFilePath = ("C:\\Users\\oyousuf\\AtomicTestTool\\AtomicTestTool\\Program2.cs");

            // export the file into exportFilePath
            File.WriteAllText(exportFilePath, file2);

            // csc exportFilePath outputPath
            Process process2 = new Process();
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.RedirectStandardInput = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.Arguments = "/C C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc /out:art.exe C:\\Users\\oyousuf\\AtomicTestTool\\AtomicTestTool\\Program2.cs";
            process2.Start();
            process2.StandardInput.Close();
            process2.WaitForExit();
        }
    }

}


//Psudo code
//atomic_test ="T1016-1"
//$cmds = (GetAtomic-Technique....).atomic-tests[0]
//    replace <<cmd_placeholder>> int Program.cs
//    cmd / csc.exe/ compile program.cs into art.exe