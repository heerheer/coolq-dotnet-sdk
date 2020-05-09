﻿// 用于 HuajiTech.CoolQ 的应用信息配置文件。
// 版本 0.2.1-alpha.2。

using HuajiTech.CoolQ;

// 指定资源管理器的默认区域性信息。通常情况下，无需更改此项。
[assembly: System.Resources.NeutralResourcesLanguage("zh-CN")]

// 指定插件的默认加载阶段。可通过在插件类上应用 PluginLoadStageAttribute 重写此项。
// 若插件于 Enabled 阶段加载，允许在构造函数内调用 API。
[assembly: PluginLoadStage((int)AppLifecycle.Enabled)]

// 指定应用的 AppID。必须更改此项。
// dev 目录下存放应用的目录名必须与 AppID 匹配。
[assembly: AppId("com.example.app")]