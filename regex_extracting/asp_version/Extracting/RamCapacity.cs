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
    public class RamCapacity : ParameterBase
    {
        public RamCapacity(Areas.Chat.Models.UserSession userSession, string sentence)
            : base(userSession, sentence)
        {
            name = "RAM����";

            #region ��ʼ��������ʽ
            // ����������ʽ
            StatementRegexs = new string[] {
                @"RAM|�ֻ��ڴ�|�����ڴ�|�ڴ�"
            };
            // ֵ������ʽ
            ValueRegexs = new string[] {
                @"(����|С��|����|����|������)*\d{1,6}(MB|GB|G|M|m|g|gb|mb)",
                @"\d{1,6}(MB|GB|G|M|m|g|gb|mb)(����|����)",
                // by WJD
                // ������֡�С�ס���������ġ�С����������
                @"(?<!��)(��)(?!С)|(?<!��|��)(С)",
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

            MatchCollection matches = Regex.Matches(str, "\\d{1,6}");

            string RamCapacity_str;
            int RamCapacity;
            if (matches.Count == 1)
            {
                if (Regex.IsMatch(str, "M|MB|m|mb"))
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str);
                }
                else if (Regex.IsMatch(str, "G|GB|g|gb"))
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str) * 1024;
                }
                else
                {
                    RamCapacity_str = matches[0].Groups[0].ToString();
                    RamCapacity = Convert.ToInt32(RamCapacity_str);
                }

                if (Regex.IsMatch(str, "����|С��|������|������|������|(?<!��)����"))
                {
                    Formated = string.Format("{0},{1}", "0", RamCapacity.ToString());
                }
                else if (Regex.IsMatch(str, "����|(?<!��)����|(?<!��)����|������|������|(?<!��)����"))
                {
                    Formated = string.Format("{0},{1}", RamCapacity.ToString(), int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "���|���"))
                {
                    double RamCapacity_double = (double)RamCapacity;
                    double down_double = RamCapacity_double * 0.8;
                    double up_double = RamCapacity_double * 1.2;
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
                    double RamCapacity_double = (double)RamCapacity;
                    double down_double = RamCapacity_double * 0.9;
                    double up_double = RamCapacity_double * 1.1;
                    int down = (int)down_double;
                    int up = (int)up_double;
                    Formated = string.Format("{0},{1}", down.ToString(), up.ToString());
                }
            }
            else if (matches.Count == 2)
            {

            }
            else if (matches.Count == 0)
            {
                if (Regex.IsMatch(str, "(?<!��)��|(?<!��)��"))
                {
                    Formated = string.Format("{0},{1}", "1025", int.MaxValue.ToString());
                }
                else if (Regex.IsMatch(str, "һ��|����"))
                {
                    Formated = string.Format("{0},{1}", "1024", "1024");
                }
                else if (Regex.IsMatch(str, "С|��"))
                {
                    Formated = string.Format("{0},{1}", "0", "1023");
                }
            }
            return Formated; 
       }

    }
}