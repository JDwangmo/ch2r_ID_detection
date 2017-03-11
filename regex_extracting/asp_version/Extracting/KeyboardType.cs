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
    public class KeyboardType : ParameterBase
    {
        public KeyboardType(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "键盘类型";

            #region 初始化正则表达式
            //描述正则表达式
            StatementRegexs = new string[] {
                @"键盘类型|键盘"
            };

            //值正则表达式
            ValueRegexs = new string[] {
                @"SureType(键盘)?|虚拟(QWERTY)?(键盘)?|T9(传统键盘)?|(QWERTY)?全键盘|无按键",
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
                    if (userSession.liveTable.Parameter["键盘类型"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["键盘类型"];
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
                 * 比如用户说：我不要XX键盘的
                 * 那么匹配上：不要
                 * 在“XX键盘”前出现“不”这个否定词，因此认为是否定形式
                 * */
                if (Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"(不)|(非(?!常))") || Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"无"))
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
             * 具体来说，就是“SureType(键盘)”跟“虚拟(QWERTY)?”是互斥关系，因此用“or”
             * 而“老人手机”跟“儿童手机”显然互斥，要用“or”
             * 但是这样处理起来非常麻烦，所以简单处理如下：
             * 都用“and”
             * 当然，这样之后根据句型如果要更新live-table里面的信息时，就要把互斥的排除
             * 简单的做法就是直接用新的值覆盖掉与它互斥的旧值
             * */

            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}