# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:  正则规则 判定的 有效语义检测方法
"""
from __future__ import print_function
from regex_lib.Price import *
from regex_lib.Brand import *
from regex_lib.Model import *
from regex_lib.Color import *
from regex_lib.ListingDate import *
from regex_lib.ExteriorDesign import *
from regex_lib.VidioCall import *
from regex_lib.OS import *
from regex_lib.PhoneType import *
from regex_lib.CpuModel import *
from regex_lib.CpuCoreNumber import *
from regex_lib.CpuFrequency import *
from regex_lib.GpuModel import *
from regex_lib.KeyboardType import *
from regex_lib.InputMode import *
from regex_lib.UserInterface import *
from regex_lib.SimCardType import *
from regex_lib.Weight import *
from regex_lib.PhoneQuality import *
from regex_lib.NetworkModel import *
from regex_lib.NetworkFrequency import *
from regex_lib.DataService import *
from regex_lib.Browser import *
from regex_lib.RomCapacity import *
from regex_lib.MemoryCardType import *
from regex_lib.RamCapacity import *
from regex_lib.MaxExpandCapacity import *
from regex_lib.HotSwappable import *
from regex_lib.ScreenSize import *
from regex_lib.ScreenColor import *
from regex_lib.ScreenMaterial import *
from regex_lib.ScreenResolution import *
from regex_lib.SuperFont import *
from regex_lib.TouchScreenType import *
from regex_lib.SensorType import *
from regex_lib.ProduceFeature import *
from regex_lib.VidioFormat import *
from regex_lib.AudioFormat import *
from regex_lib.Flash import *
from regex_lib.Java import *
from regex_lib.TV import *
from regex_lib.Radio import *
from regex_lib.Game import *
from regex_lib.Software import *
from regex_lib.FrontCameraPixel import *
from regex_lib.RearCameraPixel import *
from regex_lib.FlashType import *
from regex_lib.SensorType import *
from regex_lib.FocusMode import *
from regex_lib.ZoomMode import *
from regex_lib.ZoomMagnification import *
from regex_lib.MaxCameraSize import *
from regex_lib.ShootingMode import *
from regex_lib.CameraFeature import *
from regex_lib.GpsNavigation import *
from regex_lib.WLan import *
from regex_lib.WifiHotspot import *
from regex_lib.Bluetooth import *
from regex_lib.ModernFunction import *
from regex_lib.DataFunction import *
from regex_lib.DataInterface import *
from regex_lib.BatteryType import *
from regex_lib.BatteryCapacity import *
from regex_lib.TalkingTime import *
from regex_lib.StandbyTime import *
from regex_lib.BasicFunction import *
from regex_lib.BusinessFunction import *
from regex_lib.HeadphoneJackType import *
from regex_lib.WarrantyPolicy import *
from regex_lib.WarrantyTime import *
from regex_lib.PhoneAttachment import *
from regex_lib.Gift import *
from regex_lib.GoodsSource import *
from regex_lib.UserType import *
from regex_lib.StopProduction import *
from regex_lib.Discount import *
from regex_lib.SalesVolume import *
from regex_lib.Popularity import *
from regex_lib.ScoreCostPerformance import *
from regex_lib.ScoreAmountPhone import *
from regex_lib.ScorePerformance import *
from regex_lib.ScoreShape import *
from regex_lib.peripherals import *

__version__ = '1.3'


class RegexExtracting(object):
    """
    有效语义的检测 --- 正则方法

    """

    def __int__(self):
        pass

    @staticmethod
    def extracting(sentence):
        """
        提取有效语义

        :param sentence: 待提取的句子
        :return: (is_id, original_info_blocks, info_blocks) =  (是否有语义，原始提取的语义块 , 最终提取的语义块)
        """

        # 提取到的有效语义 列表
        # 每个元素是 RegexBase 对象
        extracting_regexs = []

        # region 1 逐个参数进行处理

        extracting_regexs.append(Price(sentence))

        # 主体属性(sentence))
        extracting_regexs.append(Brand(sentence))
        extracting_regexs.append(Model(sentence))
        extracting_regexs.append(Color(sentence))
        extracting_regexs.append(ListingDate(sentence))
        extracting_regexs.append(ExteriorDesign(sentence))
        extracting_regexs.append(VidioCall(sentence))
        extracting_regexs.append(OS(sentence))
        extracting_regexs.append(PhoneType(sentence))
        extracting_regexs.append(CpuModel(sentence))
        extracting_regexs.append(CpuCoreNumber(sentence))
        extracting_regexs.append(CpuFrequency(sentence))
        extracting_regexs.append(GpuModel(sentence))
        extracting_regexs.append(KeyboardType(sentence))
        extracting_regexs.append(InputMode(sentence))
        extracting_regexs.append(UserInterface(sentence))
        extracting_regexs.append(SimCardType(sentence))
        extracting_regexs.append(Weight(sentence))
        extracting_regexs.append(PhoneQuality(sentence))

        # 网络属性(sentence))
        extracting_regexs.append(NetworkModel(sentence))
        extracting_regexs.append(NetworkFrequency(sentence))
        extracting_regexs.append(DataService(sentence))
        extracting_regexs.append(Browser(sentence))

        # 存储属性(sentence))
        extracting_regexs.append(RomCapacity(sentence))
        extracting_regexs.append(MemoryCardType(sentence))
        extracting_regexs.append(RamCapacity(sentence))
        extracting_regexs.append(MaxExpandCapacity(sentence))
        extracting_regexs.append(HotSwappable(sentence))

        # 显示属性(sentence))
        extracting_regexs.append(ScreenSize(sentence))
        extracting_regexs.append(ScreenColor(sentence))
        extracting_regexs.append(ScreenMaterial(sentence))
        extracting_regexs.append(ScreenResolution(sentence))
        extracting_regexs.append(SuperFont(sentence))
        extracting_regexs.append(TouchScreenType(sentence))
        extracting_regexs.append(SensorType(sentence))
        extracting_regexs.append(ProduceFeature(sentence))

        # 娱乐功能(sentence))
        extracting_regexs.append(VidioFormat(sentence))
        extracting_regexs.append(AudioFormat(sentence))
        extracting_regexs.append(Flash(sentence))
        extracting_regexs.append(Java(sentence))
        extracting_regexs.append(TV(sentence))
        extracting_regexs.append(Radio(sentence))
        extracting_regexs.append(Game(sentence))
        extracting_regexs.append(Software(sentence))
        # 摄像属性(sentence))
        extracting_regexs.append(FrontCameraPixel(sentence))
        extracting_regexs.append(RearCameraPixel(sentence))
        extracting_regexs.append(FlashType(sentence))
        extracting_regexs.append(SensorType(sentence))
        extracting_regexs.append(FocusMode(sentence))
        extracting_regexs.append(ZoomMode(sentence))
        extracting_regexs.append(ZoomMagnification(sentence))
        extracting_regexs.append(MaxCameraSize(sentence))
        extracting_regexs.append(ShootingMode(sentence))
        extracting_regexs.append(CameraFeature(sentence))

        # 传输功能(sentence))
        extracting_regexs.append(GpsNavigation(sentence))
        extracting_regexs.append(WLan(sentence))
        extracting_regexs.append(WifiHotspot(sentence))
        extracting_regexs.append(Bluetooth(sentence))
        extracting_regexs.append(ModernFunction(sentence))
        extracting_regexs.append(DataFunction(sentence))
        extracting_regexs.append(DataInterface(sentence))

        # 电池属性(sentence))
        extracting_regexs.append(BatteryType(sentence))
        extracting_regexs.append(BatteryCapacity(sentence))
        extracting_regexs.append(TalkingTime(sentence))
        extracting_regexs.append(StandbyTime(sentence))

        # 普通功能属性(sentence))
        extracting_regexs.append(BasicFunction(sentence))
        extracting_regexs.append(BusinessFunction(sentence))
        # extracting_regexs.append(ExpandProgramPlatform(sentence))

        # 插孔属性(sentence))
        extracting_regexs.append(HeadphoneJackType(sentence))
        # extracting_regexs.append(DataWireType(sentence))

        # 保修属性(sentence))
        extracting_regexs.append(WarrantyPolicy(sentence))
        extracting_regexs.append(WarrantyTime(sentence))
        # extracting_regexs.append(WarrantyDetail(sentence))

        # 其它属性(sentence))
        extracting_regexs.append(Peripherals(sentence))
        extracting_regexs.append(PhoneAttachment(sentence))
        extracting_regexs.append(Gift(sentence))
        extracting_regexs.append(GoodsSource(sentence))
        extracting_regexs.append(UserType(sentence))
        extracting_regexs.append(StopProduction(sentence))
        extracting_regexs.append(Discount(sentence))
        extracting_regexs.append(SalesVolume(sentence))
        extracting_regexs.append(Popularity(sentence))

        # 得分属性(sentence))
        extracting_regexs.append(ScoreCostPerformance(sentence))
        extracting_regexs.append(ScoreAmountPhone(sentence))
        extracting_regexs.append(ScorePerformance(sentence))
        extracting_regexs.append(ScoreShape(sentence))

        # 评价性语义(sentence))
        # extracting_regexs.append(BatteryScore(sentence))
        # extracting_regexs.append(ScreenScore(sentence))
        # extracting_regexs.append(ShootScore(sentence))
        # extracting_regexs.append(MediaScore(sentence))
        # extracting_regexs.append(OutlookScore(sentence))
        # extracting_regexs.append(CostScore(sentence))
        # extracting_regexs.append(Advantage(sentence))
        # extracting_regexs.append(Disadvantage
        # endregion
        # region 2 去除为空的属性
        extracting_regexs = [regex for regex in extracting_regexs if len(regex.info_meta_data_list) > 0]
        # 保存原始提取结果，logging用
        original_extracting_result = RegexExtracting.to_dict(extracting_regexs)
        # endregion
        # region 3 冲突处理
        RegexExtracting.conflict_process(extracting_regexs)
        # 再去除一次去除为空的属性
        extracting_regexs = [regex for regex in extracting_regexs if len(regex.info_meta_data_list) > 0]
        # endregion

        if len(extracting_regexs) > 0:
            # print('有语义')
            # for regex in extracting_regexs:
            #     print('-' * 50)
            #     print(unicode(regex))
            return True, original_extracting_result, RegexExtracting.to_dict(extracting_regexs)
        else:
            # print('无语义')
            return False, original_extracting_result, RegexExtracting.to_dict(extracting_regexs)

    @staticmethod
    def is_overlay(item_i, item_j):
        """
        判断两个语义块是否相交

        :param item_i:
        :param item_j:
        :return:
        """
        if item_j.right_index <= item_i.left_index:
            # item_i 在右边，item_j 在左边，比如  item_i 区间是[2,4] ,item_j 区间是[6,9]
            return False

        if item_i.right_index <= item_j.left_index:
            # item_i 在左边，item_j 在右边，比如  item_i 区间是[6,9] ,item_j 区间是[2,4]
            return False
        # 其他情况都相交
        return True

    @staticmethod
    def get_score(item_J, item_j):
        """
        计算语义块的分数，用于冲突处理

        :param item_J: 待计算分数的语义块 的所属 属性下 的所有语义块集合
        :param item_j: 待计算分数的语义块
        :return:
        """
        score = 0
        # 该语义块的所属属性下的 语义块个数越多，分数越大
        score += len(item_J.info_meta_data_list) * 1000
        # 匹配到的语义块的内容长度，比如  “红米”的分数要比“红”高
        score += len(item_j.regex_value) * 200
        # 假如是 描述型的语义块（属性名），分数更高
        score += 300 if item_j.is_statement else 0
        return score

    @staticmethod
    def conflict_process(extracting_regexs):
        """
        冲突处理 --- 判断是否有语义块相交

        :param extracting_regexs: 待进一步处理的语义块集合
        :return:
        """
        for a in range(len(extracting_regexs)):
            items_a = extracting_regexs[a]
            for items_b in extracting_regexs[a + 1:]:

                for item_i in items_a.info_meta_data_list:
                    for item_j in items_b.info_meta_data_list:

                        if RegexExtracting.is_overlay(item_i, item_j):
                            # 假如两个语义块相交，怎进行冲突处理，去除掉一个
                            # print(unicode(item_i))
                            # print('-----------')
                            # print(unicode(item_j))
                            # print('------------------')
                            score_i = RegexExtracting.get_score(items_a, item_i)
                            score_j = RegexExtracting.get_score(items_b, item_j)
                            # region 修正分数 --- 长串匹配,比如 "小米"  和 "小" ,则去掉 "小"
                            if item_i.left_index <= item_j.left_index and item_j.right_index <= item_i.right_index:
                                # 即 item_i 包含 item_j, 直接长串匹配,把 短的 item_j去掉
                                # 比如 "小米"  和 "小" ,则去掉 "小"
                                if item_i.regex_value != item_j.regex_value:
                                    # 避免是 两个一样的,比如 "小" 和 "小",这种情况是不知道去掉哪个的
                                    score_i = 1
                                    score_j = 0
                            elif item_j.left_index <= item_i.left_index and item_i.right_index <= item_j.right_index:
                                # 即 item_j 包含 item_i, 直接长串匹配,把 短的 item_i 去掉
                                if item_i.regex_value != item_j.regex_value:
                                    # 避免是 两个一样的,比如 "小" 和 "小",这种情况是不知道去掉哪个的
                                    score_i = 0
                                    score_j = 1
                                    # endregion
                            # print(score_i, score_j)
                            # 去除掉分数低的语义块
                            if score_i > score_j:
                                items_b.info_meta_data_list.remove(item_j)
                            elif score_i < score_j:
                                items_a.info_meta_data_list.remove(item_i)
                            else:
                                # 分数相同暂不处理
                                pass

    @staticmethod
    def to_dict(extracting_regexs):
        dd = {regex.name: regex.to_dict() for regex in extracting_regexs}
        return dd


if __name__ == '__main__':
    # print(RegexExtracting.extracting(u'价格'))
    # print(RegexExtracting.extracting(u'随便'))
    # print(RegexExtracting.extracting(u'移动4G'))
    # print(RegexExtracting.extracting(u'小米3'))
    # print(RegexExtracting.extracting(u'4.7'))
    # print(RegexExtracting.extracting(u'红米 、 诺基亚'))
    print(RegexExtracting.extracting(u'那就小米红米Note把'))
    # print(RegexExtracting.extracting(u'普通'))
    # print(RegexExtracting.extracting(u'你'))
