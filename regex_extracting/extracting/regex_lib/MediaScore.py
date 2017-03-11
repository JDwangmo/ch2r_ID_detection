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


class MediaScore(RegexBase):
    name = '影音娱乐'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '影音娱乐|娱乐功能|娱乐'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(?<=(影音娱乐|娱乐功能|娱乐).*)(好)',
                '好(?=.*(影音娱乐|娱乐功能|娱乐))',
                '(?<=(影音娱乐|娱乐功能|娱乐))如何|(?<=(影音娱乐|娱乐功能|娱乐))怎么样|(?<=影音娱乐|娱乐功能|娱乐))怎样|(?<=影音娱乐|娱乐功能|娱乐))好不|(?<=影音娱乐|娱乐功能|娱乐))行不',
                '随意|随便|都可以',
        ]
        # endregion
        super(MediaScore, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = MediaScore('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))