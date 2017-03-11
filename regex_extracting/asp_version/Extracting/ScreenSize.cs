/*
 * author : xm
 * date : 2013/9/3
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class ScreenSize : ParameterBase
    {
        public ScreenSize(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "主屏尺寸";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"主屏尺寸|屏幕|主屏",
                @"寸|吋|英寸"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                //@"((?<![0-9a-zA-Z.])\d(.\d{1,2})?).*((?<![0-9a-zA-Z.])\d(.\d{1,2})?)",
                //@"(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])\d(.\d{1,2})?(寸|吋|英寸)(左右|以上|以下)*",
                //@"(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])[1-8]{1}\.\d(?!(寸|吋|英寸))(左右|以上|以下)*",
                @"(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])\d+\.*\d*(寸|吋|英寸)*(左右|以上|以下)*",
                // by WJD
                // 如果出现“小米”，则这里的“小”不算容量
                @"(?<!多)(宽)|(?<!多)(大)(?!小)|(?<!大|多)(小)",
                @"差不多|适中|一般",
                @"最大|最小|最多|最少|最高|最低|多一点|高一点|低一点|少一点|再少|再低|再高|再大|再小|更大|更小|好一点",
                @"随意|随便|都可以|其他|别的",
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

                MatchCollection matches = Regex.Matches(str, @"\d(.\d{1,2})?");

                if (matches.Count >= 2)
                {
                    string num1 = matches[0].Groups[0].ToString();
                    string num2 = matches[1].Groups[0].ToString();
                    list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(num1) * 0.99, Convert.ToDouble(num2) * 1.01);
                }
                else if(matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    if (Regex.IsMatch(str, @"至多|最多|(?<!不)少于|不多于|(?<!不)小于|不大于|不高于|(?<!不)低于|不超过|以下"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, num);
                    }
                    else if (Regex.IsMatch(str, @"好一点|至少|最少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
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
                    if (Regex.IsMatch(str, @"最大|最高|最多"))
                    {
                        decimal? max = -1;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (max < onePhone.ScreenSize)
                                    max = onePhone.ScreenSize;
                            }
                            if (max < 0)
                            {
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(max) * 0.95, Convert.ToDouble(max) * 1.05);
                            }
                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (max < onePhone.ScreenSize)
                                    max = onePhone.ScreenSize;
                            }
                            if (max < 0)
                            {
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(max) * 0.95, Convert.ToDouble(max) * 1.05);
                            }
                        }
                    }
                    else if (Regex.IsMatch(str, @"最小|最低|最少"))
                    {
                        decimal? min = int.MaxValue;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (min > onePhone.ScreenSize)
                                    min = onePhone.ScreenSize;
                            }
                            if (min == int.MaxValue)
                            {
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(min) * 0.95, Convert.ToDouble(min) * 1.05);
                            }
                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (min > onePhone.ScreenSize)
                                    min = onePhone.ScreenSize;
                            }
                            if (min == int.MaxValue)
                            {
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(min) * 0.95, Convert.ToDouble(min) * 1.05);
                            }
                        }
                    }
                    else if (Regex.IsMatch(str, @"大一点|高一点|再高|更高|再大|更大"))
                    {
                        if (userSession.liveTable.Parameter["主屏尺寸"] == null)
                        {
                            list[i].normalData = string.Format("{0},{1}", 4, int.MaxValue);
                        }
                        else
                        {
                            list[i].normalData = string.Format("{0},{1}", Convert.ToInt32(((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["主屏尺寸"]).items[0].rawData) * 1.1, int.MaxValue);
                        }
                    }
                    else if (Regex.IsMatch(str, @"少一点|低一点|再低|再少|更低|更少"))
                    {
                        if (userSession.liveTable.Parameter["主屏尺寸"] == null)
                        {
                            list[i].normalData = string.Format("{0},{1}", 0, 4);
                        }
                        else
                        {
                            list[i].normalData = string.Format("{0},{1}", 0, Convert.ToInt32(((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["主屏尺寸"]).items[0].rawData) * 1.1);
                        }
                    }
                    else if (Regex.IsMatch(str, @"好|大|宽"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 5.0, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"一般|差不多|适中"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 3.0, 5.5);
                    }
                    else if (Regex.IsMatch(str, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["主屏尺寸"] != null)
                        {
                            String[] ScreenSizes = ((String)userSession.liveTable.Parameter["主屏尺寸"]).Split(',');
                            list[i].normalData = string.Format("{0},{1}", ScreenSizes[0], ScreenSizes[1]);
                            list[i].isNegative = true;
                        }
                        /**
                         * 仍有其他主屏尺寸为null的情况
                         * 别的未测试
                         * */
                    }
                    else if (Regex.IsMatch(str, @"随意|随便|都可以"))
                    {
                        decimal? minScreenSize = int.MaxValue;
                        decimal? maxScreenSize = int.MinValue;
                        if (userSession.liveTable.Candidates.Count == 0)
                        {
                            List<Ch2R.Models.CellPhone> phoneList = Code.Inquiry.Screening(userSession.liveTable.Parameter, new ValidInfo());
                            foreach (var onePhone in phoneList)
                            {
                                if (minScreenSize > onePhone.ScreenSize)
                                    minScreenSize = onePhone.ScreenSize;
                                if (maxScreenSize < onePhone.ScreenSize)
                                    maxScreenSize = onePhone.ScreenSize;
                            }
                            if (minScreenSize == int.MaxValue || maxScreenSize == int.MinValue)
                            {
                                // 出现最便宜或最高的的主屏尺寸是最大整数的情况是因为根据用户提出的条件不能找到手机，也就是数据库手机都不合适，一般不会出现这种糊涂的问法
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minScreenSize) * 0.95, Convert.ToDouble(maxScreenSize) * 1.05);
                            }

                        }
                        else
                        {
                            List<Ch2R.Models.CellPhone> phoneList = userSession.liveTable.Candidates;
                            foreach (var onePhone in phoneList)
                            {
                                if (minScreenSize > onePhone.ScreenSize)
                                    minScreenSize = onePhone.ScreenSize;
                                if (maxScreenSize < onePhone.ScreenSize)
                                    maxScreenSize = onePhone.ScreenSize;
                            }
                            if (minScreenSize == int.MaxValue || maxScreenSize == int.MinValue)
                            {
                                // 出现最便宜的主屏尺寸是最大整数的情况是因为根据用户提出的条件不能找到手机，不会出现这种情况
                            }
                            else
                            {
                                list[i].normalData = string.Format("{0},{1}", Convert.ToDouble(minScreenSize) * 0.95, Convert.ToDouble(maxScreenSize) * 1.05);
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
                 * 比如用户说：我不要太贵的
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