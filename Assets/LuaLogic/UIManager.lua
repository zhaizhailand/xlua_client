local BaseClass = require 'common/BaseClass'

--窗口类型
UIWindowType = {
    Normal = 1,

}

--窗口显示方式
UIWindowShowType = {
    DoNothing = 1,
    HideOther = 2,

}

--UI注册
function RegisterView()

end

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




local _M = {}

--显示UI
function _M.ShowWindow(uiname)

end

return _M


