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


class WarrantyPolicy(RegexBase):
    name = '保修政策'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '保修|修理'
        ]
        # 值正则表达式
        self.value_regexs = [
            '全国联保|享受三包服务|保修|保修政策',
                '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(WarrantyPolicy, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = WarrantyPolicy('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))