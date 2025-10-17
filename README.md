# HelloContainer - 容器管理系统
一个借助于AI(如Cursor)帮助，融合领域设计、Code clean、设计模式、单元测试，用于学习和实践高质量编码的容器管理项目。模拟容器系统，实现水容量管理核心功能，支持液体添加、移除与转移操作，可监控容器状态并处理溢出等异常情况。

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
2. 容器水可增可减，不能溢出
3. 容器可连接到多个容器，连接后水量平分
4. 水量超过80%时，出发预警

#### 实现建议
1. 尝试测试驱动开发，先完成test case，再利用AI完成业务代码和code review

## 代码质量
#### 代码规范
* 注意函数命名
* 函数尽量短小
* 对象隐藏数据，暴露行为。数据结构暴露数据，无明显的行为。
#### 代码设计
* 符合 DDD 聚合根设计原则
#### 代码性能
* 算法优化

## 领域设计

### 模型
#### Entity
* 标识
* 连续性

#### Value Object
* 无标识，不可变

#### Service
* 不属于对象，是活动/动作，而不是事物
* 无状态
* 控制接口粒度，避免Entity和Value Object耦合
* 使用场景，如：处理资金转账，涉及两个账户和一些全局规则

#### Module

### 生命周期
使用Factory创建和重建复杂对象的Aggregate，从而封装内部结构。最后，在生命周期中间和末尾使用Repository查找和检索持久化对象并封装基础设施。

#### Aggregate
* 一组相关对象集合，数据修改单元
* root，特定的entity，全局标识，外部引用
* entity，本地标识
* boundary
* 数据库查询root，其他对象遍历关联

#### Factory
什么情况下使用Factory
* 创建Object的逻辑复杂，简化创建代码
* 创建Object有异步逻辑
* 创建Object的时候想返回错误或者Object，使用Result Pattern

#### Repositories

### Event

#### DomainEvent
* Eventual Consistency
* 作用范围是领域内，解耦Domain和IntegrationEvent，把IntegrationEvent移到Domain外
* Update other Aggregates with domain event

#### IntegrationEvent
* 作用范围是领域外，与系统中其他或外部组件交互

## 项目结构 
HelloContainer/  
├── HelloContainer.Api/              
├── HelloContainer.Application/      
├── HelloContainer.Domain/    
├── HelloContainer.Infrastructure/    
└── HelloContainer.UnitTests
![image](https://github.com/user-attachments/assets/62c0fcdf-1367-4d04-9c47-0a560c07bcd4)

## 参考资料
* 《你真的会写代码吗》
* 《代码整洁之道》
* 《.NET单元测试的艺术》
* 《领域驱动设计-软件核心复杂性能应对之道》
*  eShop(https://github.com/dotnet/eShop)

## DDD Mind Map
<img width="2994" height="2012" alt="image" src="https://github.com/user-attachments/assets/38a62db9-8524-4f37-b555-e95bb3e7fa26" />

## Release 1 - 初始发布 (2025-08-23)
- **核心领域功能**: 容器容量管理、跨容器连接与均衡、溢出与阈值预警处理。
- **后端 API (.NET 8)**: 分层架构（Domain/Application/Infrastructure）、领域/集成事件、仓储与工作单元、全局异常处理中间件、AutoMapper 映射、FluentValidation 校验。
- **消息与异步**: 集成 RabbitMQ（生产/消费），提供 Outbox 背景服务加工集成事件；新增 Azure Functions 作为异步处理与账本（Ledger）消费者。
- **前端**: React + Vite 初版 UI，支持容器可视化与基本操作入口。
- **测试**: 单元测试与集成测试覆盖容量计算、连通均衡、异常分支等关键用例。

## Release 2
- **基础设施 (Azure)**: 基于 Bicep 的模块化基础设施（`container-apps-environment`、`container-app-api`、`container-app-function`、`container-app-web`、`container-registry`、`cosmos-db`、`service-bus`、`storage`）。
- **安全与策略**: 引入 OPA。
- **本地开发**: Docker Compose 一键启动（RabbitMQ、CosmosDB Emulator、API、Function、Web）。
- **Central Package Management**:
  * https://devblogs.microsoft.com/dotnet/introducing-central-package-management/
  * https://github.com/Webreaper/CentralisedPackageConverter
