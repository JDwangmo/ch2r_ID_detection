/*
 * author : wxp
 * date : 2013/10/07
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class RearCameraPixel : ParameterBase
    {
        public RearCameraPixel(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "后置摄像头像素";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[]
            {
                @"后置(的)*像头(的)*像素|后置(的)*摄像头|(?<!后置)摄像头|(?<!后置摄像头)像素|后置(的)*(?<!摄像头)|(?<!后置)摄像(头)*"
            };
            // 值正则表达式
            ValueRegexs = new string[]
            {
                /* 涉及到数字的好像都会遇到范围和比较的问题
                 */
                @"(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(\d{1,8}万(像素)?)(左右|以上|以下)*",
                @"最大|最小|最多|最少|最高|最低|多一点|高一点|低一点|少一点|好一点",
                @"(?<!多)高|低",
                @"随意|随便|都可以|其他|别的",
                @"(?<!多)好|(?<!多)大|(?<!多)宽|(?<!多)高"
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
                else if (matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    if (Regex.IsMatch(str, @"至多|最多|(?<!不)少于|不多于|(?<!不)小于|不大于|不高于|(?<!不)低于|不超过|以下"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, num);
                    }
                    else if (Regex.IsMatch(str, @"至少|最少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
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
                                if (max < onePhone.RearCameraPixel)
                                    max = onePhone.RearCameraPixel;
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
                                if (max < onePhone.RearCameraPixel)
                                    max = onePhone.RearCameraPixel;
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
                                if (min > onePhone.RearCameraPixel)
                                    min = onePhone.RearCameraPixel;
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
                                if (min > onePhone.RearCameraPixel)
                                    min = onePhone.RearCameraPixel;
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
                    else if (Regex.IsMatch(str, @"大一点|高一点|好一点"))
                    {
                        if (userSession.liveTable.Parameter["后置摄像头像素"] == null)
                        {
                            list[i].normalData = string.Format("{0},{1}", 200, int.MaxValue);
                        }
                        else
                        {
                            list[i].normalData = string.Format("{0},{1}", Convert.ToInt32(((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["后置摄像头像素"]).items[0].rawData) * 1.1, int.MaxValue);
                        }
                    }
                    else if (Regex.IsMatch(str, @"少一点|低一点"))
                    {
                        if (userSession.liveTable.Parameter["后置摄像头像素"] == null)
                        {
                            list[i].normalData = string.Format("{0},{1}", 0, 200);
                        }
                        else
                        {
                            list[i].normalData = string.Format("{0},{1}", 0, Convert.ToInt32(((Ch2R.Code.Extracting.InfoBlock)userSession.liveTable.Parameter["后置摄像头像素"]).items[0].rawData) * 1.1);
                        }
                    }
                    else if (Regex.IsMatch(str, @"(?<!多)好|(?<!多)大|(?<!多)宽|(?<!多)高"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 495, int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"一般|差不多|适中"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 200, 490);
                    }
                    else if (Regex.IsMatch(str, @"低"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, 199);
                    }
                    else if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                    {
                        list[i].normalData = string.Format("{0},{1}", 0, int.MaxValue);
                    }
                    else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                    {
                        if (userSession.liveTable.Parameter["后置摄像头像素"] != null)
                        {
                            String[] score = ((String)userSession.liveTable.Parameter["后置摄像头像素"]).Split(',');
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
             * 摄像头参数信息是相互独立的
             */
            return ib;
        }
    }
}