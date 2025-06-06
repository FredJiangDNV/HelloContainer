# HelloContainer - 容器管理系统
一个借助于AI(如Windsurf)帮助，融合领域设计、Code clean、设计模式、单元测试，用于学习和实践高质量编码的容器管理项目。模拟容器系统，实现水容量管理核心功能，支持液体添加、移除与转移操作，可监控容器状态并处理溢出等异常情况。

## 核心功能
* 容器容量管理（添加 / 移除液体）
* 液体转移与状态监控
* 完整单元测试覆盖

#### 例子
  有a、b、c、d四个容器，当执行以下操作时，容器里的水变成如下图
  * a.addWater(12);
  * d.addWater(8);
  * a.connectTo(b);
  * b.connectTo(c);
  ![image](https://github.com/user-attachments/assets/3edeebb9-a481-4291-85f5-ba7d5cfab78f)

#### 业务细节
1. 容器大小固定，创建后不可修改
2. 容器水可曾可减，不能溢出
3. 容器可连接到多个容器，连接后水量平分
4. 水量超过80%，出发预警

## 代码质量
#### 代码规范
* 注意函数命名
* 函数尽量短小
#### 代码设计
* 符合 DDD 聚合根设计原则
#### 代码性能
* 算法优化

## 项目结构 
HelloContainer/  
├── HelloContainer.Api/              
├── HelloContainer.Application/      
├── HelloContainer.Domain/    
├── HelloContainer.Infrastructure/    
└── HelloContainer.UnitTests

## 参考资料
* 《你真的会写代码吗》
* 《代码整洁之道》
* 《.NET单元测试的艺术》
* 《领域驱动设计-软件核心复杂性能应对之道》
*  eShop(https://github.com/dotnet/eShop)
