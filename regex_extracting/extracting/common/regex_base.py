# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe: 所有属性规则的父类：比如 Price 等都是该类的子类
"""
from __future__ import print_function

__version__ = '1.1'
import re
from info_meta_data import InfoMetadata


class RegexBase(object):
    """
    所有属性规则的父类：比如 Price 等都是该类的子类

    """
    # 属性名
    name = ''
    # 要处理的句子
    sentence = ''
    # 描述型规则 --- 一般是属性（价格、价位）
    statement_regexs = None
    value_regexs = None

    def __int__(self):

        # 提取到的有效语义 列表
        # 每个元素是 InfoMetadata 对象
        self.info_meta_data_list = []

    def __str__(self):

        return u'\n------------------\n'.join([unicode(item) for item in self.info_meta_data_list])
        # return ''
        # return u'\n------------------\n'.join([unicode(item) for item in self.info_meta_data_list])

    def to_dict(self):
        """
        转换成字典

        :return:
        """
        return {idx:item.to_dict() for idx,item in enumerate(self.info_meta_data_list)}

    def regex_process(self):
        # 出现过的匹配值
        present_regex_values = set()
        # region 处理描述性正则表达式数组
        pattern = '|'.join(['(%s)' % regex for regex in self.statement_regexs])
        pattern = pattern.decode('utf8')
        # print(pattern)
        match_result = re.search(pattern, self.sentence)
        if match_result:
            info_meta_data = InfoMetadata()
            info_meta_data.raw_data = self.sentence
            info_meta_data.regex_name = self.name
            info_meta_data.regex_value = match_result.group(0)

            info_meta_data.is_statement = True

            info_meta_data.leftIndex = match_result.start(0)
            info_meta_data.rightIndex = match_result.end(0)
            # 添加
            self.info_meta_data_list.append(info_meta_data)
            present_regex_values.add(info_meta_data.regex_value)
        # endregion

        # region 处理值性正则表达式数组
        pattern = '|'.join(['(%s)' % regex for regex in self.value_regexs])
        pattern = pattern.decode('utf8')
        # print(pattern)
        match_result = re.search(pattern, self.sentence)
        if match_result:
            if not match_result.group(0) in present_regex_values:
                # 没出现过，才添加
                info_meta_data = InfoMetadata()
                info_meta_data.raw_data = self.sentence
                info_meta_data.regex_name = self.name
                info_meta_data.regex_value = match_result.group(0)

                info_meta_data.is_statement = False

                info_meta_data.leftIndex = match_result.start(0)
                info_meta_data.rightIndex = match_result.end(0)
                # 添加
                self.info_meta_data_list.append(info_meta_data)

                # endregion
