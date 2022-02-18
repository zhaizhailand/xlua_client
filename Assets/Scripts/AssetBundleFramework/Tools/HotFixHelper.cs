/***
*
*	Title:"热更新框架"项目
*			静态帮助类
*
*	Description:
*			 功能：
*				提供热更新需要的公共通用方法
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

using System.IO;
using System.Security.Cryptography;//MD5编码的命名空间
using System.Text;

namespace HotFixFramework
{
	public class HotFixHelper
	{

		/// <summary>
		/// 根据指定文件（lua/text/ab包/Json）的路径，生成MD5编码
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileMD5(string filePath)
        {
			StringBuilder sb = new StringBuilder();
			//去除空格
			filePath = filePath.Trim();

			//打开文件
			using(FileStream fs = new FileStream(filePath, FileMode.Open))
            {
				MD5 md5 = new MD5CryptoServiceProvider();
				
				//指定文件，转二进制数组
				byte[] result = md5.ComputeHash(fs);
				for(int i = 0; i < result.Length; i++)
                {
					//x2表示输出按照16进制，且为2位对齐输出
					sb.Append(result[i].ToString("x2"));
                }
            }				
			return sb.ToString();
		}


	}//Class_end
}