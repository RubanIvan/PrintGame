using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintGame.Proc
{
    public static class FileSize
    {
        public static string GetSizeString(long numBytes)
        {
            string fileSize = "";

            if (numBytes > 1073741824)
                fileSize = String.Format("{0:0.00} Gb", (double)numBytes / 1073741824);
            else if (numBytes > 1048576)
                fileSize = String.Format("{0:0.00} Mb", (double)numBytes / 1048576);
            else
                fileSize = String.Format("{0:0} Kb", (double)numBytes / 1024);

            if (fileSize == "0 Kb")
                fileSize = "1 Kb";  // min.							
            return fileSize;
        }
    }
}