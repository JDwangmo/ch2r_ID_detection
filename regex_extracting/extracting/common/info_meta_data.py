# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe: 定义 提取的语义 信息块 类
"""
from __future__ import print_function

__version__ = '1.3'


class InfoMetadata(object):
    """
    提取的语义 信息块 类

    """
    # 原始输入
    raw_data = ''

    # 匹配到规则类名
    regex_name = ''
    # 是否描述型规则 --- 一般是属性（价格、价位）
    is_statement = True

    # 匹配到的规则
    regex_value = ''

    # 匹配到的字符串起点
    left_index = 0
    # 匹配到的字符串终点
    right_index = 0

    def __init__(self):
        pass

    def __str__(self):
        return u'raw_data:{0}\nregex_name:{1}\nregex_value:{2}\nis_statement:{3}'.format(self.raw_data,
                                                                                         self.regex_name.decode('utf8'),
                                                                                         self.regex_value,
                                                                                         self.is_statement)

    def to_dict(self):
        dd = {
            'regex_name': self.regex_name,
            'regex_value': self.regex_value,
            'is_statement': self.is_statement,
            'left_index': self.left_index,
            'right_index': self.right_index,
        }
        return dd
