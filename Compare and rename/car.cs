using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_CAR
{
    public class FileData
    {
        private string filename;
        private Int64 size;
        private DateTime timeStamp;

        // Constructor
        // Note: No return type given, not even void. Has the same name as Class
        public FileData(string TheFilename)
        {
            this.filename = TheFilename;
        }

        public FileData(string TheFilename, Int64 TheSize, DateTime TheTimeStamp)
        {
            this.filename = TheFilename;
            this.size = TheSize;
            this.timeStamp = TheTimeStamp;
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public Int64 Size
        {
            get { return size; }
            set { size = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string TimeStampFilename
        {
            get
            {
                return Path.GetDirectoryName(Filename) + "\\" + DateTimeToString(TimeStamp) + Path.GetExtension(Filename);
            }
        }

        private string AddZero(int number, int zeroes)
        {
            string res = number.ToString();
            int cnt = zeroes - res.Length;
            for (int i = 0; i < cnt; i++)
            {
                res = "0" + res;
            }
            return res;
        }

        private string DateTimeToString(DateTime dt)
        {
            return AddZero(dt.Year, 4) + AddZero(dt.Month, 2) + AddZero(dt.Day, 2) + " " + AddZero(dt.Hour, 2) + AddZero(dt.Minute, 2) + AddZero(dt.Second, 2) + AddZero(dt.Millisecond, 3);
        }

    }

    public class FileDataList
    {
        public List<FileData> Items;
        public string folder = "";

        private static string[] files;

        public FileDataList(string theFolder)
        {
            Items = new List<FileData>();
            folder = theFolder;
            BuildFileList();
            BuildItems();
        }

        public int Count
        {
            get { return Items.Count; }
        }

        private void BuildFileList()
        {
            try
            {
                files = Directory.GetFiles(folder, "*");

            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Couldn't built File List (Access Denied)");
            }

            for (int i = 0; i < files.Length; i++)
            {
                Items.Add(new FileData(files[i]));
            }
        }

        private void BuildItems()
        {
            // Convert to foreach for demonstration

            FileInfo fi;

            for (int i = 0; i < Items.Count; i++)
            {
                fi = new FileInfo(Items[i].Filename);
                Items[i].Size = fi.Length;
                Items[i].TimeStamp = fi.CreationTime;
//                Items[i].TimeStamp.Milli
            }

        }
    }

    class Program
    {
        private static string folder = "";

        // Declare variables

        private static double CalcPercent(int lille, int stor, int digits)
        {
            double result = (((double)lille / (double)stor) * (double)100);
            return Math.Round(result, digits);
            // *** KEY SYNTAX - DIVISION ***
            // Delphi: (lille / stor) * 100;
            // C#    : (((double)lille / (double)stor) * (double)100);
            // Note  : Typecasting of doubles are required. Will return 0 otherwise
        }

        private static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        static void FindDublicates(List<string> deleteFiles)
        {
            FileDataList FileList1 = new FileDataList(folder);
            FileDataList FileList2 = new FileDataList(folder);

            for (int i1 = 0; i1 < FileList1.Count; i1++)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Scanning " + CalcPercent(i1, FileList1.Count, 0).ToString() + "% --- " + System.IO.Path.GetFileName(FileList1.Items[i1].Filename) + "...                                  ");

                for (int i2 = i1 + 1; i2 < FileList2.Count; i2++)
                {
                    if ((FileList1.Items[i1].Size == FileList2.Items[i2].Size) &&
                        (FileList1.Items[i1].Filename != FileList2.Items[i2].Filename))
                    {
                        if (FileCompare(FileList1.Items[i1].Filename, FileList2.Items[i2].Filename))
                        {
                            if (FileList1.Items[i1].TimeStamp >= FileList2.Items[i2].TimeStamp) // File1 is newest
                            {
                                deleteFiles.Add(FileList2.Items[i2].Filename);
                            }
                            else
                            {
                                deleteFiles.Add(FileList1.Items[i1].Filename);
                            }
                            break;
                        }
                    }
                }
            }
        }

        static void DeleteFilesInList(List<string> deleteFiles)
        {
            string s = "";
            Console.WriteLine("");
            Console.WriteLine("Deleting Files");
            for (int i = 0; i < deleteFiles.Count; i++)
            {
                Console.Write("Delete " + deleteFiles[i] + " ... ");
                try
                {
                    s = "done";
                    File.Delete(deleteFiles[i]);
                }
                catch (Exception e)
                {
                    s = "Error (" + e.Message.ToString();
                }
                Console.WriteLine(s);
            }
        }

        static void RenameFilesToTimeStamp(string folder)
        {
            string s = "";
            Console.WriteLine("Renaming files");

            FileDataList FileList = new FileDataList(folder);

            for (int i = 0; i < FileList.Count; i++)
            {
                Console.Write("Rename " + Path.GetFileName(FileList.Items[i].Filename) + " --> " + FileList.Items[i].TimeStampFilename + " ... ");
                if (FileList.Items[i].Filename == FileList.Items[i].TimeStampFilename)
                {
                    s = "skip";
                }
                else
                {
                    s = "done";
                    while (File.Exists(FileList.Items[i].TimeStampFilename))
                    {
                        FileList.Items[i].TimeStamp = FileList.Items[i].TimeStamp.AddMilliseconds(1);
                    }
                    try
                    {
                        File.Move(FileList.Items[i].Filename, FileList.Items[i].TimeStampFilename);
                    }
                    catch (System.IO.IOException e)
                    {
                        s = "Error " + e.Message.ToString();
                    }
                    
                }
                Console.WriteLine(s);
            }


        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                NoParam();
                return; // Should in theory work like the Delphi "EXIT" here
            }

            
            folder = args[0];

            // Ensure trailing backslash
            if (folder[folder.Length - 1].ToString() != "\\")
            {
                folder = folder + "\\";
            }
            // *** KEY SYNTAX - CHAR AND STRING ***
            // Delphi: if folder[Length(folder) - 1] <> "\" then begin
            // C#    : if (folder[folder.Length - 1].ToString() != "\\")
            // Note  : String array start from 0, not 1 like in delphi

            Console.WriteLine("Scanning Folder = " + folder);

            List<string> deleteFiles = new List<string>();
            FindDublicates(deleteFiles);
            DeleteFilesInList(deleteFiles);
            RenameFilesToTimeStamp(folder);
        }

        static void NoParam()
        {
            Console.WriteLine("No folder passed as Parameters");
            Console.WriteLine("");
            Console.WriteLine("eg.");
            Console.WriteLine("car \"C:\\temp\\video\"");
        }


    }
}
