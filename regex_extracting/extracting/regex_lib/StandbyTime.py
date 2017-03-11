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


class StandbyTime(RegexBase):
    name = '理论待机时间'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '待机'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(多于|少于|大于|小于|等于|超过|不低于)?(\d+小时|\d+天|\d+分钟)',
            '久|长|一般|适中|差不多|短',
            '随意|随便|都可以'
        ]
        # endregion
        super(StandbyTime, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = StandbyTime('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(str(info_meta_data))
