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
    public class InputMode : ParameterBase
    {
        public InputMode(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "输入方式";
            #region 初始化正则表达式
            //描述正则表达式
            StatementRegexs = new string[] {
                @"输入方式|输入"
            };
            //值正则表达式
            ValueRegexs = new string[] {
                @"手写|键盘|手写键盘|语音输入|语音",
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
                    if (userSession.liveTable.Parameter["输入方式"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["输入方式"];
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
                 * 比如用户说：我不要手写的
                 * 句子貌似好好奇葩
                 * 那么匹配上：不
                 * 在“手写”前出现“不”这个否定词，因此认为是否定形式
                 * */
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

            /* 在这里处理“与”和“或”关系的值
             * 具体来说，就是“手写方式”跟“语音”不是互斥关系，因此用“and”
             * 但是一般没人无聊的又要手写又是语音的吧
             * 所以简单点
             * 都用“or”
             * */
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}