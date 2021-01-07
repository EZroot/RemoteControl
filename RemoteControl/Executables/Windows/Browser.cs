using RemoteControl.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemoteControl.Executables.Windows
{
    public class Browser
    {
        public static void OpenUrl(string url)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("start " + url);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            ConsoleDisplay.Write(cmd.StandardOutput.ReadToEnd());
        }

        public static void Close()
        {
            foreach (Process process in Process.GetProcesses())
            {
                string pname = process.ProcessName;

                string plower = pname.ToLower();

                string title = process.MainWindowTitle;

                if (plower.Contains("chrome") || plower.Contains("firefox"))
                {
                    Console.WriteLine($"{pname} {title}");
                    process.Kill();
                }
            }
        }
    }
}
