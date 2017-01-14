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


class ScorePerformance(RegexBase):
    name = '性能得分'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '性能'
        ]
        # 值正则表达式
        self.value_regexs = [
            # todo
            # '(?<=性能.*)(好)',
            '性能好',
            '性能高',
            '(?<!多)高(?=.*性能)',
            '(?<=性能)如何|(?<=性能)怎么样|(?<=性能)怎样|(?<=性能)好不|(?<=性能)行不',
            '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(ScorePerformance, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = ScorePerformance('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(str(info_meta_data))
