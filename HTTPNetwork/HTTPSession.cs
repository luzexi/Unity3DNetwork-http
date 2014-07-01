using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SimpleJSON;



//  HttpSession.cs
//  Author: Lu Zexi
//  2013-11-12


namespace Game.Network
{

    /// <summary>
    /// HTTP会话
    /// </summary>
    public class HTTPSession
    {
        private string m_strURL = "";   //主地址
        private HTTPDispatch m_cDispatch;   //调度类
		private HTTPPacketRequest m_cLastPacket;   //最近的一次数据包
		private Queue<HTTPPacketRequest> m_lstReadyPacket = new Queue<HTTPPacketRequest>();  //预备发送数据包

        public HTTPSession(string url, IHTTPDispatchFactory factory)
        {
            this.m_strURL = url;
            this.m_cDispatch = factory.Create(this);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="url_sub">子地址</param>
        /// <param name="arg">参数</param>
        public void Send(HTTPPacketRequest packet)
        {
            this.m_cLastPacket = packet;
            HTTPLoader.GoWWW(this.m_strURL + this.m_cLastPacket.GetAction() + this.m_cLastPacket.ToParam(), HTTPCallBack);
        }

        /// <summary>
        /// 发送准备
        /// </summary>
        /// <param name="packet"></param>
		public void SendReady(HTTPPacketRequest packet)
        {
            this.m_lstReadyPacket.Enqueue(packet);
        }

        /// <summary>
        /// 发送准备好的数据包
        /// </summary>
        public bool Send()
        {
            if (this.m_lstReadyPacket.Count <= 0)
                return false;
            this.m_cLastPacket = this.m_lstReadyPacket.Dequeue();
            HTTPLoader.GoWWW(this.m_strURL + this.m_cLastPacket.GetAction() + this.m_cLastPacket.ToParam(), HTTPCallBack);
            return true;
        }

        /// <summary>
        /// 重新发送
        /// </summary>
        public void ReSend()
        {
			HTTPLoader.GoWWW(this.m_strURL + this.m_cLastPacket.GetAction() + this.m_cLastPacket.ToParam(), HTTPCallBack);
        }

        /// <summary>
        /// HTTP回调
        /// </summary>
        /// <param name="www"></param>
        private void HTTPCallBack(WWW www)
        {
            try
            {
                if (www.error != null)
                {
                    //GAME_LOG.ERROR("ERROR HTTP : " + www.error);
                    Debug.LogError("ERROR HTTP : " + www.error);
                    this.m_cDispatch.OnDisconnect();
                }
                else
                {
                    Debug.Log(www.text);
                    //JSonReader read = new JSonReader();
					JSONNode obj = JSON.Parse(www.text);
                    Debug.Log(obj.ToString());
                    HTTPPacketBase packet = this.m_cDispatch.CreatePacket(this.m_cLastPacket.GetAction(), obj);
                    if (packet != null)
                    {
                        this.m_cDispatch.AckPacket(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
                this.m_cDispatch.OnDataError();
            }
        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        public void Update()
        {
            if (this.m_cDispatch != null)
                this.m_cDispatch.Update();
        }
    }


}