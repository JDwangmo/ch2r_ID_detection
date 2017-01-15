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

__version__ = '1.1'


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
        :return: (is_id, info_blocks) =  (是否有语义， 提取的语义块)
        """

        # 提取到的有效语义 列表
        # 每个元素是 RegexBase 对象
        extracting_regexs = []
        # region 逐个参数进行处理

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

        extracting_regexs = [regex for regex in extracting_regexs if len(regex.info_meta_data_list) > 0]

        if len(extracting_regexs) > 0:
            # print('有语义')
            for regex in extracting_regexs:
                print('-' * 50)
                print(unicode(regex))
            return True, RegexExtracting.to_dict(extracting_regexs)
        else:
            # print('无语义')
            return False, RegexExtracting.to_dict(extracting_regexs)

    @staticmethod
    def to_dict(extracting_regexs):
        dd = {regex.name: regex.to_dict() for regex in extracting_regexs}
        return dd


if __name__ == '__main__':
    # print(RegexExtracting.extracting(u'价格'))
    # print(RegexExtracting.extracting(u'随便'))
    print(RegexExtracting.extracting(u'移动4G'))
    # print(RegexExtracting.extracting(u'普通'))
    # RegexExtracting.extracting(u'4000元')
