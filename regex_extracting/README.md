## ch2r_ID_detection  ---- regex_extracting
### Describe
- 当前版本 1.2
- C h 2 R In-Domain's detection 
- 通过正则提取 有效语义
- 主要是将原有的在 Asp.Net 版本 转换 成Python版本

### Project Structure
- [asp_version](https://github.com/JDwangmo/ch2r_ID_detection/tree/master/regex_extracting/asp_version)
    - 原有 C h 2 R Asp.Net 版本，作为参考

- [extracting](https://github.com/JDwangmo/ch2r_ID_detection/tree/master/regex_extracting/extracting)
    - Python版本
    - 正则 提取
    
### Dependence lib

### To Do List
- 还有部分规则没有移植完毕，比如 C#下支持规则的 非固定长度断言，而Python下没办法，还没解决;
- 部分规则还需要进一步处理，比如 “你好”等会被匹配到，这些应该属于超出领域话语，不属于ID话语;

### User Manual
- 1 运行 `regex_extracting/extracting/extracting.py`
- 2 > `print(RegexExtracting.extracting(u'随便'))`


### Change Log
- version 1.2:
    - 2017-01-27
    - 修正部分bug，比如 
        - 1）“小米3”  ---> 主屏尺寸: 【小】【3.8,4.2】   ---->  修正长串匹配，这里的 “小米”作为一个整体，
            - 修改后识别结果为 不会将这里的“小”作为一个有效语义。
            - [RAM容量]的“小”同步更正。
        - 2）"4.7" ---> 型号中“.”不做分隔符，本来识别为【4】【7】,现更正为【4.7】
        - 3）可以遍历所有出现的语义块，而不只是第一个出现的语义块，比如 "红米、诺基亚"，本来识别为【红米】,现更正为【红米】【诺基亚】
    - 增加冲突处理，比如 “红米的手机” 会被识别成：{品牌：【红米】，颜色：【红】}，
        - 接下来通过冲突处理，会去掉颜色语义块，剩下{品牌：【红米】}
    
- version 1.1:
    - 2017-01-15
    - 修正部分bug，比如 人气 等
    - [网络制式]正则里 添加 `4G`的匹配，支持 `4G网络` 等的匹配
    - 修正部分todo

- version 1.0:
    - 2017-01-13
    - init this module(初始化模块)
    - Initial release.
    - 正则检测有效语义
