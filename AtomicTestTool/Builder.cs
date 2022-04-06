/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtomicTestTool
{
    internal class Builder
    {
        public static void Main()
        {
            string atomicTestNum = "T1016";

            string tests =
            "(Get-AtomicTechnique -Path C:\\atomicredteam\\atomics\\" 
            + atomicTestNum + "\\" + atomicTestNum + ".yaml).atomic_tests[0].executor.command";

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
            string cmd = string.Join("\n", line);
            
            //Psudo code
            //atomic_test ="T1016-1"
            //$cmds = (GetAtomic-Technique....).atomic-tests[0]
            //    replace <<cmd_placeholder>> int Program.cs
            //    cmd / csc.exe/ compile program.cs into art.exe

        }
    }

}
