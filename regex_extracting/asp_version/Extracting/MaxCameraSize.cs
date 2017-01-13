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
    public class MaxCameraSize : ParameterBase
    {
        public MaxCameraSize(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "最大拍照尺寸";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"拍照尺寸|最大像素|照片尺寸|拍摄尺寸"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 这里的最大照相尺寸会出现范围和比较的问题
                 * 所以需要考虑某些特定的词语
                 */
                @"(\d{2,4}\*\d{2,4}像素)",
                @"(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(\d{2,4}\*\d{2,4}像素)",
                @"随意|随便|都可以|其他|别的"
                //@"最大|最高|最多|最少|最低|最小|大一点|多一点|高一点|小一点|少一点|低一点"
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
                    if (userSession.liveTable.Parameter["最大拍照尺寸"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["最大拍照尺寸"];
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
                 * 比如用户说：我不要最大拍照尺寸小于1600*800的手机
                 * 那么匹配上：最大拍照尺寸、小于、1600*800
                 * 在“最大拍照尺寸”前出现“不”这个否定词，因此认为是否定形式
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
             * 用户对最大拍照尺寸可以是多种多样的
             */
            return ib;
        }
    }
}