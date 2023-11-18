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

public class Script : IScript
{
	public override string GetScriptName() 
	{
		return "Process Screen Protector";
	}
	
	//[DllImport("kernel32.dll", SetLastError = true)]
	//[return: MarshalAs(UnmanagedType.Bool)]
	//static extern bool AllocConsole();
		
    public override void OnEnabled() 
	{
	
		
		//AllocConsole();
		
		if (!Directory.Exists("C:\\Program Files\\WinBooster\\Libs"))
		{
			Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Libs");
		}
		
		using (WebClient wc = new WebClient())
		{
			if (!File.Exists("C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x32.dll")) {
				wc.DownloadFile("https://github.com/WinBooster/WinBooster_Scripts/raw/main/libs/Process%20Screen%20Protector/CaptureDisabler%20x32.dll", "C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x32.dll");
			}
			if (!File.Exists("C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll")) {
				wc.DownloadFile("https://github.com/WinBooster/WinBooster_Scripts/raw/main/libs/Process%20Screen%20Protector/CaptureDisabler%20x64.dll", "C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll");
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
				text.Width = 217;
				text.HorizontalAlignment = HorizontalAlignment.Left;
				
				Button hide = new Button();
				hide.Margin = new Thickness(0, 0, 7, 0);
				hide.SetValue(Grid.RowProperty, 0);
				hide.SetValue(Grid.ColumnProperty, 1);
				hide.HorizontalAlignment = HorizontalAlignment.Right;
				TextBlock button_text = new TextBlock();
				button_text.Text = "Hide";
				
				hide.Content = button_text;
				
				hide.Click += ((a, b) => {
					string process_name = text.Text;
					var processes = System.Diagnostics.Process.GetProcessesByName(process_name);
					DllInjector injector = new DllInjector();
					foreach (System.Diagnostics.Process process in processes) {
						if (process.MainWindowHandle != IntPtr.Zero) {
							try {
								bool x64 = process.Is64Bit();
								if (x64) {
									bool success = injector.Inject(process.Id, "C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x64.dll");
									if (success) {
										GrowlInfo growl = new GrowlInfo
										{
											Message = "Success hide process x64: " + process_name,
											ShowDateTime = true,
										};
										Growl.InfoGlobal(growl);
									}
								}
								else {
									bool success = injector.Inject(process.Id, "C:\\Program Files\\WinBooster\\Libs\\CaptureDisabler_x32.dll");
									if (success) {
										GrowlInfo growl = new GrowlInfo
										{
											Message = "Success hide process x32: " + process_name,
											ShowDateTime = true,
										};
										Growl.InfoGlobal(growl);
									}
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
				
				stack.Children.Add(grid);
				
				settings.Children.Add(stack);
			}
		});
	}
}