/*
 * author : zxr
 * date : 2013/10/19
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class Popularity : ParameterBase
    {
        public Popularity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "人气";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"  ",
                @"人气"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(?<!多)高|低|一般|普通|主流",
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
                    //话说人气他都能说出规格来？
                }
                else if (matches.Count == 1)
                {
                    //同上，客户不可能说出人气的具体数值吧，他都不知道我们的设定
                }
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"低"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, 70);
                    }
                    else if (Regex.IsMatch(str, @"(?<!多)高") || Regex.IsMatch(str, @"主流"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 70, 90);
                    }
                    else if (Regex.IsMatch(str, @"一般|普通"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 90, 100);
                    }
                    else if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, 100);
                    }
                    else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["人气"] != null)
                        {
                            String[] score = ((String)userSession.liveTable.Parameter["人气"]).Split(',');
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
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要低人气的
                 * 那么匹配上：低
                 * 在“低”前出现“不”这个否定词，因此认为是否定形式
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