﻿/*
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
    public class BusinessFunction : ParameterBase
    {
        public BusinessFunction(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "商务功能";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"商务功能|商务"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                @"世界时间|数据备份|数据备份",
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
                    if (userSession.liveTable.Parameter["商务功能"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["商务功能"];
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
                 * 比如用户说：我不需要世界时间 
                 * 那么匹配上：世界时间
                 * 在“世界时间”前出现“不”这个否定词，因此认为是否定形式
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
             * 用户对商务功能可以有多个要求
             */
            return ib;
        }
    }
}