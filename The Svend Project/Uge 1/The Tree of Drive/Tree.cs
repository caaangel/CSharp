using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_CSharp_Syntax
{
    class Program
    {

        public const string lineVertical = "│";
        public const string lineHorizontal = "─";
        public const string lineVerticalToHorizontal = "└";
        public const string lineBranchOff = "├";

        public static string newBranchNotLast = lineBranchOff + lineHorizontal + lineHorizontal + lineHorizontal;
        public static string newBranchLast = lineVerticalToHorizontal + lineHorizontal + lineHorizontal + lineHorizontal;
        public static string runningBranch = lineVertical + "   ";
        
        public static string startFolder = Directory.GetCurrentDirectory();

    	static void Header()
    	{
            Console.WriteLine("Folder PATH listing");
    	}

        static void Footer()
        {
            Console.WriteLine("Listing Complete!");
            Console.WriteLine("Press ENTER to exit");
        }

    	
    	static void WalkTree(string folder, string indentStr, bool isLast)
    	{
            bool itIsLast;
            string theIndentStr;
            string theIndentStr2;
            string[] directories;

            if (isLast)
            {
                theIndentStr = newBranchLast;
                theIndentStr2 = "    ";
            }
            else
            {
                theIndentStr = newBranchNotLast;
                theIndentStr2 = runningBranch;
            }

            try
            {
                directories = Directory.GetDirectories(folder, "*");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(indentStr + theIndentStr + Path.GetFileName(folder) + " (Access Denied)");
                return;
            }

            Console.WriteLine(indentStr + theIndentStr + Path.GetFileName(folder));

            for(int i = 0; i < directories.Length; i++)
            {
                string directory = directories[i];

                if (i == directories.Length - 1)
                {
                    itIsLast = true;
                }
                else
                {
                    itIsLast = false;
                }
                WalkTree(directory, indentStr + theIndentStr2, itIsLast);
            }

	    }

        static void StartWalking(string folder)
        {
            bool itIsLast;
            string directory;
            string[] directories;

            try
            {
                directories = Directory.GetDirectories(folder, "*");
            }
            catch (DirectoryNotFoundException e)
            {
//                Console.WriteLine("Directory Not Found");
                Console.WriteLine(e.Message);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("(Access Denied)");
                return;
            }


            Console.WriteLine(folder.ToUpper());

            for (int i = 0; i < directories.Length; i++)
            {
                directory = directories[i];

                if (i == directories.Length - 1)
                {
                    itIsLast = true;
                }
                else
                {
                    itIsLast = false;
                }

                WalkTree(directory, "", itIsLast);

            }
        }

        static void ProcessParameters(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToUpper() == "/A")
                {
                    newBranchNotLast = "+---";
                    newBranchLast = "\\---";
                    runningBranch = "|   ";
                }
                else
                {
                    startFolder = args[i];
                }
            }
        }

        static void Main(string[] args)
        {
            ProcessParameters(args);
            Header();
            StartWalking(startFolder);
            Footer();
		    Console.ReadLine();
        }
    }
}