## ch2r_ID_detection
### Describe
- 当前版本version1.2
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
- 3 在线API服务
    - 可以 通过API 直接访问： http://119.29.81.170:10545/id_detection/regex/rawInput=品牌

### Important Date
- version 1.2:
    - 2017-01-27
    - 修正`regex_extracting`部分bug，比如 
        - 1）“小米3”  ---> 主屏尺寸: 【小】【3.8,4.2】   ---->  修正长串匹配，这里的 “小米”作为一个整体，
            - 修改后识别结果为 不会将这里的“小”作为一个有效语义。
    - 增加冲突处理
    
- version 1.1:
    - 2017-01-15
    - 修正`regex_extracting`部分bug，比如 人气 等
        - [网络制式]正则里 添加 `4G`的匹配，支持 `4G网络` 等的匹配
        - 修正部分todo

- version 1.0:
    - 2017-01-13
    - init this module(初始化模块)
    - Initial release.
    - 正则检测有效语义
