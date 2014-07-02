//
//using System.Collections.Generic;
//using SimpleJSON;
////using CodeTitans.JSon;
//
////  HTTPDispatch.cs
////  Author: Lu Zexi
////  2013-11-12
//
//namespace Game.Network
//{
//
//    /// <summary>
//    /// 调度基础类
//    /// </summary>
//    public abstract class HTTPDispatch
//    {
//        protected HTTPSession m_cSession; //HTTP会话
//        private NetQueue<HTTPPacketAck> m_cAckQueue;   //应答数据包队列
//        private Dictionary<string, HTTPHandleBase> m_mapHandles;      //句柄映射集合
//        private Dictionary<string, HTTPPacketFactory> m_mapPacketFactorys;       //数据包工厂映射集合
//
//        public HTTPDispatch()
//        {
//            this.m_cAckQueue = new NetQueue<HTTPPacketAck>(64 * 256);
//            this.m_cAckQueue.Clear();
//            this.m_mapHandles = new Dictionary<string, HTTPHandleBase>();
//            this.m_mapPacketFactorys = new Dictionary<string, HTTPPacketFactory>();
//        }
//
//        /// <summary>
//        /// 设置会话
//        /// </summary>
//        /// <param name="session"></param>
//        public void SetSession(HTTPSession session)
//        {
//            this.m_cSession = session;
//        }
//
//        /// <summary>
//        /// 数据错误事件
//        /// </summary>
//        public abstract void OnDataError();
//
//        /// <summary>
//        /// 网络断开事件
//        /// </summary>
//        public abstract void OnDisconnect();
//
//        /// <summary>
//        /// 超时
//        /// </summary>
//        public abstract void OnTimeOut();
//
//        /// <summary>
//        /// 逻辑更新
//        /// </summary>
//        /// <returns></returns>
//        public virtual bool Update()
//        {
//            for (int i = 0; i < 5 && i < this.m_cAckQueue.Size; i++)
//            {
//                HTTPPacketAck packet;
//                bool done = this.m_cAckQueue.Dequeue(out packet);
//                {
//                    if (packet == null)
//                        continue;
//                    if (done && this.m_mapHandles.ContainsKey(packet.GetAction()))
//                    {
//                        Excute(packet);
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }
//
//        /// <summary>
//        /// 执行数据包
//        /// </summary>
//        /// <param name="packet"></param>
//        public virtual void Excute(HTTPPacketAck packet)
//        {
//            if (this.m_mapHandles.ContainsKey(packet.GetAction()))
//            {
//                this.m_mapHandles[packet.GetAction()].Excute(packet);
//            }
//        }
//
//        /// <summary>
//        /// 注册句柄
//        /// </summary>
//        /// <param name="handle"></param>
//        /// <returns></returns>
//        protected bool RegistHandle(HTTPHandleBase handle)
//        {
//            if (this.m_mapHandles.ContainsKey(handle.GetAction()))
//            {
//                return false;
//            }
//            this.m_mapHandles.Add(handle.GetAction(), handle);
//            return true;
//        }
//
//        /// <summary>
//        /// 注册数据包
//        /// </summary>
//        /// <param name="packet"></param>
//        /// <returns></returns>
//        protected bool RegistFactory(HTTPPacketFactory factory)
//        {
//            if (this.m_mapPacketFactorys.ContainsKey(factory.GetPacketAction()))
//            {
//                return false;
//            }
//            this.m_mapPacketFactorys.Add(factory.GetPacketAction(), factory);
//            return true;
//        }
//
//        /// <summary>
//        /// 加入响应数据包
//        /// </summary>
//        /// <param name="packet"></param>
//        /// <returns></returns>
//        public virtual void AckPacket(HTTPPacketAck packet)
//        {
//            this.m_cAckQueue.Enqueue(packet);
//        }
//
//        /// <summary>
//        /// 创建ID对应的数据包
//        /// </summary>
//        /// <param name="cmd"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//		public HTTPPacketAck CreatePacket(string packetID, JSONNode obj)
//        {
//            if (this.m_mapPacketFactorys.ContainsKey(packetID))
//            {
//                return this.m_mapPacketFactorys[packetID].Create(obj);
//            }
//            return null;
//        }
//    }
//
//
//}