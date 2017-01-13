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
    public class BatteryCapacity : ParameterBase
    {
        public BatteryCapacity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "电池容量";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"电池容量"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(大于|超过|不小于)\d{3,5}(m|M)?(a|A)?(h|H)?",
                @"(?<!多)高|一般|适中|低",
                @"随意|随便|都可以|其他|别的",
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                {
                    list[i].normalData = "";
                }
                else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                {
                    if (userSession.liveTable.Parameter["电池容量"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["电池容量"];
                        list[i].isNegative = true;
                    }
                    else
                    {
                        /**
                         * 仍有其他价格为null的情况
                         * 别的未测试
                         * */
                    }
                }
                else
                {
                    list[i].normalData = list[i].rawData;
                }

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
            int capacity = 0;

            MatchCollection matches = Regex.Matches(str, "\\d+");

            if (matches.Count == 1)
            {
                str_tmp = matches[0].Groups[0].ToString();
                capacity = Convert.ToInt32(str_tmp);

                if (Regex.IsMatch(str, "以下|小于|不超过|不多于|不高于|不大于|(?<!不)低于"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "0", capacity.ToString(), str);
                }
                else if (Regex.IsMatch(str, "以上|(?<!不)大于|(?<!不)超过|不少于|不低于|不小于|(?<!不)高于"))
                {
                    Formated = string.Format("[{0},{1}]({2})", capacity.ToString(), int.MaxValue.ToString(), str);
                }
                else if (Regex.IsMatch(str, "左右|大概|差不多"))
                {
                    double capacity_double = (double)capacity;
                    double down_double = capacity_double * 0.8;
                    double up_double = capacity_double * 1.2;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("[{0},{1}]({2})", down.ToString(), up.ToString(), str);
                }
                else
                {
                    double capacity_double = (double)capacity;
                    double down_double = capacity_double * 0.9;
                    double up_double = capacity_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("[{0},{1}]({2})", down.ToString(), up.ToString(), str);
                }
            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "(?<!多)高|(?<!多)大"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "2001", int.MaxValue.ToString(), str);
                }
                else if (Regex.IsMatch(str, "一般|适中"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "1000", "2000", str);
                }
                else if (Regex.IsMatch(str, "低|小"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "0", "999", str);
                }
            }
            else if (matches.Count > 1)
            {

            }

            return Formated;
        }
    }
}