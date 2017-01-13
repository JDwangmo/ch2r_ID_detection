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


class CpuCoreNumber(RegexBase):
    name = 'CPU核数'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            'CPU核数|核数|核'
        ]
        # 值正则表达式
        self.value_regexs = [
            '单核|双核|四核|八核|(?<!\d)\d{1,2}(?!\d)(核)?'
                '单核|双核|四核|八核|[1-8]核',
                '随意|随便|都可以|其他|别的'
        ]
        # endregion
        super(CpuCoreNumber, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = CpuCoreNumber('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))