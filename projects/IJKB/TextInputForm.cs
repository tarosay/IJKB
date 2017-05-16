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
    public partial class TextInputForm : Form
    {
        public static bool RomajiOutputFlg = false;         //ローマ字出力を行うフラグ
        public static string RomajiText = "";               //ローマ字入力文字列が入力されている

        private int OwnerX = 0;
        private int OwnerY = 0;
        private int OwnerWidth = 1;
        private int OwnerHeight = 1;
        private ImeMode InputIMEMode = ImeMode.On;      //テキスト入力のIMEモード

        public TextInputForm(int x, int y, int w, int h, ImeMode imemode)
        {
            InitializeComponent();

            OwnerX = x;
            OwnerY = y;
            OwnerWidth = w;
            OwnerHeight = h;

            InputIMEMode = imemode;
        }

        private void TextInputForm_Load(object sender, EventArgs e)
        {
            //親画面の下に表示する
            this.Location = new Point(OwnerX, OwnerY + OwnerHeight);
            this.Width = OwnerWidth;

            // 最大化ボタンを無効にする
            this.MaximizeBox = false;

            tbxInput.Text = "";
            RomajiText = "";
            TextInputForm.RomajiOutputFlg = false;

            tbxInput.ImeMode = this.InputIMEMode;
            tbxInput.Width = OwnerWidth - 15;
        }

        /// <summary>
        /// フォームが生成された後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInputForm_Shown(object sender, EventArgs e)
        {
            if (Form.ActiveForm == null)
            {
                if (InputIMEMode != ImeMode.On)
                {
                    tbxInput.ImeMode = ImeMode.On;
                }
            }
            this.Activate();
        }

        /// <summary>
        /// ローマ字を出力する準備をしてウィンドウを閉じます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            RomajiText = tbxInput.Text;
            TextInputForm.RomajiOutputFlg = true;

            //フォームを閉じる
            FormClose();
        }

        /// <summary>
        /// フォームを閉じる処理です
        /// </summary>
        private void FormClose()
        {
            //フォームを閉じる
            this.Close();

            //フォームの解放
            this.Dispose();
        }

        /// <summary>
        /// 閉じる直前に走ります
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// メインフォームでこのフォームが閉じられたときに走ります。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


    }
}
