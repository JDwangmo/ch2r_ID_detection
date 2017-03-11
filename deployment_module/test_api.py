# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe: 测试发布在服务器上的API
"""
from __future__ import print_function
import requests

__version__ = '1.3'

TAG_URL = 'http://119.29.81.170:10545/id_detection/regex/rawInput=%s'
# 输入要测试的句子
sentences = list(open('test_sentences.txt'))
for sentence in sentences:
    sentence = sentence.strip()
    r = requests.get(TAG_URL % sentence)
    if r.status_code == 200:
        if r.json()['is_id']:
            print('[%s]含有效语义' % sentence)
        else:
            print('[%s]无有效语义' % sentence)
    else:
        print('网站访问不到!')
