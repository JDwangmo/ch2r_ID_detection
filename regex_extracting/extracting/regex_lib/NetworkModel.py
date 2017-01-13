# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:
"""
from __future__ import print_function
from regex_extracting.extracting.common.regex_base import RegexBase

__version__ = '1.0'


class NetworkModel(RegexBase):
    name = '网络制式'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '网络制式'
        ]
        # 值正则表达式
        self.value_regexs = [
            '单卡双模|单卡多模|双卡单模|双卡双模|双卡双待|双模双待|移动3G|移动2G|联通3G|联通2G|电信3G|电信2G|移动|联通|电信|3G|2G',
                '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(NetworkModel, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = NetworkModel('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))