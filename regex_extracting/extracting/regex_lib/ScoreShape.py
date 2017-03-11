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


class ScoreShape(RegexBase):
    name = '外观得分'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '外观|外形|样子|外貌'
        ]
        # 值正则表达式
        self.value_regexs = [
            # todo
            # '(?<=外观.*)(好看|好)',
            '(?<=外观)如何|(?<=外观)怎么样|(?<=外观)怎样|(?<=外观)好不|(?<=外观)行不',
            '(?<=样子)如何|(?<=样子)怎么样|(?<=样子)怎样|(?<=样子)好不|(?<=样子)行不',
            '(?<=外形)如何|(?<=外形)怎么样|(?<=外形)怎样|(?<=外形)好不|(?<=外形)行不',
            '好看吗|好不好看',
            '美观|可爱',
            '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(ScoreShape, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = ScoreShape('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(str(info_meta_data))
