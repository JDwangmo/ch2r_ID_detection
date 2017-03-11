# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-30'
    Email:   '383287471@qq.com'
    Describe: ID Detection 服务器运行程序
"""

import web
import json
import sys

__version__ = '1.3'

sys.path.append('.')
sys.path.append('..')

print sys.path

from regex_extracting.extracting.extracting import RegexExtracting

urls = (
    '/id_detection/regex/rawInput=(.*)', 'IDDetectionMain'
)

app = web.application(urls, globals(), False)


class IDDetectionMain:
    def GET(self, rawInput):
        rawInput = rawInput.strip()
        # print rawInput
        # 空输入时..
        if len(rawInput) == 0:
            status = 'False|input is null'
        else:
            status = 'True|regex detection'

        is_id, original_info_blocks, info_blocks = RegexExtracting.extracting(rawInput)
        result = {'rawInput': rawInput,
                  'status': status,
                  'is_id': is_id,
                  'original_info_blocks': original_info_blocks,
                  'info_blocks': info_blocks
                  }
        web.header('Content-Type', 'application/json')
        result = json.dumps(result)
        return result


if __name__ == "__main__":
    app.run()
