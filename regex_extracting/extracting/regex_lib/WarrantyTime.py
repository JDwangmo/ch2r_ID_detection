# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:
"""
from __future__ import print_function
from regex_extracting.extracting.common.regex_base import RegexBase

__version__ = '1.3'


class WarrantyTime(RegexBase):
    name = '质保时间'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '质保时间|保修时间'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(至少|最少|不少于|多余|不小于|不低于|超过|差不多|大概|大约|左右)*(\d年|\d{1,2}月)(以上|差不多|大概|大约|左右)*',
                '(\d年|\d{1,2}月).(\d年|\d{1,2}月)',
        ]
        # endregion
        super(WarrantyTime, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = WarrantyTime('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))