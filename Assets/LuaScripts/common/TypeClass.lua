--[[
    Title:
        Lua UI 框架

    Description:
        类注册，类管理

    Author : Zhaiyurong

    Date : 2022.2

    Modify:

]]

local _M = {}
local TypeClasses = {}

function _M.RegisterClass(type, class)
    print("Register new Class, type = ", type)
    TypeClasses[type] = class
end


return _M