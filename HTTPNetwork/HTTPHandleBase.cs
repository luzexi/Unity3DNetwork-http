


//  HandleBase.cs
//  Author: Lu Zexi
//  2013-11-12

namespace Game.Network
{

    /// <summary>
    /// 句柄基础类
    /// </summary>
    public abstract class HTTPHandleBase
    {
        public abstract string GetAction();   //获取句柄Action
        public abstract bool Excute(HTTPPacketAck packet); //句柄执行方法
    }


}