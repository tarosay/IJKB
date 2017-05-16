using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IchigoJamKeyBoard
{
    public partial class SoftKeyBoard : Form
    {
        private int OwnerX = 0;
        private int OwnerY = 0;
        private int OwnerWidth = 1;
        private int OwnerHeight = 1;

        private Bitmap BmpMain = null;
        private int CharaNum = -1;

        public SoftKeyBoard(int x, int y, int w, int h)
        {
            InitializeComponent();

            OwnerX = x;
            OwnerY = y;
            OwnerWidth = w;
            OwnerHeight = h;
        }

        private void SoftKeyBoard_Load(object sender, EventArgs e)
        {
            ////親画面の右に表示する
            //this.Location = new Point(OwnerX + OwnerWidth, OwnerY);

            //親画面の中央に表示する
            this.Location = new Point(OwnerX + OwnerWidth / 2 - this.Width / 2, OwnerY + OwnerHeight / 2 - this.Height / 2);

            // 最大化ボタンを無効にする
            this.MaximizeBox = false;

            //グラフィックスのセット
            BmpMain = (Bitmap)pbxMain.Image;    //.Clone();
        }

        /// <summary>
        /// マウスがクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxMain_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(CharaNum.ToString());

            if (CharaNum < 0) { return; }

            byte bin = 0;
            if (CharaNum >= 5 && CharaNum <= 10)
            {
                bin = (byte)(244 + CharaNum);
            }
            else if (CharaNum == 11)
            {
                bin = 232;
            }
            else if (CharaNum == 12)
            {
                bin = 237;
            }
            else if (CharaNum == 15)
            {
                bin = 244;
            }
            else if (CharaNum >= 17 && CharaNum <= 18)
            {
                bin = (byte)((CharaNum - 17) + 150);
            }
            else if (CharaNum >= 19 && CharaNum <= 20)
            {
                bin = (byte)((CharaNum - 19) + 148);
            }
            else if (CharaNum >= 21 && CharaNum <= 23)
            {
                bin = (byte)(147 - (CharaNum - 21));
            }
            else if (CharaNum >= 24 && CharaNum <= 27)
            {
                bin = (byte)((CharaNum - 24) + 152);
            }
            else if (CharaNum >= 28 && CharaNum <= 31)
            {
                bin = (byte)((CharaNum - 28) + 224);
            }
            else
            {
                bin = (byte)CharaNum;
            }

            SerialTool.SendComm(bin);
        }

        private void pbxMain_MouseLeave(object sender, EventArgs e)
        {
            int p;
            int x = 0;
            int y = 0;

            //反転を元に戻します
            if (CharaNum >= 0)
            {
                //以前の反転を戻します
                x = CharaNum % 16;
                y = CharaNum / 16;
                if (x > 15) { x = 15; }
                if (y > 15) { y = 15; }

                for (int yi = 0; yi < 33; yi++)
                {
                    for (int xi = 0; xi < 33; xi++)
                    {
                        p = BmpMain.GetPixel(xi + x * 33, yi + y * 33).ToArgb();            // ピクセルデータの取得
                        p ^= 0xffffff;                                                      // 反転色の計算
                        BmpMain.SetPixel(xi + x * 33, yi + y * 33, Color.FromArgb(p));      // ピクセルデータの設定
                    }
                }
            }
            pbxMain.Image = (Image)BmpMain;

            CharaNum = -1;
        }

        /// <summary>
        /// マウスの移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxMain_MouseMove(object sender, MouseEventArgs e)
        {
            int p;
            int x = e.X / (pbxMain.Width / 16);
            int y = e.Y / (pbxMain.Height / 16);
            if (x > 15) { x = 15; }
            if (y > 15) { y = 15; }
            int num = y * 16 + x;

            if (num == 0 || num == 13 || num == 14 || num == 16 || num == 127)
            {
                num = -1;
            }

            if (CharaNum == num)
            {
                return;
            }

            if (CharaNum >= 0)
            {
                //以前の反転を戻します
                int xb = CharaNum % 16;
                int yb = CharaNum / 16;
                if (xb > 15) { xb = 15; }
                if (yb > 15) { yb = 15; }

                for (int yi = 0; yi < 33; yi++)
                {
                    for (int xi = 0; xi < 33; xi++)
                    {
                        p = BmpMain.GetPixel(xi + xb * 33, yi + yb * 33).ToArgb();            // ピクセルデータの取得
                        p ^= 0xffffff;                                                      // 反転色の計算
                        BmpMain.SetPixel(xi + xb * 33, yi + yb * 33, Color.FromArgb(p));      // ピクセルデータの設定
                    }
                }
            }

            if (num >= 0)
            {
                for (int yi = 0; yi < 33; yi++)
                {
                    for (int xi = 0; xi < 33; xi++)
                    {
                        p = BmpMain.GetPixel(xi + x * 33, yi + y * 33).ToArgb();            // ピクセルデータの取得
                        p ^= 0xffffff;                                                      // 反転色の計算
                        BmpMain.SetPixel(xi + x * 33, yi + y * 33, Color.FromArgb(p));      // ピクセルデータの設定
                    }
                }
            }

            pbxMain.Image = (Image)BmpMain;

            CharaNum = num;
            //Debug.WriteLine(CharaNum.ToString() + ", " + x.ToString() + ", " + y.ToString());
        }

        /// <summary>
        /// リンクがクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkIchigoJam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //改行キーが押されると、この割り込みが発生する。それは困るので、ここでは処理しないことにした。

            ////リンク先に移動したことにする
            //lnkIchigoJam.LinkVisited = true;

            ////ブラウザで開く
            //Process.Start("http://ichigojam.net/");
        }

        /// <summary>
        /// マウスでクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkIchigoJam_Click(object sender, EventArgs e)
        {
            //リンク先に移動したことにする
            lnkIchigoJam.LinkVisited = true;

            //ブラウザで開く
            Process.Start("http://ichigojam.net/");
        }
    }
}
