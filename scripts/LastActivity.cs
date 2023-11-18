using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinBooster_WPF.ScriptAPI;
using WinBoosterNative.database.cleaner;
using Microsoft.Win32;

public class Script : IScript
{
	public override string GetScriptName() 
	{
		return "LastActivity Cleaner";
	}
    
	public override void OnCleanerInit(CleanerDataBase dataBase)
	{
		CleanerCategory lastactiv = new CleanerCategory("LastActivity");
		lastactiv.custom.Add(new LastActivityCleaner());
		dataBase.cleaners.Add(lastactiv);
	}
	
	public class SafeNames
	{
		public bool IsSafeName(string text)
        {
            bool safe = false;
            foreach (string name in names)
            {
                if (text.Contains(name))
                {
                    safe = true;
                }
            }
            foreach (string name in files)
            {
                if (text == name)
                {
                    safe = true;
                }
            }
            return safe;
        }
        public List<string> names = new List<string>()
        {
            "JAVAW.EXE",
            "JAVA.EXE",
            "OPERA_AUTOUPDATE.EXE",
            "OPERA.EXE",
            "PICASA3.EXE",
            "STEAM.EXE",
            "STEAMSERVICE.EXE",
            "STEAMWEBHELPER.EXE",
            "DISCORD.EXE",
            "CMD.EXE",
            "CONHOST.EXE",
            "SVCHOST.EXE",
            "TASKMGR.EXE",
            "TASKHOSTW.EXE",
            "SYSTRAY.EXE",
            "SYSTEMSETTINGSBROKER.EXE",
            "TEXTINPUTHOST.EXE",
            "TIWORKER.EXE",

            "rundll32.exe",
            "notepad.exe",
            "Taskmgr.exe",
            "cmd.exe",
            "rundll32.exe",
            "conhost.exe",
            "explorer.exe",
            "notepad++.exe",
            "opera.exe",
            "git.exe",

            "Discord.exe",
            "Telegram.exe",

            "PolyMC.exe",
            "Terraria.exe",
            "HL2.EXE",
            "csgo.exe",

            "regedit.exe",
            "mmc.exe",
            "TCPSVCS.EXE",
            "java.exe",
            "javaw.exe",
            "msiexec.exe",
            "OpenWith.exe",
            "readedWaitDialog.exe",
            "Everything.exe",
            "memreduct.exe",
            "Exodus.exe",
            "Steam.exe",
            "Counter-Strike Global Offensive.exe",
            "hl.exe",
            "hl2.exe",

        };
        public List<string> files = new List<string>()
        {
            "C:\\Windows\\System32\\dwm.exe",
            "C:\\Windows\\hh.exe",
            "C:\\Windows\\System32\\csrss.exe",
            "C:\\Windows\\System32\\ApplicationFrameHost.exe",
            "C:\\Windows\\System32\\WerFault.exe",
            "C:\\Windows\\System32\\wlrmdr.exe",
            "C:\\Windows\\SysWOW64\\WerFault.exe",
            "C:\\Program Files\\WinRAR\\WinRAR.exe",
            "C:\\Windows\\System32\\dllhost.exe",
            "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe",
        };
			
	}
	public class LastActivityCleaner : ICleanerWorker
	{
		public string GetCategory()
        {
            return "LastActivity";
        }
		
		public string GetFolder()
        {
            return "";
        }
        public List<string> GetFolders()
        {
            return new List<string>();
        }
		
		public bool IsAvalible() 
		{
			return true;
		}
		
		public CleanerResult TryDelete()
		{
			CleanerResult result;
            result.bytes = 0;
            result.files = 0;
			
			#region Enigma Virtual Box
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                var enigma_virtual_box = CurrentUserSoftware.OpenSubKey("Enigma Virtual Box", true);
                if (enigma_virtual_box != null)
                {
                    try
                    {
                        var enigma_virtual_box2 = enigma_virtual_box.OpenSubKey("History");
                        var val = enigma_virtual_box2.GetValue("History0").ToString();
                        result.bytes += val.Length;
                    }
                    catch { }
                    CurrentUserSoftware.DeleteSubKeyTree("Enigma Virtual Box");
                    CurrentUserSoftware.Close();
                    enigma_virtual_box.Close();
                }
            }
            catch { }
			#endregion
			
