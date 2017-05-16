using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IchigoJamKeyBoard
{
    class Scrn
    {
        public static int Width = 32;                           //スクリーンの幅
        public static int Height = 24;                          //スクリーンの高さ
        public static bool IsReady = false;                     //スクリーンが使える状態を示す

        public static int[,] Vram = new int[Width, Height];     //画面のキャラクタデータを保持している仮想画面
        public static int X = 0;                                //カーソル位置 X
        public static int Y = 0;                                //カーソル位置 Y

        public static int Cnt = 0;                                          //コマンド後の取得データ数　　0x15が来たら CmdStat=LOCATE にして Cnt=2にして、あと2バイト取得してコマンドを処理する
        public static IchigoJamKey.VKeys CmdStat = IchigoJamKey.VKeys.NON;  //現在受け付けているコマンド
    }

    class VRam
    {
    }
}
