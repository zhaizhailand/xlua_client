--[[
    Title:
        Lua UI 框架

    Description:
        UIView基类

    Author : Zhaiyurong

    Date : 2022.2

    Modify:

]]


local BaseClass = require 'common/BaseClass'

--UI基类
UIViewBase = BaseClass:Define("UIViewBase")

--UI初始化
function UIViewBase:Init()
    self.gameObject = nil
    self.transform = nil

    self.isShown = false

    self.windowType = UIWindowType.Normal
    self.showType = UIWindowShowType.DoNothing
    
    self.isForceRefresh = false
    self.callbacks = {}
end

function UIViewBase:Show(data, bNeedInvalidate, callback)

end

--UI显示完毕
function UIViewBase:OnCompletedShow()

end

--返回
function UIViewBase:ReturnWindow()

end

--UI隐藏
function UIViewBase:Hide()
    self:OnHide()
end


------ virtual functions start --------
function UIViewBase:OnInit()

end

--UI显示
function UIViewBase:Invalidate()

end

function UIViewBase:PreHide()

end

function UIViewBase:OnHide()

end

function UIViewBase:OnDestroy()

end

-- ~~~ virtual functions end ~~~