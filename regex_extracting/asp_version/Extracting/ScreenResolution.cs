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
    public class ScreenResolution : ParameterBase
    {
        public ScreenResolution(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "主屏分辨率";

            #region 初始化正则表达式
            //描述正则表达式
            StatementRegexs = new string[]{
                @"主屏分辨率|屏幕分辨率|(?<!屏幕)像素|(?<!主屏幕)(的)*分辨率|主屏幕(的)*像素"
            };
            //值正则表达式
            ValueRegexs = new string[]{
                @"\d{2,4}\*\d{2,4}像素",
                @"随意|随便|都可以|其他|别的"
            };
            #endregion


            RegexProcess();
        }

        public override void Format()
        {
            
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                MatchCollection matches = Regex.Matches(str, @"\d{2,4}\*\d{2,4}");

                if (matches.Count >= 2)
                {
                    //string num1 = matches[0].Groups[0].ToString();
                    //string num2 = matches[1].Groups[0].ToString();

                    //list[i].normalData = string.Format("{0},{1},{2}", Convert.ToDouble(num1) * 0.99, "*", Convert.ToDouble(num2) * 1.01);
                }
                else if (matches.Count == 1)
                {
                    string num = matches[0].Groups[0].ToString();

                    
                    if (Regex.IsMatch(str, @"至少|最少|不少于|(?<!不)多于|不小于|(?<!不)大于|(?<!不)高于|不低于|(?<!不)超过|以上"))
                    {
                        list[i].normalData = string.Format("{0},{1},{2}", num, "*", int.MaxValue);
                    }
                    else if (Regex.IsMatch(str, @"差不多|大概|大约|左右"))
                    {
                        list[i].normalData = string.Format("{0},{1}{2}", Convert.ToDouble(num) * 0.8, "*", Convert.ToDouble(num) * 1.2);
                    }
                    else
                    {
                        // 这里当作只有匹配到一串数字
                        list[i].normalData = string.Format("{0},{1},{2}", Convert.ToDouble(num), "*", Convert.ToDouble(num));
                    }
                }
                else if (matches.Count == 0)
                {
                    if (Regex.IsMatch(str, @"(?<!多)高|(?<!多)大"))
                    {
                        //list[i].normalData = string.Format("{0},{1}", "480*500", int.MaxValue.ToString() + "*" + int.MaxValue.ToString());
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

            /* 
             * 主屏色彩不能在同一款手机上可以共存
             * 所以用“or”
             * */

            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}