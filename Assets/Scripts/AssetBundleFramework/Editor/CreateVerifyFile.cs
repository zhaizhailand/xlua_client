/***
*
*	Title:"热更新"框架"项目
*		热更新模块：创建MD5校验文件
*
*
*	Description:
*			功能：
*				1：针对生成的AB包，与各种资源文件（lua/Manifest/Json...），生成MD5校验文件
*				2：本步骤需要在Unity的编辑器下运行，需要先提前生成AB包
*
*	Author: Zhaiyurong
*
*	Date: 2022.2
*
*	Modify:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using AssetBundleFramework;
using System.Security.Cryptography;

namespace HotFixFramework
{
	public class CreateVerifyFile
	{

		//生成校验文件
		[MenuItem("AssetBundleTools/Create AB MD5 File")]
		public static void CreateABMD5VerifyFile()
        {
			//AB输出路径
			string strABOutPath = PathTool.GetABOutPath();

			//校验文件输出路径
			string strVerifyFilePath = PathTool.GetMD5VerifyFilePath();
			
			//存储所有合法文件的相对路径信息
			List<string> fileList = new();


			//检查目录是否已经有校验文件，如过有，则覆盖
			if(File.Exists(strVerifyFilePath))
            {
				File.Delete(strVerifyFilePath);
            }

			//遍历所有AB文件，存储所有文件绝对路径
			DirectoryInfo tmpDirInfo = new DirectoryInfo(strABOutPath);
			ListFiles(tmpDirInfo, ref fileList);


			//把文件的相对路径与对应的MD5码，写入校验文件
			WriteVerifyFile(strVerifyFilePath, strABOutPath, fileList);

			AssetDatabase.Refresh();
			Debug.Log("本次生成MD5校验文件成功！");
        }

		/// <summary>
		/// 遍历当前文件夹（校验文件的输出路径）所有的文件，存储所有文件绝对路径
		/// </summary>
		/// <param name="fileSysInfo">文件（夹）信息</param>
		/// <param name="fileList">输入输出参数：把所有合法的文件（相对路径）写入的集合</param>
		private static void ListFiles(FileSystemInfo fileSysInfo, ref List<string> fileList)
        {
			DirectoryInfo dirInfo = fileSysInfo as DirectoryInfo;
			FileSystemInfo[] fileSysInfos = dirInfo.GetFileSystemInfos();
			foreach (FileSystemInfo tmpFileSysInfo in fileSysInfos)
			{
				FileInfo fileInfo = tmpFileSysInfo as FileInfo;
				//文件类型，写入集合
				if (fileInfo != null)
				{
					//过滤无效文件 .bak是Lua的备份文件
					if(fileInfo.Extension != ".meta" && fileInfo.Extension != ".bak")
                    {
						//把Windows系统路径中的分隔符转换为Unity的类型
						string strFullName = fileInfo.FullName.Replace("\\", "/");
						fileList.Add(strFullName);
						
					}
				}
				else
				{
					//文件夹，递归调用下一层文件夹
					ListFiles(tmpFileSysInfo, ref fileList);
				}
			}
        }

		/// <summary>
		/// 把文件名称与对应的MD5编码写入校验文件
		/// </summary>
		/// <param name="verifyFileOutPath">写入校验文件的路径</param>
		/// <param name="abOutPath">AB包输出路径</param>
		/// <param name="fileLists">存储所有合法文件的相对路径信息</param>
		private static void WriteVerifyFile(string verifyFileOutPath, string abOutPath,List<string> fileLists)
        {
			using (FileStream fs = new FileStream(verifyFileOutPath, FileMode.CreateNew))
            {
				using(StreamWriter sw = new StreamWriter(fs))
                {
					for(int i = 0; i < fileLists.Count; i++)
                    {
						string strFullName = fileLists[i];

						//生成MD5码
						string strFileMD5 = HotFixHelper.GetFileMD5(strFullName);

						//计算相对路径
						string strRelativePath = strFullName.Replace(abOutPath + "/", string.Empty);
						//Debug.Log(" strRelativePath: " + strRelativePath);

						//写入文件
						sw.WriteLine(strRelativePath + "|" + strFileMD5);
					}
				}
            }
        }

		
	}//Class_end
}