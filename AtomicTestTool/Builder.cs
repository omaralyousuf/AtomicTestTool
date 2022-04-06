/**
 * Author: Omar Al Yousuf
 * Date: 03/21/2022
 * Description: build executables to run hard coded atomic tests
 * **/

using System;
using System.Diagnostics;

namespace AtomicTestTool
{
    internal class Builder
    {
        public static void Main()
        {
            string atomicTestNum = "T1016";

            string[] tests = {
            "Powershell -Command Get-AtomicTechnique -Path C:\\atomicredteam\\atomics\\" + atomicTestNum + "\\" + atomicTestNum + ".yaml",
            "Powershell -Command $a.atomic_tests[0].executor.command"
            };

            Process process = new Process();

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.FileName = "cmd.exe";

            string strTest = "";
            for (int test = 0; test < tests.Length; test++)
            {
                strTest = strTest + tests[test] + " & ";
            }

            process.StartInfo.Arguments = "/C " + strTest;
            process.Start();
            process.StandardInput.Close();
            process.WaitForExit();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            Console.ReadKey();

        }
    }

}
