# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:
"""
from __future__ import print_function
from regex_extracting.extracting.common.regex_base import RegexBase

__version__ = '1.2'


class RamCapacity(RegexBase):
    name = 'RAM容量'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            'RAM|手机内存|机身内存|内存'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(大于|小于|等于|超过|不低于)*\d{1,6}(MB|GB|G|M|m|g|gb|mb)',
            '\d{1,6}(MB|GB|G|M|m|g|gb|mb)(以下|以上)',
            # ##### 规则 - 3 #####
            # 大 / 小
            # by WJD
            # 如果 小米 则“小”不是有效的语义，不会识别出来
            '(?<!多)(大)(?!小)|(?<!大|多)(小)(?!米)',
            # ###################
            '随意|随便|都可以'
        ]
        # endregion
        super(RamCapacity, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = RamCapacity('小米3')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(str(info_meta_data))
