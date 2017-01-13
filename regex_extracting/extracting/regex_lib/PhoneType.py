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


class PhoneType(RegexBase):
    name = '手机类型'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '手机类型'
        ]
        # 值正则表达式
        self.value_regexs = [
            '[234][gG](手机)?',
                '(非?)智能(手?机)?',
                '拍照手机|平板手机|商务手机|三防手机|音乐手机|时尚手机|电视手机|老人手机|儿童手机|女性手机',
               '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(PhoneType, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = PhoneType('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))