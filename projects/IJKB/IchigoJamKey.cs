using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IchigoJamKeyBoard
{
    class IchigoJamKey
    {
        public enum VKeys
        {
            NON = 0x00,         //NULL 空白
            BS = 0x08,          //BS
            TAB = 0x09,         //TAB
            ENTER = 0x0A,       //ENTER
            D_DELETE = 0x0C,    //カーソル以降すべて削除
            I_SPACE = 0x0E,     //スペースを挿入(上書きしない)
            KANA = 0x0F,        //たぶんカナ切り替え
            S_ENTER = 0x10,     //Shift-ENTER 改行して1行挿入
            INSERT = 0x11,      //挿入
            HOME = 0x12,        //HOME
            P_UP = 0x13,        //Page UP
            P_DOWN = 0x14,      //Page DOWN
            LOCATE = 0x15,      //Locate
            END = 0x17,         //END
            L_DELETE = 0x18,    //1行削除    
            ESC = 0x1B,         //ESC
            LEFT = 0x1C,        //カーソル左
            RIGHT = 0x1D,       //カーソル右
            UP = 0x1E,          //カーソル上
            DOWN = 0x1F,        //カーソル下
            SPACE = 0x20,       //スペース
            DELETE = 0x7F,      //DELETE
        }

        /// <summary>
        /// イチゴジャムで使うキーコードかどうか調べます
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool ChkKeyCode(int code,MonitorForm.CursorFigure cf)
        {
            if (cf == MonitorForm.CursorFigure.Non)
            {
                //カーソルが出ていない
                return true;
            }
            else
            {
                //カーソルが出ている
                if (code == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}