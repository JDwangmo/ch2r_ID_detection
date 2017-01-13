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
    public class Model : ParameterBase
    {
        public Model(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "型号";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"型号"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"(?<![0-9a-zA-Z_-])[0-9a-zA-Z_-]{1,20}(?![0-9a-zA-Z_-])(?!.*元|块)",
                //@"随意|随便|都可以|其他|别的"//还未改
            };
            #endregion

            RegexProcess();

            // 在这里排除掉那些HTC，OPPO，LG，TCL，Yahoo等英文品牌，可能还会有其它情况要排除,把Hello，Hi等等排除
            for (int i = 0; i < list.Count; i++)
            {
                if (Regex.IsMatch(list[i].rawData, @"^(RAM|ROM|iphone|HTC|OPPO|LG|TCL|Yahoo|SAMSUNG|Hello|Hi)$"))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

            // 然后对类似HTCT329W的情况进行处理,只提取T329W
            for (int i = 0; i < list.Count; i++)
            {

                if (Regex.IsMatch(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)"))
                {
                    //int brandBegin = list[i].rawData.IndexOf(@"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo)");
                    int brandBegin = Regex.Match(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)").Index;
                    if (brandBegin == 0)
                    {
                        int lengthOfBrand = list[i].rawData.Length - Regex.Replace(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)", "").Length;
                        list[i].rawData = Regex.Replace(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)", "");
                        list[i].leftIndex += lengthOfBrand;
                    }
                    else
                    {
                        int lengthOfBrand = list[i].rawData.Length - Regex.Replace(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)", "").Length;
                        list[i].rawData = Regex.Replace(list[i].rawData, @"(HTC|htc|OPPO|oppo|LG|lg|TCL|tcl|Yahoo|yahoo|SAMSUNG|samsung)", "");
                        list[i].rightIndex -= lengthOfBrand;
                    }
                }
            }
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                list[i].normalData = list[i].rawData;
                // 感觉上一般是不会出现：我不要XX型号的吧
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