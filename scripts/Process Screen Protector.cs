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
using WinBoosterNative.injector.Driver_Exploit;

public class Script : IScript
{
	public override string GetScriptName() 
	{
		return "Process Screen Protector";
	}


    public override void OnEnabled() 
	{
		if (!Directory.Exists("C:\\Program Files\\WinBooster\\Libs"))
		{
			Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Libs");
		}

		using (WebClient wc = new WebClient())
		{
			if (!File.Exists("C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll")) {
				wc.DownloadFile("https://github.com/WinBooster/WinBooster_Scripts/raw/main/libs/Process%20Screen%20Protector/CaptureDisabler%20x64.dll", "C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll");
			}
			if (!File.Exists("C:\\Program Files\\WinBooster\\Libs\\CaptureEnabler_x64.dll")) {
				wc.DownloadFile("https://github.com/WinBooster/WinBooster_Scripts/raw/main/libs/Process%20Screen%20Protector/CaptureEnabler%20x64.dll", "C:\\Program Files\\WinBooster\\Libs\\CaptureEnabler_x64.dll");
			}
		}


		AntiScreenShareForm antiScreenForm = App.auth.main.antiScreenForm;
		antiScreenForm.Dispatcher.Invoke(() =>
		{
			var settings_find = antiScreenForm.FindName("antiScreenShareSettings");
			if (settings_find != null && settings_find.GetType() == typeof(StackPanel)) {
				StackPanel settings = (StackPanel)settings_find;
			
				StackPanel stack = new StackPanel();
				
				Grid grid = new Grid();
				
				for (int i = 0; i > 2; i++)
				{
					var rowDefinition = new RowDefinition();
					rowDefinition.Height = GridLength.Auto;
					grid.RowDefinitions.Add(rowDefinition);
				}
				
				for (int i = 0; i > 2; i++)
				{
					var colDefinition = new ColumnDefinition();
					colDefinition.Width = GridLength.Auto;
					grid.ColumnDefinitions.Add(colDefinition);
				}
				
				HandyControl.Controls.TextBox text = new HandyControl.Controls.TextBox();
				text.Margin = new Thickness(5, 0, 0, 0);
				text.SetValue(Grid.RowProperty, 0);
				text.SetValue(Grid.ColumnProperty, 0);
				text.SetValue(HandyControl.Controls.InfoElement.PlaceholderProperty, "Process name");
				text.Width = 147;
				text.HorizontalAlignment = HorizontalAlignment.Left;
				
				Button hide = new Button();
				hide.Margin = new Thickness(0, 0, 7, 0);
				hide.SetValue(Grid.RowProperty, 0);
				hide.SetValue(Grid.ColumnProperty, 1);
				hide.HorizontalAlignment = HorizontalAlignment.Right;
				
				Thickness margin = hide.Margin;
				margin.Right = 75;
				hide.Margin = margin;
				
				TextBlock button_text = new TextBlock();
				button_text.Text = "Hide";
				
				hide.Content = button_text;
				
				InjectionOptions options = new InjectionOptions();
				
				hide.Click += ((a, b) => {
					string process_name = text.Text;
					var processes = System.Diagnostics.Process.GetProcessesByName(process_name);
					foreach (System.Diagnostics.Process process in processes) {
						if (process.MainWindowHandle != IntPtr.Zero) {
							bool x64 = process.Is64Bit();
							try {
								if (x64) {
									LoadLibraryInjection injector = new LoadLibraryInjection(process, ExecutionType.CreateThread, options);
									
									injector.InjectImage("C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll");
									GrowlInfo growl = new GrowlInfo
									{
										Message = "Success hided process: " + process_name,
									};
									Growl.InfoGlobal(growl);
								}
								else {
									GrowlInfo growl = new GrowlInfo
									{
										Message = "x32 Processes not support",
										ShowDateTime = true,
										IconKey = "WarningGeometry",
										IconBrushKey = "WarningBrush",
										IsCustom = true
									};
									Growl.InfoGlobal(growl);
								}
							}
							catch { 
								GrowlInfo growl = new GrowlInfo
								{
									Message = "Error hide process: " + process_name,
									ShowDateTime = true,
									IconKey = "WarningGeometry",
									IconBrushKey = "WarningBrush",
									IsCustom = true
								};
								Growl.InfoGlobal(growl);
							}
						}
					}
				});
				
				Button unhide = new Button();
				unhide.Margin = new Thickness(0, 0, 7, 0);
				unhide.SetValue(Grid.RowProperty, 0);
				unhide.SetValue(Grid.ColumnProperty, 1);
				unhide.HorizontalAlignment = HorizontalAlignment.Right;
				TextBlock button_text2 = new TextBlock();
				button_text2.Text = "UnHide";
				unhide.Content = button_text2;
				unhide.Click += ((a, b) => {
					string process_name = text.Text;
					var processes = System.Diagnostics.Process.GetProcessesByName(process_name);
					foreach (System.Diagnostics.Process process in processes) {
						if (process.MainWindowHandle != IntPtr.Zero) {
							bool x64 = process.Is64Bit();
							try {
								if (x64) {
									LoadLibraryInjection injector = new LoadLibraryInjection(process, ExecutionType.CreateThread, options);
									
									injector.InjectImage("C:\\Program Files\\WinBooster\\Libs\\CaptureEnabler_x64.dll");
									GrowlInfo growl = new GrowlInfo
									{
										Message = "Success hided process: " + process_name,
									};
									Growl.InfoGlobal(growl);
								}
								else {
									GrowlInfo growl = new GrowlInfo
									{
										Message = "x32 Processes not support",
										ShowDateTime = true,
										IconKey = "WarningGeometry",
										IconBrushKey = "WarningBrush",
										IsCustom = true
									};
									Growl.InfoGlobal(growl);
								}
							}
							catch { 
								GrowlInfo growl = new GrowlInfo
								{
									Message = "Error hide process: " + process_name,
									ShowDateTime = true,
									IconKey = "WarningGeometry",
									IconBrushKey = "WarningBrush",
									IsCustom = true
								};
								Growl.InfoGlobal(growl);
							}
						}
					}
				});
				
				grid.Children.Add(text);
				grid.Children.Add(hide);
				grid.Children.Add(unhide);
				
				stack.Children.Add(grid);
				
				settings.Children.Add(stack);
			}
		});
	}
}
