/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            string atomicTestSubNum = "0";

            string tests =
            "(Get-AtomicTechnique -Path C:\\atomicredteam\\atomics\\" 
            + atomicTestNum + "\\" + atomicTestNum + ".yaml).atomic_tests[" + atomicTestSubNum + "].executor.command";

            // start a process to use cmd.exe
            Process process2 = new Process();
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.RedirectStandardInput = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.Arguments = "/C Powershell -Command " + tests;
            process2.Start();
            process2.StandardInput.Close();
            process2.WaitForExit();

            // insert the atomic command into an array
            string[] line = new string[30];
            int indexer = 0;

            while (!process2.StandardOutput.EndOfStream)
            {
                line[indexer] = process2.StandardOutput.ReadLine();
                indexer++;
            }

            // remove empty elements from the array
            List<string> y = line.ToList<string>();
            y.RemoveAll(p => string.IsNullOrEmpty(p));
            line = y.ToArray();

            // Replace " with \" for all array elements in order for cmd to read them
            for (int i = 0; i < line.Length; i++)
            {
                    if (line[i].Contains("\""))
                    {
                        line[i] = line[i].Replace("\"", "\\\"");
                    }
            }

            // Join commands as a string with each one wrapped by double quotes
            string cmd = string.Join("\", \"", line);

            // getting current directory of AtomicTestTool
            var dir = Directory.GetCurrentDirectory();
            string newDir = dir.Substring(0, dir.Length - 9);

            // var file = FileReader("path-here")
            var file = File.ReadAllText(newDir + "\\Program.cs");

            // file.replace("//replace-here",line-array)
            var file2 = file.Replace("-a", cmd);

            // exportFilePath = AnyFilePath\SomeFolderName\anyFilename.cs
            var exportFilePath = (newDir + "\\Program2.cs");

            // export the file into exportFilePath
            File.WriteAllText(exportFilePath, file2);

            // cmd / csc.exe/ compile program.cs into art.exe
            string compileCSC = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc " +
                                "/out:" + newDir + "art.exe " +
                                newDir + "Program2.cs";

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
