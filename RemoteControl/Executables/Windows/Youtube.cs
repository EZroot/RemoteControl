using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using RemoteControl.Display;

namespace RemoteControl.Executables.Windows
{
    public class Youtube
    {
        public static void PlayVideo(string url, bool autoplay)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("start "+url);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            ConsoleDisplay.Write(cmd.StandardOutput.ReadToEnd());
        }
    }
}
