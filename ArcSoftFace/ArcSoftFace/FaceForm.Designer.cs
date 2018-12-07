using ArcSoftFace.SDKUtil;
using System;

namespace ArcSoftFace
{
    partial class FaceForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            Form_Closed(null,null);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceForm));
            this.picture1 = new System.Windows.Forms.PictureBox();
            this.lblImageList = new System.Windows.Forms.Label();
            this.chooseMultiImgBtn = new System.Windows.Forms.Button();
            this.chooseImgBtn = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.matchBtn = new System.Windows.Forms.Button();
            this.btnClearFaceList = new System.Windows.Forms.Button();
            this.lblCompareImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
            this.SuspendLayout();
            // 
            // picture1
            // 
            this.picture1.BackColor = System.Drawing.Color.White;
            this.picture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picture1.Location = new System.Drawing.Point(575, 25);
            this.picture1.Name = "picture1";
            this.picture1.Size = new System.Drawing.Size(429, 422);
            this.picture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picture1.TabIndex = 1;
            this.picture1.TabStop = false;
            // 
            // lblImageList
            // 
            this.lblImageList.AutoSize = true;
            this.lblImageList.Location = new System.Drawing.Point(12, 9);
            this.lblImageList.Name = "lblImageList";
            this.lblImageList.Size = new System.Drawing.Size(35, 12);
            this.lblImageList.TabIndex = 7;
            this.lblImageList.Text = "图库:";
            // 
            // chooseMultiImgBtn
            // 
            this.chooseMultiImgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chooseMultiImgBtn.Location = new System.Drawing.Point(14, 470);
            this.chooseMultiImgBtn.Name = "chooseMultiImgBtn";
            this.chooseMultiImgBtn.Size = new System.Drawing.Size(147, 26);
            this.chooseMultiImgBtn.TabIndex = 32;
            this.chooseMultiImgBtn.Text = "注册人脸";
            this.chooseMultiImgBtn.UseVisualStyleBackColor = true;
            this.chooseMultiImgBtn.Click += new System.EventHandler(this.ChooseMultiImg);
            // 
            // chooseImgBtn
            // 
            this.chooseImgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chooseImgBtn.Location = new System.Drawing.Point(575, 470);
            this.chooseImgBtn.Name = "chooseImgBtn";
            this.chooseImgBtn.Size = new System.Drawing.Size(154, 26);
            this.chooseImgBtn.TabIndex = 30;
            this.chooseImgBtn.Text = "选择识别图";
            this.chooseImgBtn.UseVisualStyleBackColor = true;
            this.chooseImgBtn.Click += new System.EventHandler(this.ChooseImg);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "alai_152784032385984494.jpg");
            // 
            // listView1
            // 
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(14, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(527, 422);
            this.listView1.TabIndex = 33;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // matchBtn
            // 
            this.matchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.matchBtn.Location = new System.Drawing.Point(829, 470);
            this.matchBtn.Name = "matchBtn";
            this.matchBtn.Size = new System.Drawing.Size(145, 26);
            this.matchBtn.TabIndex = 34;
            this.matchBtn.Text = "开始匹配";
            this.matchBtn.UseVisualStyleBackColor = true;
            this.matchBtn.Click += new System.EventHandler(this.matchBtn_Click);
            // 
            // btnClearFaceList
            // 
            this.btnClearFaceList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFaceList.Location = new System.Drawing.Point(396, 470);
            this.btnClearFaceList.Name = "btnClearFaceList";
            this.btnClearFaceList.Size = new System.Drawing.Size(145, 26);
            this.btnClearFaceList.TabIndex = 35;
            this.btnClearFaceList.Text = "清空人脸库";
            this.btnClearFaceList.UseVisualStyleBackColor = true;
            this.btnClearFaceList.Click += new System.EventHandler(this.btnClearFaceList_Click);
            // 
            // lblCompareImage
            // 
            this.lblCompareImage.AutoSize = true;
            this.lblCompareImage.Location = new System.Drawing.Point(573, 10);
            this.lblCompareImage.Name = "lblCompareImage";
            this.lblCompareImage.Size = new System.Drawing.Size(47, 12);
            this.lblCompareImage.TabIndex = 36;
            this.lblCompareImage.Text = "识别图:";
            // 
            // FaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 507);
            this.Controls.Add(this.lblCompareImage);
            this.Controls.Add(this.btnClearFaceList);
            this.Controls.Add(this.matchBtn);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.chooseMultiImgBtn);
            this.Controls.Add(this.chooseImgBtn);
            this.Controls.Add(this.lblImageList);
            this.Controls.Add(this.picture1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FaceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picture1;
        private System.Windows.Forms.Label lblImageList;
        private System.Windows.Forms.Button chooseMultiImgBtn;
        private System.Windows.Forms.Button chooseImgBtn;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button matchBtn;
        private System.Windows.Forms.Button btnClearFaceList;
        private System.Windows.Forms.Label lblCompareImage;
    }
}

