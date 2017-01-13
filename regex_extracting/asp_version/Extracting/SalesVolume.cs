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
    public class SalesVolume : ParameterBase
    {
        public SalesVolume(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "平均月销量";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"平均月销量|月销量|销量|没有销售|没有出售"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 这里的返回值有应该也可以是一个数值
                 * 而且这里用户的提问有可能更加复杂一些，比如说：我想月销量前十的手机
                 * 这里的信息提取应该更复杂一些
                 * 下面有我的一些信息提取的想法
                 * 能力拙计，所以现在还没实现
                 */
                @"(?<!多)高|(?<!多)低",
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
                    if (userSession.liveTable.Parameter["平均月销量"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["平均月销量"];
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
                 * 比如用户说：我不要销售量低的
                 * 那么匹配上：销售、低
                 * 在“销售”前出现“不”这个否定词，因此认为是否定形式
                 */

                /* 这里应该像价钱一样能够转成一个类似区间的形式，而且还要进行类似得分百分比的处理（为了应付销售量前十的提问）
                 * 在我看来销售量是用户比较关注的一个点，提问的信息和形式应该是多种多样的
                 * 应该要做更加细节的处理，不要让打击在用户最感兴趣的点打击用户的兴趣和热情
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
             * 用户对销售量的要求可以是无所谓的，所以可以是多条件一起搜
             */
            return ib;
        }
    }
}