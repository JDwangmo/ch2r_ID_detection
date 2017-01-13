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
    public class StandbyTime : ParameterBase
    {
        public StandbyTime(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "���۴���ʱ��";

            #region ��ʼ��������ʽ
            // ����������ʽ
            StatementRegexs = new string[] {
                @"����"
            };
            // ֵ������ʽ
            ValueRegexs = new string[] {
                @"(����|����|����|С��|����|����|������)?(\d+Сʱ|\d+��|\d+����)",
                @"��|��|һ��|����|���|��",
                @"����|���|������"
            };
            #endregion

            RegexProcess();
        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                string str = list[i].rawData;

                list[i].normalData = ToInterval(str);

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

        public string ToInterval(string str)
        {
            string Formated = "";
            string str_tmp = "";
            int time = 0;

            MatchCollection matches = Regex.Matches(str, "\\d+");

            if (matches.Count == 1)
            {

                if (Regex.IsMatch(str, "��|��"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp) * 24;
                }
                else if (Regex.IsMatch(str, "Сʱ"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp);
                }
                else if (Regex.IsMatch(str, "����"))
                {
                    str_tmp = matches[0].Groups[0].ToString();
                    time = Convert.ToInt32(str_tmp) / 60;
                }

                if (Regex.IsMatch(str, "����|С��|������|������|������|������|(?<!��)����"))
                {
                    Formated = string.Format("{0},{1}", "0", time.ToString());
                }
                else if (Regex.IsMatch(str, "����|(?<!��)����|(?<!��)����|������|������|��С��|(?<!��)����"))
                {
                    Formated = string.Format("{0},{1}", time.ToString(), int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "����|���|���"))
                {
                    double time_double = (double)time;
                    double down_double = time_double * 0.8;
                    double up_double = time_double * 1.2;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
                else if (Regex.IsMatch(str, @"����|���|������"))
                {
                    Formated = string.Format("{0},{1}", 0, int.MaxValue);
                }
                else
                {
                    double time_double = (double)time;
                    double down_double = time_double * 0.9;
                    double up_double = time_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "��|��"))
                {
                    Formated = string.Format("{0},{1}", "401", int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "һ��|����|���"))
                {
                    Formated = string.Format("{0},{1}", "200", "400");
                }
                else if (Regex.IsMatch(str, "��"))
                {
                    Formated = string.Format("{0},{1}", "0", "199");
                }
            }
            else if (matches.Count > 1)
            {

            }
            return Formated;
        }

    }
}