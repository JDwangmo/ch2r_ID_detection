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
    public class RamCapacity : ParameterBase
    {
        public RamCapacity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "RAM容量";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"RAM|手机内存|机身内存|内存"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(大于|小于|等于|超过|不低于)*\d{1,6}(MB|GB|G|M|m|g|gb|mb)",
                @"\d{1,6}(MB|GB|G|M|m|g|gb|mb)(以下|以上)",
                // by WJD
                // 如果出现“小米”，则这里的“小”不算容量
                @"(?<!多)(大)(?!小)|(?<!大|多)(小)",
                @"随意|随便|都可以"
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

            string RamCapacity_str;
            int RamCapacity;
            if (matches.Count == 1)
            {
                if (Regex.IsMatch(str, "M|MB|m|mb"))
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str);
                }
                else if (Regex.IsMatch(str, "G|GB|g|gb"))
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str) * 1024;
                }
                else
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str);
                }

                if (Regex.IsMatch(str, "以下|小于|不超过|不高于|不大于|(?<!不)低于"))
                {
                    Formated = string.Format("{0},{1}", "0", RamCapacity.ToString());
                }
                else if (Regex.IsMatch(str, "以上|(?<!不)大于|(?<!不)超过|不少于|不低于|(?<!不)高于"))
                {
                    Formated = string.Format("{0},{1}", RamCapacity.ToString(), int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "大概|差不多"))
                {
                    double RamCapacity_double = (double)RamCapacity;
                    double down_double = RamCapacity_double * 0.8;
                    double up_double = RamCapacity_double * 1.2;
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
                    double RamCapacity_double = (double)RamCapacity;
                    double down_double = RamCapacity_double * 0.9;
                    double up_double = RamCapacity_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
            }
            else if (matches.Count == 2)
            {

            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "(?<!多)大|(?<!多)高"))
                {
                    Formated = string.Format("{0},{1}", "1025", int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "一般|适中"))
                {
                    Formated = string.Format("{0},{1}", "1024", "1024");
                }
                else if (Regex.IsMatch(str, "小|低"))
                {
                    Formated = string.Format("{0},{1}", "0", "1023");
                }
            }
            return Formated; 
       }

    }
}