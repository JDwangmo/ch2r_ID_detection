/*
 * author : wgs
 * date : 2013/9/11
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class RomCapacity : ParameterBase
    {
        public RomCapacity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "ROM容量";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"ROM"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(大于|小于|等于|超过|不低于)*\d{1,6}(MB|GB|G|M|m|g|gb|mb)",
                @"\d{1,6}(MB|GB|G|M|m|g|gb|mb)(以下|以上)",
                @"随意|随便|都可以",
                @"(?<!多)大|(?<!多)高"
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                list[i].normalData = ToInterval(str);

                list[i].isNegative = false;
            }

        }

        public override InfoBlock ToInfoBlock()
        {
            List<InfoUnit> L = new List<InfoUnit>();
            foreach (InfoMetadata im in list)
            {
                InfoUnit iu = new InfoUnit();
                iu.normalData = im.normalData;
                iu.rawData = im.rawData;
                iu.isNegative = im.isNegative;
                L.Add(iu);
            }
            InfoBlock ib = new InfoBlock(name, "and", L);

            return ib;
        }

        public string ToInterval(string str)
        {
            string Formated = "";
            MatchCollection matches = Regex.Matches(str, "\\d{1,6}");

            string RomCapacity_str;
            int RomCapacity;
            if (matches.Count == 1)
            {
                if (Regex.IsMatch(str, "G|GB|g|gb"))  // by WGS
                {
                    RomCapacity_str = matches[0].Groups[0].ToString();
                    RomCapacity = Convert.ToInt32(RomCapacity_str);
                }
                else if (Regex.IsMatch(str, "M|MB|m|mb"))
                {
                    RomCapacity_str = matches[0].Groups[0].ToString();
                    RomCapacity = Convert.ToInt32(RomCapacity_str) / 1024;
                }
                else
                {
                    RomCapacity_str = matches[0].Groups[0].ToString();
                    RomCapacity = Convert.ToInt32(RomCapacity_str);
                }

                if (Regex.IsMatch(str, "以下|小于|不超过|不高于|不大于|(?<!不)低于"))
                {
                    Formated = string.Format("{0},{1}", "0", RomCapacity.ToString());
                }
                else if (Regex.IsMatch(str, "以上|(?<!不)大于|(?<!不)超过|不少于|不低于|(?<!不)高于"))
                {
                    Formated = string.Format("{0},{1}", RomCapacity.ToString(), int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "大概|差不多"))
                {
                    double RomCapacity_double = (double)RomCapacity;
                    double down_double = RomCapacity_double * 0.8;
                    double up_double = RomCapacity_double * 1.2;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
                else if (Regex.IsMatch(str, @"随意|随便|都可以"))
                {
                    Formated = string.Format("{0},{1}", 0, int.MaxValue);
                }
                else
                {
                    double RomCapacity_double = (double)RomCapacity;
                    double down_double = RomCapacity_double * 0.9;
                    double up_double = RomCapacity_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
            }
            else if (matches.Count == 2)
            {

            }
            else if (matches.Count == 0) // by HQ
            {
                if (Regex.IsMatch(str, "(?<!多)大|(?<!多)高"))
                {
                    Formated = string.Format("{0},{1}", "16", int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "一般|适中"))
                {
                    Formated = string.Format("{0},{1}", "8", "16");
                }
                else if (Regex.IsMatch(str, "小|低"))
                {
                    Formated = string.Format("{0},{1}", "0", "8");
                }
            }
            return Formated;
        }

    }
}