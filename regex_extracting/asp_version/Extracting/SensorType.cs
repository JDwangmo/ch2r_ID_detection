﻿/*
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
    public class SensorType : ParameterBase
    {
        public SensorType(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "感应器类型";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"感应器|传感器"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                @"重力|加速|光线|距离",
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
                    if (userSession.liveTable.Parameter["感应器类型"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["感应器类型"];
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
            /* 
             * 感应器类型在同一款手机上可以共存
             * 所以用“and”
             * */
            return ib;
        }
    }
}