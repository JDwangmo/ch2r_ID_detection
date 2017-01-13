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


class BasicFunction(RegexBase):
    name = '基本功能'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '基本功能|功能'
        ]
        # 值正则表达式
        self.value_regexs = [
            '短信|SMS|彩信|MMS|录音功能|飞行模式|情景模式|主题模式|闹钟功能|计算器|电子词典|备忘录|日程表|日历功能',
                '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(BasicFunction, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = BasicFunction('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))