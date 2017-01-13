/*
 * author : xm
 * date : 2013/9/2
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class CpuCoreNumber : ParameterBase
    {
        public CpuCoreNumber(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "CPU核数";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"CPU核数|核数|核"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                //@"单核|双核|四核|八核|(?<!\d)\d{1,2}(?!\d)(核)?"
                @"单核|双核|四核|八核|[1-8]核",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                /* 这里认为说到CPU核数的值只有一个
                 * 然后还要把核字去掉如果有的话
                 * */
                string str = list[i].rawData;
                MatchCollection matches = Regex.Matches(str, @"\d{1,2}");
                if (matches.Count == 1)
                {
                    list[i].normalData = string.Format("{0},{1}", matches[0].Groups[0].ToString(), matches[0].Groups[0].ToString());
                }
                if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, "单"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "1", "1");
                    }
                    else if (Regex.IsMatch(str, "双"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "2", "2");
                    }
                    else if (Regex.IsMatch(str, "四"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "4", "4");
                    }
                    else if (Regex.IsMatch(str, "八"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "8", "8");
                    }
                    else if (Regex.IsMatch(str, "十六"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "16", "16");
                    }
                    else if (Regex.IsMatch(str, "随意|随便|都可以"))
                    {
                        list[i].normalData = string.Format("{0},{1}", "1", "16");
                    }
                                    else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                {
                    if (userSession.liveTable.Parameter["CPU核数"] != null)
                    {
                        String[] score = ((String)userSession.liveTable.Parameter["CPU核数"]).Split(',');
                        list[i].normalData = string.Format("{0},{1}", score[0], score[1]);
                        list[i].isNegative = true;
                    }
                    /**
                     * 仍有其他价格为null的情况
                     * 别的未测试
                     * */
                }
                    
                }

                /* 这里处理一下否定情况
                 * 不过貌似不需要
                 * */
                if (Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"(不)|(非(?!常))"))
                {
                    list[i].isNegative = true;
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
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}