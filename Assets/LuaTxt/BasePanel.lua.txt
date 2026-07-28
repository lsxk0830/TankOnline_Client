Object:subClass("BasePanel")

BasePanel.panelObj = nil      -- 面板物体
BasePanel.controls = {}       --控件名、控件本身
BasePanel.isInitEvent = false -- 是否注册过事件，避免重复注册

function BasePanel:Init()
    if (self.panelObj == nil) then
        self.panelObj = ABManager:LoadRes("ui", self.panelName, typeof(GameObject))
        self.panelObj.transform:SetParent(Canvas, false)
        --找所有UI控件存起来
        -- 为了避免找各种无用空间，定一个规范，空间命名按照控件类型命名，
        -- Button Btn名字
        -- Toggle Tog名字
        -- Image Img名字
        -- Text Text名字
        -- TextMeshProUGUI TMP_Text名字
        -- ScrollView SV名字
        local allControls = self.panelObj:GetComponentsInChildren(typeof(UIBehaviour))
        --[[
        MainPanel (面板根)
        ├── RoleBtn          → Button + Image    → 名字含 "Btn" ✅ 存入
        ├── SkillBtn         → Button + Image    → 名字含 "Btn" ✅ 存入
        ├── BgImg            → Image             → 名字含 "Img" ✅ 存入
        ├── TitleText        → Text              → 名字含 "Text" ✅ 存入
        └── SomeDecorator    → Image             → 名字不含规范关键字 ❌ 丢弃
        ]] --
        for i = 0, allControls.Length - 1 do
            local controlName = allControls[i].name
            --print("找到控件", controlName, allControls[i]:GetType().Name)
            if (string.find(controlName, "Btn") ~= nil) or
                (string.find(controlName, "Tog") ~= nil) or
                (string.find(controlName, "Img") ~= nil) or
                (string.find(controlName, "Text") ~= nil) or
                (string.find(controlName, "TMP_Text") ~= nil) or
                (string.find(controlName, "SV") ~= nil) then
                -- 获取控件类型
                local typeName = allControls[i]:GetType().Name
                -- 避免一个对象挂多个UI组件，出现覆盖问题
                if (self.controls[controlName] ~= nil) then
                    self.controls[controlName][typeName] = allControls[i]
                else
                    self.controls[controlName] = { [typeName] = allControls[i] }
                end
                --[[
                最终形式
                {
                    BtnRole = {Image = 控件, Button = 控件},
                    TextRole = {Text = 控件}
                    TogTest = {Toggle = 控件, Image = 控件}
                }
                ]] --
            end
        end
    end
end

-- 得到控件 根据物体对象名 + 控件类型字符串名(Button, Toggle, Image, Text, TMP_Text, ScrollView)
function BasePanel:GetControl(name, typeName)
    if self.controls[name] ~= nil then
        local control = self.controls[name]
        if control[typeName] ~= nil then
            return control[typeName]
        else
            print("控件类型错误,找不到", name, typeName)
        end
        return nil
    end
end

function BasePanel:ShowMe()
    self:Init()
    self.panelObj:SetActive(true)
end

function BasePanel:HideMe()
    self.panelObj:SetActive(false)
end
