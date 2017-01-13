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


class ListingDate(RegexBase):
    name = '上市日期'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '上市日期|上市'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(大概|大约|差不多)*(2000|20001|2002|2003|2004|2005|2006|2007|2008|2009|2010|2011|2012|2013|2014|2015|2016|2017|2017|2018|2019|2020)(年份|年)*((\d|10|11|12)月)?(以前|以后)*',
                '(\d|10|11|12)月',
                '本月|这个月|上个月|今年|去年|上一年|前年|最近|近期|最新|刚刚|新款',
                '新出|刚出',
                '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(ListingDate, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = ListingDate('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))