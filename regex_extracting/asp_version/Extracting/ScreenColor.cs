/*
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
    public class ScreenColor : ParameterBase
    {
        public ScreenColor(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "主屏色彩";

            #region 初始化正则表达式
            //描述正则表达式
            StatementRegexs = new string[]{
                @"主屏色彩"
            };
            //值正治表达式
            ValueRegexs = new string[]{
                @"\d{1,4}万色",
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
                    if (userSession.liveTable.Parameter["主屏色彩"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["主屏色彩"];
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
                /*
                 * 此处认为用户不会说
                 * 我不要XX万色的手机
                 * 
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

            /* 
             * 主屏色彩不能在同一款手机上可以共存
             * 所以用“or”
             * */

            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}