using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace LPL_Sysutils
{
    class SysUtils
    {
        public string PathToOk(string folder)
        {
            // Ensure trailing backslash
            if (folder[folder.Length - 1].ToString() != "\\")
            {
                folder = folder + "\\";
            }
            return folder;
        }

        public string ApplicationPath()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
            return Path.GetDirectoryName(a.Location);            
        }

        public string PathToApplication()
        {
            return PathToOk(ApplicationPath());
        }
    }


    class program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sysutils test");

            var sysUtils = new SysUtils();
            Console.WriteLine("ApplicationPath = " + sysUtils.ApplicationPath());
            Console.WriteLine("PathToApplication = " + sysUtils.PathToApplication());
        }
    }
}