﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dreamfly.JavaEstateCodeGenerator.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 获取路径下所有文件以及子文件夹中文件
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <param name="fileList">存放所有文件的全路径</param>
        /// <returns></returns>
        public static Dictionary<string, FileInfo> GetFile(string path, Dictionary<string, FileInfo> fileList)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                fileList.Add(f.FullName, f); //添加文件路径到列表中
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFile(d.FullName, fileList);
            }

            return fileList;
        }

        /// <summary>
        /// 输出文件并新建文件
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <param name="content">输出的内容</param>
        /// <returns></returns>
        public static void OutputFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path: path, mode: FileMode.OpenOrCreate,
                access: FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs); //创建写入流
                sw.WriteLine(content); // 写入转换后的模板内容
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 得到文件中的内容
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <returns></returns>
        public static async Task<string> GetFileContent(string path)
        {
            using (FileStream fs = new FileStream(path: path, mode: FileMode.Open, access: FileAccess.ReadWrite))
            {
                int fsLen = (int)fs.Length;
                byte[] heByte = new byte[fsLen];
                int r = await fs.ReadAsync(heByte, 0, heByte.Length);
                string content = System.Text.Encoding.UTF8.GetString(heByte);
                return content;
            }
        }

        public static void DeleteFile(string sourcePath, string fileName)
        {
            var filePath = Path.Combine(sourcePath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (Directory.Exists(sourcePath)
                && Directory.GetDirectories(sourcePath).Length == 0
                && Directory.GetFiles(sourcePath).Length == 0)
            {
                Directory.Delete(sourcePath);
            }
        }

        public static void CreateFile(string generatePath, string fileName, string content)
        {
            string path = Path.GetTempPath();
            Directory.CreateDirectory(path);
            string file = Path.Combine(path, fileName);
            File.WriteAllText(file, content, Encoding.UTF8);
            try
            {
                if (string.IsNullOrEmpty(generatePath))
                {
                    generatePath = @"..\..\";
                }

                if (!Directory.Exists(generatePath))
                {
                    Directory.CreateDirectory(generatePath);
                }

                string sourceFilePath = Path.Combine(generatePath, fileName);
                if (File.Exists(sourceFilePath))
                {
                    File.Delete(sourceFilePath);
                }

                File.Copy(file, sourceFilePath);
            }
            finally
            {
                File.Delete(file);
            }
        }

        #region bool SaveFile(string filePath, byte[] bytes) 文件保存，

        /// <summary>
        ///  文件保存，特别是有些文件放到数据库，可以直接从数据取二进制，然后保存到指定文件夹
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="bytes">文件二进制</param>
        /// <returns></returns>
        public static bool SaveFile(string filePath, byte[] bytes)
        {
            bool result = true;
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region 判断文件夹是否存在

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">文件夹地址</param>
        /// <returns></returns>
        public static bool DirectoryExist(string path)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 创建文件夹

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static bool DirectoryAdd(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //新建文件夹  
                return true;
            }

            return false;
        }

        #endregion
    }

}