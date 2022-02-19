--[[
    Title:
        Lua UI 框架

    Description:
        UI管理类

    Author : Zhaiyurong

    Date : 2022.2

    Modify:

]]

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



local _M = {}

--显示UI
function _M.ShowWindow(uiname)

end

return _M


