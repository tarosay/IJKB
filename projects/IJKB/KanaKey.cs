using RamGecTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IchigoJamKeyBoard
{
    class KanaChara
    {
        public char code = (char)0x0;
        public string kana = "";
    }

    class Keyboards
    {
        public static bool ShiftKey = false;        //シフトキーが押されている
        public static bool CtrlKey = false;         //コントロールキーが押されている
    }

    class FKey
    {
        public static string[] fkey = { "cls\\n", "load ", "save ", "list\\n", "run\\n", "?free()", "OUT 0\\n", "video 1", "files\\n", "goto" };
    }

    class KanaKey
    {
        public static int KanaMode = 0;
        public static bool EisuuInputFlg = true;
        public static int KanaRO = 0;               //0:'ﾛ'でも'ｰ'でもない 226:ﾛ, 220:ｰ

        private static KanaChara[] Kana = new KanaChara[256];
        private static KanaChara[] Eisu = new KanaChara[256];
        private static KanaChara[] ShiftEisu = new KanaChara[256];

        /// <summary>
        /// カナキーデータを初期化します
        /// </summary>
        internal static void KanaKeyInit()
        {
            for (int i = 0; i < 256; i++)
            {
                KanaChara kc = new KanaChara();
                Kana[i] = kc;

                kc = new KanaChara();
                Eisu[i] = kc;

                kc = new KanaChara();
                ShiftEisu[i] = kc;
            }

            Kana[(char)"3"[0]].kana = "ｱ";
            Kana[(char)"e"[0]].kana = "ｲ";
            Kana[(char)"4"[0]].kana = "ｳ";
            Kana[(char)"5"[0]].kana = "ｴ";
            Kana[(char)"6"[0]].kana = "ｵ";
            Kana[(char)"t"[0]].kana = "ｶ";
            Kana[(char)"g"[0]].kana = "ｷ";
            Kana[(char)"h"[0]].kana = "ｸ";
            Kana[(char)":"[0]].kana = "ｹ";
            Kana[(char)"b"[0]].kana = "ｺ";
            Kana[(char)"x"[0]].kana = "ｻ";
            Kana[(char)"d"[0]].kana = "ｼ";
            Kana[(char)"r"[0]].kana = "ｽ";
            Kana[(char)"p"[0]].kana = "ｾ";
            Kana[(char)"c"[0]].kana = "ｿ";
            Kana[(char)"q"[0]].kana = "ﾀ";
            Kana[(char)"a"[0]].kana = "ﾁ";
            Kana[(char)"z"[0]].kana = "ﾂ";
            Kana[(char)"w"[0]].kana = "ﾃ";
            Kana[(char)"s"[0]].kana = "ﾄ";
            Kana[(char)"u"[0]].kana = "ﾅ";
            Kana[(char)"i"[0]].kana = "ﾆ";
            Kana[(char)"1"[0]].kana = "ﾇ";
            Kana[(char)","[0]].kana = "ﾈ";
            Kana[(char)"k"[0]].kana = "ﾉ";
            Kana[(char)"f"[0]].kana = "ﾊ";
            Kana[(char)"v"[0]].kana = "ﾋ";
            Kana[(char)"2"[0]].kana = "ﾌ";
            Kana[(char)"^"[0]].kana = "ﾍ";
            Kana[(char)"-"[0]].kana = "ﾎ";
            Kana[(char)"j"[0]].kana = "ﾏ";
            Kana[(char)"n"[0]].kana = "ﾐ";
            Kana[(char)"]"[0]].kana = "ﾑ";
            Kana[(char)"/"[0]].kana = "ﾒ";
            Kana[(char)"m"[0]].kana = "ﾓ";
            Kana[(char)"7"[0]].kana = "ﾔ";
            Kana[(char)"8"[0]].kana = "ﾕ";
            Kana[(char)"9"[0]].kana = "ﾖ";
            Kana[(char)"o"[0]].kana = "ﾗ";
            Kana[(char)"l"[0]].kana = "ﾘ";
            Kana[(char)"."[0]].kana = "ﾙ";
            Kana[(char)";"[0]].kana = "ﾚ";
            Kana[226].kana = "ﾛ";
            Kana[(char)"0"[0]].kana = "ﾜ";
            Kana[(char)"y"[0]].kana = "ﾝ";
            Kana[(char)"@"[0]].kana = "ﾞ";
            Kana[(char)"["[0]].kana = "ﾟ";

            Kana[(char)" "[0]].kana = " ";

            Kana[220].kana = "ｰ";
            Kana[(char)">"[0]].kana = "｡";
            Kana[(char)"{"[0]].kana = "｢";
            Kana[(char)"}"[0]].kana = "｣";
            Kana[(char)"<"[0]].kana = "､";
            Kana[(char)"?"[0]].kana = "･";

            Kana[166].kana = "ｦ";
            Kana[(char)"#"[0]].kana = "ｧ";
            Kana[(char)"E"[0]].kana = "ｨ";
            Kana[(char)"$"[0]].kana = "ｩ";
            Kana[(char)"%"[0]].kana = "ｪ";
            Kana[(char)"&"[0]].kana = "ｫ";
            Kana[(char)"'"[0]].kana = "ｬ";
            Kana[(char)"("[0]].kana = "ｭ";
            Kana[(char)")"[0]].kana = "ｮ";
            Kana[(char)"Z"[0]].kana = "ｯ";

            Kana[(char)"3"[0]].code = (char)0xB1;
            Kana[(char)"e"[0]].code = (char)0xB2;
            Kana[(char)"4"[0]].code = (char)0xB3;
            Kana[(char)"5"[0]].code = (char)0xB4;
            Kana[(char)"6"[0]].code = (char)0xB5;
            Kana[(char)"t"[0]].code = (char)0xB6;
            Kana[(char)"g"[0]].code = (char)0xB7;
            Kana[(char)"h"[0]].code = (char)0xB8;
            Kana[(char)":"[0]].code = (char)0xB9;
            Kana[(char)"b"[0]].code = (char)0xBA;
            Kana[(char)"x"[0]].code = (char)0xBB;
            Kana[(char)"d"[0]].code = (char)0xBC;
            Kana[(char)"r"[0]].code = (char)0xBD;
            Kana[(char)"p"[0]].code = (char)0xBE;
            Kana[(char)"c"[0]].code = (char)0xBF;
            Kana[(char)"q"[0]].code = (char)0xC0;
            Kana[(char)"a"[0]].code = (char)0xC1;
            Kana[(char)"z"[0]].code = (char)0xC2;
            Kana[(char)"w"[0]].code = (char)0xC3;
            Kana[(char)"s"[0]].code = (char)0xC4;
            Kana[(char)"u"[0]].code = (char)0xC5;
            Kana[(char)"i"[0]].code = (char)0xC6;
            Kana[(char)"1"[0]].code = (char)0xC7;
            Kana[(char)","[0]].code = (char)0xC8;
            Kana[(char)"k"[0]].code = (char)0xC9;
            Kana[(char)"f"[0]].code = (char)0xCA;
            Kana[(char)"v"[0]].code = (char)0xCB;
            Kana[(char)"2"[0]].code = (char)0xCC;
            Kana[(char)"^"[0]].code = (char)0xCD;
            Kana[(char)"-"[0]].code = (char)0xCE;
            Kana[(char)"j"[0]].code = (char)0xCF;
            Kana[(char)"n"[0]].code = (char)0xD0;
            Kana[(char)"]"[0]].code = (char)0xD1;
            Kana[(char)"/"[0]].code = (char)0xD2;
            Kana[(char)"m"[0]].code = (char)0xD3;
            Kana[(char)"7"[0]].code = (char)0xD4;
            Kana[(char)"8"[0]].code = (char)0xD5;
            Kana[(char)"9"[0]].code = (char)0xD6;
            Kana[(char)"o"[0]].code = (char)0xD7;
            Kana[(char)"l"[0]].code = (char)0xD8;
            Kana[(char)"."[0]].code = (char)0xD9;
            Kana[(char)";"[0]].code = (char)0xDA;
            Kana[226].code = (char)0xDB;
            Kana[(char)"0"[0]].code = (char)0xDC;
            Kana[(char)"y"[0]].code = (char)0xDD;
            Kana[(char)"@"[0]].code = (char)0xDE;
            Kana[(char)"["[0]].code = (char)0xDF;

            Kana[(char)" "[0]].code = (char)0x20;

            Kana[220].code = (char)0xB0;
            Kana[(char)">"[0]].code = (char)0xA1;
            Kana[(char)"{"[0]].code = (char)0xA2;
            Kana[(char)"}"[0]].code = (char)0xA3;
            Kana[(char)"<"[0]].code = (char)0xA4;
            Kana[(char)"?"[0]].code = (char)0xA5;

            Kana[166].code = (char)0xA6;
            Kana[(char)"#"[0]].code = (char)0xA7;
            Kana[(char)"E"[0]].code = (char)0xA8;
            Kana[(char)"$"[0]].code = (char)0xA9;
            Kana[(char)"%"[0]].code = (char)0xAA;
            Kana[(char)"&"[0]].code = (char)0xAB;
            Kana[(char)"'"[0]].code = (char)0xAC;
            Kana[(char)"("[0]].code = (char)0xAD;
            Kana[(char)")"[0]].code = (char)0xAE;
            Kana[(char)"Z"[0]].code = (char)0xAF;

            //0x30～0x39
            Eisu[(int)KeyboardHook.VKeys.KEY_1].kana = "1";
            Eisu[(int)KeyboardHook.VKeys.KEY_2].kana = "2";
            Eisu[(int)KeyboardHook.VKeys.KEY_3].kana = "3";
            Eisu[(int)KeyboardHook.VKeys.KEY_4].kana = "4";
            Eisu[(int)KeyboardHook.VKeys.KEY_5].kana = "5";
            Eisu[(int)KeyboardHook.VKeys.KEY_6].kana = "6";
            Eisu[(int)KeyboardHook.VKeys.KEY_7].kana = "7";
            Eisu[(int)KeyboardHook.VKeys.KEY_8].kana = "8";
            Eisu[(int)KeyboardHook.VKeys.KEY_9].kana = "9";
            Eisu[(int)KeyboardHook.VKeys.KEY_0].kana = "0";
            Eisu[(int)KeyboardHook.VKeys.KEY_1].code = (char)0x31;
            Eisu[(int)KeyboardHook.VKeys.KEY_2].code = (char)0x32;
            Eisu[(int)KeyboardHook.VKeys.KEY_3].code = (char)0x33;
            Eisu[(int)KeyboardHook.VKeys.KEY_4].code = (char)0x34;
            Eisu[(int)KeyboardHook.VKeys.KEY_5].code = (char)0x35;
            Eisu[(int)KeyboardHook.VKeys.KEY_6].code = (char)0x36;
            Eisu[(int)KeyboardHook.VKeys.KEY_7].code = (char)0x37;
            Eisu[(int)KeyboardHook.VKeys.KEY_8].code = (char)0x38;
            Eisu[(int)KeyboardHook.VKeys.KEY_9].code = (char)0x39;
            Eisu[(int)KeyboardHook.VKeys.KEY_0].code = (char)0x30;

            ShiftEisu[(int)KeyboardHook.VKeys.KEY_1].kana = "!";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_2].kana = "\"";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_3].kana = "#";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_4].kana = "$";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_5].kana = "%";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_6].kana = "&";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_7].kana = "'";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_8].kana = "(";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_9].kana = ")";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_0].kana = "";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_1].code = (char)0x21;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_2].code = (char)0x22;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_3].code = (char)0x23;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_4].code = (char)0x24;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_5].code = (char)0x25;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_6].code = (char)0x26;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_7].code = (char)0x27;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_8].code = (char)0x28;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_9].code = (char)0x29;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_0].code = (char)0x0;

            //AからZは0x41～0x4A a～zは0x61～0x7A
            Eisu[(int)KeyboardHook.VKeys.KEY_A].kana = "a";
            Eisu[(int)KeyboardHook.VKeys.KEY_B].kana = "b";
            Eisu[(int)KeyboardHook.VKeys.KEY_C].kana = "c";
            Eisu[(int)KeyboardHook.VKeys.KEY_D].kana = "d";
            Eisu[(int)KeyboardHook.VKeys.KEY_E].kana = "e";
            Eisu[(int)KeyboardHook.VKeys.KEY_F].kana = "f";
            Eisu[(int)KeyboardHook.VKeys.KEY_G].kana = "g";
            Eisu[(int)KeyboardHook.VKeys.KEY_H].kana = "h";
            Eisu[(int)KeyboardHook.VKeys.KEY_I].kana = "i";
            Eisu[(int)KeyboardHook.VKeys.KEY_J].kana = "j";
            Eisu[(int)KeyboardHook.VKeys.KEY_K].kana = "k";
            Eisu[(int)KeyboardHook.VKeys.KEY_L].kana = "l";
            Eisu[(int)KeyboardHook.VKeys.KEY_M].kana = "m";
            Eisu[(int)KeyboardHook.VKeys.KEY_N].kana = "n";
            Eisu[(int)KeyboardHook.VKeys.KEY_O].kana = "o";
            Eisu[(int)KeyboardHook.VKeys.KEY_P].kana = "p";
            Eisu[(int)KeyboardHook.VKeys.KEY_Q].kana = "q";
            Eisu[(int)KeyboardHook.VKeys.KEY_R].kana = "r";
            Eisu[(int)KeyboardHook.VKeys.KEY_S].kana = "s";
            Eisu[(int)KeyboardHook.VKeys.KEY_T].kana = "t";
            Eisu[(int)KeyboardHook.VKeys.KEY_U].kana = "u";
            Eisu[(int)KeyboardHook.VKeys.KEY_V].kana = "v";
            Eisu[(int)KeyboardHook.VKeys.KEY_W].kana = "w";
            Eisu[(int)KeyboardHook.VKeys.KEY_X].kana = "x";
            Eisu[(int)KeyboardHook.VKeys.KEY_Y].kana = "y";
            Eisu[(int)KeyboardHook.VKeys.KEY_Z].kana = "z";

            Eisu[(int)KeyboardHook.VKeys.KEY_A].code = (char)0x61;
            Eisu[(int)KeyboardHook.VKeys.KEY_B].code = (char)0x62;
            Eisu[(int)KeyboardHook.VKeys.KEY_C].code = (char)0x63;
            Eisu[(int)KeyboardHook.VKeys.KEY_D].code = (char)0x64;
            Eisu[(int)KeyboardHook.VKeys.KEY_E].code = (char)0x65;
            Eisu[(int)KeyboardHook.VKeys.KEY_F].code = (char)0x66;
            Eisu[(int)KeyboardHook.VKeys.KEY_G].code = (char)0x67;
            Eisu[(int)KeyboardHook.VKeys.KEY_H].code = (char)0x68;
            Eisu[(int)KeyboardHook.VKeys.KEY_I].code = (char)0x69;
            Eisu[(int)KeyboardHook.VKeys.KEY_J].code = (char)0x6A;
            Eisu[(int)KeyboardHook.VKeys.KEY_K].code = (char)0x6B;
            Eisu[(int)KeyboardHook.VKeys.KEY_L].code = (char)0x6C;
            Eisu[(int)KeyboardHook.VKeys.KEY_M].code = (char)0x6D;
            Eisu[(int)KeyboardHook.VKeys.KEY_N].code = (char)0x6E;
            Eisu[(int)KeyboardHook.VKeys.KEY_O].code = (char)0x6F;
            Eisu[(int)KeyboardHook.VKeys.KEY_P].code = (char)0x70;
            Eisu[(int)KeyboardHook.VKeys.KEY_Q].code = (char)0x71;
            Eisu[(int)KeyboardHook.VKeys.KEY_R].code = (char)0x72;
            Eisu[(int)KeyboardHook.VKeys.KEY_S].code = (char)0x73;
            Eisu[(int)KeyboardHook.VKeys.KEY_T].code = (char)0x74;
            Eisu[(int)KeyboardHook.VKeys.KEY_U].code = (char)0x75;
            Eisu[(int)KeyboardHook.VKeys.KEY_V].code = (char)0x76;
            Eisu[(int)KeyboardHook.VKeys.KEY_W].code = (char)0x77;
            Eisu[(int)KeyboardHook.VKeys.KEY_X].code = (char)0x78;
            Eisu[(int)KeyboardHook.VKeys.KEY_Y].code = (char)0x79;
            Eisu[(int)KeyboardHook.VKeys.KEY_Z].code = (char)0x7A;

            ShiftEisu[(int)KeyboardHook.VKeys.KEY_A].kana = "A";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_B].kana = "B";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_C].kana = "C";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_D].kana = "D";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_E].kana = "E";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_F].kana = "F";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_G].kana = "G";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_H].kana = "H";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_I].kana = "I";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_J].kana = "J";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_K].kana = "K";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_L].kana = "L";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_M].kana = "M";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_N].kana = "N";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_O].kana = "O";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_P].kana = "P";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Q].kana = "Q";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_R].kana = "R";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_S].kana = "S";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_T].kana = "T";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_U].kana = "U";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_V].kana = "V";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_W].kana = "W";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_X].kana = "X";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Y].kana = "Y";
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Z].kana = "Z";

            ShiftEisu[(int)KeyboardHook.VKeys.KEY_A].code = (char)0x41;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_B].code = (char)0x42;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_C].code = (char)0x43;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_D].code = (char)0x44;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_E].code = (char)0x45;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_F].code = (char)0x46;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_G].code = (char)0x47;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_H].code = (char)0x48;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_I].code = (char)0x49;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_J].code = (char)0x4A;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_K].code = (char)0x4B;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_L].code = (char)0x4C;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_M].code = (char)0x4D;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_N].code = (char)0x4E;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_O].code = (char)0x4F;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_P].code = (char)0x50;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Q].code = (char)0x51;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_R].code = (char)0x52;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_S].code = (char)0x53;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_T].code = (char)0x54;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_U].code = (char)0x55;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_V].code = (char)0x56;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_W].code = (char)0x57;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_X].code = (char)0x58;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Y].code = (char)0x59;
            ShiftEisu[(int)KeyboardHook.VKeys.KEY_Z].code = (char)0x5A;

            Eisu[(int)KeyboardHook.VKeys.SPACE].kana = " ";
            Eisu[(int)KeyboardHook.VKeys.SPACE].code = (char)0x20;

            Eisu[(int)KeyboardHook.VKeys.OEM_MINUS].kana = "-";
            Eisu[(int)KeyboardHook.VKeys.OEM_7].kana = "^";
            Eisu[(int)KeyboardHook.VKeys.OEM_5].kana = "\\";
            Eisu[(int)KeyboardHook.VKeys.OEM_MINUS].code = (char)0x2D;
            Eisu[(int)KeyboardHook.VKeys.OEM_7].code = (char)0x5E;
            Eisu[(int)KeyboardHook.VKeys.OEM_5].code = (char)0x5C;

            ShiftEisu[(int)KeyboardHook.VKeys.OEM_MINUS].kana = "=";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_7].kana = "~";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_5].kana = "|";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_MINUS].code = (char)0x3D;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_7].code = (char)0x7E;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_5].code = (char)0x7C;

            Eisu[(int)KeyboardHook.VKeys.OEM_3].kana = "@";
            Eisu[(int)KeyboardHook.VKeys.OEM_4].kana = "[";
            Eisu[(int)KeyboardHook.VKeys.OEM_3].code = (char)0x40;
            Eisu[(int)KeyboardHook.VKeys.OEM_4].code = (char)0x5B;

            ShiftEisu[(int)KeyboardHook.VKeys.OEM_3].kana = "`";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_4].kana = "{";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_3].code = (char)0x60;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_4].code = (char)0x7B;

            Eisu[(int)KeyboardHook.VKeys.OEM_PLUS].kana = ";";
            Eisu[(int)KeyboardHook.VKeys.OEM_1].kana = ":";
            Eisu[(int)KeyboardHook.VKeys.OEM_6].kana = "]";
            Eisu[(int)KeyboardHook.VKeys.OEM_PLUS].code = (char)0x3B;
            Eisu[(int)KeyboardHook.VKeys.OEM_1].code = (char)0x3A;
            Eisu[(int)KeyboardHook.VKeys.OEM_6].code = (char)0x5D;

            ShiftEisu[(int)KeyboardHook.VKeys.OEM_PLUS].kana = "+";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_1].kana = "*";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_6].kana = "}";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_PLUS].code = (char)0x2B;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_1].code = (char)0x2A;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_6].code = (char)0x7D;

            Eisu[(int)KeyboardHook.VKeys.OEM_COMMA].kana = ",";
            Eisu[(int)KeyboardHook.VKeys.OEM_PERIOD].kana = ".";
            Eisu[(int)KeyboardHook.VKeys.OEM_2].kana = "/";
            Eisu[(int)KeyboardHook.VKeys.OEM_102].kana = "\\";
            Eisu[(int)KeyboardHook.VKeys.OEM_COMMA].code = (char)0x2C;
            Eisu[(int)KeyboardHook.VKeys.OEM_PERIOD].code = (char)0x2E;
            Eisu[(int)KeyboardHook.VKeys.OEM_2].code = (char)0x2F;
            Eisu[(int)KeyboardHook.VKeys.OEM_102].code = (char)0x5C;

            ShiftEisu[(int)KeyboardHook.VKeys.OEM_COMMA].kana = "<";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_PERIOD].kana = ">";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_2].kana = "?";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_102].kana = "_";
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_COMMA].code = (char)0x3C;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_PERIOD].code = (char)0x3E;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_2].code = (char)0x3F;
            ShiftEisu[(int)KeyboardHook.VKeys.OEM_102].code = (char)0x5F;


            Eisu[(int)KeyboardHook.VKeys.NUMPAD1].kana = "1";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD2].kana = "2";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD3].kana = "3";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD4].kana = "4";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD5].kana = "5";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD6].kana = "6";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD7].kana = "7";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD8].kana = "8";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD9].kana = "9";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD0].kana = "0";
            Eisu[(int)KeyboardHook.VKeys.NUMPAD1].code = (char)0x31;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD2].code = (char)0x32;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD3].code = (char)0x33;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD4].code = (char)0x34;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD5].code = (char)0x35;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD6].code = (char)0x36;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD7].code = (char)0x37;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD8].code = (char)0x38;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD9].code = (char)0x39;
            Eisu[(int)KeyboardHook.VKeys.NUMPAD0].code = (char)0x30;
            Eisu[(int)KeyboardHook.VKeys.DECIMAL].kana = ".";
            Eisu[(int)KeyboardHook.VKeys.DECIMAL].code = (char)0x2E;
            Eisu[(int)KeyboardHook.VKeys.ADD].kana = "+";
            Eisu[(int)KeyboardHook.VKeys.ADD].code = (char)0x2B;
            Eisu[(int)KeyboardHook.VKeys.SUBTRACT].kana = "-";
            Eisu[(int)KeyboardHook.VKeys.SUBTRACT].code = (char)0x2D;
            Eisu[(int)KeyboardHook.VKeys.MULTIPLY].kana = "*";
            Eisu[(int)KeyboardHook.VKeys.MULTIPLY].code = (char)0x2A;
            Eisu[(int)KeyboardHook.VKeys.DIVIDE].kana = "/";
            Eisu[(int)KeyboardHook.VKeys.DIVIDE].code = (char)0x2F;
        }


        /// <summary>
        /// カナコードを返します
        /// </summary>
        /// <param name="cha"></param>
        /// <returns></returns>
        public static KanaChara getKanaChara(char cha)
        {
            return Kana[cha];
        }

        public static char getEisuChar(char cha, bool shiftFlg)
        {
            if (shiftFlg == true)
            {
                return ShiftEisu[(int)cha].code;
            }
            else
            {
                return Eisu[(int)cha].code;
            }
        }
    }
}
