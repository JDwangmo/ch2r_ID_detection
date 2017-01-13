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
    public class BatteryCapacity : ParameterBase
    {
        public BatteryCapacity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "�������";

            #region ��ʼ��������ʽ
            // ����������ʽ
            StatementRegexs = new string[] {
                @"�������"
            };
            // ֵ������ʽ
            ValueRegexs = new string[] {
                @"(����|����|��С��)\d{3,5}(m|M)?(a|A)?(h|H)?",
                @"(?<!��)��|һ��|����|��",
                @"����|���|������|����|���",
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
                    if (userSession.liveTable.Parameter["�������"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["�������"];
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
            int capacity = 0;

            MatchCollection matches = Regex.Matches(str, "\\d+");

            if (matches.Count == 1)
            {
                str_tmp = matches[0].Groups[0].ToString();
                capacity = Convert.ToInt32(str_tmp);

                if (Regex.IsMatch(str, "����|С��|������|������|������|������|(?<!��)����"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "0", capacity.ToString(), str);
                }
                else if (Regex.IsMatch(str, "����|(?<!��)����|(?<!��)����|������|������|��С��|(?<!��)����"))
                {
                    Formated = string.Format("[{0},{1}]({2})", capacity.ToString(), int.MaxValue.ToString(), str);
                }
                else if (Regex.IsMatch(str, "����|���|���"))
                {
                    double capacity_double = (double)capacity;
                    double down_double = capacity_double * 0.8;
                    double up_double = capacity_double * 1.2;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("[{0},{1}]({2})", down.ToString(), up.ToString(), str);
                }
                else
                {
                    double capacity_double = (double)capacity;
                    double down_double = capacity_double * 0.9;
                    double up_double = capacity_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("[{0},{1}]({2})", down.ToString(), up.ToString(), str);
                }
            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "(?<!��)��|(?<!��)��"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "2001", int.MaxValue.ToString(), str);
                }
                else if (Regex.IsMatch(str, "һ��|����"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "1000", "2000", str);
                }
                else if (Regex.IsMatch(str, "��|С"))
                {
                    Formated = string.Format("[{0},{1}]({2})", "0", "999", str);
                }
            }
            else if (matches.Count > 1)
            {

            }

            return Formated;
        }
    }
}