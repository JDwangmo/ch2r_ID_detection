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
    public class CpuFrequency : ParameterBase
    {
        public CpuFrequency(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "CPU频率";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"CPU频率|CPU运行|CPU"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                //@"\d{1,4}(\.\d{1,3})?(MHz|GHz)?",
                @"(?<!多)高|(?<!多)快",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            /* 
             * 本想简单的进行模糊匹配就算了，结果出现个什么大概啊差不多啊之类的
             * 很明显也是需要区间的，所以还是先不理它了
             * 先简单把匹配到的转成数字存起来就是了
             * */
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, @"\d{1,4}(.\d{1,3})?");

                if (matches.Count >= 2)
                {
                    string num1 = matches[0].Groups[0].ToString();
                    string num2 = matches[1].Groups[0].ToString();
                    
                    if (Convert.ToDouble(num1) >= 100 && Convert.ToDouble(num2) >= 100)
                    {
                        /**
                         * 将MHz转成GMz
                         * */
                        list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num1) * 0.99 / 1000.0, Convert.ToDouble(num2) * 1.01 / 1000.0);
                    }
                    else
                        list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num1) * 0.99, Convert.ToDouble(num2) * 1.01);
                }
                else if(matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    /* 应该不会有人说：我要CPU频率“不超过|最多”XXX的吧
                     * if (Regex.IsMatch(str, @"至多|最多|(?<!不)少于|不多于|(?<!不)小于|不大于|不高于|(?<!不)低于|不超过|以下"))
                     * {
                     *   list[i].normalData = string.Format("{0},{1}", 0, num);
                     * }
                     * else
                     * */


                    //如上，将MHz转GMz
                    if (Convert.ToDouble(num) >= 100)
                    {
                        if (Regex.IsMatch(str, @"至少|最少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
                        {
                            list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) / 1000.0, int.MaxValue);
                        }
                        else if (Regex.IsMatch(str, @"差不多|大概|大约|左右"))
                        {
                            list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.8 / 1000.0, Convert.ToDouble(num) * 1.2 / 1000.0);
                        }
                        else
                        {
                            // 这里当作只有匹配到一串数字
                            list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.95 / 1000.0, Convert.ToDouble(num) * 1.05 / 1000.0);
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(str, @"至少|最少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
                        {
                            list[i].normalData = string.Format("{0},{1}", num, int.MaxValue);
                        }
                        else if (Regex.IsMatch(str, @"差不多|大概|大约|左右"))
                        {
                            list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.8, Convert.ToDouble(num) * 1.2);
                        }
                        else
                        {
                            // 这里当作只有匹配到一串数字
                            list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num) * 0.95, Convert.ToDouble(num) * 1.05);
                        }
                    }
                }
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"(?<!多)高|(?<!多)快"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 1.2, int.MaxValue);
                    }
                    else if(Regex.IsMatch(str, @"随意|随便|都可以"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, int.MaxValue);
                    }
                    else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["CPU频率"] != null)
                        {
                            String[] score = ((String)userSession.liveTable.Parameter["CPU频率"]).Split(',');
                            list[i].normalData = string.Format("{0},{1}", score[0], score[1]);
                            list[i].isNegative = true;
                        }
                        /**
                         * 仍有其他价格为null的情况
                         * 别的未测试
                         * */
                    }
                    else
                    {
                        // 这里当作匹配上那些需要句型配合处理的信息
                        list[i].normalData = str;
                    }
                }
                
                /* 怎么感觉没人会问：我不要xxxxCPU频率的
                 * 所以这里不进行否定处理了
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
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}