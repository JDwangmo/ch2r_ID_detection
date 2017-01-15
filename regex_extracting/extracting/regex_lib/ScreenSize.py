# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:
"""
from __future__ import print_function
from regex_extracting.extracting.common.regex_base import RegexBase

__version__ = '1.1'


class ScreenSize(RegexBase):
    name = '主屏尺寸'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '主屏尺寸|屏幕|主屏',
                '寸|吋|英寸'
        ]
        # 值正则表达式
        self.value_regexs = [
            '((?<![0-9a-zA-Z.])\d(.\d{1,2})?).*((?<![0-9a-zA-Z.])\d(.\d{1,2})?)',
                # '(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])\d(.\d{1,2})?(寸|吋|英寸)(左右|以上|以下)*',
                # '(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])[1-8]{1}\.\d(?!(寸|吋|英寸))(左右|以上|以下)*',
                '(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(?<![0-9a-zA-Z.])\d+\.*\d*(寸|吋|英寸)*(左右|以上|以下)*',
                '(?<!多)(宽)|(?<!多)(大)(?!小)|(?<!大|多)(小)',
                '差不多|适中|一般',
                '最大|最小|最多|最少|最高|最低|多一点|高一点|低一点|少一点|再少|再低|再高|再大|再小|更大|更小|好一点',
                '随意|随便|都可以|其他|别的',
                '好'
        ]
        # endregion
        super(ScreenSize, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = ScreenSize('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))