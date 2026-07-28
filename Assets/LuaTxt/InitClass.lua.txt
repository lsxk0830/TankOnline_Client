print("--初始化类别名--") --常用别名在这里定义

--Unity相关
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
SpriteAltas = CS.UnityEngine.U2D.SpriteAtlas -- 图集
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
TextAsset = CS.UnityEngine.TextAsset
WaitForSeconds = CS.UnityEngine.WaitForSeconds


--UI
UI = CS.UnityEngine.UI
Image = CS.UnityEngine.UI.Image
Button = CS.UnityEngine.UI.Button
Text = CS.UnityEngine.UI.Text
TMP_Text = CS.TMPro.TextMeshProUGUI
Toggle = CS.UnityEngine.UI.Toggle
ScrollRect = CS.UnityEngine.UI.ScrollRect
UIBehaviour = CS.UnityEngine.EventSystems.UIBehaviour

--自己写的Lua相关
require("Object")             -- 面向对象
require("SplitTools")         -- 字符串拆分
Json = require("JsonUtility") -- Json解析

-- 找对象
Canvas = GameObject.Find("Canvas").transform -- 找到Canvas对象

--自己写的C#相关
ABManager = CS.ABManager.Instance
