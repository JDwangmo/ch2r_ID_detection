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


class CpuFrequency(RegexBase):
    name = 'CPU频率'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            'CPU频率|CPU运行|CPU'
        ]
        # 值正则表达式
        self.value_regexs = [
            '\d{1,4}(\.\d{1,3})?(MHz|GHz)?',
            '(?<!多)高|(?<!多)快',
            '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(CpuFrequency, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = CpuFrequency(u'小米3')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(unicode(info_meta_data))
