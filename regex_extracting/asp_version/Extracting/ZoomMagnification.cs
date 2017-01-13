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
    public class ZoomMagnification : ParameterBase
    {
        public ZoomMagnification(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "变焦倍率";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"变焦倍率"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 这里也需要判断倍数高低，比如客户会问：我想要变焦倍率高一点的手机
                 * 也需要加入比较和范围的处理，大于、小于等
                 */
                @"(至多|至少|最多|最少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(\d{0,4}(倍))",
                @"(\d{0,4}(倍))",
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
                    if (userSession.liveTable.Parameter["变焦倍率"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["变焦倍率"];
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
                /* 这里的细节处理还需要有：范围和比较的处理
                 * 编码拙计，回头再同意修补
                 */

                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要变焦倍数少于10倍的手机 
                 * 那么匹配上：变焦倍数、10倍
                 * 在“变焦倍数、10倍”前出现“不”这个否定词，因此认为是否定形式
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
             * 闪关灯参数信息是相互独立的
             * 用户可以选择多种不同变焦倍数的手机
             */
            return ib;
        }
    }
}