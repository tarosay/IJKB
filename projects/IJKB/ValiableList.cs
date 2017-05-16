using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IchigoJamKeyBoard
{
    class ValiableList
    {
        static public bool LanguageFlg = true;

        static public string MainFormTtle = "IJKB for IchigoJam";

        static public string[] cmbKanaRomaMes = { "Kana", "Roman" };
        static public string[] tbxKeyCharaMes = { "             Here, you can drag and drop the BAS file,", "             you can upload the BAS file in IchigoJam." };

        //エラーメッセージ
        static public string errFilename = "File name is incorrect.";
        static public string errFileRead = "It failed to file read.";
        static public string errSaveNumber = "SAVE number is incorrect.";
        static public string errConnect = "Keyboard connection error";
        static public string errInputMiss = "Input value is incorrect.";
        static public string errMessage = "Error Message";

        static public string sfdSaveTitleMes = "Get the program";
        static public string sfdSaveFilterMes = "BASIC File(*.bas)|*.bas|all files(*.*)|*.*";

        static public string MonitorOptFrame = "Frame";

        /// <summary>
        /// 使用言語を調べてメッセージをセットします
        /// </summary>
        internal static void selectLanguage(string str)
        {
            Debug.WriteLine(str);
            if (str == "イチゴジャム" && LanguageFlg == true)
            {
                //日本語
                MainFormTtle = ValiableList_JP.MainFormTtle;

                errFilename = ValiableList_JP.errFilename;
                errFileRead = ValiableList_JP.errFileRead;
                errSaveNumber = ValiableList_JP.errSaveNumber;
                errConnect = ValiableList_JP.errConnect;
                errInputMiss = ValiableList_JP.errInputMiss;
                errMessage = ValiableList_JP.errMessage;

                sfdSaveTitleMes = ValiableList_JP.sfdSaveTitleMes;
                sfdSaveFilterMes = ValiableList_JP.sfdSaveFilterMes;


                //グラフのフィルタ強さの選択肢
                for (int i = 0; i < cmbKanaRomaMes.Length; i++)
                {
                    cmbKanaRomaMes[i] = ValiableList_JP.cmbKanaRomaMes[i];
                }

                //オプションのノードAuto focus
                for (int i = 0; i < tbxKeyCharaMes.Length; i++)
                {
                    tbxKeyCharaMes[i] = ValiableList_JP.tbxKeyCharaMes[i];
                }

                MonitorOptFrame = ValiableList_JP.MonitorOptFrame;
            }
        }
    }
}