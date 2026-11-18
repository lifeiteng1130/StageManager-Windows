# StageManager Windows 定制版

基于 [awaescher/StageManager](https://github.com/awaescher/StageManager) 修改的 Windows 台前调度工具。保留上游 MIT 许可证与版权声明，本仓库不是上游官方发行版。

## 下载与使用

前往 [Releases](https://github.com/lifeiteng1130/StageManager-Windows/releases) 下载 Windows x64 便携 ZIP，完整解压后运行 `StageManager.exe`。已包含 .NET 运行时，无需额外安装。支持 Windows 10 2004 或更新版本。

- 自动识别桌面窗口，按应用创建场景。
- 点击侧边卡片切换整组窗口；Shift + 点击其他卡片将其加入当前组，也可拖拽合并。
- 托盘右键 → 设置 → 侧边栏 → 显示方式，可选择“始终显示”或“自动隐藏”。常驻模式悬浮在应用之上，不自动调整应用宽度。
- 正常退出请使用托盘菜单。若上次异常退出，可在进程结束后运行 `恢复窗口.cmd`。
- 请勿同时运行 SillDeck 或其他 StageManager 实例。

## 本仓库修改

- 增加侧边栏常驻/自动隐藏设置，并持久化保存。
- 退出生命周期保护，避免未完成的场景切换再次隐藏窗口；逐窗口恢复失败隔离。
- 启动初始场景回退，以及接管前最小化状态的退出恢复。
- 增加便携数据目录 `Data`、恢复入口和独立版打包脚本。

自动接管、多应用分组、实时预览和整组切换的主要实现来自上游。基础提交：`9a401a64818c286c8e64782f42b4779f64214569`。

## 开发与验证

需要 .NET 10 SDK 和 Windows：

```powershell
dotnet test StageManager.Tests/StageManager.Tests.csproj
.\publish-portable.ps1 -DotnetPath dotnet -OutputDirectory .\Artifacts
```

最近本地验证：578 项测试通过，Windows x64 自包含发布及 WPF 启动自检通过。真实桌面交互兼容性仍需实际验证；多屏独立调度、跨重启分组恢复尚不在本定制版范围。

`portable.flag` 启用便携模式，设置和恢复日志写入程序旁的 `Data` 目录。请勿将运行数据提交到仓库。

## 许可和来源

MIT，原版权：Copyright (c) 2022 Andreas Wäscher，完整内容见 [LICENSE](LICENSE)。下面保留上游项目介绍。

---
# Stage Manager for Windows

This is an experimental approach to bring the macOS [Stage Manager](https://support.apple.com/en-us/HT213315) to Microsoft Windows.

> **Important:** This is a prototype and a feasibility study - I am not actively developing this project at the moment but I'd be happy to review and merge pull requests. 

![Stage Manager](media/StageManager%20Basics.gif)

This prototype groups applications by their executable. By switching between so called "scenes" in the sidebar, Stage Manager hides other windows and the desktop icons, helping you to focus. Scene changes are animated: the windows fly between the sidebar and the stage.

Drag a card from the sidebar onto the stage to add its windows to the current scene, or onto another card to merge both. Shift+click does the same as dragging onto the stage. The sidebar can sit on the left or the right side of the screen.

Scenes can be switched with the keyboard: hold `Ctrl+Alt+Win` and press `Tab` for the next scene (the sidebar shows the pick), `Shift+Tab` for the previous one, or `1` to `9` for the card at that position. The scene opens when you let go of the keys, `Esc` cancels. The held keys can be changed in the settings by clicking into the field and pressing the new keys. Combinations that are already used by Windows or another app are named there.

Apps that StageManager should leave alone, such as launchers or overlays, can be excluded: either on the "Apps" page of the settings (choose from the running apps or an app file, and take them off the list with the trash can) or with a right click on a card. If the card holds several apps, the menu names each of them. The windows of an excluded app get no card and are never hidden, and they are back right away when the app is included again.

The menus and settings are available in English, German, Spanish, French, Italian, Portuguese, Russian, Japanese, Korean, Simplified Chinese and Turkish. StageManager follows the display language of Windows, or you choose one in the settings. New languages are a copy of `StageManager.Core/Resources/Strings.resx`: translate the values, name the file `Strings.<language>.resx` and add the language to `Languages.cs`. A test checks that every language has all texts.

## Usage

Download and run the executable from the [Releases tab](https://github.com/awaescher/StageManager/releases/) or 
 - clone this repository
 - cd into the repository directory
 - run `dotnet run --project StageManager`

To quit, find the app's tray icon (Windows might move it into the overflow menu) and use its context menu to close the app. The tray menu also has the settings and can bring back windows that ended up out of the screen.
 
### Requirements
 - Windows 10 version 2004 or newer
 - [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)

## To do

This is an experimental fun project. I don't have any idea whether or not this is going to be a final product one day. 

|Topic|State|
|-|-|
|**Experimental stage**||
|initial windows grouping by process|✅|
|3D display of opened windows (static)|✅|
|hide/show windows of given scenes|✅|
|hide/show desktop icons|✅|
|scene management with drag&drop|✅|
|restore windows on quit/restart|✅|
|auto hide & fly-in scenes for maximized windows|✅|
|full screenshots for windows that were minimized on startup|✅|
|drag windows from other scenes into the current one|✅|
|place screenshots in relative size of the desktop|⬜|
|limit maximum scenes (like 6 for macOS?)|✅|
|limit window count per scene (like newest 5)|✅|
|tray icon to start & stop|✅|
|start with Windows|✅|
|**Product stage**||
|several apps per scene (merge, split, companion apps)|✅|
|keep the stacking order of the windows of a scene|✅|
|sidebar on the left or the right|✅|
|settings window|✅|
|switch scenes with the keyboard (configurable keys)|✅|
|translated user interface (11 languages, switchable in the settings)|✅|
|exclude apps (settings page "Apps" or right click on a card)|✅|
|bring back windows that ended up out of the screen|✅|
|virtual desktop support (pin window)|⬜|
|multi-monitor support|⬜|
|visual feedback when dragging windows from other scenes|✅|
|feature parity with macOS Stage Manager|⬜|
|**Polishing stage**||
|window animations|✅|
|live dwm thumbnails|✅|
|adjust 3D angle according to screen position|⬜|
|sidebar steps aside in the desktop view and flies back in at the screen edge|✅|

Contributions very welcome :heart:

---

Stage Manager is using a few code files to handle window tracking from [workspacer](https://github.com/workspacer/workspacer), an amazing open source project by [Rick Button](https://github.com/rickbutton).
