using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AML
{
    class ZL_Library
    {
        //=====Folder Process=====
        public static bool CheckFile(string targetdir, string filename) //目前filename需附加副檔名，未加入預設值
        {
            string TargetFile = Path.Combine(targetdir, filename);
            return File.Exists(TargetFile);
        }
        public static bool CheckFile(string targetdir, string[] filename)
        {
            for (int i = 0; i < filename.Length; i++)
            {
                if (!CheckFile(targetdir,filename[i]))
                    return false;
            }
            return true;
        }
        public static void CopyFile(string sourcedir, string targetdir, string filename)
        {
            string SourceFile = Path.Combine(sourcedir, filename);
            string TargetFile = Path.Combine(targetdir, filename);
            File.Copy(SourceFile, TargetFile, true);
        }
        public static void CopyFile(string sourcedir, string targetdir, string[] filename)
        {
            for (int i = 0; i < filename.Length; i++)
                CopyFile(sourcedir, targetdir, filename[i]);
        }
        public static void DeleteFile(string targetdir, string filename)
        {
            string TargetFile = Path.Combine(targetdir, filename);
            File.Delete(TargetFile);
        }
        public static void DeleteFile(string targetdir, string[] filename)
        {
            for (int i = 0; i < filename.Length; i++)
                DeleteFile(targetdir,filename[i]);
        }
        public static DateTime GetCreateTime(string targetdir, string filename)
        {
            string TargetFile = Path.Combine(targetdir, filename);
            return File.GetCreationTimeUtc(TargetFile);
        }
        public static DateTime GetAccessTime(string targetdir, string filename)
        {
            string TargetFile = Path.Combine(targetdir, filename);
            return File.GetLastAccessTime(TargetFile);
        }
        public static string[] ReadDir(string targetdir, string fileformat) //拆字串有危險
        {
            string[] MapElement={};
            string[] MapList = Directory.GetFiles(targetdir, "*" + "." + fileformat);
            for (int i = 0; i < MapList.Length; i++)
            {
                MapElement = MapList[i].Split('\\');
                MapList[i] = MapElement[MapElement.Length-1];
            }
            return MapList;
        }

    }
}
