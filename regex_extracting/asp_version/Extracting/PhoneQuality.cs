﻿/*
 * author : zxr
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
    public class PhoneQuality : ParameterBase
    {
        public PhoneQuality(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "手机质量";
            #region 初始化正则表达式
            //描述正则表达式
            StatementRegexs = new string[] {
                @"质量"
            };
            //值正则表达式
            ValueRegexs = new string[] {
                @"防水|防震|防尘|耐摔|防摔|耐用|耐磨|三防|摔坏",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                {
                    list[i].normalData = "";
                }
                else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                {
                    if (userSession.liveTable.Parameter["手机质量"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["手机质量"];
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
                 * 但。。好像没什么否定情况要处理
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

            /* 在这里处理“与”和“或”关系的值
             * 这里默认用户都喜欢用质量“好”的，所以都用"and"
             * */

            InfoBlock ib = new InfoBlock(name, "and", L);

            return ib;
        }
    }
}