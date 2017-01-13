/*
 * author : xm
 * date : 2013/9/2
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    public class Brand : ParameterBase
    {
        public Brand(Areas.Chat.Models.UserSession userSession, string sentence) : base(userSession, sentence)
        {
            name = "品牌";

            #region 初始化正则表达式
            // 描述正则表达式
            StatementRegexs = new string[] {
                @"品牌|牌"
            };
            // 值正则表达式
            ValueRegexs = new string[] {
                @"红米|诺基亚|nokia|Nokia|NOKIA|三星|SAMSUNG|samsung|Samsung|苹果|HTC|htc|华为|联想|lenovo|Lenovo|LENOVO|
                  步步高|酷派|金立|魅族|黑莓|天语|摩托罗拉|OPPO|oppo|经纬|飞利浦|小米|中兴|云台|LG|lg|TCL|tcl|华硕|海信|
                  长虹|海尔|康佳|夏新|纽曼|亿通|乐派|七喜|阿尔法|富士通|Yahoo|谷歌|卡西欧|优派|技嘉|惠普|多普达|东芝|爱国者|
                  明基|万利达|戴尔|中天|夏普|索尼|努比亚|锤子|LG|lg|小辣椒",
                @"随意|随便|都可以|其他|别的",
                @"好一点",
                @"好",
            };
            // 通过数据库调出数据进行初始化
            //Models.Ch2RDbContext db = new Models.Ch2RDbContext();
            //Models.Semantic sem = db.Semantics.SingleOrDefault(i => i.Name == name);
            //StatementRegexs = sem.StatementRegex.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //ValueRegexs = sem.ValueRegex.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            #endregion

            RegexProcess();

        }

        public override void Format()
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (Regex.IsMatch(list[i].rawData, @"随意|随便|都可以"))
                {
                    list[i].normalData = "";
                }
                else if (Regex.IsMatch(list[i].rawData, @"别的|其他的"))
                {
                    if (userSession.liveTable.Parameter["品牌"] != null)
                    {
                        list[i].normalData = (String)userSession.liveTable.Parameter["品牌"];
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
                else if (Regex.IsMatch(list[i].rawData, @"好|好一点"))
                { 
                    //这里不能拿总是三星的，要写手机属性的比较方法才靠谱
                    list[i].normalData = "三星";
                }
                else
                {
                    list[i].normalData = list[i].rawData;
                }
                /* 这里处理一下否定情况
                 * 做法先只是简单的判断原句中信息元数据之前有没有出现否定语
                 * 比如用户说：我不要三星的
                 * 那么匹配上：三星
                 * 在“三星”前出现“不”这个否定词，因此认为是否定形式
                 * 但会不会出现某种蛋疼的情况呢：三星的我不喜欢
                 * 擦！好吧，还有更蛋碎的：不是三星的我不要！！！
                 * 那是不是要数一下有多少个否定词然后负负得正什么的呢？！
                 * 妈的要是这种情况：你以为我会不喜欢诺基亚吗？
                 * 。。。
                 * */
                if (Regex.IsMatch(sentence.Substring(0, list[i].leftIndex), @"(不)|(非(?!常))"))
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
            InfoBlock ib = new InfoBlock(name, "or", L);

            return ib;
        }
    }
}