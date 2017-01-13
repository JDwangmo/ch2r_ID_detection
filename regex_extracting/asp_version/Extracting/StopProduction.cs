/*
 * author : wxp
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
    public class StopProduction : ParameterBase
    {
        public StopProduction(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "是否停产";
            /* 这个参数很没有存在感
             */
            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"是否停产|停产|没有停产"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                @"停产",
                //@"随便|都可以"
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i].normalData = list[i].rawData;
                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要停产的
                 * 那么匹配上：停产
                 * 在“停产”前出现“不”这个否定词，因此认为是否定形式
                 */
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
            /* 这里关系应该是“or”
             * 用户对停产与否可能不在乎，所以有可能出现停产的和不停产的都可以接受
             */
            return ib;
        }
    }
}