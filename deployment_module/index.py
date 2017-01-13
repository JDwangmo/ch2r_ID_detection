# encoding=utf8
"""
    Author:  'jdwang'
    Date:    'create date: 2017-01-13'; 'last updated date: 2017-01-13'
    Email:   '383287471@qq.com'
    Describe:
"""

import web
import json
import sys

__version__ = '1.0'

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

        is_id = RegexExtracting.extracting(rawInput)
        result = {'rawInput': rawInput,
                  'status': status,
                  'is_id': is_id
                  }
        web.header('Content-Type', 'application/json')
        result = json.dumps(result)
        return result


if __name__ == "__main__":
    app.run()