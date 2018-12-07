using System;

namespace ArcSoftFace.SDKModels
{
    /// <summary>
    /// 人脸框信息结构体
    /// </summary>
    public struct MRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override bool Equals(object obj)
        {
            if (!(obj is MRECT))
            {
                return false;
            }

            var mRECT = (MRECT)obj;
            return left == mRECT.left &&
                   top == mRECT.top &&
                   right == mRECT.right &&
                   bottom == mRECT.bottom;
        }
    }
}
