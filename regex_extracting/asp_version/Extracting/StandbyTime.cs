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
    public class StandbyTime : ParameterBase
    {
        public StandbyTime(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "理论待机时间";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"待机"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(多于|少于|大于|小于|等于|超过|不低于)?(\d+小时|\d+天|\d+分钟)",
                @"久|长|一般|适中|差不多|短",
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
            string str_tmp = "";
            int time = 0;

            MatchCollection matches = Regex.Matches(str, "\\d+");

            if (matches.Count == 1)
            {

                if (Regex.IsMatch(str, "天|日"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp) * 24;
                }
                else if (Regex.IsMatch(str, "小时"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp);
                }
                else if (Regex.IsMatch(str, "分钟"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp) / 60;
                }

                if (Regex.IsMatch(str, "以下|小于|不超过|不多于|不高于|不大于|(?<!不)低于"))
                {
                    Formated = string.Format("{0},{1}", "0", time.ToString());
                }
                else if (Regex.IsMatch(str, "以上|(?<!不)大于|(?<!不)超过|不少于|不低于|不小于|(?<!不)高于"))
                {
                    Formated = string.Format("{0},{1}", time.ToString(), int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "左右|大概|差不多"))
                {
                    double time_double = (double)time;
                    double down_double = time_double * 0.8;
                    double up_double = time_double * 1.2;
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
                    double time_double = (double)time;
                    double down_double = time_double * 0.9;
                    double up_double = time_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "长|久"))
                {
                    Formated = string.Format("{0},{1}", "401", int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "一般|适中|差不多"))
                {
                    Formated = string.Format("{0},{1}", "200", "400");
                }
                else if (Regex.IsMatch(str, "短"))
                {
                    Formated = string.Format("{0},{1}", "0", "199");
                }
            }
            else if (matches.Count > 1)
            {

            }
            return Formated;
        }

    }
}