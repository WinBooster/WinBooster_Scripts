using System;
using System.Text;
using System.Text.RegularExpressions;
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
using WinBoosterNative;
using HandyControl.Data;
using HandyControl.Controls;
using HandyControl.Tools.Extension;

public class Script : IScript
{
	
	public override string GetScriptName() 
	{
		return "Maximum electrical circuit";
	}
	
	public List<Tuple<string, string, bool>> ListSchemes()
    {
		var proc = new Process 
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "cmd.exe",
				Arguments = "/C powercfg /L",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.Default
			}
		};
		
		
        var list = new List<Tuple<string, string, bool>>();
        proc.Start();
		while (!proc.StandardOutput.EndOfStream)
		{
			string text = proc.StandardOutput.ReadLine();
			string text1 = text;
            if (!string.IsNullOrEmpty(GetSchemeID(text1)))
            {
                text1 = text1.Replace(" (", "&");
                text1 = text1.Replace(")", "&");
                string type = Regex.Match(text1, "&(.*)&").Groups[1].Value;
                string id = GetSchemeID(text1);
                if (text1.Contains("*"))
                {
                    list.Add(new Tuple<string, string, bool>(id, type, true));
                }
                else
                {
                    list.Add(new Tuple<string, string, bool>(id, type, false));
                }
            }
		}
        return list;
    }
	
	private string GetSchemeID(List<string> cmdtext)
    {
        string id = "";
        foreach (string text in cmdtext)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = GetSchemeID(text);
            }
        }
        return id;
    }
	
	private string GetSchemeID(string text)
    {
        string text1 = text;
        if (!string.IsNullOrEmpty(text1))
        {
            if (text1.Contains("GUID"))
            {
                text1 = text1.Replace(" (", "&");
                text1 = text1.Replace(")", "&");
                string id = Regex.Match(text1, "GUID схемы питания: (.*) &").Groups[1].Value;
				if (String.IsNullOrEmpty(id)) {
					id = Regex.Match(text1, "GUID �奬� ��⠭��: (.*) &").Groups[1].Value;
				}
                return id;
            }
        }
        return "";
    }
	
	 public string CreateMaximum()
    {
        List<string> cmdtext = new ProcessUtils().StartCmd("chcp 1251 & powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61");
        return GetSchemeID(cmdtext);
    }
    /* Установка активной схемы */
    public void SetActiv(string id)
    {
        new ProcessUtils().StartCmd("chcp 1251 & powercfg -SETACTIVE " + id);
    }
    /* Удаление схема */
    public void Delete(string id)
    {
        new ProcessUtils().StartCmd("chcp 1251 & powercfg /d " + id);
    }
    /* Включение и выключение */
    public void Enable(bool on)
    {
        List<Tuple<string, string, bool>> schems = ListSchemes();
        if (on)
        {
            bool find = false;
            foreach (var s in schems)
            {
                if (s.Item2 == "Максимальная производительность" || s.Item2 == "���ᨬ��쭠� �ந�����⥫쭮���")
                {
                    find = true;
                    SetActiv(s.Item1);
                    break;
                }
            }
            if (!find)
            {
                string id = CreateMaximum();
                if (!string.IsNullOrEmpty(id))
                {
                    SetActiv(id);
                }
            }
        }
        else
        {
            string standart = "";
            foreach (var s in schems)
            {
                if (s.Item2 == "Сбалансированная" || s.Item2 == "�������஢�����")
                {
                    SetActiv(s.Item1);
                    standart = s.Item1;
                }
            }
            foreach (var s in schems)
            {
                if (s.Item2 == "Максимальная производительность" || s.Item2 == "���ᨬ��쭠� �ந�����⥫쭮���")
                {
                    if (!string.IsNullOrEmpty(standart))
                    {
                        SetActiv(standart);
                    }
                    Delete(s.Item1);
                }
            }
        }
    }

    public override void OnEnabled() 
	{
		OptimizeForm optimizeForm = App.auth.main.optimizeForm;
		
		bool enabled = false;
        List<Tuple<string, string, bool>> schems = ListSchemes();
        foreach (var s in schems)
        {
            if (!s.Item3 && (s.Item2 == "Максимальная производительность" || s.Item2 == "���ᨬ��쭠� �ந�����⥫쭮���"))
            {
                Delete(s.Item1);
            }
        }

        schems = ListSchemes();
        foreach (var s in schems)
        {
            if (s.Item3 && (s.Item2 == "Максимальная производительность" || s.Item2 == "���ᨬ��쭠� �ந�����⥫쭮���"))
            {
                enabled = true;
            }
        }
			
		optimizeForm.Dispatcher.Invoke(() =>
		{
			var settings_find = optimizeForm.FindName("optimizationSettings");
			if (settings_find != null && settings_find.GetType() == typeof(StackPanel)) {
				StackPanel settings = (StackPanel)settings_find;
			
				StackPanel stack = new StackPanel();
				stack.Margin = new Thickness(5);
				
				CheckBox checkBox = new CheckBox();
				checkBox.IsChecked = enabled;
				checkBox.Checked += (a, b) => {
	
				};
				checkBox.Unchecked += (a, b) => {
	
				};
				
				TextBlock text = new TextBlock();
				text.Text = "Maximum electrical circuit";
				
				checkBox.Content = text;
				
				stack.Children.Add(checkBox);
				settings.Children.Add(stack);
			}
		});
	}
}