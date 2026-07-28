BasePanel:subClass("DailyRewardsPanel")

util = require("xlua.util")
DailyRewardsPanel.content = nil
DailyRewardsPanel.items = {}
DailyRewardsPanel.panelName = "DailyRewardsPanel"
DailyRewardsPanel.ResultPanel = nil

function DailyRewardsPanel:Init()
    self.base.Init(self)
    self.content = self.panelObj.transform:Find("Container/Datalist").transform
    self.ResultPanel = self.panelObj.transform:Find("ResultPanel").gameObject
    if (self.isInitEvent == false) then
        self.isInitEvent = true
        self:GetControl("CloseBtn", "Button").onClick:AddListener(function()
            self:HideMe()
        end)
        self:GetControl("ClaimBtn", "Button").onClick:AddListener(function()
            local week = os.date("*t").wday
            local result
            if week == 2 or week == 4 or week == 6 then
                result = 1
            elseif week == 3 or week == 5 or week == 7 then
                result = 2
            elseif week == 1 then
                result = 3
            end
            -- 先激活父节点 ResultPanel，再激活对应的结果子节点
            self.ResultPanel:SetActive(true)
            self.ResultPanel.transform:Find("Panel/" .. tostring(result)).gameObject:SetActive(true)
            self:GetControl("CloseBtn", "Button"):StartCoroutine(util.cs_generator(DelayClose, self.ResultPanel))
        end)
    end
end

function DelayClose(go)
    coroutine.yield(WaitForSeconds(2))
    go:SetActive(false)
end

function DailyRewardsPanel:ShowMe()
    self.base.ShowMe(self)

    -- 删除
    for i = 1, #self.items do
        self.items[i]:Destroy()
    end
    self.items = {}

    for i = 1, 7 do
        local item = ItemGrid:new()
        ItemGrid:Init(self.content)
        item:InitData(i)
        table.insert(self.items, item)
    end
end
