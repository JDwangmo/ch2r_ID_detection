/*
 * author : wgs
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
    public class ModernFunction : ParameterBase
    {
        public ModernFunction (Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "Modem����";

            #region ��ʼ��������ʽ
            // ����������ʽ
            StatementRegexs = new string[] {
                @"Modem"
            };
            // ֵ������ʽ
            ValueRegexs = new string[] {
                @"֧��modem",
                @"����|���|������|����|���"
            };
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (Regex.IsMatch(list[i].rawData, @"����|���|������"))
                {
                    list[i].normalData = "";
                }
                else if (Regex.IsMatch(list[i].rawData, @"���|������"))
                {
                    if (userSession.liveTable.Parameter["Modem����"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["Modem����"];
                        list[i].isNegative = true;
                    }
                    else
                    {
                        /**
                         * ���������۸�Ϊnull�����
                         * ���δ����
                         * */
                    }
                }
                else
                {
                    list[i].normalData = list[i].rawData;
                }

                if (Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"(��)|(��(?!��))"))
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

            InfoBlock ib = new InfoBlock(name, "and", L);

            return ib;
        }
    }
}