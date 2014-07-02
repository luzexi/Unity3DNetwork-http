//
//
//
//
////  DispatchFactory.cs
////  Lu Zexi
////  2012-11-1
//
//namespace Game.Network
//{
//
//    /// <summary>
//    /// 调度工厂抽象类
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class HTTPDispatchFactory<T> : IHTTPDispatchFactory where T : HTTPDispatch, new()
//    {
//        /// <summary>
//        /// 创建调度实例
//        /// </summary>
//        /// <param name="session"></param>
//        /// <returns></returns>
//        public virtual HTTPDispatch Create(HTTPSession session)
//        {
//            T t = new T();
//            t.SetSession(session);
//            return t;
//        }
//
//        /// <summary>
//        /// 销毁调度类
//        /// </summary>
//        /// <param name="dispatch"></param>
//        public virtual void Destory(HTTPDispatch dispatch)
//        {
//            //
//        }
//    }
//
//
//}