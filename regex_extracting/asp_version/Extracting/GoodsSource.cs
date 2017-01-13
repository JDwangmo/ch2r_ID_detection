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
    public class GoodsSource : ParameterBase
    {
        public GoodsSource(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "货源";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                /* 这里的参数描述货源参数是为了应付水货、韩货、欧版、港版等情况，
                 * 货源这个描述不够合理，
                 * 应该找一个更合适的参数描述，
                 * 但目前能够应付出现国家名称的问题
                 */
                @"货源|生产地|出产地"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 
                 * 这里的货源地资料明显不足
                 */
                @"中国|美国|日本|欧洲",
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
                    if (userSession.liveTable.Parameter["货源"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["货源"];
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
                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要日本的
                 * 那么匹配上：日本
                 * 在“日本”前出现“不”这个否定词，因此认为是否定形式
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
             * 用户对货物来源的要求分别搜索的
             */
            return ib;
        }
    }
}