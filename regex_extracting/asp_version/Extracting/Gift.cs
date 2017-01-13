/*
 * author : wgs
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
    public class Gift : ParameterBase
    {
        public Gift (Areas.Chat.Models.UserSession userSession, string sentence) : base(userSession, sentence)
        {
            name = "赠品";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"送|礼物|赠品"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"耳机|耳塞|贴膜|手机壳|手机套",
                @"随意|随便|都可以|其他|别的"
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
                    if (userSession.liveTable.Parameter["赠品"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["赠品"];
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

            InfoBlock ib = new InfoBlock(name, "and", L);

            return ib;
        }
    }
}