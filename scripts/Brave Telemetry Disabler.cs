using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using WinBooster_WPF.ScriptAPI;
using WinBooster_WPF;
using WinBooster_WPF.Forms;
using HandyControl.Data;
using HandyControl.Controls;
using HandyControl.Tools.Extension;

public class Script : IScript
{
	public override string GetScriptName() 
	{
		return "Brave Telemetry Disabler";
	}
	
	private List<KeyValuePair<string, string>> hosts = new List<KeyValuePair<string, string>>();
	private List<string> lines = new List<string>();
	
	private bool IsDisabled() 
	{
		lines.Clear();
		hosts.Clear();
		using (StreamReader streamReader = new StreamReader(@"C:\Windows\System32\drivers\etc\hosts", Encoding.UTF8))
		{
			string line = null;
			while ((line = streamReader.ReadLine()) != null)
			{
				lines.Add(line);
				if (!line.StartsWith("#") && !line.IsNullOrEmpty()) {
					var splited = line.Split(" ");
					if (splited.Length == 2) {
						string ip = splited[0];
						string domain = splited[1];
						
						//bool contains = hosts.ToArray().Any((a) => a.Key == ip && a.Value == domain);
						hosts.Add(new KeyValuePair<string, string>(ip, domain));
					}
				}
			}
		}
		bool contains_static = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "static.brave.com");
		bool contains_crlsets = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "crlsets.brave.com");
		bool contains_core = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "brave-core-ext.s3.brave.com");
		bool contains_static1 = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "static1.brave.com");
		bool contains_laptop = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "laptop-updates.brave.com");
		bool contains_variations = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "variations.brave.com");
		bool contains_grant = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "grant.rewards.brave.com");
		bool contains_api = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "api.rewards.brave.com");
		bool contains_rewards = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "rewards.brave.com");
		bool contains_p3a = hosts.ToArray().Any((a) => a.Key == "0.0.0.0" && a.Value == "p3a.brave.com");
		return contains_static && contains_crlsets && contains_core && contains_static1 && contains_laptop && contains_variations && contains_grant && contains_api && contains_rewards && contains_p3a;
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
					lines.Add("0.0.0.0" + " " + "static.brave.com");
					lines.Add("0.0.0.0" + " " + "crlsets.brave.com");
					lines.Add("0.0.0.0" + " " + "brave-core-ext.s3.brave.com");
					lines.Add("0.0.0.0" + " " + "static1.brave.com");
					lines.Add("0.0.0.0" + " " + "laptop-updates.brave.com");
					lines.Add("0.0.0.0" + " " + "variations.brave.com");
					lines.Add("0.0.0.0" + " " + "grant.rewards.brave.com");
					lines.Add("0.0.0.0" + " " + "api.rewards.brave.com");
					lines.Add("0.0.0.0" + " " + "rewards.brave.com");
					lines.Add("0.0.0.0" + " " + "p3a.brave.com");
					
					File.WriteAllLines(@"C:\Windows\System32\drivers\etc\hosts", lines);
					
					IsDisabled();
				};
				checkBox.Unchecked += (a, b) => {
					using (StreamReader streamReader = new StreamReader(@"C:\Windows\System32\drivers\etc\hosts", Encoding.UTF8))
					{
						int index = 0;
						string line = null;
						while ((line = streamReader.ReadLine()) != null)
						{
							var splited = line.Split(" ");
							if (splited.Length == 2) {
								string ip = splited[0].Trim();
								string domain = splited[1].Trim();
								
								if (lines[index] != null) {
									if (domain == "static.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "crlsets.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "brave-core-ext.s3.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "static1.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "laptop-updates.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "variations.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "grant.rewards.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "api.rewards.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "rewards.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
									else if (domain == "p3a.brave.com") {
										lines.RemoveAt(index);
										index--;
									}
								}
							}
							
							
							index++;
						}
					}
					
					File.WriteAllLines(@"C:\Windows\System32\drivers\etc\hosts", lines);
					
					IsDisabled();
				};
				
				TextBlock text = new TextBlock();
				text.Text = "Disable Brave Telemetry";
				
				checkBox.Content = text;
				
				stack.Children.Add(checkBox);
				settings.Children.Add(stack);
			}
		});
	}
}