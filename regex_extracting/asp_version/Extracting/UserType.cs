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
    public class UserType : ParameterBase
    {
        public UserType(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "使用对象";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                /* 感觉还需要添加其它的参数来补足这个参数的作用，比如加个叫适用职业的参数
                 * 或者把这个参数更抽象化，使其功能能够更加完善
                 */
                @"使用对象|使用人群|使用者|适合人群|适合对象|面向人群|面向年龄"    
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 这里的值不够
                 */
                @"女生|男生|老人|学生|小孩|白领",
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
                    if (userSession.liveTable.Parameter["使用对象"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["使用对象"];
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
                 * 比如用户说：我不要小孩子用的
                 * 那么匹配上：孩子
                 * 在“孩子”前出现“不”这个否定词，因此认为是否定形式
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
             * 用户对手机使用对象的要求应该是固定的某一款
             */
            return ib;
        }
    }
}