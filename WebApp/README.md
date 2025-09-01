# HelloContainer Web Application

这是一个ASP.NET Core Web应用程序，使用Azure AD进行身份验证，并调用HelloContainer API。

## 功能特性

- ✅ Azure AD集成身份验证
- ✅ 自动获取并传递Access Token到API
- ✅ 容器管理界面（创建、查看、删除）
- ✅ 加水功能
- ✅ 搜索和过滤
- ✅ 响应式设计

## 配置步骤

### 1. Azure AD应用注册

1. 在Azure Portal中创建新的应用注册
2. 设置重定向URI：`https://localhost:5001/signin-oidc`
3. 创建客户端密钥
4. 添加API权限：`api://525ab8e4-1ea2-4354-b84d-d917b08e0ad9/AllAccess`

### 2. 更新配置

在 `appsettings.json` 中更新以下配置：

```json
{
  "AzureAd": {
    "ClientId": "your-web-app-client-id",
    "ClientSecret": "your-web-app-client-secret"
  },
  "ContainerApi": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

### 3. 运行应用

```bash
cd WebApp/HelloContainer.WebApp
dotnet run
```

## 架构说明

### 认证流程
1. 用户访问Web应用
2. 重定向到Azure AD登录
3. 登录成功后获取ID Token和Access Token
4. Web应用使用Access Token调用Container API

### 主要组件

- **ContainerApiClient**: 封装API调用逻辑，自动添加Authorization头
- **HomeController**: 处理Web请求和API交互
- **Views**: Razor页面提供用户界面

### Token管理
- 使用Microsoft.Identity.Web自动管理token刷新
- 每次API调用前自动获取最新的Access Token
- 支持token缓存和刷新机制

## 安全特性

- 所有页面都需要认证
- CSRF防护
- 安全的token传递
- HTTPS强制

