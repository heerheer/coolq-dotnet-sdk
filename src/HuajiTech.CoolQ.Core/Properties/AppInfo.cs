﻿// 用于 HuajiTech.CoolQ 的应用信息配置文件。
// 最后更新于 2020-5-18 14:16。

using HuajiTech.CoolQ;

// 指定资源管理器的默认区域性信息。通常情况下，无需更改此项。
[assembly: System.Resources.NeutralCoreResourcesLanguage("zh-CN")]

// 指定插件的默认加载阶段。
// 若插件于 Enabled 阶段加载，允许在构造函数内调用 API。
// 注意：若在非 Initializing 阶段加载，删除 app.json 中的对应事件将会导致插件不被加载。
[assembly: DefaultPluginLoadStage((int)AppLifecycle.Enabled)]

// 指定应用的 AppID。必须更改此项。
// dev 目录下存放应用的目录名必须与 AppID 匹配。
[assembly: AppId("com.example.app")]

// 重写指定插件类的加载阶段。
// [assembly: PluginLoadStage(typeof(MyPlugin), (int)AppLifecycle.Initializing)]