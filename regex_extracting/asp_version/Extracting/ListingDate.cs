/*
 * author : xm
 * date : 2013/9/1
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class ListingDate : ParameterBase
    {
        public ListingDate(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "上市日期";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"上市日期|上市"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(大概|大约|差不多)*(2000|20001|2002|2003|2004|2005|2006|2007|2008|2009|2010|2011|2012|2013|2014|2015|2016|2017|2017|2018|2019|2020)(年份|年)*((\d|10|11|12)月)?(以前|以后)*",
                @"(\d|10|11|12)月",
                @"本月|这个月|上个月|今年|去年|上一年|前年|最近|近期|最新|刚刚|新款",
                @"新出|刚出",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            /* 其实上市日期比较难搞的一点就是，它是时间类型的，但又只是到年和月
             * 本想简单的进行模糊匹配就算了，结果出现个什么大概啊差不多啊之类的
             * 很明显也是需要区间的，所以还是先不理它了
             * 先简单把匹配到的转成数字存起来就是了
             * */
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, "\\d{2,4}");

                if (matches.Count > 0)
                {
                    string num = matches[0].Groups[0].ToString();
                    // 这里当作只有匹配到年
                    list[i].normalData = string.Format("{0}", num);
                }
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"上个月|这|本|今|最新|新|刚刚|最近|近期"))
                    {
                        list[i].normalData = string.Format("{0}", DateTime.Now.Year);
                    }
                    else if (Regex.IsMatch(str, @"去年|上一年"))
                    {
                        list[i].normalData = string.Format("{0}", DateTime.Now.Year - 1);
                    }
                    else if (Regex.IsMatch(str, @"前年"))
                    {
                        list[i].normalData = string.Format("{0}", DateTime.Now.Year - 2);
                    }
                    else if (Regex.IsMatch(str, @"随意|随便|都可以"))
                    {
                        list[i].normalData = "";
                    }
                    else if (Regex.IsMatch(str, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["上市日期"] != null)
                        {
                            list[i].normalData = (String)userSession.liveTable.Parameter["上市日期"];
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

                /* 怎么感觉没人会问：我不要xxxx年上市的
                 * 所以这里不进行否定处理了
                 * */
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