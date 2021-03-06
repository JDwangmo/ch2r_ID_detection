/*
 * author : xm
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
    /// <summary>
    /// 信息提取，提取的信息并没有进行组织
    /// 数字转区间不会在这里处理，但是会进行信息格式化
    /// 所谓信息格式化是指把原始数据格式化成具有一定规范的格式，统一单位等等
    /// </summary>
    public class Extracting
    {
        /// <summary>
        /// 需要提取信息的句子
        /// </summary>
        public string sentence;

        /// <summary>
        /// 用户会话
        /// </summary>
        public Areas.Chat.Models.UserSession userSession;

        /// <summary>
        /// 冲突确认反问
        /// </summary>
        public string conflictConfirm;

        /// <summary>
        /// 信息映射表
        /// </summary>
        //public Hashtable info = new Hashtable();

        /// <summary>
        /// 用于暂时存放参数类
        /// </summary>
        public List<ParameterBase> PBSet = new List<ParameterBase>();

        /// <summary>
        /// 存放提取到的原始数据
        /// </summary>
        /// <param name="userSession"></param>
        /// <param name="sentence"></param>
        public Code.Extracting.ValidInfo originalData = null;

        public Extracting(Areas.Chat.Models.UserSession userSession, string sentence)
        {
            this.sentence = sentence;
            this.userSession = userSession;

            Process();
        }



        public void Process()
        {
            #region 逐个参数进行处理
            PBSet.Add(new Price(userSession, sentence));
            // 主体属性
            PBSet.Add(new Brand(userSession, sentence));
            PBSet.Add(new Model(userSession, sentence));
            PBSet.Add(new Color(userSession, sentence));
            PBSet.Add(new ListingDate(userSession, sentence));
            PBSet.Add(new ExteriorDesign(userSession, sentence));
            PBSet.Add(new VidioCall(userSession, sentence));
            PBSet.Add(new OS(userSession, sentence));
            PBSet.Add(new PhoneType(userSession, sentence));
            PBSet.Add(new CpuModel(userSession, sentence));
            PBSet.Add(new CpuCoreNumber(userSession, sentence));
            PBSet.Add(new CpuFrequency(userSession, sentence));
            PBSet.Add(new GpuModel(userSession, sentence));
            PBSet.Add(new KeyboardType(userSession, sentence));
            PBSet.Add(new InputMode(userSession, sentence));
            PBSet.Add(new UserInterface(userSession, sentence));
            PBSet.Add(new SimCardType(userSession, sentence));
            PBSet.Add(new Weight(userSession, sentence));
            PBSet.Add(new PhoneQuality(userSession, sentence));

            // 网络属性
            PBSet.Add(new NetworkModel(userSession, sentence));
            PBSet.Add(new NetworkFrequency(userSession, sentence));
            PBSet.Add(new DataService(userSession, sentence));
            PBSet.Add(new Browser(userSession, sentence));

            // 存储属性
            PBSet.Add(new RomCapacity(userSession, sentence));
            PBSet.Add(new MemoryCardType(userSession, sentence));
            PBSet.Add(new RamCapacity(userSession, sentence));
            PBSet.Add(new MaxExpandCapacity(userSession, sentence));
            PBSet.Add(new HotSwappable(userSession, sentence));

            // 显示属性
            PBSet.Add(new ScreenSize(userSession, sentence));
            PBSet.Add(new ScreenColor(userSession, sentence));
            PBSet.Add(new ScreenMaterial(userSession, sentence));
            PBSet.Add(new ScreenResolution(userSession, sentence));
            PBSet.Add(new SuperFont(userSession, sentence));
            PBSet.Add(new TouchScreenType(userSession, sentence));
            PBSet.Add(new SensorType(userSession, sentence));
            PBSet.Add(new ProduceFeature(userSession, sentence));
            

            // 娱乐功能
            PBSet.Add(new VidioFormat(userSession, sentence));
            PBSet.Add(new AudioFormat(userSession, sentence));
            PBSet.Add(new Flash(userSession, sentence));
            PBSet.Add(new Java(userSession, sentence));
            PBSet.Add(new TV(userSession, sentence));
            PBSet.Add(new Radio(userSession, sentence));
            PBSet.Add(new Game(userSession, sentence));
            PBSet.Add(new Software(userSession, sentence));


            // 摄像属性
            PBSet.Add(new FrontCameraPixel(userSession, sentence));
            PBSet.Add(new RearCameraPixel(userSession, sentence));
            PBSet.Add(new FlashType(userSession, sentence));
            PBSet.Add(new SensorType(userSession, sentence));
            PBSet.Add(new FocusMode(userSession, sentence));
            PBSet.Add(new ZoomMode(userSession, sentence));
            PBSet.Add(new ZoomMagnification(userSession, sentence));
            PBSet.Add(new MaxCameraSize(userSession, sentence));
            PBSet.Add(new ShootingMode(userSession, sentence));
            PBSet.Add(new CameraFeature(userSession, sentence));

            // 传输功能
            PBSet.Add(new GpsNavigation(userSession, sentence));
            PBSet.Add(new WLan(userSession, sentence));
            PBSet.Add(new WifiHotspot(userSession, sentence));
            PBSet.Add(new Bluetooth(userSession, sentence));
            PBSet.Add(new ModernFunction(userSession, sentence));
            PBSet.Add(new DataFunction(userSession, sentence));
            PBSet.Add(new DataInterface(userSession, sentence));

            // 电池属性
            PBSet.Add(new BatteryType(userSession, sentence));
            PBSet.Add(new BatteryCapacity(userSession, sentence));
            PBSet.Add(new TalkingTime(userSession, sentence));
            PBSet.Add(new StandbyTime(userSession, sentence));
            

            // 普通功能属性
            PBSet.Add(new BasicFunction(userSession, sentence));
            PBSet.Add(new BusinessFunction(userSession, sentence));
            //PBSet.Add(new ExpandProgramPlatform(userSession, sentence));

            // 插孔属性
            PBSet.Add(new HeadphoneJackType(userSession, sentence));
            //PBSet.Add(new DataWireType(userSession, sentence));

            // 保修属性
            PBSet.Add(new WarrantyPolicy(userSession, sentence));
            PBSet.Add(new WarrantyTime(userSession, sentence));
            //PBSet.Add(new WarrantyDetail(userSession, sentence));

            // 其它属性
            PBSet.Add(new Peripherals(userSession, sentence));
            PBSet.Add(new PhoneAttachment(userSession, sentence));
            PBSet.Add(new Gift(userSession, sentence));
            PBSet.Add(new GoodsSource(userSession, sentence));
            PBSet.Add(new UserType(userSession, sentence));
            PBSet.Add(new StopProduction(userSession, sentence));
            PBSet.Add(new Discount(userSession, sentence));
            PBSet.Add(new SalesVolume(userSession, sentence));
            PBSet.Add(new Popularity(userSession, sentence));

            // 得分属性
            PBSet.Add(new ScoreCostPerformance(userSession, sentence));
            PBSet.Add(new ScoreAmountPhone(userSession, sentence));
            PBSet.Add(new ScorePerformance(userSession, sentence));
            PBSet.Add(new ScoreShape(userSession, sentence));
            //PBSet.Add(new Score(userSession, sentence));
            //PBSet.Add(new Score(userSession, sentence));
            //PBSet.Add(new Score(userSession, sentence));
            //PBSet.Add(new Score(userSession, sentence));

            // 评价性语义
            //PBSet.Add(new BatteryScore(userSession, sentence));
            //PBSet.Add(new ScreenScore(userSession, sentence));
            //PBSet.Add(new ShootScore(userSession, sentence));
            //PBSet.Add(new MediaScore(userSession, sentence));
            //PBSet.Add(new OutlookScore(userSession, sentence));
            //PBSet.Add(new CostScore(userSession, sentence));
            //PBSet.Add(new Advantage(userSession, sentence));
            //PBSet.Add(new Disadvantage(userSession, sentence));
            #endregion

            #region 如果list里没有元素，则把它从PBSet里移除
            for (int i = 0; i < PBSet.Count; ++i)
            {
                if (PBSet[i].list.Count <= 0)
                {
                    PBSet.RemoveAt(i);
                    i--;
                }
            }

            //这里存下的原始数据会传给句型判断和前台显示
            originalData = this.ToValidInfo();

            #endregion

            #region 补值
            /* 这里处理这样一种情况：
             * 就是上次有参数冲突无法解决时，系统进行反问
             * 那么如果用户配合，则会从反问中提到的若干参数选择一个
             * 则此时应该判断用户所说的参数是否为反问中的一个，并把上次记下的值补回去
             * */
            if (!string.IsNullOrEmpty(userSession.liveTable.LastOutput))
            {
                if (Regex.IsMatch(userSession.liveTable.LastOutput, @"请问您问的是.*?.*还是.*?？"))
                {
                    if (PBSet.Count >= 1)
                    {
                        if (PBSet[0].list.Count >= 0)
                        {
                            if (userSession.liveTable.RelatedValue != null)
                            {
                                for (int i = 0; i < PBSet.Count; i++)
                                    PBSet[i].list = userSession.liveTable.RelatedValue;
                            }
                        }
                    }
                }
            }
            #endregion

            ConflictProcess();

            #region 对有效参数信息进行格式化，其实是否要判断一下有没有冲突先呢？因为冲突的话直接反问，都不用继续处理了
            /* 注意这里的格式化也只是尽量的格式化
             * 就是说，对于那些需要配合句型才能处理的值就只是保留原始格式
             * 例如：主屏尺寸：大一点
             * 显然不能确定它是普通意义上的大一点还是跟原来的进行比较
             * 不过对于“2000块以下”这样的信息，则可以直接将其转为区间：0,2000
             * 这里的区间没加方括号主要是为了提取方便
             * */
            for (int i = 0; i < PBSet.Count; i++)
            {
                PBSet[i].Format();
            }
            #endregion

        }


        /// <summary>
        /// 冲突处理，同时移除那些混淆参数信息和那些描述性的信息
        /// </summary>
        private void ConflictProcess()
        {
            #region 计算将所有参数的分值,保留最高分的
            //for (int order = 0; order < PBSet.Count; ++order )
            //{

            //}
            #endregion

            #region 从提取的信息中任选两个不同的参数进行冲突检测
            for (int I = 0; I < PBSet.Count; I++)
            {
                for (int J = 0; J < PBSet.Count; J++)
                {
                    if (PBSet[I].name == PBSet[J].name) continue;

                    List<InfoMetadata> iuA = PBSet[I].list;
                    List<InfoMetadata> iuB = PBSet[J].list;

                    for (int i = 0; i < iuA.Count; ++i)
                    {
                        for (int j = 0; j < iuB.Count; ++j)
                        {
                            // 当两个不同的参数的相关匹配值重叠时可以断定此两参数关于此值冲突
                            if (Overlap(iuA[i].leftIndex, iuA[i].rightIndex - 1, iuB[j].leftIndex, iuB[j].rightIndex - 1))
                            {
                                // 对参数下的某个值进行算分，保留分数高的
                                int scoreA = GetScore(userSession, PBSet[I], iuA, i);
                                int scoreB = GetScore(userSession, PBSet[J], iuB, j);
                                if (scoreA > scoreB)
                                {
                                    iuB.Remove(iuB[j]);
                                    break;
                                    //j--;
                                }
                                else if (scoreB > scoreA)
                                {
                                    iuA.Remove(iuA[i]);
                                    break;
                                    //i--;
                                }
                                else
                                {
                                    /* 如果分数相同，则根据上文记录的参数来猜测要排除哪个参数
                                     * 感觉上，只要正则表达式写得好一点，用户不故意乱说，非评价类（也是就是那些好一点什么的）的参数很少冲突
                                     * 进入这里通常是这样的情况：
                                     * 主动引导时系统说：请问您想要多少像素的？
                                     * 用户来一句：一般就行
                                     * 这时价格和后置摄像头像素冲突
                                     * */

                                    if (PBSet[I].name == userSession.liveTable.RelatedParameter)
                                    {
                                        iuB.Remove(iuB[j]);
                                        break;
                                        //j--;
                                    }
                                    else if (PBSet[J].name == userSession.liveTable.RelatedParameter)
                                    {
                                        iuA.Remove(iuA[i]);
                                        break;
                                        //i--;
                                    }
                                }
                            }
                        }
                    }
                }
            }



            #endregion

            #region 删除没有任何信息单元的参数信息
            for (int i = 0; i < PBSet.Count; ++i)
            {
                if (PBSet[i].list.Count <= 0)
                {
                    PBSet.RemoveAt(i);
                    i--;
                }
            }
            #endregion



            for (int I = 0; I < PBSet.Count; I++)
            {
                for (int J = 0; J < PBSet.Count; J++)
                {
                    if (PBSet[I].name == PBSet[J].name) continue;

                    List<InfoMetadata> iuA = PBSet[I].list;
                    List<InfoMetadata> iuB = PBSet[J].list;

                    for (int i = 0; i < iuA.Count; ++i)
                    {
                        for (int j = 0; j < iuB.Count; ++j)
                        {
                            // 当两个不同的参数的相关匹配值重叠时可以断定此两参数关于此值冲突
                            if (Overlap(iuA[i].leftIndex, iuA[i].rightIndex - 1, iuB[j].leftIndex, iuB[j].rightIndex - 1))
                            {
                                // 对参数下的某个值进行算分，保留分数高的
                                int scoreA = GetScore(userSession, PBSet[I], iuA, i);
                                int scoreB = GetScore(userSession, PBSet[J], iuB, j);
                                if (scoreA > scoreB)
                                {
                                    iuB.Remove(iuB[j]);
                                    break;
                                    //j--;
                                }
                                else if (scoreB > scoreA)
                                {
                                    iuA.Remove(iuA[i]);
                                    break;
                                    //i--;
                                }
                                else
                                {
                                    /* 如果分数相同，则根据上文记录的参数来猜测要排除哪个参数
                                     * 感觉上，只要正则表达式写得好一点，用户不故意乱说，非评价类（也是就是那些好一点什么的）的参数很少冲突
                                     * 进入这里通常是这样的情况：
                                     * 主动引导时系统说：请问您想要多少像素的？
                                     * 用户来一句：一般就行
                                     * 这时价格和后置摄像头像素冲突
                                     * */

                                    if (PBSet[I].name == userSession.liveTable.RelatedParameter)
                                    {
                                        iuB.Remove(iuB[j]);
                                        break;
                                        //j--;
                                    }
                                    else if (PBSet[J].name == userSession.liveTable.RelatedParameter)
                                    {
                                        iuA.Remove(iuA[i]);
                                        break;
                                        //i--;
                                    }

                                    else
                                    {
                                        /* 是否有其它可以解决冲突的办法，比如利用统计学习方法
                                         * 如果实在没办法解决冲突怎么办呢？也许可以来一句反问
                                         * 这里给出一个反问的例子。
                                         * 注意，这里如果有多对冲突，则只会记录最后一对冲突
                                         * 不过暂时感觉冲突信息通常就是一对
                                         */
                                        conflictConfirm = string.Format("请问您问的是{0}，还是{1}？", PBSet[I].name, PBSet[J].name);
                                        //    userSession.liveTable.RelatedValue = iuA;
                                        if (userSession.liveTable.RelatedValue == null)
                                            userSession.liveTable.RelatedValue = new List<InfoMetadata>();
                                        else userSession.liveTable.RelatedValue.Clear();

                                        for (int k = 0; k < iuA.Count; k++)
                                        {
                                            userSession.liveTable.RelatedValue.Add(iuA[k]);
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
            }




            // 如果没发现冲突，则把live-table中的冲突记录值去掉
            if (string.IsNullOrEmpty(conflictConfirm)) userSession.liveTable.RelatedValue = null;




            #region 删除掉那些由描述正则表达式匹配到的数据


            for (int i = 0; i < PBSet.Count; i++)
            {
                for (int j = 0; j < PBSet[i].list.Count; j++)
                {
                    if (PBSet[i].list[j].isStatement)
                    {
                        PBSet[i].list.RemoveAt(j);
                        j--;
                    }
                }
            }
            #endregion

            /*
             * update : wgs
             * date : 2013/9/11
             * 
             * 此处针对"这款手机的性价比高吗？"，PBSet.Count原来等于5，在上面的得分比较中，多次出现分数相同的情况
             * 导致conflictConfirm非空
             * 循环结束后PBSet.Count == 1，而conflictConfirm为清空，导致错误反问用户
             * */
            if (PBSet.Count == 1)
            {
                conflictConfirm = "";
            }


        }

        /// <summary>
        /// 算分
        /// </summary>
        static public int GetScore(Areas.Chat.Models.UserSession userSession, ParameterBase tmp, List<InfoMetadata> itA, int index)
        {
            // TODO: Calculate the score by advance method
            //加入了考虑前一句输入跟前一句输出的上下文理解
            int cntStatementOnLastOutput = 0;
            int cntStatementOnLastInput = 0;
            int cntValueOnLastOutput = 0;
            int cntValueOnLastInput = 0;
            if (userSession.liveTable.LastInput != null && userSession.liveTable.LastOutput != null)
            {
                for (int i = 0; i < tmp.StatementRegexs.Length; i++)
                {
                    MatchCollection mc1 = Regex.Matches(userSession.liveTable.LastOutput, tmp.StatementRegexs[i]);
                    cntStatementOnLastOutput += mc1.Count;
                    MatchCollection mc2 = Regex.Matches(userSession.liveTable.LastInput, tmp.StatementRegexs[i]);
                    cntStatementOnLastInput += mc2.Count;
                }
            }
            if (userSession.liveTable.LastInput != null && userSession.liveTable.LastOutput != null)
            {
                for (int i = 0; i < tmp.ValueRegexs.Length; i++)
                {
                    MatchCollection mc3 = Regex.Matches(userSession.liveTable.LastOutput, tmp.ValueRegexs[i]);
                    cntValueOnLastOutput += mc3.Count;
                    MatchCollection mc4 = Regex.Matches(userSession.liveTable.LastInput, tmp.ValueRegexs[i]);
                    cntValueOnLastInput += mc4.Count;
                }
            }
            int buf = cntStatementOnLastInput * 20 + cntStatementOnLastOutput * 20 + cntValueOnLastInput * 20 + cntValueOnLastOutput * 20;
            return itA.Count * 1000 + itA[index].rawData.Length * 200 + buf + (itA[index].isStatement ? 300 : 0);
        }

        /// <summary>
        /// 判断相交
        /// </summary>
        static public bool Overlap(int p1, int p2, int p3, int p4)
        {
            if (p1 <= p4 && p2 >= p3) return true;
            if (p3 <= p2 && p4 >= p1) return true;
            return false;
        }

        /// <summary>
        /// 得到有效信息对象
        /// </summary>
        public ValidInfo ToValidInfo()
        {
            ValidInfo vi = new ValidInfo();
            foreach (ParameterBase e in PBSet)
            {
                vi.items.Add(e.ToInfoBlock());
            }
            vi.isInfoExist = vi.items.Count > 0;
            vi.conflictFeedback = this.conflictConfirm;
            return vi;
        }

        /// <summary>
        /// 将原始数据已html格式输出
        /// </summary>
        public string OriginDataToHtml()
        {
            string s = "";

            s += "<table border=\"0\" style=\"border:1px solid #CCC;\">";

            string ss = "";
            for (int i = 0; i < originalData.items.Count; i++)
            {
                string tmp = "";
                for (int j = 0; j < originalData.items[i].items.Count; ++j)
                {
                    tmp += string.Format("【{0}】", originalData.items[i].items[j].rawData);
                }
                ss += string.Format("<p>{0}: {1}</p>", originalData.items[i].name, tmp);
            }
            s += string.Format("<tr><td style=\"vertical-align:top;border-bottom:1px solid #CCC;border-right:1px solid #CCC;\">{0}</td><td style=\"border-bottom:1px solid #CCC;\">{1}</td></tr>", "信息抓取情况：", ss);
            s += "</table>";

            return s;
        }
    }


    /// <summary>
    /// 信息元数据，派生于信息单元，多了一些原始信息，比如位置，性质等
    /// </summary>
    public class InfoMetadata
    {
        public string rawData;
        public string normalData;
        public bool isNegative;
        public int leftIndex;
        public int rightIndex;
        public bool isStatement;

    }

    /// <summary>
    /// 信息单元
    /// </summary>
    public class InfoUnit
    {
        // 其实或许不需要这个初始数据，当然多个字段也不会怀孕
        public string rawData;
        public string normalData;
        public bool isNegative;

        public InfoUnit()
            : this("")
        {

        }

        public InfoUnit(string rawData)
            : this(rawData, rawData)
        {

        }

        public InfoUnit(string rawData, string normalData, bool isNegative = false)
        {
            this.rawData = rawData;
            this.normalData = normalData;
            this.isNegative = isNegative;
        }

        public string ToSQL(string p)
        {
            string sql = "";
            if (Regex.IsMatch(p, "得分$")) return sql;
            if (normalData.IndexOf(',') >= 0)
            {
                // 这里标志着值为数字区间，通常区间只有一个
                string[] num = normalData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                sql = string.Format("{0}<={1} and {2}<={3}", num[0], Code.GlobalHash.SemanticToEN[p], Code.GlobalHash.SemanticToEN[p], num[1]);
            }
            else if (Regex.IsMatch(p, "上市日期"))
            {
                // 这里用到模糊匹配
                sql = string.Format("{0} like '%{1}%'", Code.GlobalHash.SemanticToEN[p], normalData);
            }
            else if (Regex.IsMatch(p, "品牌|型号|外观设计|键盘类型|输入方式|SIM卡类型"))
            {
                // 这里是完全匹配
                sql = string.Format("{0}='{1}'", Code.GlobalHash.SemanticToEN[p], normalData);
            }
            else if (Regex.IsMatch(p, "平均月销量"))
            {
                // 这里是完全匹配 Hard wrong code
                sql = string.Format("{0}='{1}'", 1, 1);
            }
            else
            {
                // 默认为模糊匹配
                sql = string.Format("{0} like '%{1}%'", Code.GlobalHash.SemanticToEN[p], normalData);
            }
            if (isNegative) sql = string.Format("not ({0})", sql);
            return sql;
        }
    }

    /// <summary>
    /// 信息块，包含某一参数的若干信息单元
    /// </summary>
    public class InfoBlock
    {
        public List<InfoUnit> items;
        public string name;
        public string organizing;
        /// <summary>
        /// 用于判断是否是焦点语义对，只是简单处理，后期计划通过移位复原成常见句子结构、缩句处理解决焦点语义对位置不定的问题
        /// </summary>
        public bool isFocus;

        public InfoBlock(string name, string organizing, List<InfoUnit> items)
        {
            this.name = name;
            this.organizing = organizing;
            this.items = items;
        }

        public string ToSQL()
        {
            string sql = "";
            if (Regex.IsMatch(name, "得分$")) return sql;

            foreach (InfoUnit iu in items)
            {
                sql += string.Format("@({0})@", iu.ToSQL(name));
            }
            sql = sql.Replace("@@", string.Format(" {0} ", organizing));
            if (sql.Length > 2 && sql[0] == '@' && sql[sql.Length - 1] == '@')
                sql = "(" + sql.Substring(1, sql.Length - 2) + ")";
            sql = sql.Replace("@", "");
            return sql;
        }
    }

    /// <summary>
    /// 有效信息，包含若干信息块
    /// </summary>
    public class ValidInfo
    {
        public bool isInfoExist;
        public string conflictFeedback;
        public List<InfoBlock> items = new List<InfoBlock>();
        /// <summary>
        /// 在SentenceInfo中用到的字段：句型
        /// </summary>
        public String sentenceType;
        /// <summary>
        /// 在SentenceInfo中用到的字段：值是否已定
        /// </summary>
        public bool AttributeValue;

        public string ToHTML()
        {
            string s = "";

            s += "<table border=\"0\" style=\"border:1px solid #CCC;\">";

            string ss = "";
            for (int i = 0; i < items.Count; i++)
            {
                string tmp = "";
                for (int j = 0; j < items[i].items.Count; ++j)
                {
                    tmp += string.Format("【{0}】", items[i].items[j].normalData);
                }
                ss += string.Format("<p>{0}: {1}</p>", items[i].name, tmp);
            }
            s += string.Format("<tr><td style=\"vertical-align:top;border-bottom:1px solid #CCC;border-right:1px solid #CCC;\">{0}</td><td style=\"border-bottom:1px solid #CCC;\">{1}</td></tr>", "信息抓取情况：", ss);
            s += string.Format("<tr><td style=\"vertical-align:top;border-right:1px solid #CCC;\">{0}</td><td>{1}</td></tr>", "冲突确认反问：", conflictFeedback);
            s += "</table>";

            return s;
        }

        public string ToJSON()
        {
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return s;
        }
    }
}