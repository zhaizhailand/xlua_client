/***
*
*	Title:"热更新框架"项目
*			从服务器下载最新的资源文件，获取资源（ab包、Lua文件、配置文件Json/xml）
*
*	Description:
*			本脚本是核心实现脚本，开发思路：
*				1：下载资源校验文件到客户端
*				2：客户端逐条读取资源校验文件，然后与本机相同的资源文件，进行MD5码比对
*				3：MD5码不一致，说明服务器端的对应资源发生更新，则客户端发起请求下载本条最新资源
*					a：客户端没有服务器增加的文件，直接下载服务器端文件即可
*					b：客户端存在与服务器文件相同的文件名，但MD5码比对不一致，说明服务器端的对应资源发生更新，则客户端发起请求下载本条最新资源
*					c：客户端存在服务器上没有的资源文件，说明服务器端最新资源删除了部分资源，则客户度删除该资源
*				
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

using AssetBundleFramework;
using UnityEngine.Networking;

namespace HotFixFramework
{
	public class UpdateResourcesFileFromServer : MonoBehaviour
	{
		//是否启用热更新（是否联网服务器下载更新资源）
		public bool enableSelf = true;

		//PC平台资源下载路径
		private string _DownloadPath = string.Empty;

		//HTTP服务器地址
		private string _ServerURL = PathTool.SERVER_URL;


        private void Awake()
        {
			//默认启用热更新
            if(enableSelf)
            {
				_DownloadPath = PathTool.GetABOutPath();
				//检测资源，进行对比更新
				StartCoroutine(DownloadResourceAndCheckUpdate(_ServerURL));
			}
			//不启用热更新
			else
            {
				Debug.Log("### WARNING: " + GetType() + "本脚本已勾选禁用！停止从服务器下载更新服务。");
				//通知其他游戏主逻辑开始运行
				BroadcastMessage("ReceiveInfoStartRuning", SendMessageOptions.DontRequireReceiver);
            }
        }

		/// <summary>
		/// 从服务器下载资源 检测资源进行对比
		/// </summary>
		/// <param name="serverUrl">服务器下载URL</param>
		/// <returns></returns>
		IEnumerator DownloadResourceAndCheckUpdate(string serverUrl)
        {
			//参数检查
			if(string.IsNullOrEmpty(serverUrl))
            {
				Debug.LogError(GetType() + "/DownloadResourceAndCheckUpdate()/ serverUrl null，请检查！");
				yield break;
            }
			//下载校验文件
			string fileUrl = serverUrl + "/" + PathTool.GetPlatformName() + PathTool.VERIFY_FILE_PATH;
			using(UnityWebRequest webReqrest = new UnityWebRequest(fileUrl))
            {
				DownloadHandlerBuffer dhb = new DownloadHandlerBuffer();
				webReqrest.downloadHandler = dhb;
				yield return webReqrest.SendWebRequest();
				if(webReqrest.result == UnityWebRequest.Result.Success)
                {
					//下载成功
					string content = webReqrest.downloadHandler.text;
					//Debug.Log("校验文件内容：" + content);
                }
				else
                {
					//下载失败
					Debug.LogError(GetType() + "/DownloadResourceAndCheckUpdate()/ 资源校验文件下载失败！ error = " + webReqrest.error + " url = " + fileUrl);
                }
            }
        }

    }
}