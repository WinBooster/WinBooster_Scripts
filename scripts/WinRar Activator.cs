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
		return "WinRar Activator";
	}
	
	public bool IsDisabled() {
		if (!File.Exists(@"C:\Program Files\WinRAR\rarreg.key")) {
			return false;
		}
		return true;
	}
	
	public override void OnEnabled() 
	{
		bool disabled = IsDisabled();
		
		OptimizeForm optimizeForm = App.auth.main.optimizeForm;
		optimizeForm.Dispatcher.Invoke(() =>
		{
			var settings_find = optimizeForm.FindName("optimizationSettings");
			if (settings_find != null && settings_find.GetType() == typeof(StackPanel)) {
				StackPanel settings = (StackPanel)settings_find;
			
				StackPanel stack = new StackPanel();
				stack.Margin = new Thickness(5);
				
				CheckBox checkBox = new CheckBox();
				checkBox.IsChecked = disabled;
				checkBox.Checked += (a, b) => {
					using (WebClient wc = new WebClient()) {
						wc.DownloadFile("https://github.com/WinBooster/WinBooster_Scripts/raw/refs/heads/main/files/rarreg.key", @"C:\Program Files\WinRAR\rarreg.key");
					}
					
					IsDisabled();
				};
				checkBox.Unchecked += (a, b) => {
					if (File.Exists(@"C:\Program Files\WinRAR\rarreg.key")) {
						try { File.Delete(@"C:\Program Files\WinRAR\rarreg.key"); } catch { }
					}
					
					IsDisabled();
				};
				
				TextBlock text = new TextBlock();
				text.Text = "Activate WinRar";
				
				checkBox.Content = text;
				
				stack.Children.Add(checkBox);
				settings.Children.Add(stack);
			}
		});
	}
	
	
}
