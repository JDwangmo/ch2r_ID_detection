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


class Price(RegexBase):
    name = '价格'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '价格|价钱|价位|钱|元|块'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z]).*(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z])',
                '(至多|至少|最多|最少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z])[1-9]\d{1,5}(?![0-9a-zA-Z])(元|块)*(左右|以上|以下|内|以内)*',
                '(最贵|最高|最多|最便宜|最低|最少)|(贵点|便宜点|贵一点|便宜一点|高一点|低一点|更贵|更便宜|更高|更低|再少点|再高点)',
                # todo
                # '((?<=价.*?)(不高|低|少|高|差不多|一般|适中))|((普通|一般|低|(?<!多)高|差不多|((?<!多)少))(?=.*?(价|钱)))|(廉价|便宜|贵|一般|普通)',
                '随意|随便|都可以|别的|其他的',
                '好'
        ]
        # endregion
        super(Price, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = Price('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))