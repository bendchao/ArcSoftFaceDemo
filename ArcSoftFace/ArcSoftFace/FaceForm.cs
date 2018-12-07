using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ArcSoftFace.SDKModels;
using ArcSoftFace.SDKUtil;
using ArcSoftFace.Utils;
using ArcSoftFace.Entity;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Linq;

namespace ArcSoftFace
{
    public partial class FaceForm : Form
    {
        //引擎Handle
        private IntPtr pEngine = IntPtr.Zero;

        //保存左侧图片路径
        private string image1Path;

        //左侧图片人脸特征
        private IntPtr image1Feature;

        //保存对比图片的列表
        private List<string> imagePathList = new List<string>();

        //image图片缓存
        private List<Image> imageList = new List<Image>();

        //右侧图库人脸特征列表
        private List<IntPtr> imagesFeatureList = new List<IntPtr>();

        public FaceForm()
        {
            InitializeComponent();
            InitEngine();
        }

        /// <summary>
        /// 初始化引擎
        /// </summary>
        private void InitEngine()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));

            var is64CPU = Environment.Is64BitProcess;
            if (is64CPU)
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey64))
                {
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY64!");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey32))
                {
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY32!");
                    return;
                }
            }

            //激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;

            try
            {
                retCode = ASFFunctions.ASFActivation(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("无法加载 DLL") > -1)
                {
                    MessageBox.Show("请选择非Any CPU模式!");
                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }
            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            uint detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
            int detectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_HIGHER_EXT;
            int detectFaceScaleVal = 16;
            int detectFaceMaxNum = 5;
            int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_AGE | FaceEngineMask.ASF_GENDER | FaceEngineMask.ASF_FACE3DANGLE;
            retCode = ASFFunctions.ASFInitEngine(detectMode, detectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
        }

        /// <summary>
        /// “选择图片”按钮事件
        /// </summary>
        private void ChooseImg(object sender, EventArgs e)
        {
            if (pEngine == IntPtr.Zero)
            {
                MessageBox.Show("请先初始化引擎!");
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择图片";
            openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png";
            openFileDialog.Multiselect = false;
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DateTime detectStartTime = DateTime.Now;
                image1Path = openFileDialog.FileName;

                //获取文件，拒绝过大的图片
                FileInfo fileInfo = new FileInfo(image1Path);
                long maxSize = 1024 * 1024 * 2;
                if (fileInfo.Length > maxSize)
                {
                    MessageBox.Show("图像文件最大为2MB，请压缩后再导入!");
                    return;
                }

                Image srcImage = Image.FromFile(image1Path);
                //调整图像的宽度
                srcImage = ImageUtil.ScaleImage(srcImage, picture1.Width, picture1.Height);
                ImageInfo imageInfo = ImageUtil.ReadBMP(srcImage);
                ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pEngine, imageInfo);
                ASF_AgeInfo ageInfo = FaceUtil.AgeEstimation(pEngine, imageInfo, multiFaceInfo);
                ASF_GenderInfo genderInfo = FaceUtil.GenderEstimation(pEngine, imageInfo, multiFaceInfo);
                ASF_Face3DAngle face3DAngleInfo = FaceUtil.Face3DAngleDetection(pEngine, imageInfo, multiFaceInfo);
                MemoryUtil.Free(imageInfo.imgData);

                if (multiFaceInfo.faceNum < 1)
                {
                    MessageBox.Show("未检测出人脸!");
                    return;
                }

                //标记出检测到的人脸
                for (int i = 0; i < multiFaceInfo.faceNum; i++)
                {
                    MRECT rect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects + MemoryUtil.SizeOf<MRECT>() * i);
                    int orient = MemoryUtil.PtrToStructure<int>(multiFaceInfo.faceOrients + MemoryUtil.SizeOf<int>() * i);
                    int age = MemoryUtil.PtrToStructure<int>(ageInfo.ageArray + MemoryUtil.SizeOf<int>() * i);
                    int gender = MemoryUtil.PtrToStructure<int>(genderInfo.genderArray + MemoryUtil.SizeOf<int>() * i);

                    //以下角度为浮点型欧拉角，roll为侧倾角，pitch为俯仰角，yaw为偏航角
                    float roll = MemoryUtil.PtrToStructure<float>(face3DAngleInfo.roll + MemoryUtil.SizeOf<float>() * i);
                    float pitch = MemoryUtil.PtrToStructure<float>(face3DAngleInfo.pitch + MemoryUtil.SizeOf<float>() * i);
                    float yaw = MemoryUtil.PtrToStructure<float>(face3DAngleInfo.yaw + MemoryUtil.SizeOf<float>() * i);

                    srcImage = ImageUtil.MarkRectAndString(srcImage, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, age, gender);
                }

                //显示标记后的图像
                picture1.Image = srcImage;
                DateTime detectEndTime = DateTime.Now;
                ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                //提取人脸特征
                image1Feature = FaceUtil.ExtractFeature(pEngine, srcImage, out singleFaceInfo)[0];

            }
        }


        private object locker = new object();
        /// <summary>
        /// 右边多个图片选择按钮事件
        /// </summary>
        private void ChooseMultiImg(object sender, EventArgs e)
        {
            lock (locker)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "选择图片";
                openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.tif";
                openFileDialog.Multiselect = true;
                openFileDialog.FileName = string.Empty;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var numStart = imagePathList.Count;


                    //保存图片路径并显示
                    string[] fileNames = openFileDialog.FileNames;
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        imagePathList.Add(fileNames[i]);
                    }

                    //人脸检测以及提取人脸特征
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                    {
                        //禁止点击按钮
                        Invoke(new Action(delegate
                        {
                            chooseMultiImgBtn.Enabled = false;
                            matchBtn.Enabled = false;
                            btnClearFaceList.Enabled = false;
                        }));

                        //人脸检测和剪裁
                        for (int i = numStart; i < imagePathList.Count; i++)
                        {
                            Image image = Image.FromFile(imagePathList[i]);
                            ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pEngine, image);

                            if (multiFaceInfo.faceNum > 0)
                            {
                                MRECT rect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects);
                                image = ImageUtil.CutImage(image, rect.left, rect.top, rect.right, rect.bottom);
                            }

                            this.Invoke(new Action(delegate
                            {
                                if (image == null)
                                {
                                    image = Image.FromFile(imagePathList[i]);
                                }
                                imageList.Add(image);
                                imageList1.Images.Add(imagePathList[i], image);
                                listView1.Items.Add(i + "号", imagePathList[i]);
                            }));
                        }

                        //提取人脸特征
                        for (int i = numStart; i < imagePathList.Count; i++)
                        {
                            ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                            List<IntPtr> featureList = FaceUtil.ExtractFeature(pEngine, Image.FromFile(imagePathList[i]), out singleFaceInfo);
                            imagesFeatureList.Add((featureList.Count == 0) ? IntPtr.Zero : featureList[0]);
                        }

                        //允许点击按钮
                        Invoke(new Action(delegate
                        {
                            chooseMultiImgBtn.Enabled = true;
                            matchBtn.Enabled = true;
                            btnClearFaceList.Enabled = true;
                        }));
                    }));

                }
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            //卸载引擎
            int retCode = ASFFunctions.ASFUninitEngine(pEngine);
            Console.WriteLine("UninitEngine Result:" + retCode);
        }

        private void matchBtn_Click(object sender, EventArgs e)
        {
            if (image1Feature == IntPtr.Zero)
            {
                MessageBox.Show("请选择左侧图片!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (imagesFeatureList.Count == 0)
            {
                MessageBox.Show("请选择右侧对比图!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IList<FaceCompare> faceCompareList = new List<FaceCompare>();
            for (int i = 0; i < imagesFeatureList.Count; i++)
            {
                IntPtr feature = imagesFeatureList[i];
                float similarity = 0f;
                int ret = ASFFunctions.ASFFaceFeatureCompare(pEngine, image1Feature, feature, ref similarity);

                FaceCompare faceCompare = new FaceCompare();
                faceCompare.faceID = i;
                faceCompare.similarity = similarity;
                faceCompareList.Add(faceCompare);
            }
            //根据相似进行倒叙排序
            faceCompareList = faceCompareList.OrderByDescending(u => u.similarity).ToList();

            //临时变量存储
            List<string> imagePathListTemp = new List<string>();
            List<IntPtr> imagesFeatureListTemp = new List<IntPtr>();
            List<Image> imageListTemp = new List<Image>();
            //清空当前内容
            imageList1.Images.Clear();
            listView1.Items.Clear();

            for (int i = 0; i < faceCompareList.Count; i++)
            {
                int tempI = faceCompareList[i].faceID;
                imageListTemp.Add(imageList[tempI]);
                imageList1.Images.Add(imagePathList[tempI], imageList[tempI]);
                imagePathListTemp.Add(imagePathList[tempI]);
                listView1.Items.Add(string.Format("{0}号({1})", i, faceCompareList[i].similarity), imagePathList[tempI]);
                imagesFeatureListTemp.Add(imagesFeatureList[tempI]);
            }
            imageList = imageListTemp;
            imagesFeatureList = imagesFeatureListTemp;
            imagePathList = imagePathListTemp;
        }

        private void btnClearFaceList_Click(object sender, EventArgs e)
        {
            //清除数据
            imageList1.Images.Clear();
            listView1.Items.Clear();
            imagesFeatureList.Clear();
            imagePathList.Clear();
            imageList = new List<Image>();
        }
    }
}
