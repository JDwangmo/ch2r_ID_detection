﻿/*
 * author : Cnt
 * date : 2014/6/15
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace Ch2R.Code.Extracting
{
    public class Peripherals : ParameterBase
    {
        public Peripherals(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "手机部件";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"部件|手机部件|配置"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"cpu|前置摄像头|后置摄像头|电池|操作系统|WIFI|蓝牙",
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
                    if (userSession.liveTable.Parameter["手机部件"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["手机部件"];
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
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}