/*
 * author : xm
 * date : 2013/9/1
 * 
 * update : wxp
 * date : 2015/3/5
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class Price : ParameterBase
    {
        public Price(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "价格";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"价格|价钱|价位|钱|元|块"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z]).*(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z])",
                @"(至多|至少|最多|最少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z])(元|块)*(左右|以上|以下|内|以内)*",
                @"(最贵|最高|最多|最便宜|最低|最少)|(贵点|便宜点|贵一点|便宜一点|高一点|低一点|更贵|更便宜|更高|更低|再少点|再高点)",
                @"((?<=价.*?)(不高|低|少|高|差不多|一般|适中))|((普通|一般|低|(?<!多)高|差不多|((?<!多)少))(?=.*?(价|钱)))|(廉价|便宜|贵|一般|普通)",
                @"随意|随便|都可以|别的|其他的",
                @"好"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, @"\d{2,6}");

                if (matches.Count >= 2)
                {
                    string num1 = matches[0].Groups[0].ToString();
                    string num2 = matches[1].Groups[0].ToString();
                    list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num1) * 0.99, Convert.ToDouble(num2) * 1.01);
                }
                else if (matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    if (Regex.IsMatch(str, @"至多|(?<!不)少于|不多于|(?<!不)小于|不大于|不高于|(?<!不)低于|不超过|以下|内|以内"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, num);
                    }
                    else if (Regex.IsMatch(str, @"至少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
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
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"最贵|最高|最多"))
                    {
                        decimal? maxPrice = -1;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (maxPrice < onePhone.Price)
                                    maxPrice = onePhone.Price;
                            }
                            if (maxPrice < 0)
                            {
                                // 出现最贵的价格是负数的情况是因为根据用户提出的条件不能找到手机，也就是数据库手机都不合适，一般不会出现这种糊涂的问法
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(maxPrice) * 0.95, Convert.ToDouble(maxPrice) * 1.05);
                            }
                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (maxPrice < onePhone.Price)
                                    maxPrice = onePhone.Price;
                            }
                            if (maxPrice < 0)
                            {
                                // 出现最贵的价格是负数的情况是因为根据用户提出的条件不能找到手机，不会出现这种情况
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(maxPrice) * 0.95, Convert.ToDouble(maxPrice) * 1.05);
                            }
                        }
                    }
                    else if (Regex.IsMatch(str, @"最便宜|最低|最少"))
                    {
                        decimal? minPrice = int.MaxValue;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (minPrice > onePhone.Price)
                                    minPrice = onePhone.Price;
                            }
                            if (minPrice == int.MaxValue)
                            {
                                // 出现最便宜的价格是最大整数的情况是因为根据用户提出的条件不能找到手机，也就是数据库手机都不合适，一般不会出现这种糊涂的问法
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minPrice) * 0.95, Convert.ToDouble(minPrice) * 1.05);
                            }

                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (minPrice > onePhone.Price)
                                    minPrice = onePhone.Price;
                            }
                            if (minPrice == int.MaxValue)
                            {
                                // 出现最便宜的价格是最大整数的情况是因为根据用户提出的条件不能找到手机，不会出现这种情况
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minPrice) * 0.95, Convert.ToDouble(minPrice) * 1.05);
                            }
                        }
                    }
                    else if (Regex.IsMatch(str, @"贵一点|高一点|再高点|更高|贵点"))
                    {
                        if (userSession.liveTable.TalkingAbout == null)
                        {
                            if (userSession.liveTable.Parameter["价格"] == null)
                            {
                                list[i].normalData = string.Format("{0},{1}", 2000, int.MaxValue);
                            }
                            else
                            {
                                string[] infobounds = ((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["价格"]).items[0].normalData.Split(new char[] { ',' });
                                double infomin = Convert.ToDouble(infobounds[0]);
                                double infomax = Convert.ToDouble(infobounds[1]);
                                list[i].normalData = string.Format("{0},{1}", (int)infomax, int.MaxValue);
                            }
                        }
                        else
                        { 
                                list[i].normalData = string.Format("{0},{1}", Convert.ToInt32(userSession.liveTable.TalkingAbout.Price), int.MaxValue);
                        }
                    }
                    else if (Regex.IsMatch(str, @"便宜一点|低一点|再低点|更低|便宜点"))
                    {
                        if (userSession.liveTable.TalkingAbout == null)
                        {
                            if (userSession.liveTable.Parameter["价格"] == null)
                            {
                                list[i].normalData = string.Format("{0},{1}", 0, 2000);
                            }
                            else
                            {
                                string[] infobounds = ((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["价格"]).items[0].normalData.Split(new char[] { ',' });
                                double infomin = Convert.ToDouble(infobounds[0]);
                                double infomax = Convert.ToDouble(infobounds[1]);
                                list[i].normalData = string.Format("{0},{1}", 0, (int)infomin);
                            }
                        }
                        else
                        {
                            list[i].normalData = string.Format("{0},{1}", 0, Convert.ToInt32(userSession.liveTable.TalkingAbout.Price));
                        }
                    }
                    else if (Regex.IsMatch(str, @"不高|低|(?<!多)少|便宜|廉价|好"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, 2000);
                    }
                    else if (Regex.IsMatch(str, @"(?<!多)高|(?<!不)贵"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 2000, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"一般|普通|差不多|适中"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 1000, 2500);
                    }
                    else if(Regex.IsMatch(str, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["价格"] != null)
                        {
                            String[] prices = ((String)userSession.liveTable.Parameter["价格"]).Split(',');
                            list[i].normalData = string.Format("{0},{1}", prices[0], prices[1]);
                            list[i].isNegative = true;
                        }
                        /**
                         * 仍有其他价格为null的情况
                         * 别的未测试
                         * */
                    }
                    else if (Regex.IsMatch(str, @"随意|随便|都可以"))
                    {
                        decimal? minPrice = int.MaxValue;
                        decimal? maxPrice = int.MinValue;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (minPrice > onePhone.Price)
                                    minPrice = onePhone.Price;
                                if (maxPrice < onePhone.Price)
                                    maxPrice = onePhone.Price;
                            }
                            if (minPrice == int.MaxValue||maxPrice == int.MinValue)
                            {
                                // 出现最便宜或最高的的价格是最大整数的情况是因为根据用户提出的条件不能找到手机，也就是数据库手机都不合适，一般不会出现这种糊涂的问法
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minPrice) * 0.95, Convert.ToDouble(maxPrice) * 1.05);
                            }

                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (minPrice > onePhone.Price)
                                    minPrice = onePhone.Price;
                                if (maxPrice < onePhone.Price)
                                    maxPrice = onePhone.Price;
                            }
                            if (minPrice == int.MaxValue || maxPrice == int.MinValue)
                            {
                                // 出现最便宜的价格是最大整数的情况是因为根据用户提出的条件不能找到手机，不会出现这种情况
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minPrice) * 0.95, Convert.ToDouble(maxPrice) * 1.05);
                            }
                        }

                    }
                    else
                    {
                        // 这里当作匹配上那些需要句型配合处理的信息
                        list[i].normalData = str;
                    }
                }
                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要贵的
                 * 那么匹配上：贵
                 * 在“贵”前出现“不”这个否定词，因此认为是否定形式
                 * */
                if (sentence.Length > list[i].leftIndex)
                {
                    if (Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"(不)|(非(?!常))"))
                    {
                        list[i].isNegative = true;
                    }
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