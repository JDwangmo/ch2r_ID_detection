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
    public class GpuModel : ParameterBase
    {
        public GpuModel(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "GPU型号";

            #region 初始化正则表达式
            //描述正则表达式

            StatementRegexs = new string[]{
                @"GPU型号"
            };
            //值正则表达式

            ValueRegexs = new string[]{
                @"Imagination|Nvidia|Adreno|高通|SPREADTRUM|ARM|PowerVR",
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
                    if (userSession.liveTable.Parameter["GPU型号"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["GPU型号"];
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

                /* 
                 * 这里应该不会出现如：
                 * 我不要XXX的GPU型号的手机
                 * 这么专业的话吧
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

            /**
             * 不同类型的GPU应该不能同时存在吧
             * 所以这里用“or”
             * */
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}