/*
 * author : ccp
 * date : 2015/1/23
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class Advantage: ParameterBase
    {
        public Advantage(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "优点";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"优点|长处|特长|好处"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                @"短信|SMS|彩信|MMS|录音功能|飞行模式|情景模式|主题模式|闹钟功能|计算器|电子词典|备忘录|日程表|日历功能",
                @"随意|随便|都可以|别的|其他的"
          
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
                    if (userSession.liveTable.Parameter["优点"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["优点"];
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
                 * 比如用户说：我不要SMS功能 
                 * 那么匹配上：SMS功能
                 * 在“功能”前出现“不”这个否定词，因此认为是否定形式
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
            InfoBlock ib = new InfoBlock(name, "and", L);
            /* 这里关系应该是“and”
             * 用户对基础功能可以有多个要求
             */
            return ib;
        }
    }
}