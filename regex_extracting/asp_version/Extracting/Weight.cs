/*
 * author : zxr
 * date : 2013/9/11
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace Ch2R.Code.Extracting
{
    public class Weight : ParameterBase
    {
        public Weight(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "手机重量";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"手机重量|重不重|有多重"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(\d{1,4}(.\d{1,3})?克|g)|轻薄|轻|笨重|重",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, @"\d{2,6}");

                if (matches.Count >= 2)
                {
                    string num1 = matches[0].Groups[0].ToString();
                    string num2 = matches[1].Groups[0].ToString();
                    list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num1) * 0.99, Convert.ToDouble(num2) * 1.01);
                }
                else if (matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    if (Regex.IsMatch(str, @"(?<!不)小于|不大于|不高于|(?<!不)低于|不超过|以下"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, num);
                    }
                    else if (Regex.IsMatch(str, @"至少|最少|不少于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
                    {
                        list[i].normalData = string.Format("{0},{1}", num, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"差不多|大概|大约|左右|上下"))
                    {
                        list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.8, Convert.ToDouble(num) * 1.2);
                    }
                    else
                    {
                        // 这里当作只有匹配到一串数字
                        list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.95, Convert.ToDouble(num) * 1.05);
                    }
                }
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"(偏|稍|略)?轻(些|点|一点)?"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, 80);
                    }
                    else if (Regex.IsMatch(str, @"(偏|稍|略)?重(些|点|一点)?"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 130, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"一般|差不多|适中"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 80, 120);
                    }
                    else if (Regex.IsMatch(str, @"随意|随便|都可以"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["手机重量"] != null)
                        {
                            String[] score = ((String)userSession.liveTable.Parameter["手机重量"]).Split(',');
                            list[i].normalData = string.Format("{0},{1}", score[0], score[1]);
                            list[i].isNegative = true;
                        }
                    /**
                     * 仍有其他价格为null的情况
                     * 别的未测试
                     * */
                    }

                    else
                    {
                        // 这里当作匹配上那些需要句型配合处理的信息
                        list[i].normalData = str;
                    }
                }
                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要太重的
                 * 那么匹配上：重
                 * 在“贵”前出现“不”这个否定词，因此认为是否定形式
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