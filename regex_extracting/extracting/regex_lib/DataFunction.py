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


class DataFunction(RegexBase):
    name = '数据功能'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '数据功能'
        ]
        # 值正则表达式
        self.value_regexs = [
            'DLNA技术|无线AP|PC数据同步',
                 '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(DataFunction, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = DataFunction('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))