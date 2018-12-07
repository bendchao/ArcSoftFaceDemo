using System;

namespace ArcSoftFace.SDKModels
{
    /// <summary>
    /// 3D人脸角度检测结构体，可参考https://jingyan.baidu.com/article/0bc808fc2c0e851bd485b9ce.html进行深入理解
    /// </summary>
    public struct ASF_Face3DAngle
    {
        public IntPtr roll;
        public IntPtr yaw;
        public IntPtr pitch;
        /// <summary>
        /// 是否检测成功，0成功，其他为失败
        /// </summary>
        public IntPtr status;
        public int num;
    }
}
