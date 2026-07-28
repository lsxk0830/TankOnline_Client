Object:subClass("ItemGrid")

ItemGrid.obj = nil  -- 格子物体
ItemGrid.Icon = nil -- 格子图标
ItemGrid.Day_Text = nil
ItemGrid.Count_Text = nil

function ItemGrid:Init(father)
    self.obj = ABManager:LoadRes("ui", "Item_Day", typeof(GameObject))
    self.Icon = self.obj.transform:Find("Icon"):GetComponent(typeof(Image))
    self.Day_Text = self.obj.transform:Find("DayPanel/CountText"):GetComponent(typeof(TMP_Text))
    self.Count_Text = self.obj.transform:Find("CountPanel/CountText"):GetComponent(typeof(TMP_Text))

    self.obj.transform:SetParent(father, false)
end

function ItemGrid:InitData(day)
    local spriteAtlas = ABManager:LoadRes("ui", "ItemDay_IconAtlas", typeof(SpriteAltas))
    local tempDay = (day % 2 == 0) and 1 or 0
    local tempStr = (tempDay == 0) and "Coin" or "Diamond"
    self.Icon.sprite = spriteAtlas:GetSprite(tempStr)
    self.Day_Text.text = tostring(day) .. "天"
    self.Count_Text.text = tostring(day * 100)
end

-- 加自己的逻辑
function ItemGrid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
    self.Icon = nil
    ItemGrid.Day_Text = nil
    ItemGrid.Count_Text = nil
end
