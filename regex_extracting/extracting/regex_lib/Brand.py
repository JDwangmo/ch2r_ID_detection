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


class Brand(RegexBase):
    name = '品牌'

    def __init__(self, sentence):
        # 要处理的输入句子

        self.sentence = sentence

        # region 1 初始化正则表达式
        # 描述正则表达式
        self.statement_regexs = [
            '品牌|牌'
        ]
        # 值正则表达式
        self.value_regexs = [
            '红米|诺基亚|nokia|Nokia|NOKIA|三星|SAMSUNG|samsung|Samsung|苹果|HTC|htc|华为|联想|lenovo|Lenovo|LENOVO|\
                  步步高|酷派|金立|魅族|黑莓|天语|摩托罗拉|OPPO|oppo|经纬|飞利浦|小米|中兴|云台|LG|lg|TCL|tcl|华硕|海信|\
                  长虹|海尔|康佳|夏新|纽曼|亿通|乐派|七喜|阿尔法|富士通|Yahoo|谷歌|卡西欧|优派|技嘉|惠普|多普达|东芝|爱国者|\
                  明基|万利达|戴尔|中天|夏普|索尼|努比亚|锤子|LG|lg|小辣椒',
            '随意|随便|都可以|其他|别的',
            '好一点',
            '好',
        ]
        # endregion
        super(Brand, self).__int__()
        self.regex_process()


if __name__ == '__main__':
    price = Brand(u'三星、诺基亚')
    for info_meta_data in price.info_meta_data_list:
        print('-' * 80)
        print(unicode(info_meta_data))
