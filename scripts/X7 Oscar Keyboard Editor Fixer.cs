using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinBooster_WPF.ScriptAPI;
using WinBoosterNative.database.cleaner;
using WinBoosterNative;
using WinBoosterNative.winapi;
using Microsoft.Win32;
using System.Windows.Controls;
using WinBooster_WPF;
using WinBooster_WPF.Forms;
using System.Windows;
using System.Net;
using HandyControl.Data;
using HandyControl.Controls;
using System.Runtime.InteropServices;
using WinBoosterNative.injector;
using System.Diagnostics;
using WinBoosterNative.database.error_fix;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

public class Script : IScript
{
	public override string GetScriptName() 
	{
		return "X7 Oscar Keyboard Editor Fixer";
	}
	
	public override void OnEnabled() 
	{
		
	}
	public override void OnErrorFixerInit(ErrorFixDataBase dataBase) 
	{
		dataBase.workers.Add(new OscarKeyboardFixer());
	}
	
	public class OscarKeyboardFixer : IErrorFixerWorker
	{
		string dir = @"C:\Program Files (x86)\X7 Oscar Keyboard Editor";
		public string GetName()
		{
			return "Not saving macros in Oscar Keyboard Editor";
		}
		
		public bool IsAvalible()
		{
			if (Directory.Exists(dir)) {
				if (Directory.Exists(dir + @"\ScriptsMacros")) {
					if (Directory.Exists(dir + @"\ScriptsMacros\Russian")) {
						if (Directory.Exists(dir + @"\ScriptsMacros\Russian\StandardFile")) {
							if (!File.Exists(dir + @"\ScriptsMacros\Russian\StandardFile\Макрос.amc")) {
								return true;
							}
							if (!File.Exists(dir + @"\ScriptsMacros\Russian\StandardFile\Макро.amc")) {
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public bool TryFix()
		{
			if (Directory.Exists(dir)) {
				using (WebClient wc = new WebClient()) {
					string file = wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/refs/heads/main/files/%D0%9C%D0%B0%D0%BA%D1%80%D0%BE%D1%81.amc");
					
					if (!File.Exists(dir + @"\ScriptsMacros\Russian\StandardFile\Макрос.amc")) {
						using (var sw  = new StreamWriter(File.Open(dir + @"\ScriptsMacros\Russian\StandardFile\Макрос.amc", FileMode.CreateNew), Encoding.GetEncoding("utf-32"))) {
							sw.WriteLine(file);             
						}
					}
					if (!File.Exists(dir + @"\ScriptsMacros\Russian\StandardFile\Макро.amc")) {
						using (var sw  = new StreamWriter(File.Open(dir + @"\ScriptsMacros\Russian\StandardFile\Макро.amc", FileMode.CreateNew), Encoding.GetEncoding("utf-32"))) {
							sw.WriteLine(file);             
						}
					}
				}
				return true;
			}
			return false;
		}
	}
}
