/*
 * author : wgs
 * date : 2013/9/11
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class WarrantyTime : ParameterBase
    {
        public WarrantyTime(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "质保时间";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"质保时间|保修时间"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(至少|最少|不少于|多余|不小于|不低于|超过|差不多|大概|大约|左右)*(\d年|\d{1,2}月)(以上|差不多|大概|大约|左右)*",
                @"(\d年|\d{1,2}月).(\d年|\d{1,2}月)",
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, @"\d年|\d{1,2}月");

                if (matches.Count >= 2)
                {
                    string num1 = matches[0].Groups[0].ToString();
                    string num2 = matches[1].Groups[0].ToString();

                    if (num1.IndexOf("年") != -1)
                        num1 = String.Format("{0}年", Convert.ToDouble(num1.Substring(0,num1.Length-1)) * 0.99);
                    else
                        num1 = String.Format("{0}年", Convert.ToDouble(num1.Substring(0,num1.Length-1))/12 * 0.99);

                    if (num2.IndexOf("年") != -1)
                        num2 = String.Format("{0}年", Convert.ToDouble(num2.Substring(0, num2.Length - 1)) * 1.01);
                    else
                        num2 = String.Format("{0}年", Convert.ToDouble(num2.Substring(0, num2.Length - 1)) / 12 * 1.01);

                    list[i].normalData = string.Format("{0},{1}", num1, num2);
                }
                else if (matches.Count == 1)
                    {
                        string num = matches[0].Groups[0].ToString();

                        if (Regex.IsMatch(str, @"至少|最少|不少于|多余|不小于|不低于|超过|以上"))
                        {
                            if (num.IndexOf("年") != -1)
                                num = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1)) * 0.99);
                            else
                                num = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1)) / 12 * 0.99);
                            list[i].normalData = string.Format("{0},{1}", num, "5年");
                        }
                        else // @"差不多|大概|大约|左右", 或其他情况
                        {
                            string num1, num2;

                            if (num.IndexOf("年") != -1)
                            {
                                num1 = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1)) * 0.8);
                                num2 = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1)) * 1.2);
                            }
                            else
                            {
                                num1 = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1))/12 * 0.8);
                                num2 = String.Format("{0}年", Convert.ToDouble(num.Substring(0, num.Length - 1))/12 * 1.2);
                            }

                            list[i].normalData = string.Format("{0},{1}", num1, num2);
                        }
                    }
                    else if (matches.Count == 0)
                    {
                        if (Regex.IsMatch(str, @"长"))
                        {
                            list[i].normalData = string.Format("{0},{1}","1年","5年");
                        }
                        else
                        {
                            // 这里当作匹配上那些需要句型配合处理的信息
                            list[i].normalData = str;
                        }
                    }

              list[i].isNegative = false;
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

            return ib;
        }

    }
}