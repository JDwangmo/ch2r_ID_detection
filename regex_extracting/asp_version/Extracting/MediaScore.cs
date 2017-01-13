using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class MediaScore:ParameterBase
    {
        public MediaScore(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "影音娱乐";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"影音娱乐|娱乐功能|娱乐"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(?<=(影音娱乐|娱乐功能|娱乐).*)(好)",
                @"好(?=.*(影音娱乐|娱乐功能|娱乐))",
                @"(?<=(影音娱乐|娱乐功能|娱乐))如何|(?<=(影音娱乐|娱乐功能|娱乐))怎么样|(?<=影音娱乐|娱乐功能|娱乐))怎样|(?<=影音娱乐|娱乐功能|娱乐))好不|(?<=影音娱乐|娱乐功能|娱乐))行不",
                "随意|随便|都可以",
            };
            #endregion

            RegexProcess();
        }
        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                /* 其实这里怎么把“高”变成区间呢
                 * 感觉性价比是相对的，也就是性价比高的手机或许可以认为是已有的那些手机里性价比高的部分手机
                 * 所以合理的做法应该是，对数据库里的手机进行一次算分，按分数降序，然后取前10%的分数作为“高”的数字区间
                 * */

                List<Models.CellPhone> pset = (new Models.Ch2RDbContext()).CellPhones.ToList();

                List<decimal?> scores = new List<decimal?>();
                for (int j = 0; j < pset.Count; j++)
                {
                    scores.Add(Code.Inquiry.CalculateScore(pset[j], "手机总得分"));
                }
                int score_index = (int)(pset.Count * 0.10);
                scores.OrderByDescending(r => r.Value);
                decimal? score_value = scores[scores.Count - 1 - score_index];

                //if (Regex.IsMatch(list[i].rawData, "好"))
                if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                {
                    list[i].normalData = "";
                }
                else
                {
                    list[i].normalData = string.Format("{0},{1}", score_value, int.MaxValue);
                }
                /* 这里处理一下否定情况
                 * 其实有人会说“我不要性价比高的”么
                 * 或者说“我不要性价比低的”吗
                 * 好的，委委地假设不会，处理变得简单，世界变得美好
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
