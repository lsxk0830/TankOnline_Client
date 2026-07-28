print("--Lua入口执行--")

require("InitClass") -- 初始化所有准备好的类别名

require("BasePanel") -- 初始化面板基类
require("ItemGrid")
require("DailyRewardsPanel")

DailyRewardsPanel:ShowMe() -- 显示主面板
