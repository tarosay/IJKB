using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IchigoJamKeyBoard
{
    public partial class MonitorForm : Form
    {
        public static bool BlnMonitorActive = false;                    //フォームがアクティブかどうかのフラグ
        public static int FrameRate = 10;                               //一秒間に表示する枚数
        //public static CursorFigure CursorNow = CursorFigure.Upper;      //カーソルの形状を表す
        public static CursorFigure CursorNow = CursorFigure.Insert;       //カーソルの形状を表す

        private Bitmap[] Chara = new Bitmap[256];                       //イチゴジャムのキャラクタデータ
        private int OwnerX = 0;
        private int OwnerY = 0;
        private int OwnerWidth = 1;
        private int OwnerHeight = 1;
        private DateTime DtmInterval = DateTime.Now;
        private Graphics GrpSource = null;
        private CursorFigure CursorMode = CursorFigure.Insert;
        private bool ResetFlg = false;                                  //画面リセットして?PEEK(#901)の結果を見るフラグ
        private bool RefreshFlg = true;                                 //画面のリフレッシュフラグです

        /// <summary>
        /// Print処理を排他的に行うために用意
        /// </summary>
        private AutoResetEvent ArePrint = new AutoResetEvent(true);

        private AutoResetEvent AreImageTrace = new AutoResetEvent(true);

        /// <summary>
        /// カーソルの形状
        /// </summary>
        public enum CursorFigure
        {
            Non,            //表示しない
            Insert,         //挿入モード
            OverWrite,      //上書きモード
        }
        private bool BlnCursorFlash = true;                         //カーソルの点滅を表す

        private SolidBrush S_Black = new SolidBrush(Color.Black);
        private SolidBrush S_White = new SolidBrush(Color.White);

        #region //********     フォーム起動時の処理       ********//
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonitorForm(int x, int y, int w, int h)
        {
            InitializeComponent();

            OwnerX = x;
            OwnerY = y;
            OwnerWidth = w;
            OwnerHeight = h;
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_Load(object sender, EventArgs e)
        {
            //フォームがキーイベントを取れるようにする
            this.KeyPreview = true;

            //pbxSourceの初期化
            pbx256x192.Image = new Bitmap(pbx256x192.Width, pbx256x192.Height);
            GrpSource = Graphics.FromImage(pbx256x192.Image);
            //pbx256x192を黒で塗りつぶします
            GrpSource.FillRectangle(S_Black, 0, 0, 256, 192);

            //pbxMainの初期化
            pbxMonitor.SizeMode = PictureBoxSizeMode.StretchImage;

            MonitorSize(2.0);

            //AreImageTrace.WaitOne();
            {
                //pbxMonitor.Image = (Image)pbx256x192.Image.Clone();
                pbxMonitor.Image = (Image)pbx256x192.Image;
            }
            //AreImageTrace.Set();

            //親画面の中央に表示する
            //this.Location = new Point(OwnerX + OwnerWidth / 2 - this.Width / 2, OwnerY + OwnerHeight / 2 - this.Height / 2);
            //親画面の右に表示する
            this.Location = new Point(OwnerX + OwnerWidth, OwnerY);

            //タイトル表示
            this.Text = this.Text + " " + GetTitle(true);

            //イチゴジャムのキャラクタデータの読み込み
            Bitmap bmpCharas = new Bitmap(pbxIchigoCharas.Image);
            int n = 0;
            Color cc;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    Chara[n] = new Bitmap(8, 8);
                    Graphics.FromImage(Chara[n]).Clear(Color.White);

                    for (int j = 0; j < 8; j++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            cc = bmpCharas.GetPixel(x * 8 + i, y * 8 + j);
                            if (cc.G == Color.White.G)
                            {
                                //Debug.Write("1");
                                Graphics.FromImage(Chara[n]).FillRectangle(S_Black, i, j, 1, 1);
                            }
                            else
                            {
                                //Debug.Write("0");
                            }
                        }
                        //Debug.WriteLine("");
                    }
                    //Debug.WriteLine("");

                    ////保存します
                    //Chara[n].Save("Chara" + n.ToString("000") + ".bmp", ImageFormat.Bmp);
                    n++;
                }
            }
            bmpCharas.Dispose();

            //カーソルの初期化
            if (CursorNow != CursorFigure.Non)
            {
                chkCursor.Checked = true;
            }
            else
            {
                chkCursor.Checked = false;
            }

            //VRAMの内容をセットする
            SetVRAM();

        }

        /// <summary>
        /// フォームを表示した後の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_Shown(object sender, EventArgs e)
        {
            //フォーカスをpbxMonitorに持っていく
            pbxMonitor.Focus();

            //スクリーンが使える状態である
            Scrn.IsReady = true;
        }

        /// <summary>
        /// フォームが閉じられるときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("フォームを閉じました。MainFormで割り込みが走ります。");

            for (int i = 0; i < 256; i++)
            {
                if (Chara[i] != null)
                {
                    Chara[i].Dispose();
                    Chara[i] = null;
                }
            }
            GrpSource.Dispose();
            //GrpMain.Dispose();
        }

        /// <summary>
        /// メインフォームのタイトルをセットします
        /// </summary>
        /// <param name="exportfilename"></param>
        /// <param name="buildpartFlg"></param>
        private string GetTitle(bool buildpartFlg)
        {
            //自分自身のバージョン情報を取得する
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            //タイトルの表示
            if (buildpartFlg == true)
            {
                return string.Concat(" Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString(), ".", ver.FileBuildPart.ToString());
            }
            return string.Concat(" Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString());
        }
        #endregion

        #region //********     モニタサイズ変更の処理       ********//
        /// <summary>
        /// モニタサイズの変更
        /// </summary>
        /// <param name="ratio"></param>
        private void MonitorSize(double ratio)
        {
            if (ratio <= 0.0) { return; }

            int width = (int)(256.0 * ratio);
            int height = (int)(192.0 * ratio);

            this.Width = width;
            this.Height = height;
            this.Width += width - pbxMonitor.Width;
            this.Height += height - pbxMonitor.Height;

            //Debug.WriteLine("Ratio=" + ratio.ToString());
            //Debug.WriteLine("this.Width=" + this.Width.ToString() + ", this.height=" + this.Height.ToString());
            //Debug.WriteLine("pbxMonitor.Width=" + pbxMonitor.Width.ToString() + ", pbxMonitor.Height=" + pbxMonitor.Height.ToString());
        }

        /// <summary>
        /// フォームのサイズが変更されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_SizeChanged(object sender, EventArgs e)
        {
            //ここで、Windows10 で落ちる原因のインスタンスの有無を調べてブロック文をいれる。

            //たぶん、こんなイメージではないかと・・・       if (pbxMonitor.Image == null || pbx256x192.Image == null) { return; }
            if (pbxMonitor == null || pbx256x192 == null) { return; }

            if (pbxMonitor.Width <= 2 || pbxMonitor.Height <= 2) { return; }

            AreImageTrace.WaitOne();
            {
                //pbxMonitor.Image = (Image)pbx256x192.Image.Clone();
                pbxMonitor.Image = (Image)pbx256x192.Image;
            }
            AreImageTrace.Set();
        }

        private void MnuRC1bai_Click(object sender, EventArgs e)
        {
            MonitorSize(1.0);
        }
        private void MnuRC2bai_Click(object sender, EventArgs e)
        {
            MonitorSize(2.0);
        }
        private void MnuRC3bai_Click(object sender, EventArgs e)
        {
            MonitorSize(3.0);
        }
        private void MnuRC4bai_Click(object sender, EventArgs e)
        {
            MonitorSize(4.0);
        }

        /// <summary>
        /// 画面をクリアする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuResetScreen_Click(object sender, EventArgs e)
        {
            //画面をシンクロするためにリセットします
            ScreenReset();
        }

        /// <summary>
        /// IchigoJamサイトに飛ぶ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuCC_Click(object sender, EventArgs e)
        {
            //ブラウザで開く
            Process.Start("http://ichigojam.net/");
        }
        #endregion

        #region //********     画面にキャラクタを表示する処理       ********//
        /// <summary>
        /// 画面にPrintします
        /// </summary>
        /// <param name="dat"></param>
        public void Print(int dat)
        {
            ArePrint.WaitOne();
            {
                timCursor.Enabled = false;  //カーソルの割り込みを止めます
                BlnCursorFlash = false;     //カーソルを消す処理をする設定にします
                ViewCursor();               //カーソルの処理をします

                Debug.WriteLine("LC " + Scrn.X.ToString() + ", " + Scrn.Y.ToString() + ":? CHR$(" + dat.ToString() + ") = 0x" + dat.ToString("X3"));

                //キャラクタを表示する
                charaSet(dat);

                timCursor.Enabled = true;   //カーソルの割り込みを復帰させます
            }
            ArePrint.Set();
        }

        /// <summary>
        /// キャラクタデータを処理する
        /// </summary>
        /// <param name="dat"></param>
        private void charaSet(int dat)
        {
            bool RecvFlg = true;

            if (dat > 255)
            {
                RecvFlg = false;
                dat -= 0x100;
            }

            switch (Scrn.CmdStat)
            {
                case IchigoJamKey.VKeys.NON:
                    #region //********     特殊コードではない       ********//
                    if (dat == (int)IchigoJamKey.VKeys.ENTER)
                    #region
                    {                        
                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            //行末位置にカーソルを持っていきます
                            SearchLineEnd(ref Scrn.X, ref Scrn.Y);


                            //改行します
                            LF();
                        }
                        else
                        {
                            //ENTERを受信した
                            if (RecvFlg == true)
                            {
                                //改行します
                                LF();
                            }
                        }

                        Scrn.Cnt = 0;

                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.LOCATE)
                    #region
                    {
                        //LOCATEコマンド
                        Scrn.CmdStat = IchigoJamKey.VKeys.LOCATE;
                        Scrn.Cnt = 2;
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.P_UP)       //カーソルを画面最上部に持っていく
                    #region
                    {
                        Scrn.X = 0;
                        Scrn.Y = 0;
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.P_DOWN)     //カーソルを画面最下部に持っていく
                    #region
                    {
                        Scrn.X = 0;
                        Scrn.Y = Scrn.Height - 1;
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.HOME)       //カーソルを行の先頭に持っていく
                    #region
                    {
                        Scrn.X = 0;
                        while (Scrn.Y > 0)
                        {
                            //上の行の右端を調べる
                            if (Scrn.Vram[Scrn.Width - 1, Scrn.Y - 1] > 0)
                            {
                                Scrn.Y--;
                            }
                            else
                            {
                                break;
                            }
                        }
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.END)        //カーソルを行の最後に持っていく
                    #region
                    {
                        //行末位置を検索します
                        SearchLineEnd(ref Scrn.X, ref Scrn.Y);

                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.D_DELETE)   //カーソル以降全て削除
                    #region
                    {
                        //カーソル位置のチェック
                        if (Scrn.X >= Scrn.Width) { Scrn.X = Scrn.Width - 1; }

                        for (int x = Scrn.X; x < Scrn.Width; x++)
                        {
                            Scrn.Vram[x, Scrn.Y] = 0;
                        }

                        for (int y = Scrn.Y + 1; y < Scrn.Height; y++)
                        {
                            for (int x = 0; x < Scrn.Width; x++)
                            {
                                Scrn.Vram[x, y] = 0;
                            }
                        }
                        SetVRAM();
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.UP)         //カーソル上
                    #region
                    {
                        if (Scrn.X >= Scrn.Width) { Scrn.X = Scrn.Width - 1; }

                        Scrn.Y--;
                        if (Scrn.Y < 0) { Scrn.Y = 0; }

                        //文字のあるところまで
                        if (Scrn.Vram[Scrn.X, Scrn.Y] == 0)
                        {
                            int nx = Scrn.X;
                            Scrn.X = 0;
                            for (int x = nx; x >= 0; x--)
                            {
                                if (Scrn.Vram[x, Scrn.Y] > 0)
                                {
                                    Scrn.X = x + 1;
                                    break;
                                }
                            }
                        }
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.DOWN)       //カーソル下
                    #region
                    {
                        if (Scrn.X >= Scrn.Width) { Scrn.X = Scrn.Width - 1; }

                        Scrn.Y++;
                        if (Scrn.Y >= Scrn.Height) { Scrn.Y = Scrn.Height - 1; }

                        //文字のあるところまで
                        if (Scrn.Vram[Scrn.X, Scrn.Y] == 0)
                        {
                            int nx = Scrn.X;
                            Scrn.X = 0;
                            for (int x = nx; x >= 0; x--)
                            {
                                if (Scrn.Vram[x, Scrn.Y] > 0)
                                {
                                    Scrn.X = x + 1;
                                    break;
                                }
                            }
                        }
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.LEFT)       //カーソル左
                    #region
                    {
                        if (Scrn.X >= Scrn.Width)
                        {
                            Scrn.X = Scrn.Width - 1;
                        }
                        else
                        {
                            Scrn.X--;
                        }

                        if (Scrn.X < 0)
                        {
                            Scrn.X = 0;
                            if (Scrn.Y > 0)
                            {
                                //上の行を調べる
                                if (Scrn.Vram[Scrn.Width - 1, Scrn.Y - 1] > 0)
                                {
                                    Scrn.X = Scrn.Width - 1;
                                    Scrn.Y--;
                                }
                            }
                        }
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.RIGHT)      //カーソル右
                    #region
                    {
                        if (Scrn.X >= Scrn.Width)
                        {
                            if (Scrn.Y < Scrn.Height - 1)
                            {
                                Scrn.Y--;
                                Scrn.X = 0;
                            }
                            else
                            {
                                Scrn.X = Scrn.Width - 1;
                            }
                        }

                        if (Scrn.Vram[Scrn.X, Scrn.Y] > 0)
                        {
                            Scrn.X++;
                            if (Scrn.X >= Scrn.Width)
                            {
                                Scrn.X = Scrn.Width - 1;

                                if (Scrn.Y < Scrn.Height - 1)
                                {
                                    //下の行を調べる
                                    if (Scrn.Vram[0, Scrn.Y + 1] > 0)
                                    {
                                        Scrn.X = 0;
                                        Scrn.Y++;
                                    }
                                }
                            }
                        }
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.BS)         //BS
                    #region
                    {
                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            bool delFlg = true;

                            if (Scrn.X >= Scrn.Width)
                            {
                                Scrn.X = Scrn.Width - 1;
                            }
                            else if (Scrn.X == 0)
                            {
                                if (Scrn.Y > 0)
                                {
                                    if (Scrn.Vram[Scrn.Width - 1, Scrn.Y - 1] > 0)
                                    {
                                        Scrn.X = Scrn.Width - 1;
                                        Scrn.Y--;
                                    }
                                    else
                                    {
                                        delFlg = false;
                                    }
                                }
                                else
                                {
                                    delFlg = false;
                                }
                            }
                            else
                            {
                                Scrn.X--;
                            }

                            if (delFlg == true)
                            {
                                //Deleteします。
                                int nx = Scrn.X;
                                for (int y = Scrn.Y; y < Scrn.Height; y++)
                                {
                                    for (int x = nx; x < Scrn.Width - 1; x++)
                                    {
                                        Scrn.Vram[x, y] = Scrn.Vram[x + 1, y];
                                    }

                                    if (y == Scrn.Height - 1)
                                    {
                                        Scrn.Vram[Scrn.Width - 1, y] = 0;
                                        break;
                                    }

                                    if (Scrn.Vram[0, y + 1] == 0 || Scrn.Vram[Scrn.Width - 2, y] == 0)
                                    {
                                        Scrn.Vram[Scrn.Width - 1, y] = 0;
                                        break;
                                    }
                                    Scrn.Vram[Scrn.Width - 1, y] = Scrn.Vram[0, y + 1];
                                    nx = 0;
                                }
                                SetVRAM();
                            }
                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.DELETE)     //DEL
                    #region
                    {
                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            if (Scrn.X >= Scrn.Width)
                            {
                                Scrn.X = Scrn.Width - 1;
                            }

                            //Deleteします。
                            int nx = Scrn.X;
                            for (int y = Scrn.Y; y < Scrn.Height; y++)
                            {
                                for (int x = nx; x < Scrn.Width - 1; x++)
                                {
                                    Scrn.Vram[x, y] = Scrn.Vram[x + 1, y];
                                }

                                if (y == Scrn.Height - 1)
                                {
                                    Scrn.Vram[Scrn.Width - 1, y] = 0;
                                    break;
                                }

                                if (Scrn.Vram[0, y + 1] == 0 || Scrn.Vram[Scrn.Width - 2, y] == 0)
                                {
                                    Scrn.Vram[Scrn.Width - 1, y] = 0;
                                    break;
                                }
                                Scrn.Vram[Scrn.Width - 1, y] = Scrn.Vram[0, y + 1];
                                nx = 0;
                            }
                            SetVRAM();
                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.I_SPACE)    //スペースの挿入
                    #region
                    {
                        //強制スペース挿入
                        CursorFigure cf = CursorNow;
                        CursorNow = CursorFigure.Insert;
                        CharaPrint(0x20);
                        CursorNow = cf;

                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.KANA)       //かなモード
                    #region
                    {
                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.S_ENTER)    //行の分割
                    #region
                    {
                        if (CursorNow != CursorFigure.Non)
                        {
                            //行末を探す                            
                            int x = Scrn.X;
                            int y = Scrn.Y;
                            SearchLineEnd(ref x, ref y);

                            //現在の行頭から行末までをキャッシュする
                            List<int> cacheCode = new List<int>();
                            for (int iy = Scrn.Y; iy <= y; iy++)
                            {
                                for (int ix = 0; ix < Scrn.Width; ix++)
                                {
                                    cacheCode.Add(Scrn.Vram[ix, iy]);
                                }
                            }

                            //カーソルが一番上か、それとも、一番下の行が空行であれば、カーソル以下を下にスクロールする
                            if (Scrn.Y == 0 || (Scrn.Y<Scrn.Height-1 && Scrn.Vram[0, Scrn.Height - 1] == 0))
                            {
                                //現在の行の1つ下の行を空ける
                                InsertLine(Scrn.Y, 1);

                                //現在のカーソル位置より右側の文字を削除する
                                for (int ix = Scrn.X; ix < Scrn.Width; ix++)
                                {
                                    Scrn.Vram[ix, Scrn.Y] = 0;
                                }

                                Scrn.Y++;
                            }
                            else
                            {
                                //現在のカーソル位置より右側の文字を削除する
                                for (int ix = Scrn.X; ix < Scrn.Width; ix++)
                                {
                                    Scrn.Vram[ix, Scrn.Y] = 0;
                                }

                                //現在の行の1つ上の行を空ける
                                InsertLine(Scrn.Y + 1, 0);
                            }

                            x = 0;
                            y = Scrn.Y;

                            //キャッシュした文字のカーソル位置以降を書き戻す
                            for (int i = Scrn.X; i < cacheCode.Count; i++)
                            {
                                Scrn.Vram[x, y] = cacheCode[i];
                                x++;
                                if (x >= Scrn.Width)
                                {
                                    x = 0;
                                    y++;
                                    if (y >= Scrn.Height)
                                    {
                                        break;
                                    }
                                    if (Scrn.X > 0)
                                    {
                                        for (int ix = x; ix < Scrn.Width; ix++)
                                        {
                                            Scrn.Vram[ix, y] = 0;
                                        }
                                    }
                                }
                            }
                            Scrn.X = 0;

                            SetVRAM();
                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.INSERT)     //挿入/上書きの切り替え
                    #region
                    {
                        if (CursorNow != CursorFigure.Non)
                        {
                            if (CursorNow == CursorFigure.Insert)
                            {
                                CursorMode = CursorFigure.OverWrite;
                            }
                            else
                            {
                                CursorMode = CursorFigure.Insert;
                            }
                            CursorNow = CursorMode;

                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.L_DELETE)   //行削除
                    #region
                    {
                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            //行頭を探す
                            int y0 = SearchLineStart(Scrn.X, Scrn.Y);

                            //行末を探す                            
                            int x = Scrn.X;
                            int y1 = Scrn.Y;
                            SearchLineEnd(ref x, ref y1);

                            for (int iy = y0; iy <= y1; iy++)
                            {
                                for (int ix = 0; ix < Scrn.Width; ix++)
                                {
                                    Scrn.Vram[ix, iy] = 0;
                                }
                            }

                            Scrn.X = 0;
                            Scrn.Y = y0;

                            SetVRAM();

                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.ESC)        //エスケープキー
                    #region
                    {
                        //カーソルを出す
                        chkCursor.Checked = true;

                        Scrn.Cnt = 0;
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします
                    }
                    #endregion
                    else if (dat == (int)IchigoJamKey.VKeys.TAB)        //タブキー
                    #region
                    {
                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                CharaPrint(0x20);
                                Scrn.X++;

                                //カーソルが出ているとき
                                if (CursorNow != CursorFigure.Non)
                                {
                                    if (Scrn.X >= Scrn.Width)
                                    {
                                        LF();
                                    }
                                }
                            }
                            Scrn.Cnt = 0;
                            BlnCursorFlash = true;
                            ViewCursor();               //カーソルの処理をします
                        }
                    }
                    #endregion
                    else
                    {
                        //通常のキャラ表示をする
                        CharaPrint(dat);
                        Scrn.X++;

                        //カーソルが出ているとき
                        if (CursorNow != CursorFigure.Non)
                        {
                            if (Scrn.X >= Scrn.Width)
                            {
                                LF();
                            }
                        }
                        Scrn.Cnt = 0;
                    }
                    #endregion
                    break;

                case IchigoJamKey.VKeys.LOCATE:      //カーソル移動
                    #region //********     特殊コード(0x15)       ********//
                    if (dat >= (int)IchigoJamKey.VKeys.SPACE)
                    {
                        if (Scrn.Cnt == 2)
                        {
                            Scrn.X = dat - 0x20;
                            if (Scrn.X < 0)
                            {
                                Scrn.X = 0;
                            }
                            else if (Scrn.X >= Scrn.Width)
                            {
                                Scrn.X = Scrn.X - 1;
                            }
                        }
                        else if (Scrn.Cnt == 1)
                        {
                            Scrn.Y = dat - 0x20;
                            if (Scrn.Y < 0)
                            {
                                Scrn.Y = 0;
                            }
                            else if (Scrn.Y >= Scrn.Height)
                            {
                                Scrn.Y = Scrn.Height - 1;
                            }
                            Scrn.Cnt = 0;
                            Scrn.CmdStat = IchigoJamKey.VKeys.NON;
                        }
                        Scrn.Cnt--;
                    }
                    else
                    {
                        BlnCursorFlash = true;
                        ViewCursor();               //カーソルの処理をします

                        VramScroll((IchigoJamKey.VKeys)dat);

                        SetVRAM();

                        Scrn.CmdStat = IchigoJamKey.VKeys.NON;
                        Scrn.Cnt = 0;
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// 通常のキャラ表示
        /// </summary>
        /// <param name="dat"></param>
        private void CharaPrint(int dat)
        {
            //カーソルが出ていない。または、カーソルが出ていて
            if (IchigoJamKey.ChkKeyCode(dat, CursorNow))
            {
                //表示の前に、カーソル位置のチェック
                if (Scrn.X >= Scrn.Width)
                {
                    LF();
                }

                if (CursorNow == CursorFigure.Insert)
                {
                    #region //      挿入モード       //
                    //行の終わりを検索する
                    int x = Scrn.X;
                    int y = Scrn.Y;
                    SearchLineEnd(ref x, ref y);

                    int cacheCode = Scrn.Vram[x, y];

                    //文字を挿入します
                    for (int iy = y; iy > Scrn.Y; iy--)
                    {
                        for (int ix = Scrn.Width - 1; ix > 0; ix--)
                        {
                            Scrn.Vram[ix, iy] = Scrn.Vram[ix - 1, iy];
                        }
                        Scrn.Vram[0, iy] = Scrn.Vram[Scrn.Width - 1, iy - 1];
                    }

                    for (int ix = Scrn.Width - 1; ix > Scrn.X; ix--)
                    {
                        Scrn.Vram[ix, Scrn.Y] = Scrn.Vram[ix - 1, Scrn.Y];
                    }

                    Scrn.Vram[Scrn.X, Scrn.Y] = dat;

                    if (x == Scrn.Width - 1)
                    {
                        if (y < Scrn.Height - 1)
                        {
                            //最後尾のある行の下に1行挿入します
                            InsertLine(y, 1);
                        }
                        else
                        {
                            if (cacheCode == 0)
                            {
                                Scrn.Y--;
                                if (Scrn.Y < 0)
                                {
                                    Scrn.Y = 0;
                                }
                            }
                        }
                    }

                    SetLines(Scrn.Y, 0);
                    #endregion
                }
                else
                {
                    #region //      上書きモード       //
                    Scrn.Vram[Scrn.X, Scrn.Y] = dat;
                    SetChara(Scrn.X, Scrn.Y, dat);
                    #endregion
                }
            }
        }

        /// <summary>
        /// カーソルのある行が開始した行を探す
        /// </summary>
        /// <returns></returns>
        private static int SearchLineStart(int x, int y)
        {
            if (y == 0) { return 0; }

            y--;
            while (y >= 0)
            {
                for (int ix = 0; ix < Scrn.Width; ix++)
                {
                    if (Scrn.Vram[ix, y] == 0)
                    {
                        return y + 1;
                    }
                }
                y--;
            }
            return 0;
        }

        /// <summary>
        /// 行末位置を検索します
        /// </summary>
        private static void SearchLineEnd(ref int x, ref int y)
        {
            if (x == Scrn.Width)
            {
                x = Scrn.Width - 1;
            }

            while (y <= Scrn.Height - 1)
            {
                for (int ix = x; ix < Scrn.Width; ix++)
                {
                    if (Scrn.Vram[ix, y] == 0)
                    {
                        x = ix;
                        break;
                    }
                }

                if (Scrn.Vram[x, y] == 0)
                {
                    break;
                }
                x = 0;
                y++;
            }
            if (y >= Scrn.Height)
            {
                x = Scrn.Width - 1;
                y = Scrn.Height - 1;
            }
        }

        /// <summary>
        /// VRAMの内容をセットする
        /// </summary>
        private void SetVRAM()
        {
            for (int y = 0; y < Scrn.Height; y++)
            {
                for (int x = 0; x < Scrn.Width; x++)
                {
                    GrpSource.DrawImage(Chara[Scrn.Vram[x, y]], x * 8, y * 8, 8, 8);
                }
            }

            ImageSetting();     //pbx256x192.ImageをpbxMonitor.Imageにセットします
        }

        /// <summary>
        /// 指定した行から上または下だけ描画します
        /// </summary>
        /// <param name="ly"></param>
        private void SetLines(int ly, int mode)
        {
            if (mode == 0)
            {
                //下を表示
                for (int y = ly; y < Scrn.Height; y++)
                {
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        GrpSource.DrawImage(Chara[Scrn.Vram[x, y]], x * 8, y * 8, 8, 8);
                    }
                }
            }
            else
            {
                //上を表示
                for (int y = 0; y <= ly; y++)
                {
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        GrpSource.DrawImage(Chara[Scrn.Vram[x, y]], x * 8, y * 8, 8, 8);
                    }
                }
            }
            ImageSetting();     //pbx256x192.ImageをpbxMonitor.Imageにセットします
        }

        /// <summary>
        /// キャラクタを表示します
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="num"></param>
        private void SetChara(int x, int y, int num)
        {
            if (num < 0 || num > 255) { return; }

            GrpSource.DrawImage(Chara[num], x * 8, y * 8, 8, 8);

            if (BlnCursorFlash == true)
            {
                Bitmap bmpCharas = new Bitmap(pbx256x192.Image);
                Color cc;
                int gx;
                int gy;

                switch (CursorNow)
                {
                    case CursorFigure.Insert:
                        for (int j = 0; j < 8; j++)
                        {
                            gy = y * 8 + j;
                            for (int i = 0; i < 3; i++)
                            {
                                gx = x * 8 + i;
                                cc = bmpCharas.GetPixel(gx, gy);
                                if (cc.G == Color.White.G)
                                {
                                    GrpSource.FillRectangle(S_Black, gx, gy, 1, 1);
                                }
                                else
                                {
                                    GrpSource.FillRectangle(S_White, gx, gy, 1, 1);
                                }
                            }
                        }
                        bmpCharas.Dispose();
                        break;

                    case CursorFigure.OverWrite:
                        for (int j = 0; j < 8; j++)
                        {
                            gy = y * 8 + j;
                            for (int i = 0; i < 8; i++)
                            {
                                gx = x * 8 + i;
                                cc = bmpCharas.GetPixel(gx, gy);
                                if (cc.G == Color.White.G)
                                {
                                    GrpSource.FillRectangle(S_Black, gx, gy, 1, 1);
                                }
                                else
                                {
                                    GrpSource.FillRectangle(S_White, gx, gy, 1, 1);
                                }
                            }
                        }
                        bmpCharas.Dispose();
                        break;
                }
            }

            ImageSetting();     //pbx256x192.ImageをpbxMonitor.Imageにセットします
        }

        /// <summary>
        /// pbx256x192.ImageをpbxMonitor.Imageにセットします
        /// </summary>
        private void ImageSetting()
        {
            if (DtmInterval < DateTime.Now)
            {
                ImageRefresh();     //イメージを書き換えます

                DtmInterval = DateTime.Now.AddMilliseconds(1000.0 / (double)FrameRate);    //毎秒15枚の絵を表示する
            }
            //else
            //{
            //    Debug.WriteLine("PASSED: " + DateTime.Now.Millisecond.ToString());
            //}

            //タイマーによるリフレッシュをオンにします
            timRefresh.Enabled = true;
        }

        /// <summary>
        /// イメージを書き換えます
        /// </summary>
        private void ImageRefresh()
        {
            if (RefreshFlg == true)
            {
                AreImageTrace.WaitOne();
                {
                    while (true)
                    {
                        try
                        {
                            //pbxMonitor.Image = (Image)pbx256x192.Image.Clone();
                            pbxMonitor.Image = (Image)pbx256x192.Image;
                            break;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.StackTrace);
                            Debug.WriteLine(e.Message);
                            //Thread.Sleep(1);
                            pbxMonitor.Visible = false;
                        }
                    }
                    pbxMonitor.Visible = true;
                    //pbxMonitor.Refresh();
                }
                AreImageTrace.Set();
            }
        }

        /// <summary>
        /// 改行します
        /// </summary>
        private void LF()
        {
            Scrn.X = 0;
            Scrn.Y++;

            if (Scrn.Y >= Scrn.Height)
            {
                Scrn.Y = Scrn.Height - 1;
            }
        }

        /// <summary>
        /// カーソルのある行の上下に一行挿入する
        /// </summary>
        private void InsertLine(int ly, int mode)
        {
            if (mode == 1)
            {
                //下に1行挿入する
                for (int y = Scrn.Height - 2; y > ly; y--)
                {
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        Scrn.Vram[x, y + 1] = Scrn.Vram[x, y];
                    }
                }

                for (int x = 0; x < Scrn.Width; x++)
                {
                    Scrn.Vram[x, ly + 1] = 0;
                }
            }
            else
            {
                //上に一行挿入する
                for (int y = 1; y < ly; y++)
                {
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        Scrn.Vram[x, y - 1] = Scrn.Vram[x, y];
                    }
                }

                for (int x = 0; x < Scrn.Width; x++)
                {
                    Scrn.Vram[x, ly - 1] = 0;
                }
            }
        }

        /// <summary>
        /// 画面をリセットします
        /// </summary>
        private void ScreenReset()
        {
            //VRAMをクリアします
            for (int y = 0; y < Scrn.Height; y++)
            {
                for (int x = 0; x < Scrn.Width; x++)
                {
                    Scrn.Vram[x, y] = 0;
                }
            }
            Scrn.X = 0;
            Scrn.Y = 0;
            SetVRAM();

            //ESCキーコードを送ります
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.ESC);
            for (int i = 0; i < 120; i++)
            {
                Thread.Sleep(2);
                Application.DoEvents();
            }

            //カーソルを画面の一番上に持っていく
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(5);
                Application.DoEvents();
            }

            //カーソル以下を全て削除します
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.D_DELETE);
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(5);
                Application.DoEvents();
            }

            //カーソルを出します
            chkCursor.Checked = true;

            //画面のリフレッシュを止めます
            RefreshFlg = false;

            //挿入か上書きかを調べます
            {
                byte[] cmd = Encoding.ASCII.GetBytes("?PEEK(#901)");

                //スペースを書きます
                SerialTool.SendComm((byte)0x20);
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }

                //カーソルを画面の一番上に持っていく
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }

                //スペースを書きます
                SerialTool.SendComm((byte)0x20);
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }

                //カーソルを下に移動する
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.DOWN);
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }

                //(1,0)座標の文字を調べます
                for (int k = 0; k < cmd.Length; k++)
                {
                    SerialTool.SendComm(cmd[k]);
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(5);
                        Application.DoEvents();
                    }
                }
                SerialTool.SendComm(0x0A);
            }

            //画面のリフレッシュタイマーを使って、?PEEK(#901)の結果を調べます
            ResetFlg = true;                //画面リセットのチェックフラグを立てる
            timRefresh.Interval = 500;      //1000ms待つ
            timRefresh.Enabled = true;      //タイマースタート

            //フォーカスをpbxMonitorに持っていく
            pbxMonitor.Focus();
        }

        /// <summary>
        /// VRAMをスクロールします
        /// </summary>
        /// <param name="mode"></param>
        private void VramScroll(IchigoJamKey.VKeys ikey)
        {

            switch (ikey)
            {
                case IchigoJamKey.VKeys.UP:
                    for (int y = 1; y < Scrn.Height; y++)
                    {
                        for (int x = 0; x < Scrn.Width; x++)
                        {
                            Scrn.Vram[x, y - 1] = Scrn.Vram[x, y];
                        }
                    }
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        Scrn.Vram[x, Scrn.Height - 1] = 0;
                    }
                    break;
                case IchigoJamKey.VKeys.DOWN:
                    for (int y = Scrn.Height - 1; y >= 1; y--)
                    {
                        for (int x = 0; x < Scrn.Width; x++)
                        {
                            Scrn.Vram[x, y] = Scrn.Vram[x, y - 1];
                        }
                    }
                    for (int x = 0; x < Scrn.Width; x++)
                    {
                        Scrn.Vram[x, 0] = 0;
                    }
                    break;
                case IchigoJamKey.VKeys.LEFT:
                    for (int x = 1; x < Scrn.Width; x++)
                    {
                        for (int y = 0; y < Scrn.Height; y++)
                        {
                            Scrn.Vram[x - 1, y] = Scrn.Vram[x, y];
                        }
                    }
                    for (int y = 0; y < Scrn.Height; y++)
                    {
                        Scrn.Vram[Scrn.Width - 1, y] = 0;
                    }
                    break;
                case IchigoJamKey.VKeys.RIGHT:
                    for (int x = Scrn.Width - 1; x >= 1; x--)
                    {
                        for (int y = 0; y < Scrn.Height; y++)
                        {
                            Scrn.Vram[x, y] = Scrn.Vram[x - 1, y];
                        }
                    }
                    for (int y = 0; y < Scrn.Height; y++)
                    {
                        Scrn.Vram[0, y] = 0;
                    }
                    break;
            }


            //switch (mode)
            //{
            //    case 0:     //上にスクロール
            //        for (int y = 1; y < Scrn.Height; y++)
            //        {
            //            for (int x = 0; x < Scrn.Width; x++)
            //            {
            //                Scrn.Vram[x, y - 1] = Scrn.Vram[x, y];
            //            }
            //        }
            //        for (int x = 0; x < Scrn.Width; x++)
            //        {
            //            Scrn.Vram[x, Scrn.Height - 1] = 0;
            //        }
            //        break;

            //    case 1:     //下にスクロール
            //        for (int y = Scrn.Height - 1; y >= 1; y--)
            //        {
            //            for (int x = 0; x < Scrn.Width; x++)
            //            {
            //                Scrn.Vram[x, y] = Scrn.Vram[x, y - 1];
            //            }
            //        }
            //        for (int x = 0; x < Scrn.Width; x++)
            //        {
            //            Scrn.Vram[x, 0] = 0;
            //        }
            //        break;
            //}

        }

        /// <summary>
        /// 画面のリフレッシュタイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timRefresh_Tick(object sender, EventArgs e)
        {
            timRefresh.Enabled = false;

            ImageRefresh();     //イメージを書き換えます

            timRefresh.Interval = 1000 / FrameRate;

            //画面リセットした後の?PEEK(#901)の結果をみるフラグ
            if (ResetFlg == true)
            {
                ResetFlg = false;
                if (Scrn.Y - 2 >= 0)
                {
                    if (Scrn.Vram[0, Scrn.Y - 2] == 0x33)
                    {
                        //挿入でした
                        CursorMode = CursorFigure.Insert;
                        CursorNow = CursorMode;
                    }
                    else
                    {
                        //上書きでした
                        CursorMode = CursorFigure.OverWrite;
                        CursorNow = CursorMode;
                    }
                }

                //カーソルを画面の一番上に持っていく
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
                Thread.Sleep(20);

                //カーソル以下を全て削除します
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.D_DELETE);
                Thread.Sleep(20);

                //画面のリフレッシュを復活させる
                RefreshFlg = true;
            }
        }

        /// <summary>
        /// カーソルを点滅させるタイマーです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timCursor_Tick(object sender, EventArgs e)
        {
            ViewCursor();   //カーソルを表示します

            //反転させます
            BlnCursorFlash = !BlnCursorFlash;
        }

        /// <summary>
        /// カーソルを表示します
        /// </summary>
        private void ViewCursor()
        {
            if (CursorNow == CursorFigure.Non)
            {
                //trueだったら、今は消えている
                if (BlnCursorFlash)
                {
                    return;
                }

                if (Scrn.X > 0 && Scrn.X < Scrn.Width && Scrn.Y > 0 && Scrn.Y < Scrn.Height)
                {
                    BlnCursorFlash = false;
                    SetChara(Scrn.X, Scrn.Y, Scrn.Vram[Scrn.X, Scrn.Y]);
                }
                return;
            }

            //カーソルを表示します
            if (Scrn.X >= 0 && Scrn.X < Scrn.Width && Scrn.Y >= 0 && Scrn.Y < Scrn.Height)
            {
                SetChara(Scrn.X, Scrn.Y, Scrn.Vram[Scrn.X, Scrn.Y]);
            }
        }

        /// <summary>
        /// テスト表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuTEST_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 256; i++)
            {
                x = i % 32;
                y = i / 32;

                Scrn.Vram[x, y] = i;
            }

            SetVRAM();
        }
        #endregion

        #region //********     フォームのアクティブイベント       ********//
        /// <summary>
        /// フォームがアクティブになったとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_Activated(object sender, EventArgs e)
        {
            BlnMonitorActive = true;
        }
        /// <summary>
        /// フォームがアクティブでなくなったとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_Deactivate(object sender, EventArgs e)
        {
            BlnMonitorActive = false;
        }
        #endregion

        #region //********     オプションフォームの処理       ********//
        private void MnuOption_Click(object sender, EventArgs e)
        {
            //スクリーンが使えない状態ということをセットする
            Scrn.IsReady = false;

            //インスタンスを作成する
            OptionMonitorForm oform = new OptionMonitorForm();

            //オーナーウィンドウにthisを指定する
            oform.ShowDialog(this);

            //フォームが必要なくなったところで、Disposeを呼び出す
            oform.Dispose();

            //スクリーンが使える状態ということをセットする
            Scrn.IsReady = true;
        }

        /// <summary>
        /// カーソル表示ボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCursor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCursor.Checked == true)
            {
                if (CursorMode == CursorFigure.Insert)
                {
                    CursorNow = CursorFigure.Insert;

                }
                else
                {
                    CursorNow = CursorFigure.OverWrite;
                }
                chkMouse.Enabled = true;
            }
            else
            {
                CursorNow = CursorFigure.Non;
                chkMouse.Enabled = false;
                SetVRAM();
            }
        }

        /// <summary>
        /// 挿入モードが選択された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuCurInsert_Click(object sender, EventArgs e)
        {
            CursorMode = CursorFigure.Insert;
            CursorNow = CursorMode;
        }

        /// <summary>
        /// 上書きモードが選択された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuCurOverWrite_Click(object sender, EventArgs e)
        {
            CursorMode = CursorFigure.OverWrite;
            CursorNow = CursorMode;
        }

        #endregion

        #region //********     マウスの処理       ********//
        /// <summary>
        /// RESETボタンがマウスでクリックされた
        /// 画面をシンクロするためにリセットします
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_MouseClick(object sender, MouseEventArgs e)
        {
            //画面をシンクロするためにリセットします
            ScreenReset();
        }

        /// <summary>
        /// モニタ画面がマウスでクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxMonitor_MouseClick(object sender, MouseEventArgs e)
        {
            //カーソルが出ているとき
            if (CursorNow != CursorFigure.Non && chkMouse.Checked == true)
            {
                int cx = (int)((double)e.X / (double)pbxMonitor.Width * (double)Scrn.Width);
                int cy = (int)((double)e.Y / (double)pbxMonitor.Height * (double)Scrn.Height);

                //カーソルを画面の一番上に持っていく
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
                Thread.Sleep(20);

                //カーソルを下げる
                for (int i = 0; i < cy; i++)
                {
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.DOWN);
                    Thread.Sleep(20);
                }

                //カーソルを右に動かす
                for (int i = 0; i < cx; i++)
                {
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.RIGHT);
                    Thread.Sleep(20);
                }

            }
        }

        #endregion

        #region //********     キー入力の処理       ********//
        /// <summary>
        /// キーイベントを処理する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorForm_KeyDown(object sender, KeyEventArgs e)
        {
            //キーを殺す
            e.Handled = true;
        }
        #endregion

    }
}
