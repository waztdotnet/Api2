using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsDrawApp
{
    public static class CFileNames
    {



        /// <summary>
        /// true In ->//"C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017.ipt"
        /// out ->//"C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017-Affix"
        /// </summary>
        public static string AppendAffixToFileNameInFullFileName(string FullFileName, string DelimitorChar, string Affix)
        {
            string NewFileName = "";
            if (FullFileName != "" && Affix != "")
            {

                int StartIndexFileName = FullFileName.LastIndexOf("\\");
                int FirstDelimitorIndex = FullFileName.IndexOf(DelimitorChar, StartIndexFileName);
                int FileNameLength = FullFileName.Length - FirstDelimitorIndex;
                string Url = FullFileName.Substring(0, (FirstDelimitorIndex + 1));
                NewFileName = FullFileName.Substring(FirstDelimitorIndex, FileNameLength);
                NewFileName += Affix;
                return Url += NewFileName;
            }
            else
            {
                return NewFileName = "Error";
            }
        }
        /// <summary>
        /// true In ->"C:\Working Folder\Designs\Projects\E376-Billian\E379-P017.ipt" Out->C:\Working Folder\Designs\Projects\E376-Billian\
        /// false In ->"C:\Working Folder\Designs\Projects\E376-Billian\E379-P017.ipt" Out->C:\Working Folder\Designs\Projects\E376-Billian
        /// </summary>
        public static void GetFolderFromFullFileName(string FullFileName, ref string Url, bool IncLastSwitch)
        {
            if (FullFileName != "")
            {
                if (IncLastSwitch == true)
                {
                    Url = FullFileName.Substring(0, FullFileName.Length - (FullFileName.Length - FullFileName.LastIndexOf("\\") - 1));
                }
                else
                {
                    Url = FullFileName.Substring(0, FullFileName.Length - (FullFileName.Length - FullFileName.LastIndexOf("\\")));
                }
            }
            else
            {
                Url = "Error";
            }
        }
        //private string FileNameOne = "C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017-001-001.ipt";
        /// <summary>
        /// true In ->//"C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017.ipt" Out->P017 or P017-001
        /// 
        /// </summary>
        public static void GetFileNameFromFullFileName(string FullFileName, ref string FileName, string DelimitorChar)
        {

            if (FullFileName != "")
            {
                int StartIndexFileName = FullFileName.LastIndexOf("\\");
                int FirstDelimitorIndex = FullFileName.IndexOf(DelimitorChar, StartIndexFileName);
                int FileNameLength = FullFileName.LastIndexOf(".") - FirstDelimitorIndex;
                FileName = FullFileName.Substring(FirstDelimitorIndex, FileNameLength);
            }
            else
            {
                FileName = "Error";
            }

        }

      
        /// <summary>
        /// true In ->//"C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017.ipt" Out->P017-Qty12 or P017-001
        /// 
        /// </summary>
        public static void GetFileNameFromFullFileName(string FullFileName, ref string FileName, string DelimitorChar, string Affix)
        {

            if (FullFileName != "" && Affix != "")
            {
                int StartIndexFileName = FullFileName.LastIndexOf("\\");
                int FirstDelimitorIndex = FullFileName.IndexOf(DelimitorChar, StartIndexFileName);
                int FileNameLength = FullFileName.LastIndexOf(".") - FirstDelimitorIndex;
                FileName = FullFileName.Substring(FirstDelimitorIndex + 1, FileNameLength - 1);
                FileName += Affix;
            }
            else
            {
                FileName = "Error";
            }

        }


        /// <summary>
        /// true In ->//"C:\\Working Folder\\Designs\\Projects\\E376-Billian\\E379-P017.ipt" Out->P017-Qty12 or P017-001
        /// 
        /// </summary>
        public static string ReplaceFileNameInFullFileName(string FullFileName, string NewFileName, string DelimitorChar, string Affix)
        {

            if (FullFileName != "" && Affix != "")
                {

                    int StartIndexFileName = FullFileName.LastIndexOf("\\");
                    int FirstDelimitorIndex = FullFileName.IndexOf(DelimitorChar, StartIndexFileName);
                    int FileNameLength = FullFileName.LastIndexOf(".") - FirstDelimitorIndex;
                    string Url = FullFileName.Substring(0, (FirstDelimitorIndex + 1));
                NewFileName += Affix;
                    return Url += NewFileName;
                }
                else
                {
                    return NewFileName = "Error";
                }

        }
        public static string ReplaceFileNameInFullFileName(string FullFileName, string NewFileName, string DelimitorChar)
        {

            if (FullFileName != "")
            {

                int StartIndexFileName = FullFileName.LastIndexOf("\\");
                int FirstDelimitorIndex = FullFileName.IndexOf(DelimitorChar, StartIndexFileName);
                int FileNameLength = FullFileName.LastIndexOf(".") - FirstDelimitorIndex;
                string Url = FullFileName.Substring(0, (FirstDelimitorIndex + 1));

                return Url += NewFileName;
            }
            else
            {
                return NewFileName = "Error";
            }
        }
        /// <summary>
        /// Check PartNumberPrefix P017.ipt"
        /// 
        /// </summary>
        public static bool IsPartNumberPreFixMatch(string FileName, string PartNumberPreFixChar)
        {

            if (FileName != "")
            {
                if (FileName.StartsWith(PartNumberPreFixChar))
                {
                    return true;
                }

            }

            return false;
        }
        /// <summary>
        /// Check that X Number Prefixs Are Same E.g PG017.ipt"
        /// 
        /// </summary>
        public static bool IsPartNumberPreFixsMatch(string FileName, string PartNumberPreFixChars)
        {
            if (FileName != "")
            {
                int CountCompair = PartNumberPreFixChars.Length;
                string CompairChars = FileName.Substring(0, CountCompair);

                if (CompairChars.CompareTo(PartNumberPreFixChars) == 0)
                {
                    return true;
                }

            }

            return false;
        }
    }
}