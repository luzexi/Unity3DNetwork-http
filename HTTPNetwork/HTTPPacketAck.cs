
using SimpleJSON;
using System.Collections.Generic;


//  HTTPPacketBase.cs
//  Author: Lu Zexi
//  2013-11-12

namespace Game.Network
{
    /// <summary>
    /// HTTP数据包基础类
    /// </summary>
    public abstract class HTTPPacketAck
    {
        protected string m_strAction;    //动作URL
        public int m_iErrorCode;   //错误码
        public string m_strErrorDes;   //错误描述
        public long m_lServerTime;  //服务器时间

        public HTTPPacketAck()
        {
        }

        /// <summary>
        /// 获取动作URL
        /// </summary>
        /// <returns></returns>
        public string GetAction()
        {
            return this.m_strAction;
        }

        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <returns></returns>
        public virtual string GetRequire()
        {
            return "";
        }
    }


    /// <summary>
    /// 数据包工厂
    /// </summary>
    public abstract class HTTPPacketFactory
    {
        public abstract HTTPPacketAck Create(JSONNode json);    //创建包
        public abstract string GetPacketAction();  //获取包Action
    }


}