			#region NeverLose
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                CurrentUserSoftware.DeleteSubKeyTree("neverlose");
                CurrentUserSoftware.Close();
            }
            catch { }
            #endregion
			
			#region LastActivity
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\TypedPaths", true);
                var values = CurrentUserSoftware.GetValueNames();
                SafeNames safeNames = new SafeNames();
                foreach (var value in values)
                {
                    CurrentUserSoftware.DeleteValue(value);
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FeatureUsage\\ShowJumpView", true);
                var values = CurrentUserSoftware.GetValueNames();
                SafeNames safeNames = new SafeNames();
                foreach (var value in values)
                {
                    if (!value.StartsWith("*PID"))
                    {
                        if (!safeNames.IsSafeName(value))
                        {
                            CurrentUserSoftware.DeleteValue(value);
                        }
                    }
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Compatibility Assistant\\Store", true);
                var values = CurrentUserSoftware.GetValueNames();
                SafeNames safeNames = new SafeNames();
                foreach (var value in values)
                {
                    if (!safeNames.IsSafeName(value))
                    {
                        CurrentUserSoftware.DeleteValue(value);
                    }
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache", true);
                var values = CurrentUserSoftware.GetValueNames();
                SafeNames safeNames = new SafeNames();
                foreach (var value in values)
                {
                    if (!safeNames.IsSafeName(value))
                    {
                        CurrentUserSoftware.DeleteValue(value);
                    }
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32", true);
                var c1 = CurrentUserSoftware.OpenSubKey("CIDSizeMRU", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                CurrentUserSoftware.Close();
                c1.Close();
            } 
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU\\exe", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU\\*", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU\\rar", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU\\zip", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32", true);
                var c1 = CurrentUserSoftware.OpenSubKey("LastVisitedPidlMRU", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32", true);
                var c1 = CurrentUserSoftware.OpenSubKey("LastVisitedPidlMRULegacy", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int i))
                    {
                        var b = (byte[])c1.GetValue(value);
                        result.bytes += b.Length;
                        c1.DeleteValue(value);
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FeatureUsage\\AppSwitched", true);
                var values = c1.GetValueNames();
                foreach (var value in values)
                {
                    if (File.Exists(value))
                    {
                        FileInfo f = new FileInfo(value);
                        if (!new SafeNames().names.Contains(f.Name))
                        {
                            var b = c1.GetValue(value).ToString().Length;
                            result.bytes += b;
                            c1.DeleteValue(value);
                        }
                    }
                    else
                    {
                        if (!new SafeNames().names.Contains(value))
                        {
                            var b = c1.GetValue(value).ToString().Length;
                            result.bytes += b;
                            c1.DeleteValue(value);
                        }
                    }
                }
                c1.Close();
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RecentDocs", true);
                if (c1 != null)
                {
                    var values = c1.GetSubKeyNames();
                    foreach (var value in values)
                    {
                        var c2 = c1.OpenSubKey(value, true);
                        foreach (var value2 in c2.GetValueNames())
                        {
                            if (int.TryParse(value2, out int i))
                            {
                                var b = (byte[])c2.GetValue(value2);
                                result.bytes += b.Length;
                                c2.DeleteValue(value2);
                            }
                        }
                    }
                    c1.Close();
                }
            }
            catch { }
            try
            {
                var c1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RecentDocs", true);
                if (c1 != null)
                {
                    foreach (var value2 in c1.GetValueNames())
                    {
                        if (int.TryParse(value2, out int i))
                        {
                            var b = (byte[])c1.GetValue(value2);
                            result.bytes += b.Length;
                            c1.DeleteValue(value2);
                        }
                    }
                    c1.Close();
                }
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\RADAR\\HeapLeakDetection\\DiagnosedApplications", true);
                string[] names = CurrentUserSoftware.GetSubKeyNames();
                foreach (var name in names)
                {
                    bool find = false;
                    foreach (string name2 in new SafeNames().names)
                    {
                        if (name.ToLower() == name2.ToLower())
                        {
                            find = true;
                        }
                    }
                    if (!find)
                    {
                        CurrentUserSoftware.DeleteSubKeyTree(name);
                    }
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\OpenSavePidlMRU", true); ;
                string[] names2 = CurrentUserSoftware2.GetSubKeyNames();
                foreach (var name in names2)
                {
                    var newkey = CurrentUserSoftware2.OpenSubKey(name, true);
                    var values = newkey.GetValueNames();
                    foreach (var value_name in values)
                    {
                        if (value_name != "MRUListEx")
                        {
                            object value = newkey.GetValue(value_name);
                            if (value.GetType() == typeof(byte[]))
                            {
                                result.bytes += ((byte[])value).LongLength;
                                newkey.DeleteValue(value_name);
                            }
                        }
                    }
                }
                CurrentUserSoftware2.Close();
            }
            catch { }

            try
            {
                var CurrentUserSoftware2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Services\\bam\\State\\UserSettings", true);
                string[] names2 = CurrentUserSoftware2.GetSubKeyNames();
                foreach (var name in names2)
                {
                    var names3 = CurrentUserSoftware2.OpenSubKey(name, true);
                    var names4 = names3.GetValueNames();
					var saves_names = new SafeNames();
                    foreach (var name2 in names4)
                    {
                        if (name2.StartsWith("\\Device\\Harddisk"))
                        {
                            string name3 = name2.Substring(24);
                            string file_path = "C" + "\\" + name3;
                            if (!saves_names.IsSafeName(file_path))
                            {
                                result.bytes += name2.Length;
                                names3.DeleteValue(name2);
                            }
                        }
                    }
                }
                CurrentUserSoftware2.Close();
            }
            catch { }
			
			try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\TWinUI\\FilePicker\\LastVisitedPidlMRU", true);
                try
                {
                    CurrentUserSoftware.DeleteValue("MRUListEx");
                }
                catch { }
                CurrentUserSoftware.Close();
            }
            catch { }
            #endregion
			#region LastActivity
            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell", true);
                try
                {
                    CurrentUserSoftware.DeleteSubKeyTree("BagMRU");
                }
                catch { }
                try
                {
                    CurrentUserSoftware.DeleteSubKeyTree("Bags");
                }
                catch { }
				try
                {
                    CurrentUserSoftware.DeleteSubKeyTree("MuiCache");
                }
                catch { }
                CurrentUserSoftware.Close();
            }
            catch { }

            try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FeatureUsage", true);
                try
                {
                    CurrentUserSoftware.DeleteSubKeyTree("AppSwitchedl");
                }
                catch { }
                CurrentUserSoftware.Close();
            }
            catch { }
            try
            {
                var CurrentUserSoftware = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\RADAR\\HeapLeakDetection\\DiagnosedApplications", true);
                string[] names = CurrentUserSoftware.GetSubKeyNames();
                foreach (var name in names)
                {
                    CurrentUserSoftware.DeleteSubKeyTree(name);
                }
                CurrentUserSoftware.Close();
            }
            catch { }
            #endregion
			#region WinRar
			try
            {
                var CurrentUserSoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE\\WinRAR", true);
                try
                {
                    CurrentUserSoftware.DeleteSubKeyTree("ArcHistory");
                }
                catch { }
                CurrentUserSoftware.Close();
				var CurrentUserSoftware2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\WinRAR\\DialogEditHistory\\ArcName", true);
                try
                {
 
                    foreach (var value2 in CurrentUserSoftware2.GetValueNames())
                    {
                        CurrentUserSoftware2.DeleteValue(value2);
                    }
                }
                catch { }
                CurrentUserSoftware2.Close();
				
				var CurrentUserSoftware3 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\WinRAR\\General", true);
                try
                {
 
                     CurrentUserSoftware3.DeleteValue("LastFolder");
                }
                catch { }
                CurrentUserSoftware3.Close();
				
            }
            catch { }
			#endregion
			
			#region Inno Setup
			try
            {
				var CurrentUserSoftware2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Jordan Russell\\Inno Setup\\ScriptFileHistoryNew", true);
                try
                {
 
                    foreach (var value2 in CurrentUserSoftware2.GetValueNames())
                    {
                        CurrentUserSoftware2.DeleteValue(value2);
                    }
                }
                catch { }
                CurrentUserSoftware2.Close();
            }
            catch { }
			#endregion
			
			#region ImageMagick
			try
            {
				var CurrentUserSoftware2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ImageMagick\\IMDisplay\\Recent File List", true);
                try
                {
 
                    foreach (var value2 in CurrentUserSoftware2.GetValueNames())
                    {
                        CurrentUserSoftware2.DeleteValue(value2);
                    }
                }
                catch { }
                CurrentUserSoftware2.Close();
            }
            catch { }
			#endregion
			return result;
		}
	}
}