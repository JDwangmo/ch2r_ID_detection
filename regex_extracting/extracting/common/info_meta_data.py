# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe: 定义 提取的语义 信息块 类
"""
from __future__ import print_function

__version__ = '1.0'


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
    leftIndex = 0
    # 匹配到的字符串终点
    rightIndex = 0

    def __init__(self):
        pass

    def __str__(self):
        return u'name:{0}\nregex_name:{1}\nregex_value:{2}'.format(self.raw_data,self.regex_name.decode('utf8'),self.regex_value)