using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IchigoJamKeyBoard
{
    /// <summary>
    /// シリアル通信パラメータ
    /// </summary>
    public class Commparameter
    {
        public string PortName = "COM1";                    //ポート名(P/Invokeのように末尾に':'は付けない)
        public int BaudRate = 115200;                       //ボーレート
        public int DataBits = 8;                            //データビット
        public StopBits enmStopBits = StopBits.One;         //ストップビット
        public Parity enmParity = Parity.None;              //パリティ
        public Handshake emnHandshake = Handshake.None;     //ハンドシェイク
        public int Timeout = 1000;                          //タイムアウト(ms)
        public bool Dtr = false;                            //Data Terminal Readyの使用有無
        public bool Rts = false;                            //Request to Sendの使用有無
    }

    /// <summary>
    /// 受信ステータス
    /// </summary>
    public enum ReceiveStats
    {
        MainformProcessed,  //メインフォームで処理しているところです
        MainformFinish,     //メインフォームでの処理が終了しました
        Receiving,          //データを受信中です。
        NewData,            //新規受信データが存在します
    }

    class SerialTool
    {
        /// <summary>
        /// CommPortを排他的に使うために用意
        /// </summary>
        private static AutoResetEvent SerialLock = new AutoResetEvent(true);

        public static Queue SerialDataQueue = new Queue();                      //シリアルデータ保存キュー
        public static AutoResetEvent QueueLock = new AutoResetEvent(true);      //シリアルデータ保存キュー排他処理用


        /// <summary>
        /// シリアル通信ポート
        /// </summary>
        public static SerialPort CommPort = new SerialPort();           //通信ポート

        static public ReceiveStats enmReceiveStats = ReceiveStats.MainformFinish;

        /// <summary>
        /// シリアルポートのパラメータ
        /// </summary>
        static public Commparameter CommParam = new Commparameter();    //シリアルポートのパラメータ

        static public bool ReciveTimeOutFlag = false;                   //受信がタイムアウトしたフラグ

        static public string RecvDataText = "";                         //受信データ

        public static int[] Baurate = { 9600, 19200, 38400, 115200, 230400 };

        private static byte[] Keydata = new byte[1];

        public static List<byte> RecvSave = new List<byte>();       //保存データ受信用

        private static int RecvMethodNumber = -1;                   //受信メソッドの番号

        /// <summary>
        /// CommPortに送信します
        /// </summary>
        internal static void SendComm(byte bdata)
        {
            Keydata[0] = bdata;
            if (CommPort == null) { return; }
            if (CommPort.IsOpen == false) { return; }

            if (Scrn.IsReady == true)
            {
                QueueLock.WaitOne();
                {
                    SerialDataQueue.Enqueue((int)bdata + 256);
                }
                QueueLock.Set();
            }

            SerialLock.WaitOne();             //ロックウェイト&ロック
            {
                CommPort.Write(Keydata, 0, 1);
            }
            SerialLock.Set();                 //ロックの解除
        }

        /// <summary>
        /// シリアルポートの設定と初期化
        /// </summary>
        /// <param name="comm">シリアルポート</param>
        /// <returns>ポートの設定ができたかどうか</returns>
        internal static bool CommPortIni(string portname, int bau)
        {
            //COMMポートを閉じる(既にオープンしていたら)
            if (CommPort.IsOpen == true) { SerialTool.CommPortClose(); }

            SerialLock.WaitOne(); //ロックウェイト&ロック

            try
            {
                //シリアルポートパラメータの読み込み(XMLデータから読み込みます)
                CommParam.PortName = portname;

                CommPort.PortName = CommParam.PortName;
                CommPort.BaudRate = bau;
                CommPort.DataBits = CommParam.DataBits;
                CommPort.StopBits = CommParam.enmStopBits;
                CommPort.Parity = CommParam.enmParity;
                CommPort.Handshake = CommParam.emnHandshake;

                CommPort.ReadTimeout = CommParam.Timeout;      //タイムアウト750ms
                CommPort.WriteTimeout = CommParam.Timeout;     //タイムアウト750ms

                CommPort.DtrEnable = CommParam.Dtr;
                CommPort.RtsEnable = CommParam.Rts;

                //CommPort.DataReceived += new SerialDataReceivedEventHandler(AnyReceived);
                ChangeDataReceivedMethod(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

            bool ret = true;
            try
            {
                //ポートオープン
                CommPort.Open();
            }
            catch
            {
                MessageBox.Show(ValiableList.errConnect, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = false;
            }

            SerialLock.Set(); //ロックの解除
            return (ret);
        }

        /// <summary>
        /// シリアルポートを閉じます
        /// </summary>
        internal static void CommPortClose()
        {
            SerialLock.WaitOne(); //ロックウェイト&ロック
            {
                CommPort.Close();
                //CommPort.DataReceived -= new SerialDataReceivedEventHandler(AnyReceived);
                ChangeDataReceivedMethod(-1);
                CommPort.Dispose();
            }
            SerialLock.Set();     //ロックの解除
        }

        internal static bool IsCommPort()
        {
            return CommPort.IsOpen;
        }

        /// <summary>
        /// 受信があると呼ばれる
        /// MethodNumber = 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AnyReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //メインフォームが処理中なら待つ
            while (enmReceiveStats != ReceiveStats.MainformFinish)
            {
                Application.DoEvents();
            }

            //シリアルデータ受信中にする
            enmReceiveStats = ReceiveStats.Receiving;

            Byte[] buf = new Byte[512];

            //200ms待つ
            Thread.Sleep(200);

            RecvDataText = "";  //受信データの初期化

            int len = 0;
            SerialLock.WaitOne();  //ロックウェイト&ロック
            try
            {
                len = CommPort.Read(buf, 0, 512);
            }
            catch { }

            SerialLock.Set();    //ロック解除

            List<byte> rec = new List<byte>();

            for (int i = 0; i < len; i++)
            {
                if (Scrn.IsReady == true)
                {
                    QueueLock.WaitOne();
                    {
                        SerialDataQueue.Enqueue((int)buf[i]);
                    }
                    QueueLock.Set();
                }

                if (buf[i] != 0)
                {
                    if (buf[i] == 0x0A)
                    {
                        rec.Add(0x0D);
                        rec.Add(0x0A);
                    }
                    else
                    {
                        rec.Add(buf[i]);
                        //RecvDataText += ((char)buf[i]).ToString();
                    }
                }
            }

            RecvDataText = Encoding.GetEncoding(932).GetString(rec.ToArray());

            enmReceiveStats = ReceiveStats.NewData;
        }

        /// <summary>
        /// データ保存用に受信があると呼ばれる
        /// MethodNumber = 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AnyReceiveForSave(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] buf = new Byte[512];

            //200ms待つ
            Thread.Sleep(200);

            int len = 0;
            SerialLock.WaitOne();  //ロックウェイト&ロック
            try
            {
                len = CommPort.Read(buf, 0, 512);
            }
            catch { }

            SerialLock.Set();    //ロック解除

            for (int i = 0; i < len; i++)
            {
                RecvSave.Add(buf[i]);
            }
        }

        /// <summary>
        /// 受信処理用メソッドを切り替えます
        /// </summary>
        /// <param name="num"></param>
        public static void ChangeDataReceivedMethod(int num)
        {
            if (num == RecvMethodNumber)
            {
                return;
            }

            if (RecvMethodNumber >= 0)
            {
                switch (RecvMethodNumber)
                {
                    case 0:
                        CommPort.DataReceived -= new SerialDataReceivedEventHandler(AnyReceived);
                        break;

                    case 1:
                        CommPort.DataReceived -= new SerialDataReceivedEventHandler(AnyReceiveForSave);
                        break;
                }
            }

            if (num < 0)
            {
                RecvMethodNumber = -1;
                return;
            }

            switch (num)
            {
                case 0:
                    CommPort.DataReceived += new SerialDataReceivedEventHandler(AnyReceived);
                    break;

                case 1:
                    CommPort.DataReceived += new SerialDataReceivedEventHandler(AnyReceiveForSave);
                    break;
            }

            RecvMethodNumber = num;
        }

    }
}
