## ch2r_ID_detection
### Describe
- 当前版本version1.1
- Ch2R ID detection
- C h 2 R In-Domain's detection (有效语义检测)

### Project Structure
- [regex_extracting](https://github.com/JDwangmo/ch2r_ID_detection/tree/master/regex_extracting)
    - 正则 提取 有效语义
- [deployment_module](https://github.com/JDwangmo/ch2r_ID_detection/tree/master/deployment_module)

### Dependence lib
- web.py

### To Do List
- 还有部分规则没有移植完毕，比如 C#下支持规则的 非固定长度断言，而Python下没办法，还没解决;
- 部分规则还需要进一步处理，比如 “你好”等会被匹配到，这些应该属于超出领域话语，不属于ID话语;

### User Manual
- 1 调试：
    - 1 运行 `regex_extracting/extracting/extracting.py`
        - `print(RegexExtracting.extracting(u'随便'))`
    - 或者 运行 `deployment_module/index.py`
- 2 部署
    - 运行 `run_service.sh`
    - 测试API `test_api.py`
    
### Important Date
- version 1.1:
    - 2017-01-15
    - 修正部分bug，比如 人气 等
    - 修正部分todo

- version 1.0:
    - 2017-01-13
    - init this module(初始化模块)
    - Initial release.
    - 正则检测有效语义
