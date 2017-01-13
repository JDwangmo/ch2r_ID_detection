/*
 * author : xm
 * date : 2013/9/11
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Ch2R.Code.Extracting
{
    abstract public class ParameterBase
    {
        /// <summary>
        /// 用户会话
        /// </summary>
        public Areas.Chat.Models.UserSession userSession;

        /// <summary>
        /// 参数名
        /// </summary>
        public string name;
        
        /// <summary>
        /// 要处理的句子
        /// </summary>
        public string sentence;

        /// <summary>
        /// 信息元数据集合
        /// </summary>
        public List<InfoMetadata> list;
        
        /// <summary>
        /// 描述正则表达式
        /// </summary>
        public string[] StatementRegexs;
        /// <summary>
        /// 值正则表达式
        /// </summary>
        public string[] ValueRegexs;

        public ParameterBase(Areas.Chat.Models.UserSession userSession, string sentence)
        {
            this.userSession = userSession;
            this.sentence = sentence;
            this.list = new List<InfoMetadata>();
        }

        /// <summary>
        /// 进行正则提取处理，没有进行否定判断
        /// </summary>
        /// <param name="sentence"></param>
        public void RegexProcess()
        {
            // 处理描述性正则表达式数组
            string pattern = "";
            for (int i = 0; i < StatementRegexs.Length; ++i)
            {
                pattern += string.Format("({0})|", StatementRegexs[i]);
            }
            pattern = pattern.Substring(0, pattern.Length - 1);
            MatchCollection mc = Regex.Matches(sentence, pattern);
            foreach (Match m in mc)
            {
                InfoMetadata iu = new InfoMetadata();
                iu.rawData = m.Groups[0].Value;
                iu.normalData = this.name;  
                iu.leftIndex = m.Groups[0].Index;
                iu.rightIndex = m.Groups[0].Index + m.Groups[0].Length;
                iu.isStatement = true;
                list.Add(iu);
            }
            // 处理值性正则表达式数组
            pattern = "";
            for (int i = 0; i < ValueRegexs.Length; ++i)
            {
                pattern += string.Format("({0})|", ValueRegexs[i]);
            }
            pattern = pattern.Substring(0, pattern.Length - 1);
            mc = Regex.Matches(sentence, pattern);
            foreach (Match m in mc)
            {
                InfoMetadata iu = new InfoMetadata();
                iu.rawData = m.Groups[0].Value;
                iu.leftIndex = m.Groups[0].Index;
                iu.rightIndex = m.Groups[0].Index + m.Groups[0].Length;
                iu.isStatement = false;
                list.Add(iu);
            }
        }

        /// <summary>
        /// 格式化提取到的信息
        /// </summary>
        public virtual void Format()
        {

        }
        
        /// <summary>
        /// 生成信息块对象
        /// </summary>
        /// <returns></returns>
        public virtual InfoBlock ToInfoBlock()
        {
            return null;
        }
    }
}