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


class RearCameraPixel(RegexBase):
    name = '后置摄像头像素'

    def __init__(self,sentence):

        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '后置(的)*像头(的)*像素|后置(的)*摄像头|(?<!后置)摄像头|(?<!后置摄像头)像素|后置(的)*(?<!摄像头)|(?<!后置)摄像(头)*'
        ]
        # 值正则表达式
        self.value_regexs = [
            '(至多|至少|不少于|少于|不多于|多于|不小于|小于|不大于|大于|不高于|高于|不低于|低于|不超过|超过|差不多|大概|大约)*(\d{1,8}万(像素)?)(左右|以上|以下)*',
                '最大|最小|最多|最少|最高|最低|多一点|高一点|低一点|少一点|好一点',
                '(?<!多)高|低',
                '随意|随便|都可以|其他|别的',
                '(?<!多)好|(?<!多)大|(?<!多)宽|(?<!多)高'
        ]
        # endregion
        super(RearCameraPixel, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = RearCameraPixel('价不高')
    for info_meta_data in price.info_meta_data_list:
        print('-'*80)
        print(str(info_meta_data))