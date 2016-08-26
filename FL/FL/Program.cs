/*
 * Created by SharpDevelop.
 * User: Снежана
 * Date: 21.08.2016
 * Time: 19:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace FL
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            using (var mutex = new Mutex(false, "fl"))
            {
                if (mutex.WaitOne(TimeSpan.FromSeconds(3))) 
                    Application.Run(new MainForm(args));
                else
                {
                   int argsLength = args.Length;
                   if (argsLength != 0)
                   {
                       if (args[argsLength - 1] == "-exit")
                       {
                           Process current = Process.GetCurrentProcess();
                           Process[] pr = Process.GetProcessesByName(current.ProcessName);
                           foreach (Process i in pr)
                           {
                               if (i.Id != current.Id)
                               {
                                   if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                                   {
                                       i.Kill();
                                   }
                               }
                           }
                       }
                   }
                }
            }
		}
		
	}
}
