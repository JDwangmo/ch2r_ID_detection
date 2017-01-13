## ch2r_ID_detection  ---- regex_extracting
### Describe
- 当前版本 1.0 
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
