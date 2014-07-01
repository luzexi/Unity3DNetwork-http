


//  IDispatchFactory.cs
//  Lu Zexi
//  2012-11-1

namespace Game.Network
{

    /// <summary>
    /// 调度工厂接口
    /// </summary>
    public interface IHTTPDispatchFactory
    {
        HTTPDispatch Create(HTTPSession session);      //创建
        void Destory(HTTPDispatch dispatch);        //释放
    }


}