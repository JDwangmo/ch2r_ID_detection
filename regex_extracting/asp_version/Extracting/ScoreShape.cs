/*
 * author : zxr
 * date : 2013/10/19
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class ScoreShape : ParameterBase
    {
        public ScoreShape(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "外观得分";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"外观|外形|外貌"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(?<=外观.*)(好看|好)",
                @"(?<=外观)如何|(?<=外观)怎么样|(?<=外观)怎样|(?<=外观)好不|(?<=外观)行不",
                @"(?<=样子)如何|(?<=样子)怎么样|(?<=样子)怎样|(?<=样子)好不|(?<=样子)行不",
                @"(?<=外形)如何|(?<=外形)怎么样|(?<=外形)怎样|(?<=外形)好不|(?<=外形)行不",
                @"好看吗|好不好看",
                @"美观",
                @"可爱",
                @"随意|随便|都可以|其他|别的"
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
                    scores.Add(Code.Inquiry.CalculateScore(pset[j], "外观得分"));
                }
                int score_index = (int)(pset.Count * 0.10);
                scores.OrderByDescending(r => r.Value);
                decimal? score_value = scores[scores.Count - 1 - score_index];

                if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                {
                    list[i].normalData = string.Format("{0},{1}", 0, int.MaxValue);
                }
                else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                {
                    if (userSession.liveTable.Parameter["外观得分"] != null)
                    {
                        String[] score = ((String)userSession.liveTable.Parameter["外观得分"]).Split(',');
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